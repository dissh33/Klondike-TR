using ItemManagementService.Api.Dtos;
using MediatR;

namespace ItemManagementService.Api.Commands;

public  class IconAddCommand : IRequest<IconDto>
{
    public string? Title { get; set; }
    public byte[]? FileBinary { get; set; }
    public string? FileName { get; set; }
}