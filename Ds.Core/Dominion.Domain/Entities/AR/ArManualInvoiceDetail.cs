using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.AR
{
    public class ArManualInvoiceDetail : Entity<ArManualInvoiceDetail>
    {
        public virtual int     ArManualInvoiceDetailId { get; set; }
        public virtual int     ArManualInvoiceId       { get; set; }
        public virtual decimal Amount                  { get; set; }
        public virtual string  ItemDescription         { get; set; }
        public virtual string  ItemCode                { get; set; }
        public virtual int?    BillingYear             { get; set; }

        public virtual ArManualInvoice ArManualInvoice { get; set; }
    }
}
