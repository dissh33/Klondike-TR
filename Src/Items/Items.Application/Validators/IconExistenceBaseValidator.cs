using FluentValidation;
using Items.Api.Commands;
using Items.Application.Contracts;

namespace Items.Application.Validators;

public class IconExistenceBaseValidator<T> : AbstractValidator<T> where T : IHaveIcon
{
    public IconExistenceBaseValidator(IUnitOfWork uow)
    {
        RuleFor(x => x.IconId)
            .MustAsync(async (iconId, ct) => (await uow.IconRepository.GetById(iconId, ct)) != null)
            .WithMessage((x, id) => $"Icon with id [{id}] doesn't exist.");
    }
}