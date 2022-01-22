using ItemManagementService.Api.Commands;
using ItemManagementService.Api.Dtos;
using ItemManagementService.Api.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace ItemManagementService.Controllers;

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
    public async Task<IconDto> Post([FromForm] string? title, IFormFile file, CancellationToken ct)
    {
        var fileStream = new MemoryStream();
        await file.CopyToAsync(fileStream, ct);
        var binary = fileStream.GetBuffer();

        var request = new IconAddCommand
        {
            Title = title,
            FileBinary = binary,
            FileName = file.FileName,
        };

        return await _mediator.Send(request, ct);
    }

    [HttpPut("{id}")]
    public async Task<IconDto> Put([FromRoute] Guid id, [FromForm] string? title, IFormFile? file, CancellationToken ct)
    {
        byte[]? binary = null;

        if (file != null)
        {
            var fileStream = new MemoryStream();
            await file.CopyToAsync(fileStream, ct);
            binary = fileStream.GetBuffer(); 
        }

        var request = new IconUpdateCommand
        {
            Id = id,
            Title = title,
            FileBinary = binary,
            FileName = file?.FileName,
        };

        return await _mediator.Send(request, ct);
    }
    
    [HttpDelete("{id}")]
    public async Task<int> Delete(Guid id, CancellationToken ct)
    {
        return await _mediator.Send(new IconDeleteCommand(id), ct);
    }
}