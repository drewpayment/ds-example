using Dominion.Domain.Entities.Employee;
using Dominion.Utility.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IEmployeePersonalInfoQuery : IQuery<EmployeePersonalInfo, IEmployeePersonalInfoQuery>
    {
        IEmployeePersonalInfoQuery ByEmployeeId(int employeeId);
    }
}
