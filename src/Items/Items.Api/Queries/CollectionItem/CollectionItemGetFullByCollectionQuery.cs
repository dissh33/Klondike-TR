using Items.Api.Dtos.CollectionItem;
using MediatR;

namespace Items.Api.Queries.CollectionItem;

public class CollectionItemGetFullByCollectionQuery : IRequest<IEnumerable<CollectionItemFullWithFileDto>>
{
    public Guid CollectionId { get; set; }
}
