using Items.Api.Dtos.Collection;
using MediatR;

namespace Items.Api.Queries.Collection;

public class CollectionGetAllQuery : IRequest<IEnumerable<CollectionDto>>
{

}