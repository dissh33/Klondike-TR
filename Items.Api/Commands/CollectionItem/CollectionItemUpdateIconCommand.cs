using ItemManagementService.Api.Dtos;
using MediatR;

namespace ItemManagementService.Api.Commands.CollectionItem;

public class CollectionItemUpdateIconCommand : IRequest<CollectionItemDto>, IHaveIcon
{
    public Guid Id { get; set; }
    public Guid IconId { get; set; }
}