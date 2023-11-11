namespace eShopApp.MessageBroker.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class GenericTypeExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetGenericTypeName(this Type type)
        {
            string typeName;

            if(type.IsGenericType)
            {
                var genericTypes = string.Join(
                    ",", 
                    type.GetGenericArguments()
                        .Select(x => x.Name)
                        .ToArray());

                typeName = $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";
            }
            else
            {
                typeName = type.Name;
            }

            return typeName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="object"></param>
        /// <returns></returns>
        public static string GetGenericTypeName(this object @object)
        {
            return @object.GetType()
                .GetGenericTypeName();
        }
    }
}
