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


        RuleForEach(command => command.Positions.Select(position => position.Message)).Length(0, 500);
        RuleForEach(command => command.Positions.Select(position => position.PriceRate)).Length(4, 50);
        RuleForEach(command => command.Positions.Select(position => position.WithTrader)).InclusiveBetween(false, true);
        RuleForEach(command => command.Positions.Select(position => position.Type)).InclusiveBetween(0, 6);


        RuleForEach(command => command.Positions)
            .Must(dto => dto.OfferItems.Any());


        RuleForEach(command => command.Positions
            .SelectMany(position => position.OfferItems).Select(item => item.TradableItemId)).NotEmpty();

        RuleForEach(command => command.Positions
            .SelectMany(position => position.OfferItems).Select(item => item.Amount)).GreaterThanOrEqualTo(0);

        RuleForEach(command => command.Positions
            .SelectMany(position => position.OfferItems).Select(item => item.Type)).InclusiveBetween(0,1);
    }
}
