namespace ICMarketsTest.Core.Networks.GetNetworks;

public sealed class GetNetworksQueryExecutor : IGetNetworksQueryExecutor
{
    private readonly INetworksRepository _repository;

    public GetNetworksQueryExecutor(INetworksRepository repository)
    {
        _repository = repository;
    }

    public Task<PagedResult<Network>> ExecuteAsync(GetNetworksQuery query, CancellationToken cancellationToken = default)
    {
        var pageNumber = Math.Max(1, query.PageNumber);
        var pageSize = Math.Clamp(query.PageSize, 1, 100);

        return _repository.GetPageAsync(pageNumber, pageSize, cancellationToken);
    }
}