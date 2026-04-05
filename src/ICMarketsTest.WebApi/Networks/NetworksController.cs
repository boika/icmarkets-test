using System.Net.Mime;
using Asp.Versioning;
using ICMarketsTest.Core.Networks.GetNetwork;
using ICMarketsTest.Core.Networks.GetNetworks;
using ICMarketsTest.WebApi.Networks.GetNetwork;
using ICMarketsTest.WebApi.Networks.GetNetworks;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ICMarketsTest.WebApi.Networks;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v1/[controller]")]
[SwaggerTag("Networks operations")]
public class NetworksController : ControllerBase
{
    [HttpGet("{networkId}")]
    [SwaggerOperation("Gets network by identifier")]
    [SwaggerResponse(StatusCodes.Status200OK, "The network", typeof(GetNetworkResponse), MediaTypeNames.Application.Json)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incoming request is not valid", typeof(ValidationProblemDetails), MediaTypeNames.Application.Json)]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Network is not found", typeof(void), MediaTypeNames.Application.Json)]
    public async Task<IActionResult> Get(
        [FromRoute] GetNetworkRequest request,
        [FromServices] IGetNetworkQueryHandler queryHandler,
        [FromServices] IGetNetworkMapper mapper,
        CancellationToken cancellationToken)
    {
        var query = mapper.Map(request);
        var result = await queryHandler.HandleAsync(query, cancellationToken);

        return result is null
            ? NotFound()
            : Ok(mapper.Map(result));
    }

    [HttpGet]
    [SwaggerOperation("Gets paged list of networks in ascending order")]
    [SwaggerResponse(StatusCodes.Status200OK, "Paged list of networks in ascending order", typeof(PagedResponse<GetNetworkResponse>), MediaTypeNames.Application.Json)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incoming request is not valid", typeof(ValidationProblemDetails), MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetPage(
        [FromRoute] GetNetworksRequest request,
        [FromServices] IGetNetworksQueryHandler queryHandler,
        [FromServices] IGetNetworksMapper mapper,
        CancellationToken cancellationToken)
    {
        var query = mapper.Map(request);
        var result = await queryHandler.HandleAsync(query, cancellationToken);

        return Ok(mapper.Map(result));
    }
}