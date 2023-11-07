namespace eShopApp.Basket.Domain.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Cart
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        public ICollection<CartItem> Items { get; set; }
    }
}
