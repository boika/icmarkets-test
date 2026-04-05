namespace ICMarketsTest.Core.BlockchainSnapshots.GetBlockchainSnapshots;

public sealed class GetBlockchainSnapshotsQueryExecutor : IGetBlockchainSnapshotsQueryExecutor
{
    private readonly IBlockchainSnapshotsRepository _repository;

    public GetBlockchainSnapshotsQueryExecutor(IBlockchainSnapshotsRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<BlockchainSnapshot>> ExecuteAsync(GetBlockchainSnapshotsQuery query, CancellationToken cancellationToken = default)
    {
        var pageNumber = Math.Max(1, query.PageNumber);
        var pageSize = Math.Clamp(query.PageSize, 1, 100);

        var snapshots = await _repository.GetPageAsync(query.NetworkId, pageNumber, pageSize, cancellationToken);

        return snapshots;
    }
}