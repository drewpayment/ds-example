using Dominion.Core.Dto.Labor;
using Dominion.LaborManagement.Dto.Clock.Misc;
using Dominion.Utility.OpResult;
using System.Collections.Generic;

namespace Dominion.Core.Services.Api.ClockException
{
    public interface IClockEmployeeExceptionService
    {
        IOpResult<IEnumerable<ClockEmployeeExceptionHistoryDto>> GetClockEmployeeExceptionsByEmployeeId(int employeeId);
        IOpResult<ClockEmployeeExceptionHistoryDto> AddClockEmployeeException(RealTimePunchRequest punch, RealTimePunchResultDto savedPunch);
        IOpResult<ClockEmployeeExceptionHistoryDto> AddClockEmployeeExceptionForInputRequest(InputHoursPunchRequest request, InputHoursPunchRequestResult savedPunch);
        IOpResult<IEnumerable<ClockEmployeeExceptionHistoryDto>> GetEmployeePunchExceptionsByPunchIds(IEnumerable<int> punchIds);
        IOpResult<bool> DeleteClockEmployeeException(IEnumerable<ClockExceptionType> clockExceptionId, int punchId);
    }
}
