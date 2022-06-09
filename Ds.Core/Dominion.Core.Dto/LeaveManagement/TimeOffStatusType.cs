using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.LeaveManagement
{
    /// <summary>
    /// Types of leave management change statuses.
    /// </summary>
    /// <remarks>
    /// see dbo.RequestTimeOffStatus
    /// </remarks>
    public enum TimeOffStatusType
    {
        Pending   = 1,
        Approved  = 2,
        Declined  = 3,
        Cancelled = 4,
    }
}
