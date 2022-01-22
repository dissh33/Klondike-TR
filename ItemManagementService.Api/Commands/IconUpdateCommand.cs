using ItemManagementService.Api.Dtos;
using MediatR;

namespace ItemManagementService.Api.Commands;

public  class IconUpdateCommand : IRequest<IconDto>
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public byte[]? FileBinary { get; set; }
    public string? FileName { get; set; }
}