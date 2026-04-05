using System.Text.Json;
using ICMarketsTest.BlockCypher;

namespace ICMarketsTest.WebApi.Tests.Functional;

/// <summary>
/// Testing mock for real-world BlockCypher http client
/// </summary>
public sealed class TestBlockCypherClient : IBlockCypherClient
{
    public Task<string> GetSnapshotAsync(string network, string type, CancellationToken cancellationToken = default)
        => Task.FromResult(JsonSerializer.Serialize(new
        {
            name = $"{network}.{type}",
            time = DateTime.UtcNow,
            test = true
        }));
}