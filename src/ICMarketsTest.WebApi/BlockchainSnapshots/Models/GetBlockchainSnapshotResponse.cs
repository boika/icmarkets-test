using Swashbuckle.AspNetCore.Annotations;

namespace ICMarketsTest.WebApi.BlockchainSnapshots.Models;

public sealed class GetBlockchainSnapshotResponse
{
    [SwaggerSchema("Blockchain Snapshot unique identifier, GUID v7")]
    public required string Id { get; set; }

    [SwaggerSchema("Related network unique identifier")]
    public required string NetworkId { get; set; }

    [SwaggerSchema("Created timestamp in UTC")]
    public required DateTimeOffset CreatedAt { get; set; }

    [SwaggerSchema("Original JSON snapshot payload from external source")]
    public object? Payload { get; set; } = null;
}