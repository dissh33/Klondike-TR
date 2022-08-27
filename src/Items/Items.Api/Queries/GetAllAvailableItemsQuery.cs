using Items.Api.Dtos;
using MediatR;

namespace Items.Api.Queries;

public class GetAllAvailableItemsQuery : IRequest<GroupedTradableItemsDto>
{

}
