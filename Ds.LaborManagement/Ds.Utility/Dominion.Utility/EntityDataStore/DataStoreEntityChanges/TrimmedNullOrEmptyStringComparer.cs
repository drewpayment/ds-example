using Dominion.Utility.Constants;

namespace Dominion.Utility.EntityDataStore.DataStoreEntityChanges
{
    /// <summary>
    /// Compares two strings by checking if their trimmed non-null values are equal.
    /// </summary>
    public class TrimmedNullOrEmptyStringComparer : TypedPropertyComparer<string>
    {
        /// <summary>
        /// Determines if two strings are equal after trimming their non-null forms.
        /// </summary>
        /// <param name="original">Original object to compare against.</param>
        /// <param name="proposed">New/proposed object to compare against.</param>
        /// <returns>True, if objects are determined to be equal; otherwise, false.</returns>
        public override bool AreEqual(string original, string proposed)
        {
            return (original ?? CommonConstants.EMPTY_STRING).Trim() == (proposed ?? CommonConstants.EMPTY_STRING).Trim();
        }
    }
}
