using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Payroll
{
    public class CheckStubConfigurationDto
    {
        public virtual int CheckStubConfigurationId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual int CheckStubTaxableGrossId { get; set; }
        public virtual int CheckStubStraightPremiumId { get; set; }
        public virtual int CheckStubDatesId { get; set; }
        public virtual int CheckStubNetPayId { get; set; }
        public virtual byte PrintRoutingAccount { get; set; }
        public virtual bool IsPrintRateChange { get; set; }
        public virtual bool IsPrintSocialSecurityNumber { get; set; }
        public virtual int CheckStubVoidDaysId { get; set; }
        public virtual bool IsPrintSsLastFourDigitsOnly { get; set; }
        public virtual bool? IsPunchDetail { get; set; }
        public virtual string SpecialCheckHeader { get; set; }
        public virtual bool IsTipsInGross { get; set; }
        public virtual bool IsEarningsDetail { get; set; }
        public virtual bool IsPrintPointBal { get; set; }
        public virtual bool IsShowCheckDateInWindow { get; set; }
        public virtual bool IsLastPayPrintedAtEnd { get; set; }
        public virtual bool IsDeductionShowOnlyCurrent { get; set; }
        public virtual bool IsShowLifetimeHours { get; set; }
        public virtual bool IsMaskBankInfo { get; set; }
        public virtual bool CombineCompanyEarnings { get; set; }
    }
}
