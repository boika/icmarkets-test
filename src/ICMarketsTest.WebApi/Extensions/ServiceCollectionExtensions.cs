using ICMarketsTest.BlockCypher;
using ICMarketsTest.Configuration;
using ICMarketsTest.Core.BlockchainSnapshots.GetBlockchainSnapshot;
using ICMarketsTest.Core.BlockchainSnapshots.GetBlockchainSnapshots;
using ICMarketsTest.Core.BlockchainSnapshots.TakeBlockchainSnapshot;
using ICMarketsTest.Core.Networks.GetNetwork;
using ICMarketsTest.Core.Networks.GetNetworks;
using ICMarketsTest.Storage;
using ICMarketsTest.WebApi.BlockchainSnapshots;
using ICMarketsTest.WebApi.Networks;

namespace ICMarketsTest.WebApi.Extensions;

internal static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds blockchain snapshots command and query handlers with its dependencies
    /// </summary>
    internal static IServiceCollection AddBlockchainSnapshots(this IServiceCollection services) => services
        .AddEfCoreBlockchainSnapshotsRepository()
        .AddBlockCypherBlockchainSnapshotsProvider()
        .AddSingleton<IBlockchainSnapshotsMapper, BlockchainSnapshotsMapper>()
        .AddScoped<ITakeBlockchainSnapshotCommandHandler, TakeBlockchainSnapshotCommandHandler>()
        .AddScoped<IGetBlockchainSnapshotQueryHandler, GetBlockchainSnapshotQueryHandler>()
        .AddScoped<IGetBlockchainSnapshotsQueryHandler, GetBlockchainSnapshotsQueryHandler>();

    /// <summary>
    /// Adds networks command and query handlers with its dependencies
    /// </summary>
    internal static IServiceCollection AddNetworks(this IServiceCollection services) => services
        .AddConfigurationNetworksRepository()
        .AddSingleton<INetworksMapper, NetworksMapper>()
        .AddScoped<IGetNetworkQueryHandler, GetNetworkQueryHandler>()
        .AddScoped<IGetNetworksQueryHandler, GetNetworksQueryHandler>();
}