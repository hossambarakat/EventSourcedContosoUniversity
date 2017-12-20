using System;
using Akka.Actor;
using EventSourcedContosoUniversity.Core.Domain.Events;
using EventSourcedContosoUniversity.Core.Infrastructure.EventStore;
using EventSourcedContosoUniversity.Core.ReadModel.Repositories;
using EventStore.ClientAPI;
using MongoDB.Driver;

namespace EventSourcedContosoUniversity.Core.ReadModel.Departments
{
    public class DepartmentsProjectionActor : ReceiveActor
    {
        public class SaveEventMessage
        {
            public long CommitPosition { get; set; }
            public long PreparePosition { get; set; }
        }

        EventStoreDispatcher _dispatcher;
        public DepartmentsProjectionActor()
        {
            _dispatcher = new EventStoreDispatcher(new EventStoreConnectionFactory());
            var client = new MongoClient();
            ReceiveAsync<DepartmentCreated>(async (s) =>
            {
                var repository = new MongoRepository(client);
                await repository.Add(new DepartmentReadModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Budget = s.Budget,
                    Administrator = s.AdministratorId.ToString(),
                    StartDate = s.Startdate
                });
            });

            ReceiveAsync<DepartmentUpdated>(async (s) =>
            {
                var repository = new MongoRepository(client);
                var department = await repository.GetById<DepartmentReadModel>(s.Id);
                department.Name = s.Name;
                department.Budget = s.Budget;
                department.Administrator = s.AdministratorId.ToString();
                department.StartDate = s.Startdate;
                await repository.Update(department);
            });
            ReceiveAsync<SaveEventMessage>( async (s) =>
            {
                CatchupPositionRepository catchupPositionRepository = new CatchupPositionRepository(client);
                await catchupPositionRepository.SavePosition<DepartmentReadModel>(new Position(s.CommitPosition, s.PreparePosition));
            });
        }

        protected override void PreStart()
        {
            var sender = Self;
            var client = new MongoClient();
            CatchupPositionRepository catchupPositionRepository = new CatchupPositionRepository(client);
            var last=catchupPositionRepository.GetLastProcessedPosition<DepartmentReadModel>().Result ;
            var lastProcessedPosition = last != null ? new Position(last.CommitPosition, last.PreparePosition) : Position.Start;
            
            _dispatcher.StartDispatchingForAll(lastProcessedPosition.CommitPosition, lastProcessedPosition.PreparePosition, (o) =>
            {
                sender.Tell(o);
            },
                (long commitPosition, long preparePosition) =>
                {
                    sender.Tell(new SaveEventMessage
                    {
                        CommitPosition = commitPosition,
                        PreparePosition = preparePosition
                    });
                });
        }

        protected override void PostStop()
        {
            _dispatcher.StopDispatching(TimeSpan.FromSeconds(3000));
        }
    }
}
