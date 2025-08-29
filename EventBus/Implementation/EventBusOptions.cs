using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Implementation
{
    public record EventBusOptions
    {
        public string ExchangeName { get; set; } = "event_bus_exchange";
        public string QueueName { get; set; } = ""; // 可在微服务中设置具体队列名
        public string HostName { get; set; } = "localhost";
        public int Port { get; set; } = 5672;
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";
        public bool DurableExchange { get; set; } = true;
    }
}
