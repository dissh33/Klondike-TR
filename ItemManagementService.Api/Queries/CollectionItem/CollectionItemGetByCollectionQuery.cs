using ItemManagementService.Api.Dtos;
using MediatR;

namespace ItemManagementService.Api.Queries.CollectionItem;

public class CollectionItemGetByCollectionQuery : IRequest<IEnumerable<CollectionItemDto>>
{
    public CollectionItemGetByCollectionQuery(Guid collectionId)
    {
        CollectionId = collectionId;
    }

    public Guid CollectionId { get; set; }
}
