using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.LeaveManagement
{
    /// <summary>
    /// Types of leave management award adjustments.
    /// </summary>
    /// <remarks>
    /// See: SELECT * FROM  ServiceUnit
    /// 2014.11.07: It was decided we wouldn't support Dollars which currently is a record from the source table.
    ///     - I have left it out
    ///     - Per JudH, ScottL
    /// </remarks>
    public enum TimeOffUnitType
    {
        Hours   = 1,
        Days    = 2,
    }
}
