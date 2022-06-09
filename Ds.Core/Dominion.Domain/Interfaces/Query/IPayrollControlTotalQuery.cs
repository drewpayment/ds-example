using Dominion.Core.Dto.Payroll;
using Dominion.Domain.Entities.Payroll;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IPayrollControlTotalQuery : IQuery<PayrollControlTotal, IPayrollControlTotalQuery>
    {
        IPayrollControlTotalQuery ByPayrollId(int payrollId);

        IPayrollControlTotalQuery ByClientId(int clientId);
    }
}
