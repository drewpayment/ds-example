using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.TimeCard
{
    public enum ApprovalStatusType
    {
        /// <summary>
        ///     ''' Defines that days should not be filtered based on approval status.
        ///     ''' </summary>
        All = 1,
        /// <summary>
        ///     ''' Defines that only days that are already approved should appear.
        ///     ''' </summary>
        Approved = 2,
        /// <summary>
        ///     ''' Defines that only days that are not approved should appear.
        ///     ''' </summary>
        NotApproved = 3
    }
}
