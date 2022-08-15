using Items.Api.Dtos.CollectionItem;
using MediatR;

namespace Items.Api.Queries.CollectionItem;

public class CollectionItemGetByCollectionQuery : IRequest<IEnumerable<CollectionItemDto>>
{
    public Guid CollectionId { get; set; }
}
