using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Misc;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;

namespace Dominion.Domain.Entities.AR
{
    public class ArManualInvoice : Entity<ArManualInvoice>
    {
        public virtual int                   ArManualInvoiceId { get; set; }
        public virtual int                   ClientId          { get; set; }
        public virtual string                InvoiceNum        { get; set; }
        public virtual DateTime              InvoiceDate       { get; set; }
        public virtual decimal               TotalAmount       { get; set; }
        public virtual int                   PostedBy          { get; set; }
        public virtual bool                  IsPaid            { get; set; }
        public virtual DominionVendorOption? DominionVendorOpt { get; set; }

        public virtual Client Client { get; set; }
        public virtual ICollection<ArManualInvoiceDetail> ArManualInvoiceDetails { get; set; }
        public virtual ICollection<ArPayment> ArPayments { get; set; }
    }
}
