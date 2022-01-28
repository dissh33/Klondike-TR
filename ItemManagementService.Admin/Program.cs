using FluentValidation.AspNetCore;
using ItemManagementService.Admin.Filters;
using ItemManagementService.Api;
using ItemManagementService.Application;
using ItemManagementService.Infrastructure;
using Microsoft.AspNetCore.Mvc;


var builder = WebApplication.CreateBuilder(args);


#region Add services

builder.Services.RegisterInfrastructureServices();

builder.Services.RegisterApplicationServices();

builder.Services.RegisterRequestValidators();

builder.Services.AddControllers(options => options.Filters.Add<ApiExceptionFilterAttribute>())
                .AddFluentValidation(options => options.AutomaticValidationEnabled = false);

builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

#endregion


app.Run();
