using ItemManagementService.Api.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ItemManagementService.Api.Commands;

public class IconUpdateFileCommand : IRequest<IconDto>
{
    public Guid Id { get; set; }
    public IFormFile? File { get; set; }
}
