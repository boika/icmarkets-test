using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ICMarketsTest.WebApi.Networks.GetNetwork;

public sealed class GetNetworkRequest
{
    [Required]
    [StringLength(32)]
    [FromRoute(Name = "networkId")]
    [SwaggerSchema("Network unique identifier")]
    public required string NetworkId { get; set; }
}