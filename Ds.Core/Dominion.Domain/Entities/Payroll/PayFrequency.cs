using Dominion.Core.Dto.Payroll;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Payroll
{
    /// <summary>
    /// Details regarding a particular pay frequency (eg: Weekly, Bi-Weekly, Monthly, etc).
    /// </summary>
    public class PayFrequency : Entity<PayFrequency>
    {
        public virtual PayFrequencyType PayFrequencyId               { get; set; } 
        public virtual string           Name                         { get; set; } 
        public virtual int              AnnualPayPeriodCount         { get; set; } 
        public virtual string           Code                         { get; set; } 
        public virtual string           CalendarFrequencyDescription { get; set; } 
        public virtual string           Abbreviation                 { get; set; }
        public virtual bool             IsPayrollOption              { get; set; }
    }
}
