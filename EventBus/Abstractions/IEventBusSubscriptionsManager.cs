using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Abstractions
{
    /// <summary>
    /// 订阅管理器
    /// </summary>
    public interface IEventBusSubscriptionsManager
    {
        bool IsEmpty { get; }

        /// <summary>
        /// 添加订阅
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TH"></typeparam>
        void AddSubscription<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;

        /// <summary>
        /// 移除订阅
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TH"></typeparam>
        void RemoveSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        bool HasSubscriptionsForEvent(string eventName);

        IEnumerable<Type> GetHandlersForEvent(string eventName);

        Type? GetEventTypeByName(string eventName);

        string GetEventKey<T>();

        void Clear();
    }
}
