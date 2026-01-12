using System.Diagnostics;
using Dapper;
using Items.Infrastructure.Metrics;
using Serilog;
using Serilog.Events;

namespace Items.Infrastructure.Logging;

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

    public static async Task<T> DbCall<T>(this ILogger logger, Func<Task<T>> query, CommandDefinition cmd, DbMetrics metrics)
    {
        metrics.DbCallsCounter.Add(1);

        if (!logger.IsEnabled(LogEventLevel.Information)) return await query();

        logger.Information("\n\t Executing Sql-query \n\t {sql}\n\t with parameters \n\t {parameters}", cmd.CommandText, cmd.Parameters);

        var timer = Stopwatch.StartNew();

        var result = await query();

        logger.Information("Sql-query executed in {time} ({ms} ms)", timer.Elapsed, timer.Elapsed.TotalMilliseconds);

        return result;
    }
}