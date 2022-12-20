using NLog.Web;

namespace WeatherStationService.Configuration
{
    public static class NlogConfiguration
    {
        public static void AddNlog(this WebApplicationBuilder builder)
        {
            // NLog: Setup NLog for Dependency injection
            builder.Logging.ClearProviders();
            builder.Logging.SetMinimumLevel(LogLevel.Trace);
            builder.Host.UseNLog();
        }
    }
}
