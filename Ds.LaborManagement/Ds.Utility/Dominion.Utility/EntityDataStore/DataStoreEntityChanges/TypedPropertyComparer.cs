namespace Dominion.Utility.EntityDataStore.DataStoreEntityChanges
{
    /// <summary>
    /// Base class used to construct a comparer which implements both the generic and non-generic form of an
    /// <see cref="IPropertyComparer"/>.
    /// </summary>
    /// <typeparam name="T">Type of object being compared.</typeparam>
    public abstract class TypedPropertyComparer<T> : IPropertyComparer<T>, IPropertyComparer
    {
        /// <summary>
        /// Determines if two objects are equal.
        /// </summary>
        /// <param name="original">Original object to compare against.</param>
        /// <param name="proposed">New/proposed object to compare against.</param>
        /// <returns>True, if objects are determined to be equal; otherwise, false.</returns>
        public abstract bool AreEqual(T original, T proposed);

        /// <summary>
        /// Determines if two objects are equal.
        /// </summary>
        /// <param name="original">Original object to compare against.</param>
        /// <param name="proposed">New/proposed object to compare against.</param>
        /// <returns>True, if objects are determined to be equal; otherwise, false.</returns>
        bool IPropertyComparer.AreEqual(object original, object proposed)
        {
            return this.AreEqual((T)original, (T)proposed);
        }
    }
}
