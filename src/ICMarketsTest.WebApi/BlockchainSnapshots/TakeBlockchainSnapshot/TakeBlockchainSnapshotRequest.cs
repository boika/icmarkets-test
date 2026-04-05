using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ICMarketsTest.WebApi.BlockchainSnapshots.TakeBlockchainSnapshot;

public sealed class TakeBlockchainSnapshotRequest
{
    [Required]
    [StringLength(32)]
    [FromRoute(Name = "networkId")]
    [SwaggerSchema("Network unique identifier")]
    public required string NetworkId { get; set; }
}