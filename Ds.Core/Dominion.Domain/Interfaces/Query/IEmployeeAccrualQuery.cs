using Dominion.Domain.Entities.Employee;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IEmployeeAccrualQuery : IQuery<EmployeeAccrual, IEmployeeAccrualQuery>
    {
        IEmployeeAccrualQuery ByEmployeeId(int employeeId);
        IEmployeeAccrualQuery ByEmployeeAccrualId(int employeeAccrualId);
        IEmployeeAccrualQuery ByEmployeeIdAndClientAccrualId(int employeeId, int clientAccrualId);
        IEmployeeAccrualQuery ByIsActive(bool isActive = true);
    }
}
