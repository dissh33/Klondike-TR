using MediatR;
using Offers.Api.Dtos;

namespace Offers.Api.Commands;

public record OfferConstructCommand : IRequest<OfferDto?>
{
    public string? Title { get; init; }
    public string? Message { get; init; }

    public string? Expression { get; init; }

    public int? Type { get; init; }
    public int? Status { get; init; }

    public IEnumerable<OfferPositionAddDto> Positions { get; init; } = Enumerable.Empty<OfferPositionAddDto>();
}