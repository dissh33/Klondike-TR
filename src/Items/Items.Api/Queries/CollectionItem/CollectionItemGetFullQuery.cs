using Items.Api.Dtos.CollectionItem;
using MediatR;

namespace Items.Api.Queries.CollectionItem;

public class CollectionItemGetFullQuery : IRequest<CollectionItemFullDto>
{
    public CollectionItemGetFullQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}
