namespace ICMarketsTest.Core.Networks.GetNetwork;

public sealed record GetNetworkQuery
{
    /// <summary>
    /// Network identifier
    /// </summary>
    public required string Id { get; init; }
}