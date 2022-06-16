using App.Metrics;
using App.Metrics.Counter;

namespace ItemManagementService.Infrastructure.Metrics;

public static class MetricsRegistry
{
    public static CounterOptions DbCallsCounter => new CounterOptions
    {
        Name = "Success Calls",
        Context = "Database",
        MeasurementUnit = Unit.Calls,
    };

    public static CounterOptions DbConnectionsCounter => new CounterOptions
    {
        Name = "Open Connections",
        Context = "Database",
        MeasurementUnit = Unit.Connections,
    };
}