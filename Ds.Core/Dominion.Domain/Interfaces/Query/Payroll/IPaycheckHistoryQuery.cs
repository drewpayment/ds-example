using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Payroll;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Payroll
{
    public interface IPaycheckHistoryQuery : IQuery<PaycheckHistory, IPaycheckHistoryQuery>
    {
        IPaycheckHistoryQuery ByEmployeeId(int employeeId);
        IPaycheckHistoryQuery ByEmployeeIds(IEnumerable<int> employeeIds);
        IPaycheckHistoryQuery ByClientId(int clientId);
        IPaycheckHistoryQuery ByDateRange(DateTime? startDate, DateTime? endDate);
        IPaycheckHistoryQuery ByIsVoid(bool isVoid = true);
        IPaycheckHistoryQuery ShowCheckAfter();
        IPaycheckHistoryQuery ByIncludeAdjustments(bool isAdjustment = true);
        IPaycheckHistoryQuery ByPaycheckHistoryId(int paycheckHistoryId);
        IPaycheckHistoryQuery ByPaycheckHistoryIds(IEnumerable<int> paycheckHistoryIds);
        IPaycheckHistoryQuery ByPayrollId(int payrollId);
        IPaycheckHistoryQuery ByPayrollIds(IEnumerable<int> payrollIds);
        IPaycheckHistoryQuery ByGenPayrollHistoryId(int payrollId);
        IPaycheckHistoryQuery OrderByCheckDate();
        IPaycheckHistoryQuery ByIs1099Exempt(bool is1099Exempt = true);
        IPaycheckHistoryQuery OrderByPayrollHistoryCheckDate();
        IPaycheckHistoryQuery ByPayrollHistoryCheckDateYear(DateTime? checkDate);
        IPaycheckHistoryQuery ByThirdPartySickPay();
    }
}
