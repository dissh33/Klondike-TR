using ItemManagementService.Api.Dtos;
using MediatR;

namespace ItemManagementService.Api.Commands.Icon;

public class IconUpdateTitleCommand : IRequest<IconDto>
{
    public Guid Id { get; set; }
    public string? Title { get; set; } 
}
