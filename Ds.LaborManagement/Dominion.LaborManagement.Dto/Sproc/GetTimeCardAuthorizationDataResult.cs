using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.Sproc
{
    public class GetTimeCardAuthorizationDataResult
    {
        public IEnumerable<GetClockPayrollListByClientIDPayrollRunIDDto> ClockPayrollList { get; set; }
        public IEnumerable<GetClockFilterCategoryDto> ClockFilterCategory { get; set; }
        public IEnumerable<GetClockEmployeeApproveHoursSettingsDto> ClockEmployeeApproveHoursSettings { get; set; }
        public IEnumerable<GetClockEmployeeApproveHoursOptionsDto> ClockEmployeeApproveHoursOptions { get; set; }
        public GetClientJobCostingInfoByClientIDResultsDto ClientJobCostingInfoResults { get; set; }
        public IEnumerable<GetClockClientNoteListResultDto> ClientNotes { get; set; }
    }
}
