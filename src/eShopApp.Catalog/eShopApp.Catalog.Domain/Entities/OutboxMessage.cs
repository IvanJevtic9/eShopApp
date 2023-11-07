using eShopApp.Shared.DDAbstraction;

namespace eShopApp.Catalog.Domain.Entities
{
    public sealed class OutboxMessage : Entity
    {
        public OutboxMessage() : base(Guid.NewGuid())
        { }

        public OutboxMessage(Guid Id) : base(Id) 
        { }

        public string Type { get; set; } = string.Empty; // JSON content
        public string Content { get; set; } = string.Empty; // JSON content
        public DateTime OccuredOnUtc { get; set; }
        public DateTime? ProcessedOnUtc { get; set; }
        public string Error { get; set; } = string.Empty;
    }
}
