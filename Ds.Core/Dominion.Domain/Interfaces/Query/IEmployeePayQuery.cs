using Dominion.Domain.Entities.Employee;
using System.Collections.Generic;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IEmployeePayQuery : IEmployeePayBaseQuery<EmployeePay, IEmployeePayQuery>
    {
        IEmployeePayQuery ByClientShiftIds(IEnumerable<int?> clientShiftIds);
    }
}
