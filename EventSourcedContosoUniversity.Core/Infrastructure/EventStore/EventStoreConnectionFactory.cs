using System.Net;
using EventStore.ClientAPI;

namespace EventSourcedContosoUniversity.Core.Infrastructure.EventStore
{
    public class EventStoreConnectionFactory
    {
        private IEventStoreConnection _connection;

        public EventStoreConnectionFactory()
        {
        }

        public IEventStoreConnection Create()
        {
            if (_connection != null)
                return _connection;

            //TODO: inject configurations from application config
            var endPoint = new IPEndPoint(IPAddress.Loopback, 1113);
            _connection = EventStoreConnection.Create(endPoint);
            _connection.Disconnected += (s, e) => _connection = null;
            _connection.ErrorOccurred += (s, e) => _connection = null;
            _connection.ConnectAsync().Wait();
            

            return _connection;
        }

        public void Reset()
        {
            _connection?.Close();
            _connection = null;
        }
    }
}
