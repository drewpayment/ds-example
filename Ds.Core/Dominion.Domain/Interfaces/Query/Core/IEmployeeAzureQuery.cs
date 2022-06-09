using System.Collections.Generic;
using Dominion.Domain.Entities.Core;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Core
{
    public interface IEmployeeAzureQuery : IQuery<EmployeeAzure, IEmployeeAzureQuery>
    {
        IEmployeeAzureQuery ByEmployee(int employeeId);

        IEmployeeAzureQuery ByEmployeeList(List<int> employeeIdList);
    }
}