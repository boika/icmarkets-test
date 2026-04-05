namespace ICMarketsTest.Core.BlockchainSnapshots.GetBlockchainSnapshots;

public sealed record GetBlockchainSnapshotsQuery
{
    /// <summary>
    /// Network identifier
    /// </summary>
    public required string NetworkId { get; init; }

    /// <summary>
    /// Blockchain snapshots page number, starting from 1
    /// </summary>
    public required int PageNumber { get; init; }

    /// <summary>
    /// Blockchain snapshots page size, starting from 1 up to 100
    /// </summary>
    public required int PageSize { get; init; }
}