using FluentValidation;
using ItemManagementService.Api.Commands.Material;
using ItemManagementService.Application.Contracts;

namespace ItemManagementService.Application.Validators;

public class MaterialAddValidator : IconExistenceBaseValidator<MaterialAddCommand>
{
    public MaterialAddValidator(IUnitOfWork uow) : base(uow)
    {
        RuleFor(material => material.Name).Length(3, 250);
        RuleFor(material => material.Status).InclusiveBetween(0, 2);
        RuleFor(material => material.Type).InclusiveBetween(0, 1);
    }
}

public class MaterialUpdateValidator : IconExistenceBaseValidator<MaterialUpdateCommand>
{
    public MaterialUpdateValidator(IUnitOfWork uow) : base(uow)
    {
        RuleFor(material => material.Id).NotEmpty().NotEqual(Guid.Empty);
        RuleFor(material => material.Name).Length(3, 250);
        RuleFor(material => material.Status).InclusiveBetween(0, 2);
        RuleFor(material => material.Type).InclusiveBetween(0, 1);
    }
}

public class MaterialUpdateIconValidator : IconExistenceBaseValidator<MaterialUpdateIconCommand>
{
    public MaterialUpdateIconValidator(IUnitOfWork uow) : base(uow)
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