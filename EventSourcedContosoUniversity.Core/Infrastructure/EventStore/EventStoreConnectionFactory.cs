using System.Net;
using EventStore.ClientAPI;
using System.Linq;

namespace EventSourcedContosoUniversity.Core.Infrastructure.EventStore
{
    public class EventStoreConnectionFactory
    {
        private readonly EventStoreSettings _storeSettings;
        private IEventStoreConnection _connection;

        public EventStoreConnectionFactory(EventStoreSettings storeSettings)
        {
            _storeSettings = storeSettings;
        }

        public IEventStoreConnection Create()
        {
            if (_connection != null)
                return _connection;

            if(!IPAddress.TryParse(_storeSettings.EventStoreIP, out IPAddress iPAddress))
            {
                iPAddress = Dns.GetHostAddresses(_storeSettings.EventStoreIP).FirstOrDefault();
            }
            
            var endPoint = new IPEndPoint(iPAddress, _storeSettings.EventStorePort);
            _connection = EventStoreConnection.Create(endPoint);
            _connection.Disconnected += (s, e) =>
            {
                _connection = null;
            };
            _connection.ErrorOccurred += (s, e) =>
            {
                _connection = null;
            };
            _connection.Closed += (s, e) =>
            {
                _connection = null;
            };
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
