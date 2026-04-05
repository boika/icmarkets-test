using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ICMarketsTest.WebApi.BlockchainSnapshots.GetBlockchainSnapshot;

public sealed class GetBlockchainSnapshotRequest
{
    [Required]
    [StringLength(32)]
    [FromRoute(Name = "snapshotId")]
    [SwaggerSchema("Blockchain snapshot unique identifier, GUID v7")]
    public required string SnapshotId { get; set; }

    [Required]
    [StringLength(32)]
    [FromRoute(Name = "networkId")]
    [SwaggerSchema("Network unique identifier")]
    public required string NetworkId { get; set; }
}