using Items.Api.Dtos.CollectionItem;
using MediatR;

namespace Items.Api.Queries.CollectionItem;

public class CollectionItemGetByIdQuery : IRequest<CollectionItemDto>
{
    public CollectionItemGetByIdQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}
