using ICMarketsTest.BlockCypher;
using ICMarketsTest.Configuration;
using ICMarketsTest.Core.BlockchainSnapshots.GetBlockchainSnapshot;
using ICMarketsTest.Core.BlockchainSnapshots.GetBlockchainSnapshots;
using ICMarketsTest.Core.BlockchainSnapshots.TakeBlockchainSnapshot;
using ICMarketsTest.Core.Networks.GetNetwork;
using ICMarketsTest.Core.Networks.GetNetworks;
using ICMarketsTest.Storage;
using ICMarketsTest.WebApi.BlockchainSnapshots.GetBlockchainSnapshot;
using ICMarketsTest.WebApi.BlockchainSnapshots.GetBlockchainSnapshots;
using ICMarketsTest.WebApi.BlockchainSnapshots.TakeBlockchainSnapshot;
using ICMarketsTest.WebApi.Networks.GetNetwork;
using ICMarketsTest.WebApi.Networks.GetNetworks;

namespace ICMarketsTest.WebApi.Extensions;

internal static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds blockchain snapshots mappers, command and query handlers with its dependencies
    /// </summary>
    internal static IServiceCollection AddBlockchainSnapshots(this IServiceCollection services) => services
        .AddEfCoreBlockchainSnapshotsRepository()
        .AddBlockCypherBlockchainSnapshotsProvider()
        .AddSingleton<ITakeBlockchainSnapshotMapper, TakeBlockchainSnapshotMapper>()
        .AddScoped<ITakeBlockchainSnapshotCommandHandler, TakeBlockchainSnapshotCommandHandler>()
        .AddSingleton<IGetBlockchainSnapshotMapper, GetBlockchainSnapshotMapper>()
        .AddScoped<IGetBlockchainSnapshotQueryHandler, GetBlockchainSnapshotQueryHandler>()
        .AddSingleton<IGetBlockchainSnapshotsMapper, GetBlockchainSnapshotsMapper>()
        .AddScoped<IGetBlockchainSnapshotsQueryHandler, GetBlockchainSnapshotsQueryHandler>();

    /// <summary>
    /// Adds networks mappers, command and query handlers with its dependencies
    /// </summary>
    internal static IServiceCollection AddNetworks(this IServiceCollection services) => services
        .AddConfigurationNetworksRepository()
        .AddSingleton<IGetNetworkMapper, GetNetworkMapper>()
        .AddScoped<IGetNetworkQueryHandler, GetNetworkQueryHandler>()
        .AddSingleton<IGetNetworksMapper, GetNetworksMapper>()
        .AddScoped<IGetNetworksQueryHandler, GetNetworksQueryHandler>();
}