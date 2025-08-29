using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Implementation
{
    public class DefaultRabbitMQPersistentConnection : IDisposable
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly ILogger<DefaultRabbitMQPersistentConnection> _logger;
        private IConnection? _connection;
        private bool _disposed;

        public DefaultRabbitMQPersistentConnection(IConnectionFactory connectionFactory, ILogger<DefaultRabbitMQPersistentConnection> logger)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _logger = logger;
        }

        /// <summary>
        /// 判断是否已连接
        /// </summary>
        public bool IsConnected => _connection != null && _connection.IsOpen && !_disposed;

        /// <summary>
        /// 创建连接
        /// </summary>
        /// <returns></returns>
        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                TryConnect();
            }

            return _connection!.CreateModel();
        }

        /// <summary>
        /// 重连
        /// </summary>
        public void TryConnect()
        {
            _logger.LogInformation("RabbitMQ Client is trying to connect");

            if (IsConnected) return;

            try
            {
                _connection = _connectionFactory.CreateConnection();
            }
            catch (BrokerUnreachableException ex)
            {
                _logger.LogError(ex, "RabbitMQ broker unreachable");
                throw;
            }
            catch (SocketException ex)
            {
                _logger.LogError(ex, "Socket exception when connecting RabbitMQ");
                throw;
            }

            if (IsConnected)
            {
                _logger.LogInformation("RabbitMQ persistent connection acquired a connection");
                _connection.ConnectionShutdown += OnConnectionShutdown;
            }
            else
            {
                _logger.LogCritical("FATAL: RabbitMQ connections could not be created and opened");
            }
        }

        private void OnConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            _logger.LogWarning("RabbitMQ connection shutdown. Initiating reconnect...");
            // 尝试重连（简单策略：立即尝试）
            TryConnect();
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            try
            {
                _connection?.Dispose();
            }
            catch
            {
                // ignore
            }
        }
    }
}
