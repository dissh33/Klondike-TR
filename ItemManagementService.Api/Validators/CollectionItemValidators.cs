using FluentValidation;
using ItemManagementService.Api.Commands.CollectionItem;

namespace ItemManagementService.Api.Validators;

public class CollectionItemAddValidator : AbstractValidator<CollectionItemAddCommand>
{
    public CollectionItemAddValidator()
    {
        RuleFor(collectionItem => collectionItem.Name).Length(3, 250);
    }
}

public class CollectionItemUpdateValidator : AbstractValidator<CollectionItemUpdateCommand>
{
    public CollectionItemUpdateValidator()
    {
        RuleFor(collectionItem => collectionItem.Id).NotEmpty().NotEqual(Guid.Empty);
        RuleFor(collectionItem => collectionItem.Name).Length(3, 250);
    }
}

public class CollectionItemUpdateNameValidator : AbstractValidator<CollectionItemUpdateNameCommand>
{
    public CollectionItemUpdateNameValidator()
    {
        RuleFor(collectionItem => collectionItem.Id).NotEmpty().NotEqual(Guid.Empty);
        RuleFor(collectionItem => collectionItem.Name).Length(3, 250);
    }
}

public class CollectionItemUpdateIconValidator : AbstractValidator<CollectionItemUpdateIconCommand>
{
    public CollectionItemUpdateIconValidator()
    {
        RuleFor(collectionItem => collectionItem.Id).NotEmpty().NotEqual(Guid.Empty);
    }
}

public class CollectionItemUpdateCollectionValidator : AbstractValidator<CollectionItemUpdateCollectionCommand>
{
    public CollectionItemUpdateCollectionValidator()
    {
        RuleFor(collectionItem => collectionItem.Id).NotEmpty().NotEqual(Guid.Empty);
    }
}