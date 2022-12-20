using WeatherStationService.HttpClients;

namespace WeatherStationService.Configuration
{
    public static class HttpClientsConfiguration
    {
        public static void AddHttpClients(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            var appSettings = new AppSettings();

            configuration.GetSection("AppSettings").Bind(appSettings);

            builder.Services.AddHttpClient<OpenMeteoClient>("OpenMeteoClient", x => { x.BaseAddress = new Uri("https://api.open-meteo.com/v1"); });
            builder.Services.AddSingleton<IOpenMeteoClientFactory, OpenMeteoClientFactory>();
        }
    }
}
