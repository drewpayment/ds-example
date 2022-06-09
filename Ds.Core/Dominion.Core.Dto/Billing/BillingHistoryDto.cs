using Dominion.Core.Dto.Misc;
using System;

namespace Dominion.Core.Dto.Billing
{
    public class BillingHistoryDto
    {
        public int BillingHistoryId { get; set; }
        public int PayrollHistoryId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime? InvoiceSentDate { get; set; }
        public bool IsPaid { get; set; }
        public int ClientId { get; set; }
        public DominionVendorOption? DominionVendorOpt { get; set; }
    }
}
