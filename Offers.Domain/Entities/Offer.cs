using System.Security.Principal;
using Offers.Domain.Enums;
using Offers.Domain.SeedWork;
using Offers.Domain.TypedIds;

namespace Offers.Domain.Entities;

public class Offer : BaseEntity
{
    public OfferId? Id { get; set; }

    public string? Title { get; set; }
    public string? Message { get; set; }
    public string? Expression { get; set; }

    public OfferType Type { get; set; }
    public OfferStatus Status { get; set; }

    private Offer()
    {
        
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
}
