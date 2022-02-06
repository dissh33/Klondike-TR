using ItemManagementService.Api.Dtos;
using MediatR;

namespace ItemManagementService.Api.Queries.CollectionItem;

public class CollectionItemGetAllQuery : IRequest<IEnumerable<CollectionItemDto>>
{

}
