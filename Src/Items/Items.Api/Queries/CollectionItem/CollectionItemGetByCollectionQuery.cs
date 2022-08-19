using Items.Api.Dtos.CollectionItem;
using MediatR;

namespace Items.Api.Queries.CollectionItem;

public class CollectionItemGetByCollectionQuery : IRequest<IEnumerable<CollectionItemDto>>
{
    public CollectionItemGetByCollectionQuery(Guid collectionId)
    {
        CollectionId = collectionId;
    }

    public Guid CollectionId { get; set; }
}
