using System;
using System.Threading.Tasks;
using EventSourcedContosoUniversity.Core.Domain;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;

namespace EventSourcedContosoUniversity.Core
{
    public class EventStoreDispatcher
    {
        private readonly EventStoreConnectionFactory _eventStoreConnectionFactory;
        private EventStoreCatchUpSubscription _subscription;
        private Action<long, long> _lastProcessedUpdatedAction;
        private Position _lastProcessedPosition;
        private Action<object> _handleEvent;

        public EventStoreDispatcher(EventStoreConnectionFactory eventStoreConnectionFactory)
        {
            _eventStoreConnectionFactory = eventStoreConnectionFactory;
        }

        public void StartDispatchingForAll(
            long lastCommitPosition = 0, long lastPreparePosition = 0,
            Action<object> handleEvent=null,
            Action<long, long> lastProcessedUpdatedAction = null)
        {
            _lastProcessedPosition = new Position(lastCommitPosition, lastPreparePosition);
            _lastProcessedUpdatedAction = lastProcessedUpdatedAction;
            _handleEvent = handleEvent;
            RecoverSubscription();
        }
        private void RecoverSubscription()
        {
            if (_subscription != null)
            {
                _subscription.Stop();
            }

            var connection = _eventStoreConnectionFactory.Create();
            var settings = CatchUpSubscriptionSettings.Default;
            //TODO: read credentials from config
            _subscription = connection.SubscribeToAllFrom(_lastProcessedPosition, settings, EventAppeared, null, 
                HandleSubscriptionDropped, new UserCredentials("admin","changeit"));
        }
        public void StopDispatching(TimeSpan timeout)
        {
            _subscription?.Stop(timeout);
        }

        private void HandleSubscriptionDropped(EventStoreCatchUpSubscription sub, SubscriptionDropReason reason, Exception error)
        {
            if (reason == SubscriptionDropReason.UserInitiated)
                return;

            if (reason == SubscriptionDropReason.ProcessingQueueOverflow || reason == SubscriptionDropReason.CatchUpError)
            {
                _subscription.Stop();
                _eventStoreConnectionFactory.Reset();
                RecoverSubscription();
                return;
            }

            RecoverSubscription();
        }

        private Task EventAppeared(EventStoreCatchUpSubscription sub, ResolvedEvent @event)
        {
            var processedEvent = EventSerializer.DeserializeResolvedEvent(@event);
            if (processedEvent != null)
            {
                try
                {
                    _handleEvent(processedEvent);

                }
                catch(Exception ex) 
                {
                    //TODO: handle
                }
            }

            if (!@event.OriginalPosition.HasValue)
            {
                throw new ArgumentException("ResolvedEvent didn't come off a subscription to all (has no position).");
            }

            _lastProcessedPosition = new Position(@event.OriginalPosition.Value.CommitPosition, @event.OriginalPosition.Value.PreparePosition);

            try
            {
                _lastProcessedUpdatedAction?.Invoke(@event.OriginalPosition.Value.CommitPosition, @event.OriginalPosition.Value.PreparePosition);

            }
            catch (Exception ex)
            {
                //TODO: handle
            }

            return Task.CompletedTask;
        }

    }
}
