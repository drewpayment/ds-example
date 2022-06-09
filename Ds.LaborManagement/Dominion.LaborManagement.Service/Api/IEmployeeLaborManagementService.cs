using System.Collections.Generic;
using Dominion.Core.Dto.Labor;
using Dominion.LaborManagement.Dto.Clock;
using Dominion.LaborManagement.Dto.EmployeeLaborManagement;
using Dominion.Utility.OpResult;

namespace Dominion.LaborManagement.Service.Api
{
    public interface IEmployeeLaborManagementService
    {
        /// <summary>
        /// Get a list of time policies for display in a list.
        /// Basic data for selection purposes.
        /// Returns based on the currently selected client.
        /// </summary>
        /// <returns></returns>
        IOpResult<IEnumerable<ClockClientTimePolicyDtos.ClockClientTimePolicyListDto>> GetClockClientTimePolicyList(int? clientId);

        IOpResult<ClockEmployeeSetupDto> GetClockEmployeeSetup(int employeeId, bool filterInactive = true);
        IOpResult<IEnumerable<EmployeeTimePolicyInfoDto>> GetAvailableTimePoliciesForEmployee(int employeeId);

        IOpResult<IEnumerable<EmployeeScheduleSetupDto>> GetAvailableCompanySchedules(int clientId,
            bool activeOnly = true);

        IOpResult<IEnumerable<EmployeeCostCenterSetupDto>> GetAvailableCostCentersForEmployee(int employeeId, bool filterInactive = true);

        IOpResult<ClockEmployeeSetupDto> SaveClockEmployee(ClockEmployeeSetupDto dto);

        /// <summary>
        /// Checks the badge id and employee pin input to see if they are available.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>boolean</returns>
        IOpResult<bool> CanAssignEmployeePinToEmployee(ClockEmployeeSetupDto dto);
        IOpResult<int> GetIpadPinLength();

        IOpResult<ClockEmployeeDto> VerifyPin(string pin);
        IOpResult<IEnumerable<ClockClientTimePolicyDtos.ClockClientTimePolicyDto>> UpdateGeofenceOnTimePolicy(IEnumerable<ClockClientTimePolicyDtos.ClockClientTimePolicyDto> dto);

    }
}
