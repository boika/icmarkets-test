using System.Text.Json;
using System.Text.Json.Serialization;
using Asp.Versioning;
using ICMarketsTest.Storage;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.OpenApi;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using Serilog;

namespace ICMarketsTest.WebApi.Extensions;

internal static class WebApplicationBuilderExtensions
{
    /// <summary>
    /// Adds configuration along with all the necessary services
    /// </summary>
    internal static WebApplicationBuilder Configure(this WebApplicationBuilder builder) => builder
        .ValidateServiceProvider()
        .AddSerilogLogging()
        .AddMetrics()
        .AddAspNetCore()
        .AddSwaggerForDevelopment()
        .AddCoreServices();

    /// <summary>
    /// Ensures all the registered services can be created
    /// </summary>
    private static WebApplicationBuilder ValidateServiceProvider(this WebApplicationBuilder builder)
    {
        builder.Host
            .UseDefaultServiceProvider(static (context, options) =>
            {
                options.ValidateOnBuild = true;
                options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
            });

        return builder;
    }

    /// <summary>
    /// Adds Serilog as default logging provider
    /// </summary>
    private static WebApplicationBuilder AddSerilogLogging(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddSerilog((services, options) => options
                .ReadFrom.Configuration(builder.Configuration)
                .ReadFrom.Services(services));

        return builder;
    }

    /// <summary>
    /// Adds all the asp.net core infrastructure
    /// </summary>
    private static WebApplicationBuilder AddAspNetCore(this WebApplicationBuilder builder)
    {
        // Add MVC with JSON serialization, validation and lowercase urls
        builder.Services
            .Configure<RouteOptions>(static options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            })
            .AddValidation()
            .AddControllers(static options =>
            {
                options.ModelMetadataDetailsProviders.Add(new SystemTextJsonValidationMetadataProvider());
            })
            .AddControllersAsServices()
            .AddJsonOptions(static options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

        // Add basic API versioning
        builder.Services
            .AddApiVersioning(static options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });

        // Add basic CORS policy
        builder.Services
            .AddCors(static options => options.AddDefaultPolicy(static policy => policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()));

        // Add basic and db health checks
        builder.Services
            .AddHealthChecks()
            .AddStorageHealthCheck();

        // Add response compression
        builder.Services.AddResponseCompression(static options =>
        {
            options.Providers.Add<BrotliCompressionProvider>();
            options.Providers.Add<GzipCompressionProvider>();
        });

        // Add global exception handling
        builder.Services
            .AddProblemDetails()
            .AddExceptionHandler<ExceptionHandler>();

        return builder;
    }

    /// <summary>
    /// Adds swagger for dev environment only
    /// </summary>
    private static WebApplicationBuilder AddSwaggerForDevelopment(this WebApplicationBuilder builder)
    {
        if (builder.Environment.IsDevelopment())
        {
            builder.Services
                .AddSwaggerGen(static options =>
                {
                    options.EnableAnnotations();
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "ICMarkets Test Task API", Version = "v1" });
                });
        }

        return builder;
    }

    /// <summary>
    /// Adds domain services: mappers, commands and queries with its dependencies
    /// </summary>
    private static WebApplicationBuilder AddCoreServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddSingleton(TimeProvider.System)
            .AddNetworks()
            .AddBlockchainSnapshots();

        return builder;
    }

    /// <summary>
    /// Adds asp.net core, runtime, http client and ef core metrics along with exporting to prometheus
    /// </summary>
    private static WebApplicationBuilder AddMetrics(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddOptions<PrometheusAspNetCoreOptions>()
            .BindConfiguration(nameof(PrometheusAspNetCoreOptions));

        builder.Services
            .AddOpenTelemetry()
            .WithMetrics(static builder => builder
                .AddRuntimeInstrumentation()
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddEfCoreInstrumentation()
                .AddPrometheusExporter());

        return builder;
    }

    /// <summary>
    /// https://learn.microsoft.com/en-us/dotnet/core/diagnostics/built-in-metrics-runtime
    /// </summary>
    private static MeterProviderBuilder AddRuntimeInstrumentation(this MeterProviderBuilder builder) => builder
        .AddMeter("System.Runtime");

    /// <summary>
    /// https://learn.microsoft.com/en-us/dotnet/core/diagnostics/built-in-metrics-system-net
    /// </summary>
    private static MeterProviderBuilder AddHttpClientInstrumentation(this MeterProviderBuilder builder) => builder
        .AddMeter("System.Net.Http")
        .AddMeter("System.Net.NameResolution");

    /// <summary>
    /// https://learn.microsoft.com/en-us/ef/core/logging-events-diagnostics/metrics
    /// </summary>
    private static MeterProviderBuilder AddEfCoreInstrumentation(this MeterProviderBuilder builder) => builder
        .AddMeter("Microsoft.EntityFrameworkCore");
}