using App.Metrics;
using App.Metrics.Counter;

namespace Items.Infrastructure.Metrics;

public static class MetricsRegistry
{
    public static CounterOptions DbCallsCounter => new()
    {
        Name = "Success Calls",
        Context = "Database",
        MeasurementUnit = Unit.Calls,
    };

    public static CounterOptions DbConnectionsCounter => new()
    {
        Name = "Open Connections",
        Context = "Database",
        MeasurementUnit = Unit.Connections,
    };
}