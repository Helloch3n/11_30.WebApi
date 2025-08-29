using EventBus.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EventBus.Implementation
{
    public class EventBusRabbitMQ : IEventBus
    {
        private readonly DefaultRabbitMQPersistentConnection _persistentConnection;
        private readonly IEventBusSubscriptionsManager _subsManager;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<EventBusRabbitMQ> _logger;
        private readonly EventBusOptions _options;
        private IModel? _consumerChannel;
        private bool _consumerStarted;

        public EventBusRabbitMQ(DefaultRabbitMQPersistentConnection persistentConnection,
                                IEventBusSubscriptionsManager subsManager,
                                IServiceProvider serviceProvider,
                                ILogger<EventBusRabbitMQ> logger,
                                EventBusOptions options)
        {
            _persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
            _subsManager = subsManager ?? throw new ArgumentNullException(nameof(subsManager));
            _serviceProvider = serviceProvider;
            _logger = logger;
            _options = options;
            TryCreateConsumerChannel();
        }

        public void Dispose()
        {
            _consumerChannel?.Dispose();
        }

        /// <summary>
        /// RbbitMQ消息发布
        /// </summary>
        /// <param name="event"></param>
        public void Publish(IntegrationEvent @event)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }
            //创建连接
            using var channel = _persistentConnection.CreateModel();
            //事件名
            var eventName = @event.GetType().Name;
            //声明交换机
            channel.ExchangeDeclare(exchange: _options.ExchangeName, type: ExchangeType.Direct, durable: _options.DurableExchange);
            //创建消息
            var message = JsonSerializer.Serialize(@event, @event.GetType());
            var body = Encoding.UTF8.GetBytes(message);
            //发送消息
            var properties = channel.CreateBasicProperties();
            properties.DeliveryMode = 2; // persistent

            channel.BasicPublish(
                exchange: _options.ExchangeName,
                routingKey: eventName,
                basicProperties: properties,
                body: body);

            _logger.LogInformation("Published event to exchange {exchange}: {eventName} ({id})", _options.ExchangeName, eventName, @event.Id);
        }

        /// <summary>
        /// RbbitMQ订阅
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TH"></typeparam>
        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = _subsManager.GetEventKey<T>();

            //添加订阅关系到内存
            _subsManager.AddSubscription<T, TH>();

            // 确保队列绑定到 exchange
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            using var channel = _persistentConnection.CreateModel();

            channel.ExchangeDeclare(exchange: _options.ExchangeName, type: ExchangeType.Direct, durable: _options.DurableExchange);

            var queueName = GetOrCreateQueueName();

            // 当队列不存在时，声明队列（持久、非独占、非自动删除）
            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            // 对于 fanout exchange：绑定交换机到队列（无 routing key）
            channel.QueueBind(queue: queueName, exchange: _options.ExchangeName, routingKey: eventName);
            //幂等启动消费者
            StartBasicConsume();
        }

        /// <summary>
        /// RabbitMQ取消订阅
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TH"></typeparam>
        public void UnSubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = _subsManager.GetEventKey<T>();
            _subsManager.RemoveSubscription<T, TH>();
            // 如果没有处理器，则可以选择取消队列绑定或删除队列（这里保持队列存在，便于手动操作）
            if (!_subsManager.HasSubscriptionsForEvent(eventName))
            {
                if (!_persistentConnection.IsConnected)
                {
                    _persistentConnection.TryConnect();
                }
                using var channel = _persistentConnection.CreateModel();
                var queueName = GetOrCreateQueueName();

                try
                {
                    channel.QueueUnbind(queue: queueName, exchange: _options.ExchangeName, routingKey: eventName, arguments: null);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "解除订阅失败{ExchangeName} {eventName} {queueName}", _options.ExchangeName, eventName, queueName);
                }
            }
            _logger.LogInformation("Removed subscription for event {eventName}", _subsManager.GetEventKey<T>());
        }

        /// <summary>
        /// 创建RabbitMQ消费者连接
        /// </summary>
        private void TryCreateConsumerChannel()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            _consumerChannel?.Dispose();
            _consumerChannel = _persistentConnection.CreateModel();
            _consumerChannel.ExchangeDeclare(exchange: _options.ExchangeName, type: ExchangeType.Direct, durable: _options.DurableExchange);

            var queueName = GetOrCreateQueueName();
            _consumerChannel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            //_consumerChannel.QueueBind(queue: queueName, exchange: _options.ExchangeName, routingKey: "");
            _consumerChannel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            _consumerChannel.CallbackException += (sender, ea) =>
            {
                _logger.LogWarning(ea.Exception, "Recreating consumer channel due to exception");
                TryCreateConsumerChannel();
                if (_consumerStarted) StartBasicConsume();
            };
        }

        /// <summary>
        /// 从配置获取队列名
        /// </summary>
        /// <returns></returns>
        private string GetOrCreateQueueName()
        {
            // 如果用户在 options 里指定了队列名就用它，否则自动生成以服务为单位的队列名（每个微服务独占队列能避免重复消费）
            if (!string.IsNullOrWhiteSpace(_options.QueueName))
                return _options.QueueName;
            // 生成队列名：eventbus_queue_{machine}_{processid}
            var generated = $"eventbus_queue_{Environment.MachineName}_{Environment.ProcessId}";
            return generated;
        }

        /// <summary>
        /// 启动RabbitMQ消费者
        /// </summary>
        private void StartBasicConsume()
        {
            if (_consumerChannel == null)
                TryCreateConsumerChannel();

            if (_consumerStarted)
                return;//幂等：避免重复启动多个消费者

            var queueName = GetOrCreateQueueName();
            var consumer = new AsyncEventingBasicConsumer(_consumerChannel!);

            consumer.Received += async (model, ea) =>
            {
                var eventName = ea.RoutingKey;
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());

                try
                {
                    await ProcessEvent(eventName, message);
                    // 确认消息
                    _consumerChannel!.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message: {message}", message);
                    // 处理失败：拒绝并不重入队列（可以根据需要改成 requeue: true）
                    _consumerChannel!.BasicNack(ea.DeliveryTag, multiple: false, requeue: false);
                }
            };

            _consumerChannel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
            _logger.LogInformation("Started consuming queue {queueName}", queueName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task ProcessEvent(string eventName, string message)
        {
            // 尝试确定具体事件类型。优先使用订阅管理器中已知的事件类型：
            var eventType = _subsManager.GetEventTypeByName(eventName);
            if (eventType == null)
            {
                _logger.LogWarning("Event type could not be detected for message: {message}", message);
                return;
            }

            var integrationEvent = JsonSerializer.Deserialize(message, eventType);
            if (integrationEvent == null)
            {
                _logger.LogWarning("Deserialized event is null for type {type}", eventName);
                return;
            }

            //获取handler
            var handlers = _subsManager.GetHandlersForEvent(eventName);
            if (handlers == null) return;

            foreach (var handlerType in handlers)
            {
                using var scope = _serviceProvider.CreateScope();
                var handler = scope.ServiceProvider.GetService(handlerType);
                if (handler == null) continue;

                //调用handler
                var method = handlerType.GetMethod("Handler");
                if (method == null) continue;

                var task = (Task?)method.Invoke(handler, new[] { integrationEvent });
                if (task != null)
                {
                    await task;
                }
            }
        }
    }
}
