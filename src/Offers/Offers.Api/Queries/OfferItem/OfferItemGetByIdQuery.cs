using MediatR;
using Offers.Api.Dtos;

namespace Offers.Api.Queries.OfferItem;

public class OfferItemGetByIdQuery : IRequest<OfferItemDto>
{
    public OfferItemGetByIdQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}