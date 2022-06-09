using Dominion.Core.Dto.Payroll;
using Dominion.Core.Dto.Performance;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Core;
using Dominion.Domain.Entities.Employee;
using Dominion.Domain.Entities.Misc;
using Dominion.Domain.Entities.PerformanceReviews;
using Dominion.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;

namespace Dominion.Domain.Entities.Payroll
{
    /// <summary>
    /// Reflects a potential change to an <see cref="Employee"/>'s salary or hourly rate.  In
    /// order to apply this change to an <see cref="Employee"/> this record must be mapped into an
    /// <see cref="Misc.EffectiveDate"/> and approved/applied from there.
    /// </summary>
    public class MeritIncrease : Entity<MeritIncrease>, IHasModifiedData
    {
        public int ProposalId { get; set; }
        public int MeritIncreaseId { get; set; }
        public int EmployeeId { get; set; }
        public PayFrequencyType PayFrequencyId { get; set; }
        public IncreaseType IncreaseType { get; set; }
        public double CurrentAmount { get; set; }
        public double IncreaseAmount { get; set; }
        public double ProposedTotalAmount { get; set; }
        /// <summary>
        /// When this is null we are dealing with a salary increase.  Otherwise this is an hourly increase.
        /// </summary>
        public int? EmployeeClientRateId { get; set; }
        public int? EffectiveDateId { get; set; }
        public DateTime? ApplyMeritIncreaseOn { get; set; }
        public ApprovalStatus ApprovalStatusID { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime Modified { get; set; }
    

        public virtual Employee.Employee Employee { get; set; }
        public virtual PayFrequency PayFrequency { get; set; }
        public virtual EmployeeClientRate EmployeeClientRate { get; set; } 
        public virtual EffectiveDate EffectiveDate { get; set; }
        public virtual Proposal Proposal { get; set; }
        public virtual User.User ModifiedByUser { get; set; }
    }
}
