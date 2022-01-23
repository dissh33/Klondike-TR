using FluentValidation;
using ItemManagementService.Api.Commands;

namespace ItemManagementService.Api.Validators;

public class IconAddValidator : AbstractValidator<IconAddCommand>
{
    public IconAddValidator()
    {
        RuleFor(icon => icon.Title).MaximumLength(250);
        RuleFor(icon => icon.FileBinary).NotEmpty();
    }
}