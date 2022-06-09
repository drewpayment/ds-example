using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Utility.EntityDataStore.DataStoreEntityChanges
{
    public class BasicPropertyComparer : IPropertyComparer
    {
        public bool AreEqual(object original, object proposed)
        {
            if(original == null && proposed == null)
                return true;

            return original != null && proposed != null && original.Equals(proposed);
        }
    }
}
