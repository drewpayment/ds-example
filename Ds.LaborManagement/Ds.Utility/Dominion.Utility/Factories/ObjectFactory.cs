using System.Linq;
using Dominion.Utility.Containers;

namespace Dominion.Utility.Factories
{
    /// <summary>
    /// Methods for building generic object with specific rules.
    /// </summary>
    public static class ObjectFactory
    {
        /// <summary>
        /// Create an instance of any object that has a public default CTOR.
        /// Then set all of the string properties to string.empty using reflection.
        /// </summary>
        /// <typeparam name="T">Any object that has a public default CTOR.</typeparam>
        /// <returns>The object with all of the string properties set to string.empty.</returns>
        public static T SetAllStringPropertiesToEmpty<T>() where T : class, new()
        {
            var obj = new T();
            return obj.SetAllStringPropertiesToEmpty<T>();
        }

        /// <summary>
        /// Set all of the string properties to string.empty using reflection.
        /// </summary>
        /// <typeparam name="T">Any object.</typeparam>
        /// <returns>The object with all of the string properties set to string.empty.</returns>
        public static T SetAllStringPropertiesToEmpty<T>(this T obj) where T : class
        {
            foreach (var prop in typeof (T).GetProperties())
            {
                if (prop.PropertyType == typeof (string))
                {
                    prop.SetValue(obj, string.Empty);
                }
            }

            return obj;
        }


        /// <summary>
        /// Sets all string properties of the specified object to string.Empty EXCEPT those specified in the given PropertySelector.
        /// </summary>
        /// <typeparam name="T">Type of object to set string properties on.</typeparam>
        /// <param name="obj">Object to set string properties on.</param>
        /// <param name="propertiesToExclude">List of properties to exclude from setting to string.Empty.</param>
        /// <returns>The original object with the string properties initialized to string.Empty.</returns>
        public static T SetNullStringPropertiesToEmptyExcluding<T>(this T obj, PropertyList<T> propertiesToExclude)
            where T : class
        {
            foreach (var prop in typeof (T).GetProperties())
            {
                if (prop.PropertyType == typeof (string)
                    && (propertiesToExclude == null
                        || !propertiesToExclude.Any(x => x.GetPropertyInfo().Name == prop.Name)))
                {
                    if (prop.GetValue(obj) == null)
                        prop.SetValue(obj, string.Empty);
                }
            }

            return obj;
        }
    }
}