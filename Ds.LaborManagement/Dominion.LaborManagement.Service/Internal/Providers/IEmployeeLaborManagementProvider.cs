namespace Dominion.LaborManagement.Service.Internal.Providers
{
    using Utility.OpResult;
    using Dto.EmployeeLaborManagement;
    using Dominion.Core.Dto.Labor;
    using System.Collections.Generic;

    public interface IEmployeeLaborManagementProvider
    {
        IOpResult<bool> CanAssignEmployeePinToEmployee(ClockEmployeeSetupDto dto);
        IOpResult<IEnumerable<ClockClientTimePolicyDtos.ClockClientTimePolicyDto>> UpdateGeofenceOnTimePolicy(IEnumerable<ClockClientTimePolicyDtos.ClockClientTimePolicyDto> dto);
    }
}
