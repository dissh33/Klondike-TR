using Items.Api.Dtos;
using MediatR;

namespace Items.Api.Queries.Collection;

public class CollectionGetAllQuery : IRequest<IEnumerable<CollectionDto>>
{

}