using Dominion.Core.Dto.Performance;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Core;
using Dominion.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Payroll
{
    /// <summary>
    /// Reflects a singular potential payment to make to an <see cref="Employee.Employee"/>.  To apply this
    /// to an <see cref="Employee.Employee"/> this must be imported and processed by a <see cref="Payroll"/>
    /// </summary>
    public class OneTimeEarning : Entity<OneTimeEarning>, IHasModifiedData
    {
        public int OneTimeEarningId { get; set; }
        public int EmployeeId { get; set; }
        public IncreaseType IncreaseType { get; set; }
        public double IncreaseAmount { get; set; }
        public DateTime MayBeIncludedInPayroll { get; set; }
        public ApprovalStatus ApprovalStatusID { get; set; }
        public int ModifiedBy { get; set; }
        public User.User ModifiedByUser { get; set; }
        public DateTime Modified { get; set; }
        public int ClientEarningId { get; set; }
        /// <summary>
        /// A calculated sum based on how much the employee has been paid during a <see cref="PerformanceReviews.Review"/>'s
        /// evaluation period.  See CalculateBonusProposedTotal in the PayrollRequestProvider.
        /// </summary>
        public double ProposedTotalAmount { get; set; }
        public virtual Proposal Proposal { get; set; }
        public virtual Employee.Employee Employee { get; set; }
        public virtual ClientEarning ClientEarning { get; set; }
        /// <summary>
        /// A list of payrolls that this has been imported into.
        /// </summary>
        public virtual ICollection<OneTimeEarningPayroll> OneTimeEarningPayrolls { get; set; }
    }
}
