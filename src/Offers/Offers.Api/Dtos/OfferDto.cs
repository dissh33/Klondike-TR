namespace Offers.Api.Dtos;

public record OfferDto
{
    public string? Title { get; init; }
    public string? Message { get; init; }

    //public string? Expression { get; init; }

    public int? Type { get; init; }
    public int? Status { get; init; }

    public IEnumerable<OfferPositionDto>? Positions { get; init; }
}