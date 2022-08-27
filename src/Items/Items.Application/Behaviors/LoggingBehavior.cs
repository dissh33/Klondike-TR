using MediatR.Pipeline;
using Serilog;

namespace Items.Application.Behaviors;

public class LoggingBehavior<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger _logger;

    public LoggingBehavior(ILogger logger)
    {
        _logger = logger;
    }

    public Task Process(TRequest request, CancellationToken ct)
    {
        var requestName = typeof(TRequest).Name;

        _logger.Information("Item Management Admin Request!");
        return Task.CompletedTask;
    }
}