using System;

namespace Dominion.Core.Dto.AR
{
    public class ClientCheckPaymentDto
    {
        public decimal          InvoiceAmount      { get; set; }
        public DateTime         InvoiceDate        { get; set; }
        public int              InvoiceId          { get; set; }
        public bool             IsManualInvoice    { get; set; }
        public string           InvoiceNumber      { get; set; }
        public int              ClientId           { get; set; }
        public string           ClientCode         { get; set; }
        public string           ClientName         { get; set; }
        public decimal          PrevPaidAmount     { get; set; }
        public decimal          PaymentAmount      { get; set; }
        public DateTime         PaymentDate        { get; set; }
        public PaymentTypeEnum? PaymentType        { get; set; }
        public string           Memo               { get; set; }
        public bool             IsCredit           { get; set; }
        public bool             IsPaid             { get; set; }
    }

    public enum PaymentTypeEnum
    {
        Full = 0,
        Partial = 1
    }
}
