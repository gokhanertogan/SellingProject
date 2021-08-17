using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Features.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationRegistration(this IServiceCollection services, Type startup)
        {
            var assm = Assembly.GetExecutingAssembly();
            //var assm2 = startup.GetTypeInfo().Assembly;

            services.AddAutoMapper(assm);
            services.AddMediatR(assm);
            //services.AddMediatR(assm, assm2, typeof(CreateOrderCommandHandler).GetTypeInfo().Assembly);

            return services;
        }
    }
}
