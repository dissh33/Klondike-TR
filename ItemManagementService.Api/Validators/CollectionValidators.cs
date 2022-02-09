using FluentValidation;
using ItemManagementService.Api.Commands.Collection;

namespace ItemManagementService.Api.Validators;

public class CollectionAddValidator : AbstractValidator<CollectionAddCommand>
{
    public CollectionAddValidator()
    {

    }
}

public class CollectionUpdateValidator : AbstractValidator<CollectionUpdateCommand>
{
    public CollectionUpdateValidator()
    {

    }
}