using System;
using System.Collections.Generic;
using System.Text;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EventSourcedContosoUniversity.Core.Domain
{
    public static class EventSerializer
    {
        private const string EventClrTypeHeader = "EventClrTypeName";
        private static readonly JsonSerializerSettings SerializerSettings;

        static EventSerializer()
        {
            SerializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None };
        }

        public static EventData Create(Guid eventId, object @event, IDictionary<string, object> headers)
        {
            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event, SerializerSettings));

            var eventHeaders = new Dictionary<string, object>(headers)
                {
                    {EventClrTypeHeader, @event.GetType().AssemblyQualifiedName}
                };
            var metadata = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(eventHeaders, SerializerSettings));
            var typeName = @event.GetType().Name;

            return new EventData(eventId, typeName, true, data, metadata);
        }

        public static Event DeserializeResolvedEvent(ResolvedEvent rawEvent)
        {
            if (rawEvent.OriginalEvent.EventType.StartsWith("$") || rawEvent.OriginalEvent.EventStreamId.StartsWith("$"))
                return null;

            if (rawEvent.OriginalEvent.Metadata.Length <= 0 || rawEvent.OriginalEvent.Data.Length <= 0 )
                return null;

            return DeserializeRecordedEvent(rawEvent.OriginalEvent);
        }
        public static Event DeserializeRecordedEvent(RecordedEvent recordedEvent)
        {
            byte[] metadata = recordedEvent.Metadata;
            byte[] data = recordedEvent.Data;
            string decodedMetadata = Encoding.UTF8.GetString(metadata);
            var eventClrTypeName = JObject.Parse(decodedMetadata).Property(EventClrTypeHeader).Value;
            var @event = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(data), Type.GetType((string)eventClrTypeName));
            if(@event as Event == null)
            {
                // throw new Exception($"Failed to deserialize {eventClrTypeName} event with metadata {decodedMetadata}");
                return null;
            }
            return @event as Event;
        }
    }
}
