using System.Diagnostics;
using Dapper;
using Serilog;
using Serilog.Events;

namespace ItemManagementService.Infrastructure.Logging;

public static class LogEventsDefinitions
{
    public static async Task<T> DbCall<T>(this ILogger logger, Func<Task<T>> action, CommandDefinition cmd)
    {
        if (logger.IsEnabled(LogEventLevel.Information))
        {
            logger.Information("\n\t Executing Sql-command \n\t {@sql}\n\t with parameters \n\t {@parameters}", cmd.CommandText, cmd.Parameters);

            var timer = Stopwatch.StartNew();

            var result = await action();
            logger.Information("Sql-command executed in {time}.", timer.Elapsed);
            return result;
        }

        return await action();
    }
}