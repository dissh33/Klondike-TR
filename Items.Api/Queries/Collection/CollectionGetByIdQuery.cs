using ItemManagementService.Api.Dtos;
using MediatR;

namespace ItemManagementService.Api.Queries.Collection;

public class CollectionGetByIdQuery : IRequest<CollectionDto>
{
    public CollectionGetByIdQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}