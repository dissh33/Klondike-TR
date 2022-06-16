using Items.Api.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Items.Api.Commands.Icon;

public class IconUpdateFileCommand : IRequest<IconDto>
{
    public Guid Id { get; set; }
    public IFormFile? File { get; set; }
}
