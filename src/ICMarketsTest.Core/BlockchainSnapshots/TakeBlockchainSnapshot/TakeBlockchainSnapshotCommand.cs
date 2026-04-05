namespace ICMarketsTest.Core.BlockchainSnapshots.TakeBlockchainSnapshot;

public sealed record TakeBlockchainSnapshotCommand
{
    /// <summary>
    /// Network identifier
    /// </summary>
    public required string NetworkId { get; init; }

    /// <summary>
    /// Network name
    /// </summary>
    public required string NetworkName { get; init; }

    /// <summary>
    /// Network type
    /// </summary>
    public required string NetworkType { get; init; }
}