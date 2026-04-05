namespace ICMarketsTest.Core.Networks.GetNetworks;

public sealed class GetNetworksQueryHandler : IGetNetworksQueryHandler
{
    private readonly INetworksRepository _repository;

    public GetNetworksQueryHandler(INetworksRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<Network>> HandleAsync(GetNetworksQuery query, CancellationToken cancellationToken = default)
    {
        var pageNumber = Math.Max(1, query.PageNumber);
        var pageSize = Math.Clamp(query.PageSize, 1, 100);

        var result = await _repository.GetPageAsync(pageNumber, pageSize, cancellationToken);

        return result;
    }
}