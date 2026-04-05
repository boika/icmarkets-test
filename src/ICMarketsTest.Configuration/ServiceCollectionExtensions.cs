using ICMarketsTest.Configuration.Networks;
using ICMarketsTest.Core.Networks;
using Microsoft.Extensions.DependencyInjection;

namespace ICMarketsTest.Configuration;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds networks configuration and networks repository implementation
    /// </summary>
    public static IServiceCollection AddConfigurationNetworksRepository(this IServiceCollection services)
    {
        services
            .AddOptions<NetworksOptions>()
            .BindConfiguration(nameof(NetworksOptions))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services
            .AddSingleton<INetworksMapper, NetworksMapper>()
            .AddScoped<INetworksRepository, ConfigurationNetworksRepository>();
    }
}