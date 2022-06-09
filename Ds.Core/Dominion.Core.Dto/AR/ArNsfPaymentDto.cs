using System;

namespace Dominion.Core.Dto.AR
{
    public class ArNsfPaymentDto
    {
        public int      ArNsfPaymentId      { get; set; }
        public int      ArDepositId         { get; set; }
        public int      ClientId            { get; set; }
        public int?     GenBillingHistoryId { get; set; }
        public int?     ManualInvoiceId     { get; set; }
        public string   InvoiceNum          { get; set; }
        public DateTime PaymentDate         { get; set; }
        public decimal  Amount              { get; set; }
        public string   Memo                { get; set; }
        public int      OriginallyPostedBy  { get; set; }
        public DateTime DateCreated         { get; set; }
        public int      CreatedBy           { get; set; }
    }
}
