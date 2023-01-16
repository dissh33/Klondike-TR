using MediatR;
using Offers.Api.Dtos;

namespace Offers.Api.Queries.OfferPosition;

public class OfferPositionGetByIdQuery : IRequest<OfferPositionDto>
{
    public OfferPositionGetByIdQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}