using Swashbuckle.AspNetCore.Annotations;

namespace ICMarketsTest.WebApi.Networks.Models;

public sealed class GetNetworkResponse
{
    [SwaggerSchema("Network unique identifier")]
    public required string Id { get; set; }

    [SwaggerSchema("Network name")]
    public required string Name { get; set; }

    [SwaggerSchema("Network type")]
    public required string Type { get; set; }
}