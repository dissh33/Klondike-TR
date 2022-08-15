using Items.Api.Dtos;
using MediatR;

namespace Items.Api.Queries.CollectionItem;

public class CollectionItemGetFullByCollectionQuery : IRequest<IEnumerable<CollectionItemFullDto>>
{
    public Guid CollectionId { get; set; }
}
