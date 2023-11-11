using eShopApp.MessageBroker.EventBus.Base;
using eShopApp.MessageBroker.Models;

namespace eShopApp.MessageBroker.EventBus.Implementation.EventBusManager
{
    /// <summary>
    /// 
    /// </summary>
    public class InMemoryEventBusManager : IEventBusManager
    {
        private readonly IList<Type> _eventTypes;
        private readonly IDictionary<string, List<SubscriptionInfo>> _handlers;

        public InMemoryEventBusManager()
        {
            _eventTypes = new List<Type>();
            _handlers = new Dictionary<string, List<SubscriptionInfo>>();
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsEmpty => _handlers is { Count: 0 };

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<string> OnEventRemoved;

        /// <summary>
        /// 
        /// </summary>
        public void Clear() => _handlers.Clear();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TH"></typeparam>
        public void AddSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = GetEventKey<T>();

            RegisterSubscription(typeof(TH), eventName, false);

            if (!_eventTypes.Contains(typeof(T)))
            {
                _eventTypes.Add(typeof(T));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TH"></typeparam>
        public void RemoveSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = GetEventKey<T>();

            var subscription = FindSubscription(eventName, typeof(TH));

            if(subscription is not null)
            {
                _handlers[eventName].Remove(subscription);
                if (!_handlers[eventName].Any())
                {
                    _handlers.Remove(eventName);
                    _eventTypes.Remove(typeof(T));

                    RaiseOnEventRemoved(eventName);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TH"></typeparam>
        /// <param name="eventName"></param>
        public void AddDynamicSubscription<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler
        {
            RegisterSubscription(typeof(TH), eventName, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TH"></typeparam>
        /// <param name="eventName"></param>
        public void RemoveDynamicSubscription<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler
        {
            var subscription = FindSubscription(eventName, typeof(TH));

            if(subscription is not null)
            {
                _handlers[eventName].Remove(subscription);
                if (_handlers[eventName].Any())
                {
                    _handlers.Remove(eventName);
                    _eventTypes.Remove(typeof(TH));

                    RaiseOnEventRemoved(eventName);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool HasSubscriptionForEvent<T>() where T : IntegrationEvent
        {
            var eventName = GetEventKey<T>();

            return HasSubscriptionForEvent(eventName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public bool HasSubscriptionForEvent(string eventName) => _handlers.ContainsKey(eventName);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public string GetEventKey<T>() => typeof(T).Name;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public Type GetEventTypeByName(string eventName) => _eventTypes.SingleOrDefault(s => s.Name == eventName);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : IntegrationEvent
        {
            var key = GetEventKey<T>();

            if (_handlers.ContainsKey(key))
            {
                return _handlers[key];
            }

            return Enumerable.Empty<SubscriptionInfo>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName)
        {
            if (_handlers.ContainsKey(eventName))
            {
                return _handlers[eventName];
            }

            return Enumerable.Empty<SubscriptionInfo>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handlerType"></param>
        /// <param name="eventName"></param>
        /// <param name="isDynamic"></param>
        /// <exception cref="ArgumentException"></exception>
        private void RegisterSubscription(Type handlerType, string eventName, bool isDynamic)
        {
            if (!HasSubscriptionForEvent(eventName))
            {
                _handlers.Add(eventName, new List<SubscriptionInfo>());
            }

            if (_handlers[eventName].Any(s => s.HandlerType == handlerType))
            {
                throw new ArgumentException($"Handler Type {handlerType.Name} already registered for {eventName}", nameof(handlerType));
            }

            if (isDynamic)
            {
                _handlers[eventName].Add(SubscriptionInfo.Dynamic(handlerType));
            }
            else
            {
                _handlers[eventName].Add(SubscriptionInfo.Typed(handlerType));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="handlerType"></param>
        /// <returns></returns>
        private SubscriptionInfo FindSubscription(string eventName, Type handlerType)
        {
            if (!HasSubscriptionForEvent(eventName))
            {
                return null;
            }

            return _handlers[eventName]
                .SingleOrDefault(s => s.HandlerType == handlerType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventName"></param>
        private void RaiseOnEventRemoved(string eventName) => OnEventRemoved.Invoke(this, eventName);
    }
}
