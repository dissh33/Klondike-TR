using MediatR;
using Offers.Api.Dtos;

namespace Offers.Api.Queries.Offer;

public class OfferGetByIdQuery : IRequest<OfferDto>
{
    public OfferGetByIdQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}