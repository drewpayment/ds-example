using System;
using System.Linq.Expressions;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;

namespace Dominion.Domain.Entities.Payroll
{
    public class CheckStubConfiguration : Entity<CheckStubConfiguration>
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

        public virtual Client Client { get; set; }

        public CheckStubConfiguration()
        {
        }

        #region Filter

        /// <summary>
        /// Specification predicate selecting the CheckStubConfigurations for the given client.
        /// </summary>
        /// <param name="clientId">Client ID to filter by.</param>
        /// <returns></returns>
        public static Expression<Func<CheckStubConfiguration, bool>> ForClient(int clientId)
        {
            return x => x.ClientId == clientId;
        }

        #endregion
    }
}