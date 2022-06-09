using Dominion.Domain.Entities.Payroll;
using Dominion.Utility.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IPayrollEmailLogQuery : IQuery<PayrollEmailLog, IPayrollEmailLogQuery>
    {
        IPayrollEmailLogQuery ByClientId(int clientId);
        IPayrollEmailLogQuery ByPayrollId(int payrollId);
    }
}
