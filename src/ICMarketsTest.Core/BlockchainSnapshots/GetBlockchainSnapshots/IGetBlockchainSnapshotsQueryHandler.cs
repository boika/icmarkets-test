namespace ICMarketsTest.Core.BlockchainSnapshots.GetBlockchainSnapshots;

public interface IGetBlockchainSnapshotsQueryHandler : IQueryHandler<GetBlockchainSnapshotsQuery, PagedResult<BlockchainSnapshot>>;