﻿using Items.Api.Commands;
using Items.Api.Commands.Icon;
using Items.Api.Dtos.Icon;
using Items.Api.Queries.Icon;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Items.Admin.Controllers;

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
    public async Task<ActionResult<IEnumerable<IconDto>>> GetAll(CancellationToken ct)
    {
        var result = await _mediator.Send(new IconGetAllQuery(), ct);
        return new JsonResult(result);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<IconDto>> GetById(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new IconGetByIdQuery(id), ct);
        return new JsonResult(result);
    }

    [HttpGet("GetFile/{id}")]
    public async Task<IActionResult> GetFile(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new IconGetFileQuery(id), ct);

        var fileStream = new MemoryStream(result.FileBinary ?? Array.Empty<byte>());

        return File(fileStream, "application/octet-stream", result.FileName);
    }

    [HttpPost]
    public async Task<ActionResult<IconDto>> Create([FromForm] IconAddCommand request, CancellationToken ct)
    {
        var result = await _mediator.Send(request, ct);
        return new JsonResult(result);
    }

    [HttpPut("")]
    public async Task<ActionResult<IconDto>> Update([FromForm] IconUpdateCommand request, CancellationToken ct)
    { 
        var result = await _mediator.Send(request, ct);
        return new JsonResult(result);
    }

    [HttpPut("UpdateTitle")]
    public async Task<ActionResult<IconDto>> UpdateTitle([FromForm] IconUpdateTitleCommand request, CancellationToken ct)
    {
        var result = await _mediator.Send(request, ct);
        return new JsonResult(result);
    }

    [HttpPut("UpdateFile")]
    public async Task<ActionResult<IconDto>> UpdateFile([FromForm] IconUpdateFileCommand request, CancellationToken ct)
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