﻿using ItemManagementService.Api.Commands;
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
    public async Task<ActionResult<IEnumerable<IconDto>>> Get(CancellationToken ct)
    {
        var result = await _mediator.Send(new IconGetAllQuery(), ct);
        return new JsonResult(result);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<IconDto>> Get(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new IconGetByIdQuery(id), ct);
        return new JsonResult(result);
    }

    [HttpGet("{id}/file")]
    public async Task<IActionResult> GetFile(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new IconGetFileQuery(id), ct);
        return File(result.FileStream ?? Stream.Null, "application/octet-stream", result.FileName);
    }

    [HttpPost]
    public async Task<ActionResult<IconDto>> Post([FromForm] IconAddCommand requestModel, CancellationToken ct)
    {
        var result = await _mediator.Send(requestModel, ct);
        return new JsonResult(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<IconDto>> Put([FromForm] IconUpdateCommand requestModel, CancellationToken ct)
    { 
        var result = await _mediator.Send(requestModel, ct);
        return new JsonResult(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<int>> Delete(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new IconDeleteCommand(id), ct);
        return new JsonResult(result);
    }
}