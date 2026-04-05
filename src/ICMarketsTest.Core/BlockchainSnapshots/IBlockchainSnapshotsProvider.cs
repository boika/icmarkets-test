namespace ICMarketsTest.Core.BlockchainSnapshots;

public interface IBlockchainSnapshotsProvider
{
    /// <summary>
    /// Gets blockchain snapshot payload from external source
    /// </summary>
    Task<string> GetBlockchainSnapshotAsync(
        string networkName,
        string networkType,
        CancellationToken cancellationToken = default);
}