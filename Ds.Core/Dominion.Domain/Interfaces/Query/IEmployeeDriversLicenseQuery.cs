using Dominion.Domain.Entities.Employee;
using Dominion.Utility.Query;
 
namespace Dominion.Domain.Interfaces.Query
{
    public interface IEmployeeDriversLicenseQuery : IQuery<EmployeeDriversLicense, IEmployeeDriversLicenseQuery>
    {
        IEmployeeDriversLicenseQuery ByClientId(int clientId);
        IEmployeeDriversLicenseQuery ByEmployeeId(int employeeId);
    }
}

