using Microsoft.Extensions.Options;
using System.Net;
using WeatherStationService.Configuration;

namespace WeatherStationService
{
    public class WeatherService : IHostedService
    {
        private readonly ILogger<WeatherService> _logger;
        private readonly IOptionsMonitor<AppSettings> _options;
        private SocketServer SocketServer;

        public WeatherService(ILogger<WeatherService> logger, IOptionsMonitor<AppSettings> options)
        {
            _logger = logger;
            _options = options;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogTrace($"{nameof(WeatherService)} start running on port: {_options.CurrentValue.Port}.");

            SocketServer = new SocketServer(_logger, IPAddress.Any, _options.CurrentValue.Port);

            _logger.LogTrace("Server starting...");

            SocketServer.Start();

            _logger.LogTrace("Done!");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogTrace($"{nameof(WeatherService)} is stopping.");

            _logger.LogTrace("Server stopping...");
            SocketServer.Stop();
            _logger.LogTrace("Done!");

            return Task.CompletedTask;
        }
    }
}
