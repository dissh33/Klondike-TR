using FluentValidation;
using ItemManagementService.Api.Commands.Collection;
using ItemManagementService.Application.Contracts;

namespace ItemManagementService.Application.Validators;

public class CollectionAddValidator : IconExistenceBaseValidator<CollectionAddCommand>
{
    public CollectionAddValidator(IUnitOfWork uow) : base(uow)
    {
        RuleFor(collection => collection.Name).Length(3, 250);
        RuleFor(collection => collection.Status).InclusiveBetween(0, 2);
    }
}

public class CollectionUpdateValidator : IconExistenceBaseValidator<CollectionUpdateCommand>
{
    public CollectionUpdateValidator(IUnitOfWork uow) : base(uow)
    {
        RuleFor(collection => collection.Id).NotEmpty().NotEqual(Guid.Empty);
        RuleFor(collection => collection.Name).Length(3, 250);
        RuleFor(collection => collection.Status).InclusiveBetween(0, 2);
    }
}

public class CollectionUpdateNameValidator : AbstractValidator<CollectionUpdateNameCommand>
{
    public CollectionUpdateNameValidator()
    {
        RuleFor(collection => collection.Id).NotEmpty().NotEqual(Guid.Empty);
        RuleFor(collection => collection.Name).Length(3, 250);
    }
}

public class CollectionUpdateIconValidator : IconExistenceBaseValidator<CollectionUpdateIconCommand>
{
    public CollectionUpdateIconValidator(IUnitOfWork uow) : base(uow)
    {
        RuleFor(collection => collection.Id).NotEmpty().NotEqual(Guid.Empty);
    }
}

public class CollectionUpdateStatusValidator : AbstractValidator<CollectionUpdateStatusCommand>
{
    public CollectionUpdateStatusValidator()
    {
        RuleFor(collection => collection.Id).NotEmpty().NotEqual(Guid.Empty);
        RuleFor(collection => collection.Status).InclusiveBetween(0, 2);
    }
}