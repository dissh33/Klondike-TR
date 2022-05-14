using ItemManagementService.Api.Dtos;
using MediatR;

namespace ItemManagementService.Api.Queries.Collection;

public class CollectionGetAllQuery : IRequest<IEnumerable<CollectionDto>>
{

}