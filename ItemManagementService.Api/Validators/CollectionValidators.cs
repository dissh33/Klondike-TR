using FluentValidation;
using ItemManagementService.Api.Commands.Collection;

namespace ItemManagementService.Api.Validators;

public class CollectionAddValidator : AbstractValidator<CollectionAddCommand>
{
    public CollectionAddValidator()
    {
        RuleFor(collection => collection.Name).Length(3, 250);
        RuleFor(collection => collection.Status).InclusiveBetween(0, 2);
    }
}

public class CollectionUpdateValidator : AbstractValidator<CollectionUpdateCommand>
{
    public CollectionUpdateValidator()
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

public class CollectionUpdateIconValidator : AbstractValidator<CollectionUpdateIconCommand>
{
    public CollectionUpdateIconValidator()
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