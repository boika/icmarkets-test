using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace ICMarketsTest.WebApi.Extensions;

internal static class WebApplicationExtensions
{
    /// <summary>
    /// Registers Swagger/SwaggerUI middleware for dev environment only
    /// </summary>
    internal static void UseSwaggerForDevelopment(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
            return;

        app.UseSwagger();
        app.UseSwaggerUI(static options =>
        {
            options.RoutePrefix = string.Empty;
            options.SwaggerEndpoint("../swagger/v1/swagger.json", "Version 1");
            options.DocExpansion(DocExpansion.List);
            options.EnableDeepLinking();
        });
    }

    /// <summary>
    /// Warms up application - creates all the controllers with its dependency graphs
    /// </summary>
    internal static void Warmup(this WebApplication app)
    {
        var controllerTypes = Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(static type => type.IsSubclassOf(typeof(ControllerBase)) && !type.IsAbstract);

        using var scope = app.Services.CreateScope();

        foreach (var controllerType in controllerTypes)
        {
            scope.ServiceProvider.GetRequiredService(controllerType);
        }
    }
}