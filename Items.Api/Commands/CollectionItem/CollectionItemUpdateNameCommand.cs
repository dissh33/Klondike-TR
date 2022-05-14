using ItemManagementService.Api.Dtos;
using MediatR;

namespace ItemManagementService.Api.Commands.CollectionItem;

public class CollectionItemUpdateNameCommand : IRequest<CollectionItemDto>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
}
