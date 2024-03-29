using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Prometheus;
using FluentValidation.AspNetCore;
using Offers.Admin.Filters;
using Offers.Application;
using Offers.Infrastructure;
using Serilog;


#region HostBuilder

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.WithMachineName()
    .Enrich.WithEnvironmentName()
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3} {EnvironmentName}] {Message}{NewLine}{Exception}")
    .WriteTo.Seq(builder.Configuration["Seq"])
    .CreateLogger();


builder.Host.UseSerilog(Log.Logger);

builder.Host
    .UseMetricsWebTracking()
    .UseMetrics(options =>
    {
        options.EndpointOptions = endpointOptions =>
        {
            endpointOptions.MetricsTextEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter();
            endpointOptions.MetricsEndpointOutputFormatter = new MetricsPrometheusProtobufOutputFormatter();
        };
    });
#endregion


#region Add services

builder.Services.RegisterInfrastructureServices();

builder.Services.RegisterApplicationServices();

builder.Services.AddControllers();

builder.Services.AddControllers(options => options.Filters.Add<ApiExceptionFilterAttribute>())
    .AddFluentValidation(configuration => configuration.AutomaticValidationEnabled = false);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMetrics();
#endregion


var app = builder.Build();


#region Configure pipeline


app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsProduction())
{
    app.UseHsts();
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

#endregion


app.Run();
