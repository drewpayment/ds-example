using Dominion.Domain.Entities.Payroll;
using Dominion.Utility.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Interfaces.Query.Payroll
{
    public interface IPayrollEmployeeOverrideQuery : IQuery<PayrollEmployeeOverride, IPayrollEmployeeOverrideQuery>
    {
        IPayrollEmployeeOverrideQuery ByClientShiftIDs(List<int?> clientShiftIds);
        IPayrollEmployeeOverrideQuery ByClientDepartmentIDs(List<int?> clientDepartmentIds);
    }
}
