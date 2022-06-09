using Dominion.Utility.OpResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Labor;
using Dominion.Core.Dto.Labor.Enum;
using Dominion.Core.Dto.Sprocs;
using Dominion.LaborManagement.Dto.Sproc;

namespace Dominion.LaborManagement.Service.Api
{
    public interface ITimeCardAuthorizationService
    {
        IOpResult<GetTimeCardAuthorizationDataResult> GetData(TimeCardAuthorizationDataArgs args);

        IOpResult<IEnumerable<GetClockEmployeeApproveDateDto>> SaveTimeCardAuthorizationData(TimeCardAuthorizationSaveArgs args);
        IOpResult<IEnumerable<GetClockFilterIdsDto>> FillFilterIDDropdown(int filterCategory, int noOfPayPeriodOptions, int payPeriod, string payPeriodText, int clientId);
        IOpResult<IEnumerable<GetClockFilterIdsDto>> FillFilterIDDropdown(int filterCategory, int clientId);
        IOpResult<GetClockEmployeeApproveHoursSettingsDto> SaveDisplaySettings(GetClockEmployeeApproveHoursSettingsDto dto);

    }
}
