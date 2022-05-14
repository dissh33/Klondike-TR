using ItemManagementService.Api.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ItemManagementService.Api.Commands.Icon;

public  class IconAddCommand : IRequest<IconDto>
{
    public string? Title { get; set; }
    public IFormFile? File { get; set; }
}