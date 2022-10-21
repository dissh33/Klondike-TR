using MediatR;
using Offers.Api.Dtos;

namespace Offers.Api.Commands;

public class OfferConstructCommand : IRequest<OfferDto?>
{
    public string? Title { get; set; }
    public string? Message { get; set; }
    public string? Expression { get; set; }

    public int? Type { get; set; }
    public int? Status { get; set; }

    public IEnumerable<OfferPositionDto> Positions { get; set; } = Enumerable.Empty<OfferPositionDto>();
}