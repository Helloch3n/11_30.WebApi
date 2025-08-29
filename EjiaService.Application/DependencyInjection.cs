using EjiaService.Application.Events;
using EjiaService.Application.Handler;
using EjiaService.Application.Interface;
using EjiaService.Application.Services;
using EventBus.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjiaService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // 1 注册事件处理器
            services.AddTransient<ForgotPasswordRequestedEventHandler>();
            services.AddScoped<IEjiaMessagePush, EJiaMessagePush>();
            return services;
        }


        public static void AddApplicationEventBusSubscriptions(this IEventBus eventBus)
        {
            eventBus.Subscribe<ForgotPasswordRequestedEvent, ForgotPasswordRequestedEventHandler>();
            // 其他订阅放在这里
        }
    }
}
