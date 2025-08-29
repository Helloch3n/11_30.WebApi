using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Abstractions
{
    /// <summary>
    /// 定义事件拦截器
    /// </summary>
    /// <typeparam name="TEvent">事件类</typeparam>
    public interface IIntegrationEventHandler<in TEvent> where TEvent : IntegrationEvent
    {
        Task Handler(TEvent @event);
    }
}
