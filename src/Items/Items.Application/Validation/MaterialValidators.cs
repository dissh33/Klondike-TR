using FluentValidation;
using Items.Api.Commands.Material;
using Items.Application.Contracts;
using Items.Domain.Enums;

namespace Items.Application.Validation;

public class MaterialAddValidator : IconExistenceBaseValidator<MaterialAddCommand>
{
    public MaterialAddValidator(IUnitOfWork uow) : base(uow)
    {
        RuleFor(material => material.Name).Length(3, 250);
        RuleFor(material => material.Status).InclusiveBetween(0, ValidationConstants.ITEM_STATUS_MAX_RANGE);
        RuleFor(material => material.Type).InclusiveBetween(0, 1);
    }
}

public class MaterialUpdateValidator : IconExistenceBaseValidator<MaterialUpdateCommand>
{
    public MaterialUpdateValidator(IUnitOfWork uow) : base(uow)
    {
        RuleFor(material => material.Id).NotEmpty().NotEqual(Guid.Empty);
        RuleFor(material => material.Name).Length(3, 250);
        RuleFor(material => material.Status).InclusiveBetween(0, ValidationConstants.ITEM_STATUS_MAX_RANGE);
        RuleFor(material => material.Type).InclusiveBetween(0, ValidationConstants.MATERIAL_TYPE_MAX_RANGE);
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
        RuleFor(material => material.Status).InclusiveBetween(0, ValidationConstants.ITEM_STATUS_MAX_RANGE);
    }
}