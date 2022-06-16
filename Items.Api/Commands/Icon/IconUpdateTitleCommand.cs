using Items.Api.Dtos;
using MediatR;

namespace Items.Api.Commands.Icon;

public class IconUpdateTitleCommand : IRequest<IconDto>
{
    public Guid Id { get; set; }
    public string? Title { get; set; } 
}
