using ItemManagementService.Api.Dtos;
using MediatR;

namespace ItemManagementService.Api.Queries.CollectionItem;

public class CollectionItemGetByIdQuery : IRequest<CollectionItemDto>
{
    public CollectionItemGetByIdQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}
