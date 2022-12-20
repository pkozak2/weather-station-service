using System.Reflection;

using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;
using WeatherStationService.Swagger;

namespace WeatherStationService.Configuration
{
    public static class SwaggerConfiguration
    {
        public static void AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            var config = new AppSettings();
            var appSettings = new AppSettings();
            configuration.GetSection("AppSettings").Bind(appSettings);

            services.AddApiVersioning(
                options =>
                {
                    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                    options.AssumeDefaultVersionWhenUnspecified = true;

                    // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                    options.ReportApiVersions = true;
                });
            services.AddVersionedApiExplorer(
                options =>
                {
                    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                    // note: the specified format code will format the version as "'v'major[.minor][-status]"
                    options.GroupNameFormat = "'v'VVV";

                    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                    // can also be used to control the format of the API version in route templates
                    options.SubstituteApiVersionInUrl = true;
                });
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(
                options =>
                {
                    // add a custom operation filter which sets default values
                    options.OperationFilter<SwaggerDefaultValues>();

                    // integrate xml comments
                    options.IncludeXmlComments(XmlCommentsFilePath);

                    OpenApiSecurityScheme securityDefinition = new OpenApiSecurityScheme()
                    {
                        Name = "Bearer",
                        BearerFormat = "JWT",
                        Scheme = "bearer",
                        Description = "Specify the authorization token.",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                    };
                    options.AddSecurityDefinition("jwt_auth", securityDefinition);

                    // Make sure swagger UI requires a Bearer token specified
                    OpenApiSecurityScheme securityScheme = new OpenApiSecurityScheme()
                    {
                        Reference = new OpenApiReference()
                        {
                            Id = "jwt_auth",
                            Type = ReferenceType.SecurityScheme
                        }
                    };
                    OpenApiSecurityRequirement securityRequirements = new OpenApiSecurityRequirement()
                    {
                        { securityScheme, new string[] { } },
                    };
                    options.AddSecurityRequirement(securityRequirements);
                });
        }

        public static void ConfigureSwagger(this WebApplication app)
        {
            var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    // build a swagger endpoint for each discovered API version
                    foreach (var description in provider.ApiVersionDescriptions.Select(s => s.GroupName))
                    {
                        options.SwaggerEndpoint($"/swagger/{description}/swagger.json", description.ToUpperInvariant());
                        options.RoutePrefix = string.Empty;
                    }
                });
        }

        private static string XmlCommentsFilePath
        {
            get
            {
                var fileName = typeof(Program).GetTypeInfo().Assembly.GetName().Name + ".xml";
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            }
        }
    }
}