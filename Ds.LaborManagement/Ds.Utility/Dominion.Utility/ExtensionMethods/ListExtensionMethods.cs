using System.Collections.Generic;

namespace Dominion.Utility.ExtensionMethods
{
    public static class ListExtensionMethods
    {
        /// <summary>
        /// Convienence method for instanciating then adding all in one line.
        /// </summary>
        /// <typeparam name="T">Object type stored in list.</typeparam>
        /// <param name="list">The list instance.</param>
        /// <param name="objectToAdd">The object you want to add.</param>
        /// <returns>Instance of the list passed in.</returns>
        public static List<T> ChainAdd<T>(this List<T> list, T objectToAdd)
        {
            list.Add(objectToAdd);
            return list;
        }

        /// <summary>
        /// Convienence method for instanciating then adding all in one line.
        /// </summary>
        /// <typeparam name="T">Object type stored in list.</typeparam>
        /// <param name="list">The list instance.</param>
        /// <param name="objectToAdd">The objects you want to add.</param>
        /// <returns>Instance of the list passed in.</returns>
        public static List<T> ChainAddRange<T>(this List<T> list, IEnumerable<T> rangeToAdd)
        {
            list.AddRange(rangeToAdd);
            return list;
        }
    }
}