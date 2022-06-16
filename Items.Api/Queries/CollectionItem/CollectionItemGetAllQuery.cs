using Items.Api.Dtos;
using MediatR;

namespace Items.Api.Queries.CollectionItem;

public class CollectionItemGetAllQuery : IRequest<IEnumerable<CollectionItemDto>>
{

}
