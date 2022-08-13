using Items.Api.Dtos;
using MediatR;

namespace Items.Api.Queries.CollectionItem;

public class CollectionItemGetByCollectionQuery : IRequest<IEnumerable<CollectionItemDto>>
{
    public Guid CollectionId { get; set; }
}
