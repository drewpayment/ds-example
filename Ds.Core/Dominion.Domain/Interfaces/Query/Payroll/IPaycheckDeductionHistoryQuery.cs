using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Payroll;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Payroll
{
    public interface IPaycheckDeductionHistoryQuery : IQuery<PaycheckDeductionHistory, IPaycheckDeductionHistoryQuery>
    {
        IPaycheckDeductionHistoryQuery ByIsMemoDeduction();
        IPaycheckDeductionHistoryQuery ByPayrollId(int payrollId);
        IPaycheckDeductionHistoryQuery ByClientId(int clientId);
        IPaycheckDeductionHistoryQuery ByPaycheckPayDataHistoryIds(IEnumerable<int> paycheckPayDataHistoryIds);
        IPaycheckDeductionHistoryQuery IsDirectDeposit();
        IPaycheckDeductionHistoryQuery ByDateRange(DateTime? startDate, DateTime? endDate);
    }
}
