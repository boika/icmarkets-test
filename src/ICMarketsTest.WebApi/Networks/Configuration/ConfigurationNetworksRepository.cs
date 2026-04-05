using ICMarketsTest.Core;
using ICMarketsTest.Core.Networks;
using Microsoft.Extensions.Options;

namespace ICMarketsTest.WebApi.Networks.Configuration;

/// <summary>
/// Repository for networks on top of the configuration
/// </summary>
public sealed class ConfigurationNetworksRepository : INetworksRepository
{
    private readonly IOptionsSnapshot<NetworksOptions> _options;
    private readonly INetworksMapper _mapper;

    public ConfigurationNetworksRepository(IOptionsSnapshot<NetworksOptions> options, INetworksMapper mapper)
    {
        _options = options;
        _mapper = mapper;
    }

    public Task<Network?> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        var network = _options.Value.Networks
            .SingleOrDefault(n => n.Id == id);

        var result = network is null ? null : _mapper.Map(network);

        return Task.FromResult(result);
    }

    public Task<PagedResult<Network>> GetPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var networks = _options.Value.Networks;
        var totalRecords = networks.Count;

        var data = networks
            .OrderBy(n => n.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(n => _mapper.Map(n))
            .ToList();

        var result = new PagedResult<Network>
        {
            Data = data,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = totalRecords,
            TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize)
        };

        return Task.FromResult(result);
    }
}