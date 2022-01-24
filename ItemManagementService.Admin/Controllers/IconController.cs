using ItemManagementService.Api.Commands;
using ItemManagementService.Api.Dtos;
using ItemManagementService.Api.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ItemManagementService.Admin.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IconController : ControllerBase
{
    private readonly IMediator _mediator;

    public IconController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IEnumerable<IconDto>> Get(CancellationToken ct)
    {
        return await _mediator.Send(new IconGetAllQuery(), ct);
    }
    
    [HttpGet("{id}")]
    public async Task<IconDto> Get(Guid id, CancellationToken ct)
    {
        return await _mediator.Send(new IconGetByIdQuery(id), ct);
    }
    
    [HttpPost]
    public async Task<IconDto> Post([FromForm] IconAddCommand requestModel, CancellationToken ct)
    {
        return await _mediator.Send(requestModel, ct);
    }

    [HttpPut("{id}")]
    public async Task<IconDto> Put([FromForm] IconUpdateCommand requestModel, CancellationToken ct)
    { 
        return await _mediator.Send(requestModel, ct);
    }

    [HttpDelete("{id}")]
    public async Task<int> Delete(Guid id, CancellationToken ct)
    {
        return await _mediator.Send(new IconDeleteCommand(id), ct);
    }
}