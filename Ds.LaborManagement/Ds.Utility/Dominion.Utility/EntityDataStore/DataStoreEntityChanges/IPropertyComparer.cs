namespace Dominion.Utility.EntityDataStore.DataStoreEntityChanges
{
    /// <summary>
    /// Object capable of performing a comparison of two objects.
    /// </summary>
    public interface IPropertyComparer
    {
        /// <summary>
        /// Determines if two objects are equal.
        /// </summary>
        /// <param name="original">Original object to compare against.</param>
        /// <param name="proposed">New/proposed object to compare against.</param>
        /// <returns>True, if objects are determined to be equal; otherwise, false.</returns>
        bool AreEqual(object original, object proposed);
    }

    /// <summary>
    /// Object capable of performing a comparison of two objects of the given type.
    /// </summary>
    /// <typeparam name="T">Type of object to compare.</typeparam>
    public interface IPropertyComparer<T>
    {
        /// <summary>
        /// Determines if two objects are equal.
        /// </summary>
        /// <param name="original">Original object to compare against.</param>
        /// <param name="proposed">New/proposed object to compare against.</param>
        /// <returns>True, if objects are determined to be equal; otherwise, false.</returns>
        bool AreEqual(T original, T proposed);
    }
}
