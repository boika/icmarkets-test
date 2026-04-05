using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ICMarketsTest.WebApi.BlockchainSnapshots.GetBlockchainSnapshots;

public sealed class GetBlockchainSnapshotsRequest
{
    [Required]
    [StringLength(32)]
    [FromRoute(Name = "networkId")]
    [SwaggerSchema("Network unique identifier")]
    public required string NetworkId { get; set; }

    [Range(1, int.MaxValue)]
    [FromQuery(Name = "pageNumber")]
    [SwaggerSchema("Blockchain snapshots page number, minimum and default value is 1")]
    public int? PageNumber { get; set; }

    [Range(1, 100)]
    [FromQuery(Name = "pageSize")]
    [SwaggerSchema("Blockchain snapshots page size, maximum value is 100, default value is 10")]
    public int? PageSize { get; set; }
}