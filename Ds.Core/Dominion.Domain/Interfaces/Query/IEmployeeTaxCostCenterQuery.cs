using Dominion.Domain.Entities.Tax;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IEmployeeTaxCostCenterQuery : IQuery<EmployeeTaxCostCenter, IEmployeeTaxCostCenterQuery>
    {
        IEmployeeTaxCostCenterQuery ByEmployeeId(int employeeId);
        IEmployeeTaxCostCenterQuery ByEmployeeTaxId(int employeeTaxId);
        IEmployeeTaxCostCenterQuery ByClientCostCenterId(int clientCostCenterId);
    }
}
