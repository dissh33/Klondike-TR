using FluentValidation;
using Items.Api.Commands.Collection;
using Items.Application.Contracts;
using Items.Domain;

namespace Items.Application.Validators;

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

public class ConstructCollectionValidator : AbstractValidator<CollectionConstructCommand>
{
    public ConstructCollectionValidator()
    {
        RuleFor(command => command.Name).Length(3, 250);
        RuleFor(command => command.Status).InclusiveBetween(0, 2);

        RuleFor(command => command.Items.Count()).Equal(Constants.COLLECTION_ITEM_NUMBER)
            .WithMessage(command => $"Collection must have exactly 5 items (current number is {command.Items.Count()}).");
    }
}