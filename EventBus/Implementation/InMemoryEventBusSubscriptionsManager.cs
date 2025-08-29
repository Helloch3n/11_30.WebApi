using EventBus.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Implementation
{
    public class InMemoryEventBusSubscriptionsManager : IEventBusSubscriptionsManager
    {

        // eventName -> handlers
        private readonly ConcurrentDictionary<string, List<Type>> _handlers;
        // eventName -> eventType
        private readonly ConcurrentDictionary<string, Type> _eventTypes;

        public InMemoryEventBusSubscriptionsManager()
        {
            _handlers = new ConcurrentDictionary<string, List<Type>>();
            _eventTypes = new ConcurrentDictionary<string, Type>();
        }

        public bool IsEmpty => _handlers.IsEmpty;

        /// <summary>
        /// 添加订阅
        /// </summary>
        /// <typeparam name="T">Event</typeparam>
        /// <typeparam name="TH">Handler</typeparam>
        public void AddSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {

            //获取事件名
            var eventName = GetEventKey<T>();
            //添加event，handler绑定
            _handlers.AddOrUpdate(eventName,
                _ => new List<Type> { typeof(TH) },
                (_, list) =>
                {
                    if (!list.Contains(typeof(TH)))
                        list.Add(typeof(TH));
                    return list;
                });

            //事件名，事件类型
            _eventTypes.TryAdd(eventName, typeof(T));
        }

        /// <summary>
        /// 清空订阅
        /// </summary>
        public void Clear()
        {
            _handlers.Clear();
            _eventTypes.Clear();
        }

        /// <summary>
        /// 获取EventName
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public string GetEventKey<T>()
        {
            return typeof(T).Name;
        }

        /// <summary>
        /// 获取EventType
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public Type? GetEventTypeByName(string eventName)
        {
            if (_eventTypes.TryGetValue(eventName, out var type))
                return type;
            return null;
        }

        /// <summary>
        /// 根据Event获取Handler
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public IEnumerable<Type> GetHandlersForEvent(string eventName)
        {
            if (_handlers.TryGetValue(eventName, out var handlers))
                return handlers;
            return Enumerable.Empty<Type>();
        }

        /// <summary>
        /// 判断Event是否被订阅
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public bool HasSubscriptionsForEvent(string eventName)
        {
            return _handlers.ContainsKey(eventName);
        }

        /// <summary>
        /// 移除订阅
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TH"></typeparam>
        public void RemoveSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = GetEventKey<T>();

            if (_handlers.TryGetValue(eventName, out var handlers))
            {
                handlers.Remove(typeof(TH));
                if (handlers.Count == 0)
                {
                    _handlers.TryRemove(eventName, out _);
                    _eventTypes.TryRemove(eventName, out _);
                }
            }
        }

        public IEnumerable<Type> GetAllEventTypes()
        {
            return _eventTypes.Values.Distinct();
        }
    }
}
