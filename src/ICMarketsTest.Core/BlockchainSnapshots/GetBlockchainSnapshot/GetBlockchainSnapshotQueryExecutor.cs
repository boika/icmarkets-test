namespace ICMarketsTest.Core.BlockchainSnapshots.GetBlockchainSnapshot;

public sealed class GetBlockchainSnapshotQueryExecutor : IGetBlockchainSnapshotQueryExecutor
{
    private readonly IBlockchainSnapshotsRepository _repository;

    public GetBlockchainSnapshotQueryExecutor(IBlockchainSnapshotsRepository repository)
    {
        _repository = repository;
    }

    public async Task<BlockchainSnapshot?> ExecuteAsync(GetBlockchainSnapshotQuery query, CancellationToken cancellationToken = default)
    {
        var snapshot = await _repository.GetAsync(query.Id, query.NetworkId, cancellationToken);

        return snapshot;
    }
}