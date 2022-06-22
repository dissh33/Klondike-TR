using Items.Api.Dtos;
using MediatR;

namespace Items.Api.Queries.CollectionItem;

public class GetByCollectionCollectionItemQuery : IRequest<IEnumerable<CollectionItemDto>>
{
    public Guid CollectionId { get; set; }
}
