using System.Runtime.CompilerServices;
using ICMarketsTest.Core;
using ICMarketsTest.Core.Networks;
using ICMarketsTest.Core.Networks.GetNetwork;
using ICMarketsTest.Core.Networks.GetNetworks;
using ICMarketsTest.WebApi.Networks.Models;
using Riok.Mapperly.Abstractions;

namespace ICMarketsTest.WebApi.Networks;

public interface INetworksMapper
{
    GetNetworkQuery Map(GetNetworkRequest request);
    GetNetworkResponse Map(Network network);

    GetNetworksQuery Map(GetNetworksRequest request);
    PagedResponse<GetNetworkResponse> Map(PagedResult<Network> networks);
}

/// <summary>
/// Mapperly generates object mappings via .net source generators: https://mapperly.riok.app/docs/intro/
/// </summary>
[Mapper]
public partial class NetworksMapper : INetworksMapper
{
    [MapProperty(nameof(GetNetworkRequest.NetworkId), nameof(GetNetworkQuery.Id))]
    public partial GetNetworkQuery Map(GetNetworkRequest request);

    public partial GetNetworkResponse Map(Network network);

    [MapProperty(nameof(GetNetworksRequest.PageNumber), nameof(GetNetworksQuery.PageNumber), Use = nameof(MapPageNumber))]
    [MapProperty(nameof(GetNetworksRequest.PageSize), nameof(GetNetworksQuery.PageSize), Use = nameof(MapPageSize))]
    public partial GetNetworksQuery Map(GetNetworksRequest request);

    public partial PagedResponse<GetNetworkResponse> Map(PagedResult<Network> networks);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int MapPageNumber(int? pageNumber) => pageNumber ?? 1;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int MapPageSize(int? pageSize) => pageSize ?? 10;
}