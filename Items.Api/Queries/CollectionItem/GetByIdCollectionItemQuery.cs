using Items.Api.Dtos;
using MediatR;

namespace Items.Api.Queries.CollectionItem;

public class GetByIdCollectionItemQuery : IRequest<CollectionItemDto>
{
    public GetByIdCollectionItemQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}
