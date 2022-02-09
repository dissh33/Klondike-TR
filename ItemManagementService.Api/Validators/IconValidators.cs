using FluentValidation;
using ItemManagementService.Api.Commands.Icon;

namespace ItemManagementService.Api.Validators;

public class IconAddValidator : AbstractValidator<IconAddCommand>
{
    public IconAddValidator()
    {
        RuleFor(icon => icon.Title).Length(3, 250);
        RuleFor(icon => icon.File).NotEmpty();
    }
}

public class IconUpdateValidator : AbstractValidator<IconUpdateCommand>
{
    public IconUpdateValidator()
    {
        RuleFor(icon => icon.Id).NotEmpty().NotEqual(Guid.Empty);
        RuleFor(icon => icon.Title).Length(3, 250);
    }
}