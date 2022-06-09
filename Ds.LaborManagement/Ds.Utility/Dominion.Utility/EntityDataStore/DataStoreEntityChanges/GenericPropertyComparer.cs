using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Utility.EntityDataStore.DataStoreEntityChanges
{
    /// <summary>
    /// Creates a comparer which uses the provided rule to compare two objects.
    /// </summary>
    /// <typeparam name="T">Type to compare.</typeparam>
    public class GenericPropertyComparer<T> : TypedPropertyComparer<T>
    {
        private readonly Func<T, T, bool> _rule;
 
        /// <summary>
        /// Initializes a new <see cref="GenericPropertyComparer{T}"/> using the given comparison rule.
        /// </summary>
        /// <param name="rule">Comparison rule to use to compare two objects.</param>
        public GenericPropertyComparer(Func<T, T, bool> rule)
        {
            this._rule = rule;
        }

        public override bool AreEqual(T original, T proposed)
        {
            return this._rule(original, proposed);
        }
    }
}
