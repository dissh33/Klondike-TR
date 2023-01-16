using System.Diagnostics;
using App.Metrics;
using Dapper;
using Offers.Infrastructure.Metrics;
using Serilog;
using Serilog.Events;

namespace Offers.Infrastructure.Logging;

public static class LogEventsDefinitions
{ 
    public static async Task<T> DbCall<T>(this ILogger logger, Func<Task<T>> action, CommandDefinition cmd)
    {
        if (!logger.IsEnabled(LogEventLevel.Information)) return await action();

        logger.Information("\n\t Executing Sql-query \n\t {sql}\n\t with parameters \n\t {parameters}", cmd.CommandText, cmd.Parameters);

        var timer = Stopwatch.StartNew();

        var result = await action();

        logger.Information("Sql-query executed in {time} ({ms} ms)", timer.Elapsed, timer.Elapsed.TotalMilliseconds);
        
        return result;
    }

    public static async Task<T> DbCall<T>(this ILogger logger, Func<Task<T>> query, CommandDefinition cmd, IMetrics metrics)
    {
        if (!logger.IsEnabled(LogEventLevel.Information)) return await query();

        logger.Information("\n\t Executing Sql-query \n\t {sql}\n\t with parameters \n\t {parameters}", cmd.CommandText, cmd.Parameters);

        var timer = Stopwatch.StartNew();

        var result = await query();

        metrics.Measure.Counter.Increment(MetricsRegistry.DbCallsCounter);
        logger.Information("Sql-query executed in {time} ({ms} ms)", timer.Elapsed, timer.Elapsed.TotalMilliseconds);

        return result;
    }
}