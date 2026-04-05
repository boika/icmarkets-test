namespace ICMarketsTest.Core.BlockchainSnapshots;

public interface IBlockchainSnapshotsRepository
{
    /// <summary>
    /// Adds new single blockchain snapshot
    /// </summary>
    Task AddAsync(
        BlockchainSnapshot blockchainSnapshot,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets single blockchain snapshot by identifier and network
    /// </summary>
    Task<BlockchainSnapshot?> GetAsync(
        Guid id,
        string networkId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets paged list of blockchain snapshots by network identifier in reversed chronological order
    /// </summary>
    Task<PagedResult<BlockchainSnapshot>> GetPageAsync(
        string networkId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);
}