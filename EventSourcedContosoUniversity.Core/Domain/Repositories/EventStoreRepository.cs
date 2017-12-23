using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventStore.ClientAPI;

namespace EventSourcedContosoUniversity.Core.Domain.Repositories
{
    public class EventStoreRepository<T> : IRepository<T> where T : AggregateRoot
    {
        private const string AggregateClrTypeHeader = "AggregateClrTypeName";
        private const string CommitIdHeader = "CommitId";
        private const int WritePageSize = 500;
        private const int ReadPageSize = 500;

        private readonly IEventStoreConnection _eventStoreConnection;

        public EventStoreRepository(IEventStoreConnection eventStoreConnection)
        {
            _eventStoreConnection = eventStoreConnection;
        }

        public async Task<T> GetById(Guid id)
        {
            var streamName = AggregateIdToStreamName(typeof(T), id);
            var aggregate = ConstructAggregate<T>();

            StreamEventsSlice currentSlice;
            long nextSliceStart = 0;
            do
            {
                currentSlice = await _eventStoreConnection.ReadStreamEventsForwardAsync(streamName, nextSliceStart, ReadPageSize, false);
                nextSliceStart = currentSlice.NextEventNumber;

                var events = new List<Event>();
                foreach (var @event in currentSlice.Events)
                {
                    events.Add(EventSerializer.DeserializeResolvedEvent(@event));
                }
                aggregate.LoadsFromHistory(events);
            } while (!currentSlice.IsEndOfStream);

            return aggregate;
        }

        public async Task Save(T aggregate)
        {
            var commitHeaders = new Dictionary<string, object>
            {
                //{CommitIdHeader, aggregate.Id},
                {AggregateClrTypeHeader, aggregate.GetType().AssemblyQualifiedName}
            };

            var streamName = AggregateIdToStreamName(aggregate.GetType(), aggregate.Id);
            var newEvents = aggregate.GetUncommittedChanges().Cast<object>().ToList();
            var originalVersion = aggregate.Version - newEvents.Count;
            var expectedVersion = originalVersion < 0 ? ExpectedVersion.NoStream : originalVersion;
            var preparedEvents = newEvents.Select(e => EventSerializer.Create(Guid.NewGuid(), e, commitHeaders)).ToList();

            if (preparedEvents.Count < WritePageSize)
            {
                await _eventStoreConnection.AppendToStreamAsync(streamName, expectedVersion, preparedEvents);
            }
            else
            {
                var transaction = await _eventStoreConnection.StartTransactionAsync(streamName, expectedVersion);

                var position = 0;
                while (position < preparedEvents.Count)
                {
                    var pageEvents = preparedEvents.Skip(position).Take(WritePageSize);
                    await transaction.WriteAsync(pageEvents);
                    position += WritePageSize;
                }

                await transaction.CommitAsync();
            }

            aggregate.MarkChangesAsCommitted();
        }

        private static TAggregate ConstructAggregate<TAggregate>()
        {
            return (TAggregate)Activator.CreateInstance(typeof(TAggregate), true);
        }

        private string AggregateIdToStreamName(Type type, Guid id)
        {
            //Ensure first character of type name is lower case to follow javascript naming conventions
            return string.Format("{0}-{1}", char.ToLower(type.Name[0]) + type.Name.Substring(1), id.ToString("N"));
        }

        public Task Delete(T aggregate)
        {
            var streamName = AggregateIdToStreamName(typeof(T), aggregate.Id);
            var newEvents = aggregate.GetUncommittedChanges().Cast<object>().ToList();
            var originalVersion = aggregate.Version - newEvents.Count;
            var expectedVersion = originalVersion < 0 ? ExpectedVersion.NoStream : originalVersion;
            return _eventStoreConnection.DeleteStreamAsync(streamName, expectedVersion);
        }
    }
}
