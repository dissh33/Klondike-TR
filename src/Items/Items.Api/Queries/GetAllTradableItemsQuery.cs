using Items.Api.Dtos;
using MediatR;

namespace Items.Api.Queries;

public class GetAllTradableItemsQuery : IRequest<IEnumerable<TradableItemDto>>
{

}
