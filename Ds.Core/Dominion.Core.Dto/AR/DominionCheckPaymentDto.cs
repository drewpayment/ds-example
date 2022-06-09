using System;

namespace Dominion.Core.Dto.AR
{
    public class DominionCheckPaymentDto
    {
        public int ClientId { get; set; }
        public string ClientCode { get; set; }
        public int InvoiceId { get; set; }
        public string InvoiceNumber    { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int? CheckNumber { get; set; }
        public DateTime? CheckDate { get; set; }
        public decimal TotalAmount   { get; set; }
        public bool IsPaid { get; set; }
        public bool IsManualInvoice { get; set; }
        public int ArPaymentId { get; set; }
    }
}
