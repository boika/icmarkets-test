namespace ICMarketsTest.Core.Networks.GetNetwork;

public sealed class GetNetworkQueryExecutor : IGetNetworkQueryExecutor
{
    private readonly INetworksRepository _repository;

    public GetNetworkQueryExecutor(INetworksRepository repository)
    {
        _repository = repository;
    }

    public async Task<Network?> ExecuteAsync(GetNetworkQuery query, CancellationToken cancellationToken = default)
    {
        var network = await _repository.GetAsync(query.Id, cancellationToken);

        return network;
    }
}