using System.Collections.Generic;

namespace Dominion.Utility.EntityDataStore.DataStoreEntityChanges
{
    public interface IPropertyChangeComparator<T>
        where T : class
    {
        T CurrentEntity { get; }
        T ProposedEntity { get; }
        List<string> DataStoreFieldNamesToIgnore { get; set; }

        /// <summary>
        /// Allows per-field <see cref="IPropertyComparer"/>s to be defined. If a comparer does not exist for a given
        /// field, the <see cref="PropertyComparers.DefaultComparer"/> will be used.
        /// </summary>
        IDictionary<string, IPropertyComparer> FieldComparisonOverrides { get; set; }

        IEnumerable<PropertyChangeData> FindChanges();
    }
}