using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Payroll
{
    public partial class VendorPaymentHistory
    {
        public virtual int GenVendorPaymentHistoryId { get; set; }
        public virtual int GenPayrollHistoryId       { get; set; }
        public virtual int? ClientVendorId           { get; set; }
        public virtual int? TaxVendorId              { get; set; }
        public virtual int? DominionVendorId         { get; set; }
        public virtual decimal PaymentAmount         { get; set; }
        public virtual DateTime? LiabilityDate       { get; set; }
        public virtual int? CheckNumber              { get; set; }
        public virtual int? AccountType              { get; set; }
        public virtual string AccountNumber          { get; set; }
        public virtual string RoutingNumber          { get; set; }
        public virtual string TraceNumber            { get; set; }
        public virtual byte? CalendarDebitId         { get; set; }
        public virtual bool? IsAdjustment            { get; set; }
        public virtual int ClientId                  { get; set; }
        public virtual DateTime Modified             { get; set; }
        public virtual string ModifiedBy             { get; set; }

        public virtual PayrollHistory PayrollHistory { get; set; }

        public VendorPaymentHistory()
        {

        }

    }
}
