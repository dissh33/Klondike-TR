using FluentValidation;
using Offers.Api.Commands;

namespace Offers.Application.Validation;

public class OfferConstructValidator : AbstractValidator<OfferConstructCommand>
{
    public OfferConstructValidator()
    {
        RuleFor(command => command.Title).Length(3, 255);
        RuleFor(command => command.Status).InclusiveBetween(0, ValidationConstants.OFFER_STATUS_MAX_RANGE);
        RuleFor(command => command.Type).InclusiveBetween(0, ValidationConstants.OFFER_TYPE_MAX_RANGE);
        
        RuleForEach(command => command.Positions)
            .Must(position => position.Message?.Length <= 500)
            .WithMessage("Maximum length for Message is 500 symbols.");

        RuleForEach(command => command.Positions)
            .Must(position => position.PriceRate?.Length is >= 4 and <= 50)
            .WithMessage("PriceRate must be between 4 and 50.");

        RuleForEach(command => command.Positions.Select(position => position.Type))
            .InclusiveBetween(0, ValidationConstants.OFFER_POSITION_TYPE_MAX_RANGE)
            .WithMessage("Offer contains not allowed PositionType.");

        RuleForEach(command => command.Positions)
            .Must(dto => dto.OfferItems.Any())
            .WithMessage("Each Position must contains at least one Item.");

        RuleForEach(command => command.Positions)
            .Must(position => position.OfferItems.All(item => item.TradableItemId != Guid.Empty))
            .WithMessage("Empty TradableItemId.");

        RuleForEach(command => command.Positions)
            .Must(position => position.OfferItems.All(item => item.Amount >= 0))
            .WithMessage("Amount of Items in each Position must not be empty.");

        RuleForEach(command => command.Positions.SelectMany(position => position.OfferItems.Select(item => item.Type)))
            .InclusiveBetween(0, ValidationConstants.OFFER_ITEM_TYPE_MAX_RANGE)
            .WithMessage("Offer contains not allowed ItemType.");
    }
}
