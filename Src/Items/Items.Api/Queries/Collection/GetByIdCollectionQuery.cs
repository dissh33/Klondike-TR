using Items.Api.Dtos;
using MediatR;

namespace Items.Api.Queries.Collection;

public class GetByIdCollectionQuery : IRequest<CollectionDto>
{
    public GetByIdCollectionQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}