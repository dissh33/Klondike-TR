using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;


var serviceProvider = CreateServices();

using var scope = serviceProvider.CreateScope();
UpdateDatabase(scope.ServiceProvider);

IServiceProvider CreateServices()
{
    const string connectionString = "User ID=postgres;Password=dissh33$;Host=localhost;Port=5432;Database=klondike_trade_dev;"; //TODO: move to config

    return new ServiceCollection()
        .AddFluentMigratorCore()
        .ConfigureRunner(migration => migration
            .AddPostgres()
            .WithGlobalConnectionString(connectionString)
            .ScanIn(typeof(Program).Assembly).For.Migrations())
        .AddLogging(logging => logging.AddFluentMigratorConsole())
        .BuildServiceProvider(false);
}

void UpdateDatabase(IServiceProvider provider)
{
    var runner = provider.GetRequiredService<IMigrationRunner>();
    
    runner.ListMigrations();
}
