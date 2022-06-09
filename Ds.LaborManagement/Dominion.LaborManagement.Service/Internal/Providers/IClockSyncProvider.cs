using Dominion.Core.Services.Dto.Employee;
using Dominion.Utility.OpResult;
using System.Collections.Generic;

namespace Dominion.LaborManagement.Service.Internal.Providers
{
    public interface IClockSyncProvider
    {
        IOpResult<IEnumerable<EmployeeDto>> UpdateClockEmployeesGeofence(IEnumerable<EmployeeDto> employees);
    }
}