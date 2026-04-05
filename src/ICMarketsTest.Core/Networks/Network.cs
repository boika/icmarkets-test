namespace ICMarketsTest.Core.Networks;

public sealed record Network
{
    /// <summary>
    /// Network identifier
    /// </summary>
    /// <example>eth-main</example>
    public required string Id { get; init; }

    /// <summary>
    /// Network name
    /// </summary>
    /// <example>btc</example>
    public required string Name { get; init; }

    /// <summary>
    /// Network type (mainnet, testnet)
    /// </summary>
    /// <example>main</example>
    public required string Type { get; init; }
}