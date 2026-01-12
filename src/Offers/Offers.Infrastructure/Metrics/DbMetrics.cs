using System.Diagnostics.Metrics;

namespace Offers.Infrastructure.Metrics;

public class DbMetrics(IMeterFactory meterFactory)
{
    private readonly Meter _meter = meterFactory.Create("Offers.Database");

    public Counter<int> DbCallsCounter => _meter.CreateCounter<int>("offers.database.calls", unit: "call");
    public Counter<int> DbConnectionsCounter => _meter.CreateCounter<int>("offers.database.open_connections", unit: "connection");
}