using Microsoft.Extensions.Hosting.WindowsServices;

namespace WeatherStationService.Configuration
{
    public static class WindowsServiceConfiguration
    {
        public static void AddWindowsService(this WebApplicationBuilder builder)
        {
            var isWindowsService = WindowsServiceHelpers.IsWindowsService();
            if (isWindowsService)
            {
                builder.Host.UseWindowsService();
            }
        }
    }
}
