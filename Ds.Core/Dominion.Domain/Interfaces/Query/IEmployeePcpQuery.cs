using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Benefit;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IEmployeePcpQuery : IQuery<EmployeePcp, IEmployeePcpQuery>
    {
        IEmployeePcpQuery ByEmployeeId(int employeeId);
    }
}
