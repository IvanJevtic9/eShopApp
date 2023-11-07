using eShopApp.Shared.DDAbstraction.Base;

namespace eShopApp.Shared.DDAbstraction
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AggregateRoot : Entity
    {
        private readonly List<IDomainEvent> _domainEvents;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        protected AggregateRoot(Guid id) : base(id)
        {
            _domainEvents = new();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainEvent"></param>
        protected void RaiseDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearDomainEvents() => _domainEvents.Clear();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents.ToList();
    }
}
