using System;
using Dominion.Utility.Constants;

namespace Dominion.Utility.ExtensionMethods
{
    public static class EntityExtensionMethods
    {
        /// <summary>
        /// Checks if the given object is NEW or EXISTING based on it's ID value. 
        /// Compares the ID to the <see cref="CommonConstants.NEW_ENTITY_ID"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">Any object that has an integer based ID to check (e.g. Entity, DTO, etc).</param>
        /// <param name="idPropertySelector">ID lamba expression (e.g. x => x.MyId)</param>
        /// <returns></returns>
        public static bool IsNewEntity<T>(this T obj, Func<T, int> idPropertySelector) where T : class
        {
            return idPropertySelector(obj) <= CommonConstants.NEW_ENTITY_ID;
        }
    }
}
