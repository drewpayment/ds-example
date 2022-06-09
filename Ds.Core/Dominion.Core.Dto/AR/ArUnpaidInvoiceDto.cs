using System;

namespace Dominion.Core.Dto.AR
{
    public class ArUnpaidInvoiceDto
    {
        public int      ClientId           { get; set; }
        public int      InvoiceId          { get; set; }
        public string   InvoiceNumber      { get; set; }
        public DateTime InvoiceDate        { get; set; }
        public decimal  TotalAmount        { get; set; }
        public decimal  PreviousPaidAmount { get; set; }
    }
}
