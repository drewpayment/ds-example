using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Utility.Validation
{
    /// <summary>
    /// Indication of the reason that validation is being performed.
    /// </summary>
    /// <remarks>
    /// This is implemented as bit flags that can be ORed together.
    /// </remarks>
    [Flags]
    public enum ValidationReason
    {
        Default = 1, 
        Insert = 2, 
        Update = 4
    }
}
