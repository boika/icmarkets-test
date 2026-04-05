using System.Net.Mime;
using Asp.Versioning;
using ICMarketsTest.Core.Networks.GetNetwork;
using ICMarketsTest.Core.Networks.GetNetworks;
using ICMarketsTest.WebApi.Networks.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ICMarketsTest.WebApi.Networks;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v1/[controller]")]
[SwaggerTag("Networks operations")]
public class NetworksController : ControllerBase
{
    private readonly INetworksMapper _mapper;

    public NetworksController(INetworksMapper mapper)
    {
        _mapper = mapper;
    }

    [HttpGet("{networkId}")]
    [SwaggerOperation("Gets network by identifier")]
    [SwaggerResponse(StatusCodes.Status200OK, "The network", typeof(GetNetworkResponse), MediaTypeNames.Application.Json)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incoming request is not valid", typeof(ValidationProblemDetails), MediaTypeNames.Application.Json)]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Network is not found", typeof(void), MediaTypeNames.Application.Json)]
    public async Task<IActionResult> Get(
        [FromRoute] GetNetworkRequest request,
        IGetNetworkQueryExecutor queryExecutor,
        CancellationToken cancellationToken)
    {
        var query = _mapper.Map(request);
        var result = await queryExecutor.ExecuteAsync(query, cancellationToken);

        return result is null
            ? NotFound()
            : Ok(_mapper.Map(result));
    }

    [HttpGet]
    [SwaggerOperation("Gets paged list of networks in ascending order")]
    [SwaggerResponse(StatusCodes.Status200OK, "Paged list of networks in ascending order", typeof(PagedResponse<GetNetworkResponse>), MediaTypeNames.Application.Json)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incoming request is not valid", typeof(ValidationProblemDetails), MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetPage(
        [FromRoute] GetNetworksRequest request,
        IGetNetworksQueryExecutor queryExecutor,
        CancellationToken cancellationToken)
    {
        var query = _mapper.Map(request);
        var result = await queryExecutor.ExecuteAsync(query, cancellationToken);

        return Ok(_mapper.Map(result));
    }
}