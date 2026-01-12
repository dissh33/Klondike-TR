using FluentValidation;
using Items.Application.Exceptions;
using MediatR;

namespace Items.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        if (!_validators.Any()) return await next(ct);

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
        _validators.Select(validator => validator.ValidateAsync(context, ct)));

        var failures = validationResults
            .Where(validationResult => validationResult.Errors.Any())
            .SelectMany(validationResult => validationResult.Errors)
            .ToList();

        if (failures.Count != 0)
            throw new RequestValidationException(failures);

        return await next(ct);
    }
}