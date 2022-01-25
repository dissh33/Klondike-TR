﻿using System.Reflection;
using ItemManagementService.Application.RequestsLogic.Validation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ItemManagementService.Application
{
    public static class DependencyInjection
    {
        public static void RegisterApplicationServices(this IServiceCollection services)
        {
            var currentAssembly = Assembly.GetExecutingAssembly();

            services.AddMediatR(currentAssembly);
            services.AddAutoMapper(currentAssembly);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        }
    }
}