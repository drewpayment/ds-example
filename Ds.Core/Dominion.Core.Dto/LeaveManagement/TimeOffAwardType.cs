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
    /// see dbo.LeaveManagementPendingAwardType
    /// </remarks>
    public enum TimeOffAwardType
    {
        Award               = 1,
        CarryOver           = 2,
        CarryOverExpiration = 3,
        Adjustment          = 4
    }
}
