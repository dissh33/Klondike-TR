using Items.Api.Dtos.CollectionItem;
using MediatR;

namespace Items.Api.Commands.CollectionItem;

public class CollectionItemUpdateNameCommand : IRequest<CollectionItemDto>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
}
