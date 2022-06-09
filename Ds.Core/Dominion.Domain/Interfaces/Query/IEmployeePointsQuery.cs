using Dominion.Domain.Entities.Employee;
using Dominion.Utility.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IEmployeePointsQuery : IQuery<EmployeePoints, IEmployeePointsQuery>
    {
        IEmployeePointsQuery ByEmployeeId(int employeeId);

        IEmployeePointsQuery ByClientId(int clientId);

        IEmployeePointsQuery ByEmployeeIds(IEnumerable<int> empIds);

        IEmployeePointsQuery ByExpireDate(DateTime endDate);
        IEmployeePointsQuery ByDate(DateTime startDate, DateTime endDate);
    }
}
