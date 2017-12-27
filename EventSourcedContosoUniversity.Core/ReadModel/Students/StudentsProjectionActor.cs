using System;
using Akka.Actor;
using EventSourcedContosoUniversity.Core.Domain.Events;
using EventSourcedContosoUniversity.Core.Infrastructure.EventStore;
using EventSourcedContosoUniversity.Core.ReadModel.Repositories;
using EventSourcedContosoUniversity.Core.ReadModel.Students;
using EventStore.ClientAPI;

namespace EventSourcedContosoUniversity.Core.ReadModel.Departments
{
    public class StudentsProjectionActor : ReceiveActor
    {
        readonly EventStoreDispatcher _dispatcher;
        private readonly ICatchupPositionRepository _catchupPositionRepository;

        public class SaveEventMessage
        {
            public long CommitPosition { get; set; }
            public long PreparePosition { get; set; }
        }
        public StudentsProjectionActor(EventStoreDispatcher dispatcher, IReadModelRepository repository, ICatchupPositionRepository catchupPositionRepository)
        {
            _catchupPositionRepository = catchupPositionRepository;
            _dispatcher = dispatcher;
            ReceiveAsync<StudentCreated>(async (s) =>
            {
                await repository.Add(new StudentReadModel
                {
                    Id = s.Id,
                    LastName = s.LastName,
                    FirstMidName = s.FirstMidName,
                    EnrollmentDate = s.EnrollmentDate
                });
            });

            ReceiveAsync<StudentUpdated>(async (s) =>
            {
                var student = await repository.GetById<StudentReadModel>(s.Id);
                student.FirstMidName = s.FirstMidName;
                student.LastName = s.LastName;
                student.EnrollmentDate = s.EnrollmentDate;
                await repository.Update(student);
            });
            ReceiveAsync<StudentDeleted>(async (s) =>
            {
                var student = await repository.GetById<StudentReadModel>(s.Id);
                await repository.Delete(student);
            });
            ReceiveAsync<SaveEventMessage>( async (s) =>
            {
                await _catchupPositionRepository.SavePosition<StudentReadModel>(new Position(s.CommitPosition, s.PreparePosition));
            });
        }

        protected override void PreStart()
        {
            var sender = Self;
            var last= _catchupPositionRepository.GetLastProcessedPosition<StudentReadModel>().Result ;
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
