using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ICMarketsTest.WebApi.Networks.Models;

public sealed class GetNetworksRequest
{
    [Range(1, int.MaxValue)]
    [FromQuery(Name = "pageNumber")]
    [SwaggerSchema("Networks page number, minimum and default value is 1")]
    public int? PageNumber { get; set; }

    [Range(1, 100)]
    [FromQuery(Name = "pageSize")]
    [SwaggerSchema("Networks page size, maximum value is 100, default value is 10")]
    public int? PageSize { get; set; }
}