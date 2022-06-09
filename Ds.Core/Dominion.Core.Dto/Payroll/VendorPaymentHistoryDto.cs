using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Payroll
{
    public partial class VendorPaymentHistoryDto
    {
        public int GenVendorPaymentHistoryId { get; set; }
        public int GenPayrollHistoryId       { get; set; }
        public int? ClientVendorId           { get; set; }
        public int? TaxVendorId              { get; set; }
        public int? DominionVendorId         { get; set; }
        public decimal PaymentAmount         { get; set; }
        public DateTime? LiabilityDate       { get; set; }
        public int? CheckNumber              { get; set; }
        public int? AccountType              { get; set; }
        public string AccountNumber          { get; set; }
        public string RoutingNumber          { get; set; }
        public string TraceNumber            { get; set; }
        public byte? CalendarDebitId         { get; set; }
        public bool? IsAdjustment            { get; set; }
        public int ClientId                  { get; set; }
        public DateTime Modified             { get; set; }
        public string ModifiedBy             { get; set; }
        public DateTime? CheckDate { get; set; }
    }
}
