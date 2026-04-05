using ICMarketsTest.Storage;
using ICMarketsTest.WebApi.Extensions;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Host is starting");

    var app = WebApplication
        .CreateBuilder(args)
        .Configure()
        .Build();

    app.UseResponseCompression();
    app.UseExceptionHandler();
    app.UseRouting();
    app.UseCors();
    app.UseSwaggerForDevelopment();
    app.UseSerilogRequestLogging();
    app.MapControllers();
    app.MapHealthChecks("/health");
    app.MapPrometheusScrapingEndpoint();
    app.MigrateStorage();
    app.Warmup();

    await app.RunAsync();

    Log.Information("Host is stopping");
    return 0;
}
catch (Exception exception)
{
    Log.Fatal(exception, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.Information("Host shut down");
    await Log.CloseAndFlushAsync();
}