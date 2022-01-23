using FluentValidation;
using ItemManagementService.Api.Commands;

namespace ItemManagementService.Api.Validators;

public class IconUpdateValidator : AbstractValidator<IconUpdateCommand>
{
    public IconUpdateValidator()
    {
        RuleFor(icon => icon.Id).NotEmpty();
        RuleFor(icon => icon.Title).MaximumLength(250);
    }
}
