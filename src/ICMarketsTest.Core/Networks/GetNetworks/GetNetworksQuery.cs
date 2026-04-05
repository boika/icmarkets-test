namespace ICMarketsTest.Core.Networks.GetNetworks;

public sealed record GetNetworksQuery
{
    /// <summary>
    /// Networks page number, starting from 1
    /// </summary>
    public required int PageNumber { get; init; }

    /// <summary>
    /// Networks page size, starting from 1 up to 100
    /// </summary>
    public required int PageSize { get; init; }
}