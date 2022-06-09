using System;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Payroll
{
    public partial class ClientMatch : Entity<ClientMatch>
    {
        public virtual int ClientMatchId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual string Description { get; set; }
        public virtual int? DeductionInId { get; set; }
        public virtual int DeductionOutId { get; set; }
        public virtual double Amount { get; set; }
        public virtual byte AmountTypeId { get; set; }
        public virtual double Maximum { get; set; }
        public virtual byte MaximumTypeId { get; set; }
        public virtual byte MinimumAge { get; set; }
        public virtual short WaitingPeriod { get; set; }
        public virtual double YtdMax { get; set; }
        public virtual bool IsAllowMatchWhenZero { get; set; }
        public virtual int? ClientDeductionId { get; set; }
        public virtual double Amount2 { get; set; }
        public virtual double Maximum2 { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual bool IsMaxAcrossMatch { get; set; }
        public virtual bool IsPartTimeEmployees { get; set; }
        public virtual bool IsFullTimeEmployees { get; set; }
        public virtual bool IsSalaryEmployees { get; set; }
        public virtual bool IsHourlyEmployees { get; set; }
        public virtual bool? IsUseEnrollmentDates { get; set; }
        public virtual double MinimumContribution { get; set; }
        public virtual byte MinimumContributionTypeId { get; set; }
        public virtual double Minimum { get; set; }
        public virtual byte MinimumTypeId { get; set; }
        public virtual bool IsUseCheckDate { get; set; }
        public virtual decimal Amount3 { get; set; }
        public virtual decimal Maximum3 { get; set; }

        // Foreign Keys
        public virtual ClientDeduction ClientDeductionIn { get; set; }
        public virtual ClientDeduction ClientDeductionOut { get; set; }
    }
}
