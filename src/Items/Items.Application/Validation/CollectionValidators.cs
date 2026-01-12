using FluentValidation;
using Items.Api.Commands.Collection;
using Items.Application.Contracts;
using Items.Domain;
using Items.Domain.Enums;

namespace Items.Application.Validation;

public class CollectionAddValidator : IconExistenceBaseValidator<CollectionAddCommand>
{
    public CollectionAddValidator(IUnitOfWork uow) : base(uow)
    {
        RuleFor(collection => collection.Name).Length(3, 250);
        RuleFor(collection => collection.Status).InclusiveBetween(0, ValidationConstants.ITEM_STATUS_MAX_RANGE);
    }
}

public class CollectionUpdateValidator : IconExistenceBaseValidator<CollectionUpdateCommand>
{
    public CollectionUpdateValidator(IUnitOfWork uow) : base(uow)
    {
        RuleFor(collection => collection.Id).NotEmpty().NotEqual(Guid.Empty);
        RuleFor(collection => collection.Name).Length(3, 250);
        RuleFor(collection => collection.Status).InclusiveBetween(0, ValidationConstants.ITEM_STATUS_MAX_RANGE);
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
        RuleFor(collection => collection.Status).InclusiveBetween(0, ValidationConstants.ITEM_STATUS_MAX_RANGE);
    }
}

public class CollectionConstructValidator : AbstractValidator<CollectionConstructCommand>
{
    public CollectionConstructValidator()
    {
        RuleFor(command => command.Name).Length(3, 250);
        RuleFor(command => command.Status).InclusiveBetween(0, ValidationConstants.ITEM_STATUS_MAX_RANGE);

        RuleFor(command => command.Items.Count()).Equal(DomainConstants.ITEMS_NUMBER_IN_COLLECTION)
            .WithMessage(command => $"Collection must have exactly 5 items (current number is {command.Items.Count()}).");
    }
}