using FluentValidation;
using Items.Api.Commands.CollectionItem;
using Items.Application.Contracts;

namespace Items.Application.Validators;

public class CollectionItemAddValidator : IconExistenceBaseValidator<CollectionItemAddCommand>
{
    public CollectionItemAddValidator(IUnitOfWork uow) : base(uow)
    {
        RuleFor(collectionItem => collectionItem.Name).Length(3, 250);

        RuleFor(x => x.CollectionId)
            .MustAsync(async (iconId, ct) => (await uow.CollectionRepository!.GetById(iconId, ct)) != null)
            .WithMessage((x, id) => $"Collection with id [{id}] doesn't exist.");
    }
}

public class CollectionItemUpdateValidator : IconExistenceBaseValidator<CollectionItemUpdateCommand>
{
    public CollectionItemUpdateValidator(IUnitOfWork uow) : base(uow)
    {
        RuleFor(collectionItem => collectionItem.Id).NotEmpty().NotEqual(Guid.Empty);
        RuleFor(collectionItem => collectionItem.Name).Length(3, 250);

        RuleFor(x => x.CollectionId)
            .MustAsync(async (iconId, ct) => (await uow.CollectionRepository!.GetById(iconId, ct)) != null)
            .WithMessage((x, id) => $"Collection with id [{id}] doesn't exist.");
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

public class CollectionItemUpdateIconValidator : IconExistenceBaseValidator<CollectionItemUpdateIconCommand>
{
    public CollectionItemUpdateIconValidator(IUnitOfWork uow) : base(uow)
    {
        RuleFor(collectionItem => collectionItem.Id).NotEmpty().NotEqual(Guid.Empty);
    }
}

public class CollectionItemUpdateCollectionValidator : AbstractValidator<CollectionItemUpdateCollectionCommand>
{
    public CollectionItemUpdateCollectionValidator(IUnitOfWork uow)
    {
        RuleFor(collectionItem => collectionItem.Id).NotEmpty().NotEqual(Guid.Empty);
        RuleFor(x => x.CollectionId)
            .MustAsync(async (collectionId, ct) => (await uow.CollectionRepository!.GetById(collectionId, ct)) != null)
            .WithMessage((x, id) => $"Collection with id [{id}] doesn't exist.");
    }
}