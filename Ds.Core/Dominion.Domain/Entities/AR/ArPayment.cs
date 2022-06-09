using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Billing;
using Dominion.Domain.Entities.Clients;

namespace Dominion.Domain.Entities.AR
{
    public class ArPayment : Entity<ArPayment>
    {
        public virtual int ArPaymentId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual int? GenBillingHistoryId { get; set; }
        public virtual int? ManualInvoiceId { get; set; }
        public virtual int? ArDepositId { get; set; }
        public virtual string InvoiceNum { get; set; }
        public virtual DateTime PaymentDate { get; set; }
        public virtual decimal Amount { get; set; }
        public virtual string Memo { get; set; }
        public virtual bool IsCredit { get; set; }
        public virtual int PostedBy { get; set; }
        public virtual bool IsNsf { get; set; }
        public virtual int? MarkedNsfBy { get; set; }
        public virtual DateTime? MarkedNsfDate { get; set; }

        public virtual Client          Client          { get; set; }
        public virtual BillingHistory  BillingHistory  { get; set; }
        public virtual ArManualInvoice ArManualInvoice { get; set; }
        public virtual ArDeposit       ArDeposit       { get; set; }
    }
}
