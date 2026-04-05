namespace ICMarketsTest.Core.BlockchainSnapshots.GetBlockchainSnapshot;

public sealed class GetBlockchainSnapshotQueryHandler : IGetBlockchainSnapshotQueryHandler
{
    private readonly IBlockchainSnapshotsRepository _repository;

    public GetBlockchainSnapshotQueryHandler(IBlockchainSnapshotsRepository repository)
    {
        _repository = repository;
    }

    public async Task<BlockchainSnapshot?> HandleAsync(GetBlockchainSnapshotQuery query, CancellationToken cancellationToken = default)
    {
        var snapshot = await _repository.GetAsync(query.Id, query.NetworkId, cancellationToken);

        return snapshot;
    }
}