using Items.Api.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Items.Api.Commands.Icon;

public  class IconUpdateCommand : IRequest<IconDto>
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public IFormFile? File { get; set; }
}