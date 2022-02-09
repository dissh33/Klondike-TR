using FluentValidation;
using ItemManagementService.Api.Commands.Material;

namespace ItemManagementService.Api.Validators;

public class MaterialAddValidator : AbstractValidator<MaterialAddCommand>
{
    public MaterialAddValidator()
    {
        RuleFor(material => material.Name).Length(3, 250);
        RuleFor(material => material.Status).InclusiveBetween(0, 2);
        RuleFor(material => material.Type).InclusiveBetween(0, 1);
    }
}

public class MaterialUpdateValidator : AbstractValidator<MaterialUpdateCommand>
{
    public MaterialUpdateValidator()
    {
        RuleFor(material => material.Id).NotEmpty().NotEqual(Guid.Empty);
        RuleFor(material => material.Name).Length(3, 250);
        RuleFor(material => material.Status).InclusiveBetween(0, 2);
        RuleFor(material => material.Type).InclusiveBetween(0, 1);
    }
}

public class MaterialUpdateIconValidator : AbstractValidator<MaterialUpdateIconCommand>
{
    public MaterialUpdateIconValidator()
    {
        RuleFor(material => material.Id).NotEmpty().NotEqual(Guid.Empty);
    }
}

public class MaterialUpdateStatusValidator : AbstractValidator<MaterialUpdateStatusCommand>
{
    public MaterialUpdateStatusValidator()
    {
        RuleFor(material => material.Id).NotEmpty().NotEqual(Guid.Empty);
        RuleFor(material => material.Status).InclusiveBetween(0, 2);
    }
}