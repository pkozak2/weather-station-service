using Microsoft.AspNetCore.HttpLogging;
using NLog;
using NLog.Web;
using WeatherStationService;
using WeatherStationService.Configuration;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");
try
{
    var webApplicationOptions = new WebApplicationOptions()
    {
        ContentRootPath = AppContext.BaseDirectory,
        Args = args,
        ApplicationName = System.Diagnostics.Process.GetCurrentProcess().ProcessName
    };

    var builder = WebApplication.CreateBuilder(webApplicationOptions);

    ConfigurationManager configuration = builder.Configuration;

    builder.Services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

    builder.AddNlog();
    builder.AddWindowsService();
    builder.AddServices();

    builder.Services.AddHttpLogging(options =>
    {
        options.LoggingFields = HttpLoggingFields.RequestPath
            | HttpLoggingFields.RequestBody
            | HttpLoggingFields.ResponseStatusCode;
    });


    // Add services to the container.
    builder.Services.AddHostedService<WeatherService>();

    builder.Services.AddControllers();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    app.ConfigureSwagger();

    app.UseCors(x => x
        .AllowAnyMethod()
        .AllowAnyHeader()
        .SetIsOriginAllowed(_ => true) // allow any origin
        .AllowCredentials());

    app.MapControllers();

    app.Run();
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}