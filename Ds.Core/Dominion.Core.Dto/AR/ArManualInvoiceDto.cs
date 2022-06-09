using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Misc;

namespace Dominion.Core.Dto.AR
{
    public class ArManualInvoiceDto
    {
        public int                  ArManualInvoiceId { get; set; }
        public int                  ClientId          { get; set; }
        public string               InvoiceNum        { get; set; }
        public DateTime             InvoiceDate       { get; set; }
        public decimal              TotalAmount       { get; set; }
        public int                  PostedBy          { get; set; }
        public bool                 IsPaid            { get; set; }
        public DominionVendorOption? DominionVendorOpt { get; set; }
        public string InvoiceNumAndDate => $"{InvoiceNum} - {InvoiceDate:MM/dd/yy}";

        public ICollection<ArManualInvoiceDetailDto> ManualInvoiceDetails { get; set; }
    }
}
