using System.Diagnostics.Metrics;

namespace Items.Infrastructure.Metrics;

public class DbMetrics(IMeterFactory meterFactory)
{
    private readonly Meter _meter = meterFactory.Create("Items.Database");

    public Counter<int> DbCallsCounter => _meter.CreateCounter<int>("items.database.calls", unit: "call");
    public Counter<int> DbConnectionsCounter => _meter.CreateCounter<int>("items.database.open_connections", unit: "connection");
}