namespace ICMarketsTest.Core.Networks.GetNetworks;

public interface IGetNetworksQueryExecutor : IQueryExecutor<GetNetworksQuery, PagedResult<Network>>;