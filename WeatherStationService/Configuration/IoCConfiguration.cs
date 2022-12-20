namespace WeatherStationService.Configuration
{
    public static class IoCConfiguration
    {
        public static IServiceCollection AddServices(this WebApplicationBuilder builder)
        {
            var services = builder.Services;
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.WriteIndented = true; // format json responses
                });
            services.AddSwagger(builder.Configuration);

            return services;
        }
    }
}
