namespace eShopApp.Shared.DDAbstraction
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Entity : IEquatable<Entity>
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; private init; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public Entity(Guid id)
        {
            Id = id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(Entity left, Entity right)
        {
            return left is not null && 
                right is not null &&
                left.Equals(right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Entity left, Entity right)
        {
            return !(left == right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Entity other) => Equals(other as object);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;

            if(obj.GetType() != GetType())
                return false;

            if(obj is not Entity entity)
                return false;

            return entity.Id == Id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode() * 41;
        }
    }
}
