using System.Runtime.CompilerServices;
using System.Text.Json;
using ICMarketsTest.Core.BlockchainSnapshots;
using ICMarketsTest.Core.BlockchainSnapshots.GetBlockchainSnapshot;
using Riok.Mapperly.Abstractions;

namespace ICMarketsTest.WebApi.BlockchainSnapshots.GetBlockchainSnapshot;

public interface IGetBlockchainSnapshotMapper
{
    GetBlockchainSnapshotQuery Map(GetBlockchainSnapshotRequest request);
    GetBlockchainSnapshotResponse Map(BlockchainSnapshot snapshot);
}

/// <summary>
/// Mapperly generates object mappings via .net source generators: https://mapperly.riok.app/docs/intro/
/// </summary>
[Mapper]
public partial class GetBlockchainSnapshotMapper : IGetBlockchainSnapshotMapper
{
    [MapProperty(nameof(GetBlockchainSnapshotRequest.SnapshotId), nameof(GetBlockchainSnapshotQuery.Id))]
    public partial GetBlockchainSnapshotQuery Map(GetBlockchainSnapshotRequest request);

    [MapProperty(nameof(BlockchainSnapshot.Id), nameof(GetBlockchainSnapshotResponse.Id), StringFormat = "N")]
    public partial GetBlockchainSnapshotResponse Map(BlockchainSnapshot snapshot);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static object? MapPayload(string payload) => JsonSerializer.Deserialize<dynamic>(payload);
}