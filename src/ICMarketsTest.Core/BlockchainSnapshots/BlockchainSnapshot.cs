namespace ICMarketsTest.Core.BlockchainSnapshots;

public sealed record BlockchainSnapshot
{
    /// <summary>
    /// Blockchain snapshot identifier, GUID v7
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// Network identifier
    /// </summary>
    public required string NetworkId { get; init; }

    /// <summary>
    /// Original JSON snapshot payload from external source
    /// </summary>
    public required string Payload { get; init; }

    /// <summary>
    /// Created timestamp in UTC
    /// </summary>
    public required DateTimeOffset CreatedAt { get; init; }
}