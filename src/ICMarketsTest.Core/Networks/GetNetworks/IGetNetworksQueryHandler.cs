namespace ICMarketsTest.Core.Networks.GetNetworks;

public interface IGetNetworksQueryHandler : IQueryHandler<GetNetworksQuery, PagedResult<Network>>;