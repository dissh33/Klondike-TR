using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Offers.Application.Exceptions;
using ILogger = Serilog.ILogger;

namespace Offers.Admin.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly ILogger _logger;
    private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

    public ApiExceptionFilterAttribute(ILogger logger)
    {
        _logger = logger;

        _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
        {
            { typeof(RequestValidationException), HandleValidationException },
            { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException },
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

        if (_exceptionHandlers.ContainsKey(type))
        {
            _exceptionHandlers[type].Invoke(context);
            return;
        }

        if (!context.ModelState.IsValid)
        {
            HandleInvalidModelStateException(context);
            return;
        }

        HandleUnknownException(context);
    }

    private void HandleValidationException(ExceptionContext context)
    {
        var exception = (RequestValidationException)context.Exception;

        var details = new ValidationProblemDetails(exception.Errors)
        {
            Status = StatusCodes.Status400BadRequest,
        };

        context.Result = new BadRequestObjectResult(details);

        var json = new JsonResult(context.Result);

        _logger.Information("Validation failed. Errors: {result}", details.Errors);

        context.ExceptionHandled = true;
    }
    
    private void HandleUnauthorizedAccessException(ExceptionContext context)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status401Unauthorized,
            Title = "Unauthorized.",
            Type = "https://tools.ietf.org/html/rfc7235#section-3.1",  //TODO: Authorization
        };

        context.Result = new ObjectResult(details);

        context.ExceptionHandled = true;
    }
    
    private void HandleInvalidModelStateException(ExceptionContext context)
    {
        var details = new ValidationProblemDetails(context.ModelState)
        {
            Title = "Invalid Model State.",
        };

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;
    }

    private void HandleUnknownException(ExceptionContext context)
    {
        throw context.Exception;
    }
}