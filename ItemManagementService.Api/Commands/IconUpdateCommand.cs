using ItemManagementService.Api.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ItemManagementService.Api.Commands;

public  class IconUpdateCommand : IRequest<IconDto>
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public IFormFile? File { get; set; }
}