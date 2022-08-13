using Items.Api.Dtos;
using MediatR;

namespace Items.Api.Queries.CollectionItem;

public class GetAllCollectionItemQuery : IRequest<IEnumerable<CollectionItemDto>>
{

}
