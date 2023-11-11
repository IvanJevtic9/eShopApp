namespace eShopApp.MessageBroker.Models
{
    public class SubscriptionInfo
    {
        public bool IsDynamic { get; init; }

        public Type HandlerType { get; init; }

        internal SubscriptionInfo(bool isDynamic, Type handlerType)
        {
            IsDynamic = isDynamic;
            HandlerType = handlerType;
        }

        internal static SubscriptionInfo Dynamic(Type handlerType) =>
            new SubscriptionInfo(true, handlerType);

        internal static SubscriptionInfo Typed(Type handlerType) => 
            new SubscriptionInfo(false, handlerType);
    }
}
