using FluentValidation;
using Offers.Api.Commands;

namespace Offers.Application.Validators;

public class OfferConstructValidator : AbstractValidator<OfferConstructCommand>
{
    public OfferConstructValidator()
    {
        RuleFor(command => command.Title).Length(3, 255);
        RuleFor(command => command.Status).InclusiveBetween(0, 5);
        RuleFor(command => command.Type).InclusiveBetween(0, 1);
        
        RuleForEach(command => command.Positions)
            .Must(position => position.Message?.Length <= 500)
            .WithMessage("Maximum length for Message is 500 symbols.");

        RuleForEach(command => command.Positions)
            .Must(position => position.PriceRate?.Length is >= 4 and <= 50)
            .WithMessage("PriceRate must be between 4 and 50.");

        RuleForEach(command => command.Positions)
            .Must(position => position.Type is >= 0 and <= 6)
            .WithMessage("Not allowed PositionType.");

        RuleForEach(command => command.Positions)
            .Must(dto => dto.OfferItems.Any())
            .WithMessage("Each Position must contains at least one Item.");

        RuleForEach(command => command.Positions)
            .Must(position => position.OfferItems.All(item => item.TradableItemId != Guid.Empty))
            .WithMessage("Empty TradableItemId.");

        RuleForEach(command => command.Positions)
            .Must(position => position.OfferItems.All(item  => item.Amount >= 0))
            .WithMessage("Amount of Item must be greater or equal 0.");

        RuleForEach(command => command.Positions)
            .Must(position => position.OfferItems.All(item => item.Type is >= 0 and <= 1))
            .WithMessage("Not allowed Item Type.");
    }
}
