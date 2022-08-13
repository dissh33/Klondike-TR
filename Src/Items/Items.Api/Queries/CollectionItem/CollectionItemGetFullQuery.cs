using Items.Api.Dtos;
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
