using Items.Api.Dtos;
using MediatR;

namespace Items.Api.Queries.Collection;

public class GetAllCollectionQuery : IRequest<IEnumerable<CollectionDto>>
{

}