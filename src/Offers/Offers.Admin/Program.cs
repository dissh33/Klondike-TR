using FluentValidation.AspNetCore;
using Offers.Admin.Filters;
using Offers.Application;
using Offers.Infrastructure;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using Serilog;


#region HostBuilder

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.WithMachineName()
    .Enrich.WithEnvironmentName()
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3} {EnvironmentName}] {Message}{NewLine}{Exception}")
    .WriteTo.Seq(builder.Configuration["Seq"] ?? string.Empty)
    .CreateLogger();

builder.Host.UseSerilog(Log.Logger);

#endregion


#region Add services

builder.Services.RegisterInfrastructureServices();
builder.Services.RegisterApplicationServices(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddControllers(options => options.Filters.Add<ApiExceptionFilterAttribute>());
//builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

builder.Services.AddOpenTelemetry()
                .ConfigureResource(resource =>
                {
                    resource.AddService(serviceName: builder.Environment.ApplicationName);
                })
                .WithMetrics(metrics =>
                {
                    metrics.AddAspNetCoreInstrumentation();
                    metrics.AddPrometheusExporter();
                });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
app.MapPrometheusScrapingEndpoint();

#endregion

app.Run();
