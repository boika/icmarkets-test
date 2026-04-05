using ICMarketsTest.Core.Networks;
using ICMarketsTest.Core.Networks.GetNetwork;
using Riok.Mapperly.Abstractions;

namespace ICMarketsTest.WebApi.Networks.GetNetwork;

public interface IGetNetworkMapper
{
    GetNetworkQuery Map(GetNetworkRequest request);
    GetNetworkResponse Map(Network network);
}

/// <summary>
/// Mapperly generates object mappings via .net source generators: https://mapperly.riok.app/docs/intro/
/// </summary>
[Mapper]
public partial class GetNetworkMapper : IGetNetworkMapper
{
    [MapProperty(nameof(GetNetworkRequest.NetworkId), nameof(GetNetworkQuery.Id))]
    public partial GetNetworkQuery Map(GetNetworkRequest request);

    public partial GetNetworkResponse Map(Network network);
}