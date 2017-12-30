using System;
using Akka.Actor;
using EventSourcedContosoUniversity.Core.Infrastructure.EventStore;
using EventSourcedContosoUniversity.Core.ReadModel.Repositories;
using EventStore.ClientAPI;
using Serilog;

namespace EventSourcedContosoUniversity.Core.ReadModel
{
    public class BaseProjectionActor<T> : ReceiveActor where T : IMongoDocument
    {
        readonly EventStoreDispatcher _dispatcher;
        private readonly ICatchupPositionRepository _catchupPositionRepository;
        public Position LastProcessedPosition { get; private set; }

        public class SaveEventMessage
        {
            public long CommitPosition { get; set; }
            public long PreparePosition { get; set; }
        }
        public BaseProjectionActor(EventStoreDispatcher dispatcher, IReadModelRepository repository, ICatchupPositionRepository catchupPositionRepository)
        {
            _catchupPositionRepository = catchupPositionRepository;
            _dispatcher = dispatcher;

            ReceiveAsync<SaveEventMessage>(async (s) =>
            {
                var position = new Position(s.CommitPosition, s.PreparePosition);
                await _catchupPositionRepository.SavePosition<T>(position);
                LastProcessedPosition = position;
            });
        }

        protected override void PreStart()
        {
            var sender = Self;
            var last = _catchupPositionRepository.GetLastProcessedPosition<T>().Result;
            LastProcessedPosition = last != null ? new Position(last.CommitPosition, last.PreparePosition) : Position.Start;

            _dispatcher.StartDispatchingForAll(LastProcessedPosition.CommitPosition, LastProcessedPosition.PreparePosition, (o) =>
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
