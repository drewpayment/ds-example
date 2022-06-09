using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;

namespace Dominion.Domain.Entities.AR
{
    public class ArNsfPayment : Entity<ArNsfPayment>
    {
        public virtual int      ArNsfPaymentId      { get; set; }
        public virtual int      ArDepositId         { get; set; }
        public virtual int      ClientId            { get; set; }
        public virtual int?     GenBillingHistoryId { get; set; }
        public virtual int?     ManualInvoiceId     { get; set; }
        public virtual string   InvoiceNum          { get; set; }
        public virtual DateTime PaymentDate         { get; set; }
        public virtual decimal  Amount              { get; set; }
        public virtual string   Memo                { get; set; }
        public virtual int      OriginallyPostedBy  { get; set; }
        public virtual DateTime DateCreated         { get; set; }
        public virtual int      CreatedBy           { get; set; }

        public virtual ArDeposit ArDeposit { get; set; }
        public virtual Client    Client    { get; set; }
    }
}
