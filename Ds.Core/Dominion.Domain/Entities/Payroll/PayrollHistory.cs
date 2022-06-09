using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Payroll;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Billing;
using Dominion.Domain.Entities.Clients;

namespace Dominion.Domain.Entities.Payroll
{
    /// <summary>
    /// Entity representation of the dbo.genPayrollHistory table.
    /// </summary>
    public partial class PayrollHistory : Entity<PayrollHistory>
    {
        public virtual int             PayrollHistoryId { get; set; } 
        public virtual int             PayrollId        { get; set; } 
        public virtual DateTime        CheckDate        { get; set; } 
        public virtual DateTime        PeriodStartDate  { get; set; } 
        public virtual DateTime        PeriodEndDate    { get; set; } 
        public virtual string          BankAccount      { get; set; } 
        public virtual int?            BankId           { get; set; } 
        public virtual string          AltBankAccount   { get; set; } 
        public virtual int?            AltBankId        { get; set; } 
        public virtual string          TaxAccount       { get; set; } 
        public virtual int?            TaxBankId        { get; set; } 
        public virtual string          DebitAccount     { get; set; } 
        public virtual int?            DebitBankId      { get; set; } 
        public virtual DateTime        Modified         { get; set; } 
        public virtual int             ModifiedBy       { get; set; } 
        public virtual int             ClientId         { get; set; } 
        public virtual PayrollRunType? PayrollRunTypeId { get; set; } 

        //REVERSE NAVIGATION
        public virtual ICollection<BillingHistory> BillingHistory { get; set; } // many-to-one;
        public virtual ICollection<PaycheckHistory> PaycheckHistory { get; set; }
        public virtual ICollection<VendorPaymentHistory> VendorPaymentHistories { get; set; }
        public virtual Client Client { get; set; }
        public virtual Payroll Payroll { get; set; }
        //add VendorHistory
    }
}
