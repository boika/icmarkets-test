namespace ICMarketsTest.Core.Networks.GetNetwork;

public sealed class GetNetworkQueryHandler : IGetNetworkQueryHandler
{
    private readonly INetworksRepository _repository;

    public GetNetworkQueryHandler(INetworksRepository repository)
    {
        _repository = repository;
    }

    public async Task<Network?> HandleAsync(GetNetworkQuery query, CancellationToken cancellationToken = default)
    {
        var network = await _repository.GetAsync(query.Id, cancellationToken);

        return network;
    }
}