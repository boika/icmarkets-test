using System.Net.Mime;
using Asp.Versioning;
using ICMarketsTest.Core.BlockchainSnapshots.GetBlockchainSnapshot;
using ICMarketsTest.Core.BlockchainSnapshots.GetBlockchainSnapshots;
using ICMarketsTest.Core.BlockchainSnapshots.TakeBlockchainSnapshot;
using ICMarketsTest.Core.Networks.GetNetwork;
using ICMarketsTest.WebApi.BlockchainSnapshots.GetBlockchainSnapshot;
using ICMarketsTest.WebApi.BlockchainSnapshots.GetBlockchainSnapshots;
using ICMarketsTest.WebApi.BlockchainSnapshots.TakeBlockchainSnapshot;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ICMarketsTest.WebApi.BlockchainSnapshots;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v1/networks/{networkId}/snapshots")]
[SwaggerTag("Blockchain snapshots operations")]
public class BlockchainSnapshotsController : ControllerBase
{
    [HttpPost]
    [SwaggerOperation("Takes blockchain snapshot for network")]
    [SwaggerResponse(StatusCodes.Status200OK, "Operation succeeded", typeof(void), MediaTypeNames.Application.Json)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incoming request is not valid", typeof(ValidationProblemDetails), MediaTypeNames.Application.Json)]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Network is not found", typeof(void), MediaTypeNames.Application.Json)]
    public async Task<IActionResult> Take(
        [FromRoute] TakeBlockchainSnapshotRequest request,
        [FromServices] IGetNetworkQueryHandler queryHandler,
        [FromServices] ITakeBlockchainSnapshotCommandHandler commandHandler,
        [FromServices] ITakeBlockchainSnapshotMapper mapper,
        CancellationToken cancellationToken)
    {
        var query = mapper.Map(request);
        var result = await queryHandler.HandleAsync(query, cancellationToken);
        if (result is null)
        {
            return NotFound();
        }

        var command = mapper.Map(result);
        await commandHandler.HandleAsync(command, cancellationToken);

        return Ok();
    }

    [HttpGet("{snapshotId}")]
    [SwaggerOperation("Gets blockchain snapshot by identifier and network")]
    [SwaggerResponse(StatusCodes.Status200OK, "The blockchain snapshot", typeof(GetBlockchainSnapshotResponse), MediaTypeNames.Application.Json)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incoming request is not valid", typeof(ValidationProblemDetails), MediaTypeNames.Application.Json)]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Blockchain snapshot is not found", typeof(void), MediaTypeNames.Application.Json)]
    public async Task<IActionResult> Get(
        [FromRoute] GetBlockchainSnapshotRequest request,
        [FromServices] IGetBlockchainSnapshotQueryHandler queryHandler,
        [FromServices] IGetBlockchainSnapshotMapper mapper,
        CancellationToken cancellationToken)
    {
        var query = mapper.Map(request);
        var result = await queryHandler.HandleAsync(query, cancellationToken);

        return result is null
            ? NotFound()
            : Ok(mapper.Map(result));
    }

    [HttpGet]
    [SwaggerOperation("Gets paged list of blockchain snapshots in reverse chronological order")]
    [SwaggerResponse(StatusCodes.Status200OK, "Paged list of blockchain snapshots in reverse chronological order", typeof(PagedResponse<GetBlockchainSnapshotResponse>), MediaTypeNames.Application.Json)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incoming request is not valid", typeof(ValidationProblemDetails), MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetPage(
        [FromRoute] GetBlockchainSnapshotsRequest request,
        [FromServices] IGetBlockchainSnapshotsQueryHandler queryHandler,
        [FromServices] IGetBlockchainSnapshotsMapper mapper,
        CancellationToken cancellationToken)
    {
        var query = mapper.Map(request);
        var result = await queryHandler.HandleAsync(query, cancellationToken);

        return Ok(mapper.Map(result));
    }
}