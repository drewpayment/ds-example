namespace Dominion.Core.Dto.AR
{
    public class ArManualInvoiceDetailDto
    {
        public int ArManualInvoiceDetailId { get; set; }
        public int ArManualInvoiceId { get; set; }
        public decimal Amount { get; set; }
        public string ItemDescription { get; set; }
        public string ItemCode { get; set; }
        public int? BillingYear { get; set; }
    }
}
