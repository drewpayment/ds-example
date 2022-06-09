using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.TimeCard
{
    /// <summary>
    /// Defines a class that represents the current approval status for a day/cost center.
    /// </summary>
    public class EmployeeDayApprovalStatus
    {
        /// <summary>
        ///         ''' Whether the day/cost center can currently be approved by the current user.
        ///         ''' </summary>
        ///         ''' <returns></returns>
        public bool CanBeApproved { get; set; } = false;
        /// <summary>
        ///         ''' Whether the day/cost center can eventually be approved by the current user, but may or may not
        ///         ''' be able to currently approve due to errors such as missing punches or pending benefits.
        ///         ''' </summary>
        ///         ''' <returns></returns>
        public bool WillBeApprovable { get; set; } = false;
        /// <summary>
        ///         ''' Whether the day/cost center needs approval.
        ///         ''' </summary>
        ///         ''' <returns></returns>
        public bool NeedsApproval { get; set; } = false;
        /// <summary>
        ///         ''' Whether the day/cost center is currently approved.
        ///         ''' </summary>
        ///         ''' <returns></returns>
        public bool IsApproved { get; set; } = false;
        /// <summary>
        ///         ''' The user that approved the day/cost center.
        ///         ''' </summary>
        ///         ''' <returns></returns>
        public string ApprovingUser { get; set; } = null;
    }
}
