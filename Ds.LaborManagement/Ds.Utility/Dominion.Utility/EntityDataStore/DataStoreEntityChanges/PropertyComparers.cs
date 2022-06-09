using System;
using System.Collections.Generic;

namespace Dominion.Utility.EntityDataStore.DataStoreEntityChanges
{
    /// <summary>
    /// Provides static access to <see cref="IPropertyComparer"/>s.
    /// </summary>
    public static class PropertyComparers
    {
        private static readonly Dictionary<Type, IPropertyComparer> Comparers = new Dictionary<Type, IPropertyComparer>(); 

        /// <summary>
        /// The default <see cref="IPropertyComparer"/> to use in object comparisons (see 
        /// <see cref="PropertyChangeComparator{T}"/>).
        /// </summary>
        public static IPropertyComparer DefaultComparer
        {
            get { return Get<BasicPropertyComparer>(); }
        }

        /// <summary>
        /// Gets the statically defined instance of the requested <see cref="IPropertyComparer"/> type.
        /// </summary>
        /// <typeparam name="TComparer">Type of comparer to get the static instance of.</typeparam>
        /// <returns></returns>
        public static TComparer Get<TComparer>() 
            where TComparer : IPropertyComparer, new()
        {
            IPropertyComparer comparer;
            Comparers.TryGetValue(typeof(TComparer), out comparer);

            if (comparer == null)
            {
                comparer = new TComparer();
                Comparers.Add(typeof(TComparer), comparer);
            }

            return (TComparer)comparer;
        }

        /// <summary>
        /// Creates a new <see cref="IPropertyComparer"/> which compares two objects using the provided rule.
        /// </summary>
        /// <typeparam name="T">Type being compared.</typeparam>
        /// <param name="comparisonRule">The rule to use to compare two objects of the given type.</param>
        /// <returns></returns>
        public static IPropertyComparer Create<T>(Func<T, T, bool> comparisonRule)
        {
           return new GenericPropertyComparer<T>(comparisonRule);
        } 
    }
}
