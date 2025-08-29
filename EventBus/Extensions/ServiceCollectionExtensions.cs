using EventBus.Abstractions;
using EventBus.Implementation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEventBusRabbitMQ(this IServiceCollection services, Action<EventBusOptions>? configureOptions = null)
        {
            var options = new EventBusOptions();
            configureOptions?.Invoke(options);

            // RabbitMQ connection factory
            services.AddSingleton(sp =>
            {
                var factory = new ConnectionFactory()
                {
                    HostName = options.HostName,
                    Port = options.Port,
                    UserName = options.UserName,
                    Password = options.Password,
                    DispatchConsumersAsync = true, // 使用异步消费者
                    RequestedHeartbeat = TimeSpan.FromSeconds(30), // 心跳间隔
                    AutomaticRecoveryEnabled = true // 自动重连
                };
                return factory;
            });

            // 持久连接
            services.AddSingleton<DefaultRabbitMQPersistentConnection>(sp =>
            {
                var factory = sp.GetRequiredService<ConnectionFactory>();
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();
                return new DefaultRabbitMQPersistentConnection(factory, logger);
            });

            // 订阅管理器（内存）
            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            // EventBus 自身
            services.AddSingleton<IEventBus>(sp =>
            {
                var persistentConnection = sp.GetRequiredService<DefaultRabbitMQPersistentConnection>();
                var subsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                return new EventBusRabbitMQ(persistentConnection, subsManager, sp, logger, options);
            });

            return services;
        }
    }
}
