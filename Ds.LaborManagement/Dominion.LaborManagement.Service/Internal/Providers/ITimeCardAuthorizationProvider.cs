using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Labor;
using Dominion.Core.Dto.Labor.Enum;
using Dominion.Core.Dto.Sprocs;
using Dominion.Domain.Entities.Labor;
using Dominion.LaborManagement.Dto.Sproc;
using Dominion.Utility.OpResult;

namespace Dominion.LaborManagement.Service.Internal.Providers
{
    public interface ITimeCardAuthorizationProvider
    {
        IOpResult<GetTimeCardAuthorizationDataResult> GetData(TimeCardAuthorizationDataArgs args);
        IOpResult<int> SaveTimeCardAuthorizationData(InsertClockEmployeeApproveDateArgsDto args);
        IOpResult<IEnumerable<GetClockEmployeeApproveDateDto>> GetClockEmployeeApproveDate(GetClockEmployeeApproveDateArgsDto args);
        IOpResult<IEnumerable<GetClockFilterIdsDto>> GetClockFilterIDs(GetClockFilterIdsArgsDto args);
        IOpResult<IEnumerable<GetClockFilterIdsDto>> FillFilterIDDropdown(int filterCategory, int noOfPayPeriodOptions, int payPeriod, string payPeriodText, int clientId);
        IOpResult<IEnumerable<GetClockFilterIdsDto>> FillFilterIDDropdown(int filterCategory, int clientId);
        IOpResult<GetClockEmployeeApproveHoursSettingsDto> SaveDisplaySettings(GetClockEmployeeApproveHoursSettingsDto dto);
        IOpResult<ClockEmployeeApproveHoursSettings> CreateNewDisplaySettings(ClockEmployeeApproveHoursSettings entity);
        IOpResult<ClockEmployeeApproveDateDto> SaveTimeCardApprovalRow(ClockEmployeeApproveDateDto dto, bool isApproveByCostCenter, bool holdCommit = true);
        IOpResult<IEnumerable<ClockEmployeeApproveDateDto>> RecalculateWeeklyActivityByApproveDates(TimeCardAuthorizationSaveArgs args);
        IOpResult DeleteClockEmployeeApproveDate(int id, bool commit = false);
        void SaveClockEmployeeApproveDate(ClockEmployeeApproveDateDto dto, ClockEmployeeApproveDateDto curr);
    }
}
