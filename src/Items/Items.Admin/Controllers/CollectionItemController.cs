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
    public async Task<ActionResult<IEnumerable<CollectionItemDto>>> GetAll(CancellationToken ct)
    {
        var result = await _mediator.Send(new CollectionItemGetAllQuery(), ct);
        return new JsonResult(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CollectionItemDto>> GetById(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new CollectionItemGetByIdQuery(id), ct);
        return new JsonResult(result);
    }

    [HttpGet("GetFull/{id}")]
    public async Task<ActionResult<CollectionItemFullDto>> GetFull(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new CollectionItemGetFullQuery(id), ct);
        return new JsonResult(result);
    }

    [HttpGet("GetByCollection")]
    public async Task<ActionResult<IEnumerable<CollectionItemDto>>> GetByCollection([FromForm] Guid collectionId, CancellationToken ct)
    {
        var result = await _mediator.Send(new CollectionItemGetByCollectionQuery(collectionId), ct);
        return new JsonResult(result);
    }

    [HttpGet("GetFullByCollection")]
    public async Task<ActionResult<IEnumerable<CollectionItemFullDto>>> GetFullByCollection([FromForm] Guid collectionId, CancellationToken ct)
    {
        var result = await _mediator.Send(new CollectionItemGetFullByCollectionQuery(collectionId), ct);
        return new JsonResult(result);
    }

    [HttpPost]
    public async Task<ActionResult<CollectionItemDto>> Create([FromForm] CollectionItemAddCommand request, CancellationToken ct)
    {
        var result = await _mediator.Send(request, ct);
        return new JsonResult(result);
    }

    [HttpPut("")]
    public async Task<ActionResult<CollectionItemDto>> Update([FromForm] CollectionItemUpdateCommand request, CancellationToken ct)
    {
        var result = await _mediator.Send(request, ct);
        return new JsonResult(result);
    }

    [HttpPut("UpdateName")]
    public async Task<ActionResult<CollectionItemDto>> UpdateName([FromForm] CollectionItemUpdateNameCommand request, CancellationToken ct)
    {
        var result = await _mediator.Send(request, ct);
        return new JsonResult(result);
    }

    [HttpPut("UpdateIcon")]
    public async Task<ActionResult<CollectionItemDto>> UpdateIcon([FromForm] CollectionItemUpdateIconCommand request, CancellationToken ct)
    {
        var result = await _mediator.Send(request, ct);
        return new JsonResult(result);
    }

    [HttpPut("UpdateCollection")]
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