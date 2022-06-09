using Dominion.Core.Dto.Labor;
using Dominion.Domain.Entities.Employee;
using Dominion.LaborManagement.Dto.Clock;
using Dominion.LaborManagement.Dto.Clock.Misc;
using Dominion.LaborManagement.Dto.Scheduling;
using Dominion.Utility.Mapping;
using Dominion.Utility.OpResult;
using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Payroll;
using Dominion.Core.Services.Dto.Employee;
using Dominion.Core.Dto.Geofence;

namespace Dominion.LaborManagement.Service.Internal.Providers
{
    public interface IEmployeePunchProvider
    {

        IOpResult<IEnumerable<TimeClockClientDto>> GetAccessibleClients(int[] clientIds);

        IOpResult<IEnumerable<ClockEmployeePunchDto>> GetEmployeePunches(int employeeId);
        IOpResult<IEnumerable<ClockEmployeePunchDto>> GetEmployeePunchesByIdList(int employeeId, int[] clockEmployeePuncIdList);

        IOpResult<IEnumerable<ClockEmployeePunchDto>> GetEmployeePunches(int employeeId, DateTime startDate,
            DateTime endDate);

        IOpResult<IEnumerable<ClockEmployeePunchDto>> GetEmployeePunches(int employeeId, DateTime shiftDate);

        IOpResult<IEnumerable<ClockClientHolidayDto>> GetClientHolidays(int clientId);

        IOpResult<IEnumerable<ClockClientLunchDto>> GetClientLunches(int clientId);

        IOpResult<IEnumerable<ClockClientLunchSelectedDto>> GetClientLunchesSelected(int clientId);

        IOpResult<IEnumerable<ClockClientRulesDto>> GetClientRules(int clientId);

        IOpResult<IEnumerable<ClockClientTimePolicyDtos.ClockClientTimePolicyDto>> GetClientTimePolicies(int clientId);

        IOpResult<IEnumerable<ClockEmployeeDto>> GetClockEmployees(int clientId);

        IOpResult<IEnumerable<ClockClientRulesSummaryDto>> GetClockClientRulesSummary<TMapper>(TMapper mapper,
            int? employeeId = null, int? clientId = null)
            where TMapper : ExpressionMapper<EmployeePay, ClockClientRulesSummaryDto>;
        IOpResult<CheckPunchTypeResultDto> GetNextPunchTypeDetail(int employeeId, DateTime? punchTime = null, string ipAddress = null, bool includeEmployeeClockConfig = false, bool isHwClockPunch = false);
        IOpResult<PunchTypeItemResult> GetPunchTypeItems(int employeeId);
        IOpResult<IEnumerable<EarningPunchTypeItem>> GetEarningPunchTypeItems(int clientId, bool hideOtherEarnings = true,
            ClientEarningCategory? inludeOnlyEarningCategory = null);
        IOpResult<RealTimePunchResult> ProcessRealTimePunch(RealTimePunchRequest request);
        IOpResult<CanEmployeePunchDto> CanEmployeePunchFromIp(int employeeId, string ipAddress);
        IOpResult<RealTimePunchPairResult> ProcessRealTimePunchPair(RealTimePunchRequest first, RealTimePunchRequest second);
        IOpResult<ClockCostCenterRequirementType> GetClockCostCenterRequirementClientOption(int clientId);
        IOpResult<EmployeeClockConfiguration> GetEmployeeClockConfiguration(int employeeId, DateTime punchHistoryStartTime, DateTime punchHistoryEndTime);

        IOpResult<InputHoursPunchRequestResult> ProcessInputHoursPunchRequest(ClockEmployeeBenefitDto request);

        IOpResult<IEnumerable<PunchRerunRawDto>> GetRerunPunches(DateTime startDate, DateTime endDate, IEnumerable<int> clientIds = null);
        IOpResult<IEnumerable<PunchRerunClientDto>> GetRerunPunchesGroupedByClient(DateTime startDate, DateTime endDate, IEnumerable<int> clientIds = null);
        IOpResult<PunchRerunInfoDto> RerunCalculateWeeklyActivity(PunchRerunInfoDto punch, bool shouldRound);
        IOpResult<IEnumerable<(PunchRerunInfoDto Punch, IOpResult Result)>> RerunCalculateWeeklyActivity(
            IEnumerable<PunchRerunInfoDto> punches, bool shouldRound);
        IOpResult<EmployeeDto> GetEmployeeWithPunchesByEmployeeId(int employeeId, DateTime startDate, DateTime endDate);
        IOpResult<RealTimePunchResultDto> ProcessRealTimePunchAttempt(ClockEmployeePunchAttemptDto request);
        IOpResult<IEnumerable<ClockTimePolicyEmployeeDto>> GetEmployeesByTimePolicy(IEnumerable<int> timePolicyIds);
    }
}