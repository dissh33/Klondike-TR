using Items.Api.Dtos;
using MediatR;

namespace Items.Api.Queries.Collection;

public class CollectionGetFullQuery : IRequest<CollectionFullDto>
{
    public CollectionGetFullQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}