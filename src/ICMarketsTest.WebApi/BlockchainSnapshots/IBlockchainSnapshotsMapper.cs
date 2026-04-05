using System.Runtime.CompilerServices;
using System.Text.Json;
using ICMarketsTest.Core;
using ICMarketsTest.Core.BlockchainSnapshots;
using ICMarketsTest.Core.BlockchainSnapshots.GetBlockchainSnapshot;
using ICMarketsTest.Core.BlockchainSnapshots.GetBlockchainSnapshots;
using ICMarketsTest.Core.BlockchainSnapshots.TakeBlockchainSnapshot;
using ICMarketsTest.Core.Networks;
using ICMarketsTest.Core.Networks.GetNetwork;
using ICMarketsTest.WebApi.BlockchainSnapshots.Models;
using Riok.Mapperly.Abstractions;

namespace ICMarketsTest.WebApi.BlockchainSnapshots;

public interface IBlockchainSnapshotsMapper
{
    GetNetworkQuery Map(TakeBlockchainSnapshotRequest request);
    TakeBlockchainSnapshotCommand Map(Network network);

    GetBlockchainSnapshotQuery Map(GetBlockchainSnapshotRequest request);
    GetBlockchainSnapshotResponse Map(BlockchainSnapshot snapshot);

    GetBlockchainSnapshotsQuery Map(GetBlockchainSnapshotsRequest request);
    PagedResponse<GetBlockchainSnapshotResponse> Map(PagedResult<BlockchainSnapshot> snapshots);
}

/// <summary>
/// Mapperly generates object mappings via .net source generators: https://mapperly.riok.app/docs/intro/
/// </summary>
[Mapper]
public partial class BlockchainSnapshotsMapper : IBlockchainSnapshotsMapper
{
    [MapProperty(nameof(TakeBlockchainSnapshotRequest.NetworkId), nameof(GetNetworkQuery.Id))]
    public partial GetNetworkQuery Map(TakeBlockchainSnapshotRequest request);

    [MapProperty(nameof(Network.Id), nameof(TakeBlockchainSnapshotCommand.NetworkId))]
    [MapProperty(nameof(Network.Name), nameof(TakeBlockchainSnapshotCommand.NetworkName))]
    [MapProperty(nameof(Network.Type), nameof(TakeBlockchainSnapshotCommand.NetworkType))]
    public partial TakeBlockchainSnapshotCommand Map(Network network);

    [MapProperty(nameof(GetBlockchainSnapshotRequest.SnapshotId), nameof(GetBlockchainSnapshotQuery.Id))]
    public partial GetBlockchainSnapshotQuery Map(GetBlockchainSnapshotRequest request);

    [MapProperty(nameof(BlockchainSnapshot.Id), nameof(GetBlockchainSnapshotResponse.Id), StringFormat = "N")]
    public partial GetBlockchainSnapshotResponse Map(BlockchainSnapshot snapshot);

    [MapProperty(nameof(GetBlockchainSnapshotsRequest.PageNumber), nameof(GetBlockchainSnapshotsQuery.PageNumber), Use = nameof(MapPageNumber))]
    [MapProperty(nameof(GetBlockchainSnapshotsRequest.PageSize), nameof(GetBlockchainSnapshotsQuery.PageSize), Use = nameof(MapPageSize))]
    public partial GetBlockchainSnapshotsQuery Map(GetBlockchainSnapshotsRequest request);

    public partial PagedResponse<GetBlockchainSnapshotResponse> Map(PagedResult<BlockchainSnapshot> snapshots);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int MapPageNumber(int? pageNumber) => pageNumber ?? 1;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int MapPageSize(int? pageSize) => pageSize ?? 10;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static object? MapPayload(string payload) => JsonSerializer.Deserialize<dynamic>(payload);
}