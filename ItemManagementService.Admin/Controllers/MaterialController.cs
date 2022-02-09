using ItemManagementService.Api.Commands;
using ItemManagementService.Api.Commands.Collection;
using ItemManagementService.Api.Commands.Material;
using ItemManagementService.Api.Dtos;
using ItemManagementService.Api.Queries.Collection;
using ItemManagementService.Api.Queries.Material;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace ItemManagementService.Admin.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MaterialController : ControllerBase
{
    private readonly IMediator _mediator;

    public MaterialController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MaterialDto>>> Get(CancellationToken ct)
    {
        var result = await _mediator.Send(new MaterialGetAllQuery(), ct);
        return new JsonResult(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MaterialDto>> Get(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new MaterialGetByIdQuery(id), ct);
        return new JsonResult(result);
    }

    [HttpGet("filter")]
    public async Task<ActionResult<IEnumerable<MaterialDto>>> GetByFilter([FromQuery] MaterialGetByFilterQuery request, CancellationToken ct)
    {
        var result = await _mediator.Send(request, ct);
        return new JsonResult(result);
    }

    [HttpPost]
    public async Task<ActionResult<MaterialDto>> Post([FromForm] MaterialAddCommand request, CancellationToken ct)
    {
        var result = await _mediator.Send(request, ct);
        return new JsonResult(result);
    }

    [HttpPut("update")]
    public async Task<ActionResult<MaterialDto>> Put([FromForm] MaterialUpdateCommand request, CancellationToken ct)
    {
        var result = await _mediator.Send(request, ct);
        return new JsonResult(result);
    }
    
    [HttpPut("update/icon")]
    public async Task<ActionResult<MaterialDto>> UpdateIcon([FromForm] MaterialUpdateIconCommand request, CancellationToken ct)
    {
        var result = await _mediator.Send(request, ct);
        return new JsonResult(result);
    }

    [HttpPut("update/status")]
    public async Task<ActionResult<MaterialDto>> UpdateStatus([FromForm] MaterialUpdateStatusCommand request, CancellationToken ct)
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