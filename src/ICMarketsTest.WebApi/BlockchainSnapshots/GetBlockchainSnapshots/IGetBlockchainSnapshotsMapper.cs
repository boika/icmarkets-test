using System.Runtime.CompilerServices;
using System.Text.Json;
using ICMarketsTest.Core;
using ICMarketsTest.Core.BlockchainSnapshots;
using ICMarketsTest.Core.BlockchainSnapshots.GetBlockchainSnapshots;
using ICMarketsTest.WebApi.BlockchainSnapshots.GetBlockchainSnapshot;
using Riok.Mapperly.Abstractions;

namespace ICMarketsTest.WebApi.BlockchainSnapshots.GetBlockchainSnapshots;

public interface IGetBlockchainSnapshotsMapper
{
    GetBlockchainSnapshotsQuery Map(GetBlockchainSnapshotsRequest request);
    PagedResponse<GetBlockchainSnapshotResponse> Map(PagedResult<BlockchainSnapshot> snapshots);
}

/// <summary>
/// Mapperly generates object mappings via .net source generators: https://mapperly.riok.app/docs/intro/
/// </summary>
[Mapper]
public partial class GetBlockchainSnapshotsMapper : IGetBlockchainSnapshotsMapper
{
    [MapProperty(nameof(GetBlockchainSnapshotsRequest.PageNumber), nameof(GetBlockchainSnapshotsQuery.PageNumber), Use = nameof(MapPageNumber))]
    [MapProperty(nameof(GetBlockchainSnapshotsRequest.PageSize), nameof(GetBlockchainSnapshotsQuery.PageSize), Use = nameof(MapPageSize))]
    public partial GetBlockchainSnapshotsQuery Map(GetBlockchainSnapshotsRequest request);

    public partial PagedResponse<GetBlockchainSnapshotResponse> Map(PagedResult<BlockchainSnapshot> snapshots);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int MapPageNumber(int? pageNumber) => pageNumber ?? 1;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int MapPageSize(int? pageSize) => pageSize ?? 10;

    [MapProperty(nameof(BlockchainSnapshot.Id), nameof(GetBlockchainSnapshotResponse.Id), StringFormat = "N")]
    private partial GetBlockchainSnapshotResponse Map(BlockchainSnapshot snapshot);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static object? MapPayload(string payload) => JsonSerializer.Deserialize<dynamic>(payload);
}