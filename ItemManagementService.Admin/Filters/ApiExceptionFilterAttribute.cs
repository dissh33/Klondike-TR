using ItemManagementService.Application.RequestsLogic.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ItemManagementService.Admin.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

    public ApiExceptionFilterAttribute()
    {
        _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
        {
            { typeof(RequestValidationException), HandleValidationException },
        };
    }

    public override void OnException(ExceptionContext context)
    {
        HandleException(context);

        base.OnException(context);
    }

    private void HandleException(ExceptionContext context)
    {
        Type type = context.Exception.GetType();
        _exceptionHandlers[type].Invoke(context);
    }

    private void HandleValidationException(ExceptionContext context)
    {
        var exception = (RequestValidationException)context.Exception;

        var details = new ValidationProblemDetails(exception.Errors)
        {
            Type = "Bad Request"
        };

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;
    }
}