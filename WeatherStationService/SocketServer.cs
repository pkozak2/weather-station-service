using NetCoreServer;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace WeatherStationService
{
    public class SocketServer : UdpServer
    {
        private ILogger _logger;

        public SocketServer(ILogger logger, IPAddress address, int port) : base(address, port) {

            _logger = logger;
        }

        protected override void OnReceived(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            _logger.LogTrace("Incoming: " + Encoding.UTF8.GetString(buffer, (int)offset, (int)size));
            base.OnReceived(endpoint, buffer, offset, size);
        }

        protected override void OnError(SocketError error)
        {
            _logger.LogError($"Echo UDP server caught an error with code {error}");
        }
    }
}
