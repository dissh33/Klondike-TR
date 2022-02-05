using ItemManagementService.Api.Dtos;
using MediatR;

namespace ItemManagementService.Api.Commands;

public class CollectionAddCommand : IRequest<CollectionDto>
{
    public string? Name { get; set; }
    public Guid? IconId { get; set; }
}
