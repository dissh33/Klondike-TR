using MediatR;
using Offers.Api.Dtos;

namespace Offers.Api.Queries.OfferItem;

public class OfferItemGetByPositionQuery : IRequest<IEnumerable<OfferItemDto>>
{
    public OfferItemGetByPositionQuery(Guid positionId)
    {
        PositionId = positionId;
    }

    public Guid PositionId { get; set; }
}