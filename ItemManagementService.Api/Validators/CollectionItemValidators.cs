using FluentValidation;
using ItemManagementService.Api.Commands.CollectionItem;

namespace ItemManagementService.Api.Validators;

public class CollectionItemAddValidator : AbstractValidator<CollectionItemAddCommand>
{
    public CollectionItemAddValidator()
    {

    }
}

public class CollectionItemUpdateValidator : AbstractValidator<CollectionItemUpdateCommand>
{
    public CollectionItemUpdateValidator()
    {

    }
}