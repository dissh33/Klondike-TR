using FluentValidation;
using ItemManagementService.Api.Commands;
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