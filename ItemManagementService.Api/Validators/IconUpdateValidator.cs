using FluentValidation;
using ItemManagementService.Api.Commands;
using ItemManagementService.Api.Commands.Icon;

namespace ItemManagementService.Api.Validators;

public class IconUpdateValidator : AbstractValidator<IconUpdateCommand>
{
    public IconUpdateValidator()
    {
        RuleFor(icon => icon.Id).NotEmpty();
        RuleFor(icon => icon.Title).Length(3, 250);
    }
}
