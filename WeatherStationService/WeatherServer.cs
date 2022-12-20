namespace WeatherStationService
{
    public class WeatherServer : IHostedService, IDisposable
    {
        private readonly ILogger<WeatherServer> _logger;

        public WeatherServer(ILogger<WeatherServer> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogTrace($"{nameof(WeatherServer)} start running.");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogTrace($"{nameof(WeatherServer)} is stopping.");
            return Task.CompletedTask;
        }

        public void Dispose()
        {
           
        }
    }
}
