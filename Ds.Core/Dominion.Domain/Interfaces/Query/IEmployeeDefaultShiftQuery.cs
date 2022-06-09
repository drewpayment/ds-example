using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Labor;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IEmployeeDefaultShiftQuery : IQuery<EmployeeDefaultShift, IEmployeeDefaultShiftQuery>
    {
        IEmployeeDefaultShiftQuery ByEmployeesIds(IEnumerable<int> employeeIds);
        IEmployeeDefaultShiftQuery ByGroupScheduleId(int groupScheduleId);
        IEmployeeDefaultShiftQuery ByScheduleGroupId(int scheduleGroupId);
    }
}
