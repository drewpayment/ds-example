using Dominion.Core.Dto.Employee;
using Dominion.Domain.Entities.Employee;
using Dominion.Domain.Entities.Tax;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Query on a <see cref="FilingStatusInfo"/> data source.
    /// </summary>
    public interface IEmployeeStatusQuery : IQuery<EmployeeStatus, IEmployeeStatusQuery>
    {
        IQueryResult<EmployeePayAndEmployeeStatusAndEmployee> JoinEmployeePayEmployeeStatusAndEmployee(
            IEmployeePayQuery empPayQuery,
            IEmployeeQuery empQuery, int clientId);

        IQuery<EmployeeStatus> ByActive(bool isActive);

        IQueryResult<EmployeePayAndEmployeeStatusAndEmployeeWithReason> JoinEmployeePayEmployeeStatusAndEmployeeAndReason(
            IEmployeePayQuery empPayQuery,
            IEmployeeQuery empQuery, int clientId);
    }
}
