using MediatR;
using Offers.Api.Dtos;
using Offers.Api.Helpers;

namespace Offers.Api.Queries.Offer;

public record OfferGetByPageQuery : IRequest<PaginationWrapper<OfferDto>?>
{
    public int Page { get; init; }
    public int PageSize { get; init; }

    public Dictionary<string, string?>? OrderBy { get; init; }
}
