using ICMarketsTest.Core.BlockchainSnapshots.TakeBlockchainSnapshot;
using ICMarketsTest.Core.Networks;
using ICMarketsTest.Core.Networks.GetNetwork;
using Riok.Mapperly.Abstractions;

namespace ICMarketsTest.WebApi.BlockchainSnapshots.TakeBlockchainSnapshot;

public interface ITakeBlockchainSnapshotMapper
{
    GetNetworkQuery Map(TakeBlockchainSnapshotRequest request);
    TakeBlockchainSnapshotCommand Map(Network network);
}

/// <summary>
/// Mapperly generates object mappings via .net source generators: https://mapperly.riok.app/docs/intro/
/// </summary>
[Mapper]
public partial class TakeBlockchainSnapshotMapper : ITakeBlockchainSnapshotMapper
{
    [MapProperty(nameof(TakeBlockchainSnapshotRequest.NetworkId), nameof(GetNetworkQuery.Id))]
    public partial GetNetworkQuery Map(TakeBlockchainSnapshotRequest request);

    [MapProperty(nameof(Network.Id), nameof(TakeBlockchainSnapshotCommand.NetworkId))]
    [MapProperty(nameof(Network.Name), nameof(TakeBlockchainSnapshotCommand.NetworkName))]
    [MapProperty(nameof(Network.Type), nameof(TakeBlockchainSnapshotCommand.NetworkType))]
    public partial TakeBlockchainSnapshotCommand Map(Network network);
}