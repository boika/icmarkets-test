using ICMarketsTest.Core.Networks;
using Riok.Mapperly.Abstractions;

namespace ICMarketsTest.Configuration.Networks;

public interface INetworksMapper
{
    Network Map(NetworkOptions networkOptions);
}

/// <summary>
/// Mapperly generates object mappings via .net source generators: https://mapperly.riok.app/docs/intro/
/// </summary>
[Mapper]
public partial class NetworksMapper : INetworksMapper
{
    public partial Network Map(NetworkOptions networkOptions);
}