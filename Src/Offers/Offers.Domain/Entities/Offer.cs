using Offers.Domain.Enums;
using Offers.Domain.SeedWork;
using Offers.Domain.TypedIds;

namespace Offers.Domain.Entities;

public class Offer : BaseEntity
{
    public OfferId Id { get; }

    public string? Title { get; }
    public string? Message { get; }
    public string? Expression { get; }

    public OfferType Type { get; }
    public OfferStatus Status { get; }

    private readonly List<OfferPosition> _positions = new();
    public IReadOnlyList<OfferPosition> Positions => _positions;

    private Offer()
    {
        Id = new OfferId(Guid.NewGuid());
    }

    public Offer(
        string? title,
        string? message,
        string? expression,
        OfferType type = OfferType.Default,
        OfferStatus status = OfferStatus.Draft,
        Guid? id = null)
    {
        Id = new OfferId(id);
        Title = title;
        Message = message;
        Expression = expression;
        Type = type;
        Status = status;
    }

    public OfferPositionId AddPosition(
        string? priceRate,
        bool withTrader,
        string? message,
        OfferPositionType type,
        Guid? id = null)
    {
        var position = new OfferPosition(Id, priceRate, withTrader, message, type, id);
        _positions.Add(position);

        return position.Id;
    }
}
