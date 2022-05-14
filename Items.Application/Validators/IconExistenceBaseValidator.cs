using FluentValidation;
using ItemManagementService.Api.Commands;
using ItemManagementService.Application.Contracts;
using MediatR;

namespace ItemManagementService.Application.Validators;

public class IconExistenceBaseValidator<T> : AbstractValidator<T> where T : IHaveIcon
{
    public IconExistenceBaseValidator(IUnitOfWork uow)
    {
        RuleFor(x => x.IconId)
            .MustAsync(async (iconId, ct) => (await uow.IconRepository!.GetById(iconId, ct)) != null)
            .WithMessage((x, id) => $"Icon with id [{id}] doesn't exist.");
    }
}