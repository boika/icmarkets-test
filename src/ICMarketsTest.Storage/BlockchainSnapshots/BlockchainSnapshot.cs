namespace ICMarketsTest.Storage.BlockchainSnapshots;

public sealed class BlockchainSnapshot
{
    public Guid Id { get; set; }

    public string NetworkId { get; set; } = string.Empty;

    public string Payload { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}