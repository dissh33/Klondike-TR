using Items.Api.Dtos.CollectionItem;
using MediatR;

namespace Items.Api.Queries.CollectionItem;

public class CollectionItemGetFullByCollectionQuery : IRequest<IEnumerable<CollectionItemFullWithFileDto>>
{
    public CollectionItemGetFullByCollectionQuery(Guid collectionId)
    {
        CollectionId = collectionId;
    }

    public Guid CollectionId { get; set; }
}
