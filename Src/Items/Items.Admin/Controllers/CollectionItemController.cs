using Items.Api.Commands;
using Items.Api.Commands.CollectionItem;
using Items.Api.Dtos.CollectionItem;
using Items.Api.Queries.CollectionItem;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Items.Admin.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CollectionItemController : ControllerBase
{
    private readonly IMediator _mediator;

    public CollectionItemController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CollectionItemDto>>> Get(CancellationToken ct)
    {
        var result = await _mediator.Send(new CollectionItemGetAllQuery(), ct);
        return new JsonResult(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CollectionItemDto>> Get(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new CollectionItemGetByIdQuery(id), ct);
        return new JsonResult(result);
    }

    [HttpGet("{id}/full")]
    public async Task<ActionResult<CollectionItemFullDto>> GetFull(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new CollectionItemGetFullQuery(id), ct);
        return new JsonResult(result);
    }

    [HttpGet("collection")]
    public async Task<ActionResult<IEnumerable<CollectionItemDto>>> GetByCollection([FromQuery] CollectionItemGetByCollectionQuery request, CancellationToken ct)
    {
        var result = await _mediator.Send(request, ct);
        return new JsonResult(result);
    }

    [HttpGet("collection/full")]
    public async Task<ActionResult<IEnumerable<CollectionItemFullDto>>> GetFullByCollection([FromQuery] CollectionItemGetFullByCollectionQuery request, CancellationToken ct)
    {
        var result = await _mediator.Send(request, ct);
        return new JsonResult(result);
    }

    [HttpPost]
    public async Task<ActionResult<CollectionItemDto>> Post([FromForm] CollectionItemAddCommand request, CancellationToken ct)
    {
        var result = await _mediator.Send(request, ct);
        return new JsonResult(result);
    }

    [HttpPut("update")]
    public async Task<ActionResult<CollectionItemDto>> Put([FromForm] CollectionItemUpdateCommand request, CancellationToken ct)
    {
        var result = await _mediator.Send(request, ct);
        return new JsonResult(result);
    }

    [HttpPut("update/name")]
    public async Task<ActionResult<CollectionItemDto>> UpdateName([FromForm] CollectionItemUpdateNameCommand request, CancellationToken ct)
    {
        var result = await _mediator.Send(request, ct);
        return new JsonResult(result);
    }

    [HttpPut("update/icon")]
    public async Task<ActionResult<CollectionItemDto>> UpdateIcon([FromForm] CollectionItemUpdateIconCommand request, CancellationToken ct)
    {
        var result = await _mediator.Send(request, ct);
        return new JsonResult(result);
    }

    [HttpPut("update/collection")]
    public async Task<ActionResult<CollectionItemDto>> UpdateCollection([FromForm] CollectionItemUpdateCollectionCommand request, CancellationToken ct)
    {
        var result = await _mediator.Send(request, ct);
        return new JsonResult(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<int>> Delete(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new DeleteByIdCommand(id), ct);
        return new JsonResult(result);
    }
}