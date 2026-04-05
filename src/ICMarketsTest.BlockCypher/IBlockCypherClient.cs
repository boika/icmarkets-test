using Refit;

namespace ICMarketsTest.BlockCypher;

/// <summary>
/// REST API client interface for BlockCypher
/// </summary>
public interface IBlockCypherClient
{
    [Get("/v1/{network}/{type}")]
    Task<string> GetSnapshotAsync(string network, string type, CancellationToken cancellationToken = default);
}