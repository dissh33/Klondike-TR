using ItemManagementService.Api.Commands;
using ItemManagementService.Api.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ItemManagementService.Admin.Controllers;

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
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }
    
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
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

    [HttpDelete("{id}")]
    public async Task<ActionResult<int>> Delete(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new DeleteByIdCommand(id), ct);
        return new JsonResult(result);
    }
}