//using MediatR.Pipeline;
//using Serilog;

//namespace ItemManagementService.Application.RequestsLogic.Behaviors;

//public class LoggingMiddleWare<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
//{
//    private readonly ILogger _logger;

//    public LoggingMiddleWare(ILogger logger)
//    {
//        _logger = logger;
//    }

//    public Task Process(TRequest request, CancellationToken ct)
//    {
//        var requestName = typeof(TRequest).Name;

//        _logger.Information("Item Management Admin Request!");
//        return Task.CompletedTask;
//    }
//}