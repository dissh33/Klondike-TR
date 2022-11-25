using MediatR;
using Offers.Api.Dtos;

namespace Offers.Api.Queries.OfferPosition;

public class OfferPositionGetByOfferQuery : IRequest<IEnumerable<OfferPositionDto>>
{
    public OfferPositionGetByOfferQuery(Guid offerId)
    {
        OfferId = offerId;
    }

    public Guid OfferId { get; set; }
}