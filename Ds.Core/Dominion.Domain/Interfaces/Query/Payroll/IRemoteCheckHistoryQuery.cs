using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Payroll;
using Dominion.Utility.Query;


namespace Dominion.Domain.Interfaces.Query.Payroll
{
    public interface IRemoteCheckHistoryQuery : IQuery<RemoteCheckHistory, IRemoteCheckHistoryQuery>
    {
        IRemoteCheckHistoryQuery ByRemoteCheckHistoryId(int genRemoteCheckHistoryId);

        IRemoteCheckHistoryQuery ByPayCheckHistoryId(int paycheckHistoryId);

        IRemoteCheckHistoryQuery ByVendorPaymentHistoryId(int vendorPaymentHistoryId);

        IRemoteCheckHistoryQuery ByPayrollId(int payrollId);

        IRemoteCheckHistoryQuery ByVendorPaymentHistoryIds(IEnumerable<int> vendorPaymentHistoryIds);
    }
}
