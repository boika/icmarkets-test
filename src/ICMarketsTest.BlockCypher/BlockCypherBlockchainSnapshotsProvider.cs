using ICMarketsTest.Core.BlockchainSnapshots;

namespace ICMarketsTest.BlockCypher;

/// <summary>
/// Blockchain snapshots provider on top of the BlockCypher public API
/// </summary>
public sealed class BlockCypherBlockchainSnapshotsProvider : IBlockchainSnapshotsProvider
{
    private readonly IBlockCypherClient _client;

    public BlockCypherBlockchainSnapshotsProvider(IBlockCypherClient client)
    {
        _client = client;
    }

    public async Task<string> GetBlockchainSnapshotAsync(
        string networkName,
        string networkType,
        CancellationToken cancellationToken = default)
    {
        var payload = await _client
            .GetSnapshotAsync(networkName, networkType, cancellationToken);

        return payload;
    }
}