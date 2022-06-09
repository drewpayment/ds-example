using System;

namespace Dominion.Core.Dto.AR
{
    public class ArPaymentDto
    {
        public int       ArPaymentId         { get; set; }
        public int       ClientId            { get; set; }
        public string ClientCode { get; set; }
        public string ClientName { get; set; }
        public int?      GenBillingHistoryId { get; set; }
        public int?      ManualInvoiceId     { get; set; }
        public int?      ArDepositId         { get; set; }
        public string    InvoiceNum          { get; set; }
        public DateTime InvoiceDate { get; set;}
        public DateTime  PaymentDate         { get; set; }
        public decimal   Amount              { get; set; }
        public string    Memo                { get; set; }
        public bool      IsCredit            { get; set; }
        public int       PostedBy            { get; set; }
        public bool      IsNsf               { get; set; }
        public int?      MarkedNsfBy         { get; set; }
        public DateTime? MarkedNsfDate       { get; set; }
    }
}
