using Items.Api.Dtos.CollectionItem;
using MediatR;

namespace Items.Api.Queries.CollectionItem;

public class CollectionItemGetAllQuery : IRequest<IEnumerable<CollectionItemDto>>
{

}
