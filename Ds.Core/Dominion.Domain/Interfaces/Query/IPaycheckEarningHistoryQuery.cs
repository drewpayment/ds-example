using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Payroll;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IPaycheckEarningHistoryQuery : IQuery<PaycheckEarningHistory, IPaycheckEarningHistoryQuery>
    {
        IPaycheckEarningHistoryQuery ByClientId(int clientId);
        IPaycheckEarningHistoryQuery ByEarningId(int earningId);
        IPaycheckEarningHistoryQuery ByEarnings(List<int> earnings);
        IPaycheckEarningHistoryQuery ByYear(int year);
        IPaycheckEarningHistoryQuery ByEmployeeId(int year);
        IPaycheckEarningHistoryQuery ByEmployeeIds(IEnumerable<int> employeeIds);
        IPaycheckEarningHistoryQuery ByDateRange(DateTime? startDate, DateTime? endDate);
        IPaycheckEarningHistoryQuery ByRegularPay();
        IPaycheckEarningHistoryQuery ByBasePay();
    }
}
