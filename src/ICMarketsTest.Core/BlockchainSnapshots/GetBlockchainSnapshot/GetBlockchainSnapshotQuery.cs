namespace ICMarketsTest.Core.BlockchainSnapshots.GetBlockchainSnapshot;

public sealed record GetBlockchainSnapshotQuery
{
    /// <summary>
    /// Blockchain snapshot identifier
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// Network identifier
    /// </summary>
    public required string NetworkId { get; init; }
}