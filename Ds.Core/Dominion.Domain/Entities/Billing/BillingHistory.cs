using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Misc;
using Dominion.Domain.Entities.AR;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Payroll;

namespace Dominion.Domain.Entities.Billing
{
    public partial class BillingHistory : Entity<BillingHistory>
    {
        public virtual int                   BillingHistoryId  { get; set; } 
        public virtual int                   PayrollHistoryId  { get; set; } 
        public virtual decimal               TotalAmount       { get; set; } 
        public virtual DateTime              InvoiceDate       { get; set; } 
        public virtual string                InvoiceNumber     { get; set; } 
        public virtual DateTime?             InvoiceSentDate   { get; set; } 
        public virtual bool                  IsPaid            { get; set; }
        public virtual int                   ClientId          { get; set; }
        public virtual DominionVendorOption? DominionVendorOpt { get; set; }

        //REVERSE NAVIGATION
        public virtual ICollection<BillingHistoryDetail> BillingHistoryDetails { get; set; } // many-to-one;
        public virtual ICollection<ArPayment> ArPayments { get; set; }

        //FOREIGN KEYS
        public virtual PayrollHistory PayrollHistory { get; set; } 
        public virtual Client Client { get; set; }
    }
}
