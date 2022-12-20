using Microsoft.Extensions.Options;
using WeatherStationService.Configuration;

namespace WeatherStationService.HttpClients
{
    public interface IOpenMeteoClientFactory
    {
        public OpenMeteoClient GetClient();
    }

    public class OpenMeteoClientFactory : IOpenMeteoClientFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public OpenMeteoClientFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public OpenMeteoClient GetClient()
        {
            return _serviceProvider.GetRequiredService<OpenMeteoClient>();
        }
    }

    public class OpenMeteoClient
    {
        private readonly HttpClient _httpClient;
        private readonly IOptionsMonitor<AppSettings> _options;

        public OpenMeteoClient(HttpClient httpClient, IOptionsMonitor<AppSettings> options)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _options = options;
        }
    }
}
