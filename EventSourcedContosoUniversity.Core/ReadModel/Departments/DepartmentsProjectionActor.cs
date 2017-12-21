using System;
using Akka.Actor;
using EventSourcedContosoUniversity.Core.Domain.Events;
using EventSourcedContosoUniversity.Core.Infrastructure.EventStore;
using EventSourcedContosoUniversity.Core.ReadModel.Repositories;
using EventStore.ClientAPI;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace EventSourcedContosoUniversity.Core.ReadModel.Departments
{
    public class DepartmentsProjectionActor : ReceiveActor
    {
        readonly EventStoreDispatcher _dispatcher;
        private readonly ICatchupPositionRepository _catchupPositionRepository;

        public class SaveEventMessage
        {
            public long CommitPosition { get; set; }
            public long PreparePosition { get; set; }
        }
        public DepartmentsProjectionActor(IReadModelRepository repository, ICatchupPositionRepository catchupPositionRepository)
        {
            _catchupPositionRepository = catchupPositionRepository;
            _dispatcher = new EventStoreDispatcher(new EventStoreConnectionFactory());
            var client = new MongoClient();
            ReceiveAsync<DepartmentCreated>(async (s) =>
            {
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
                var department = await repository.GetById<DepartmentReadModel>(s.Id);
                department.Name = s.Name;
                department.Budget = s.Budget;
                department.Administrator = s.AdministratorId.ToString();
                department.StartDate = s.Startdate;
                await repository.Update(department);
            });
            ReceiveAsync<SaveEventMessage>( async (s) =>
            {
                await _catchupPositionRepository.SavePosition<DepartmentReadModel>(new Position(s.CommitPosition, s.PreparePosition));
            });
        }

        protected override void PreStart()
        {
            var sender = Self;
            var last= _catchupPositionRepository.GetLastProcessedPosition<DepartmentReadModel>().Result ;
            var lastProcessedPosition = last != null ? new Position(last.CommitPosition, last.PreparePosition) : Position.Start;
            
            _dispatcher.StartDispatchingForAll(lastProcessedPosition.CommitPosition, lastProcessedPosition.PreparePosition, (o) =>
            {
                sender.Tell(o);
            },
                (commitPosition, preparePosition) =>
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
