using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Payroll;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Payroll
{
    public interface IVendorPaymentHistoryQuery : IQuery<VendorPaymentHistory, IVendorPaymentHistoryQuery>
    {
        IVendorPaymentHistoryQuery ByPayrollId(int payrollId);
        IVendorPaymentHistoryQuery ByClientId(int clientId);
        IVendorPaymentHistoryQuery IsAdjustment(bool isAdjustment);
        IVendorPaymentHistoryQuery ByVendorPaymentHistoryId(int vendorPaymentHistoryId);
        IVendorPaymentHistoryQuery ByClientVendorId(int clientVendorId);
        IVendorPaymentHistoryQuery ByTaxVendorId(int taxVendorId);
        IVendorPaymentHistoryQuery ByDominionVendorId(int dominionVendorId);
        IVendorPaymentHistoryQuery ByDateRange(DateTime? startDate, DateTime? endDate);

    }
}
