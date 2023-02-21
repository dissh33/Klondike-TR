using MediatR;
using Offers.Api.Dtos;

namespace Offers.Api.Queries.Offer;

public record OfferGetByPageQuery : IRequest<IEnumerable<OfferDto>>
{
    public int Page { get; init; }
    public int PageSize { get; init; }

    public string[]? OrderBy { get; init; }
}
