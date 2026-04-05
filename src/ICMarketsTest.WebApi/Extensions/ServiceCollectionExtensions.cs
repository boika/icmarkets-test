using ICMarketsTest.BlockCypher;
using ICMarketsTest.Core.BlockchainSnapshots.GetBlockchainSnapshot;
using ICMarketsTest.Core.BlockchainSnapshots.GetBlockchainSnapshots;
using ICMarketsTest.Core.BlockchainSnapshots.TakeBlockchainSnapshot;
using ICMarketsTest.Core.Networks;
using ICMarketsTest.Core.Networks.GetNetwork;
using ICMarketsTest.Core.Networks.GetNetworks;
using ICMarketsTest.Storage;
using ICMarketsTest.WebApi.BlockchainSnapshots;
using ICMarketsTest.WebApi.Networks;
using ICMarketsTest.WebApi.Networks.Configuration;

namespace ICMarketsTest.WebApi.Extensions;

internal static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds blockchain snapshots commands and queries with its dependencies
    /// </summary>
    internal static IServiceCollection AddBlockchainSnapshots(this IServiceCollection services) => services
        .AddEfCoreBlockchainSnapshotsRepository()
        .AddBlockCypherBlockchainSnapshotsProvider()
        .AddSingleton<IBlockchainSnapshotsMapper, BlockchainSnapshotsMapper>()
        .AddScoped<ITakeBlockchainSnapshotCommandExecutor, TakeBlockchainSnapshotCommandExecutor>()
        .AddScoped<IGetBlockchainSnapshotQueryExecutor, GetBlockchainSnapshotQueryExecutor>()
        .AddScoped<IGetBlockchainSnapshotsQueryExecutor, GetBlockchainSnapshotsQueryExecutor>();

    /// <summary>
    /// Adds networks commands and queries with its dependencies
    /// </summary>
    internal static IServiceCollection AddNetworks(this IServiceCollection services) => services
        .AddConfigurationNetworksRepository()
        .AddSingleton<INetworksMapper, NetworksMapper>()
        .AddScoped<IGetNetworkQueryExecutor, GetNetworkQueryExecutor>()
        .AddScoped<IGetNetworksQueryExecutor, GetNetworksQueryExecutor>();

    private static IServiceCollection AddConfigurationNetworksRepository(this IServiceCollection services)
    {
        services
            .AddScoped<INetworksRepository, ConfigurationNetworksRepository>()
            .AddOptions<NetworksOptions>()
            .BindConfiguration(nameof(NetworksOptions))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }
}