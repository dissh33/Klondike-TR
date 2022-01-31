using FluentValidation.AspNetCore;
using ItemManagementService.Admin.Filters;
using ItemManagementService.Api;
using ItemManagementService.Application;
using ItemManagementService.Infrastructure;
using Serilog;


#region HostBuilder

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog(Log.Logger);
#endregion


#region Add services

builder.Services.RegisterInfrastructureServices();

builder.Services.RegisterApplicationServices();

builder.Services.RegisterRequestValidators();

builder.Services.AddControllers(options => options.Filters.Add<ApiExceptionFilterAttribute>())
                .AddFluentValidation(options => options.AutomaticValidationEnabled = false);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#endregion


var app = builder.Build();


#region Configure pipeline


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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