using Dominion.Core.Dto.Labor;
using Dominion.Utility.OpResult;
using System.Collections.Generic;

namespace Dominion.LaborManagement.Service.Internal.ClockException
{
    public interface IClockEmployeeExceptionProvider
    {
        IOpResult<IEnumerable<ClockEmployeeExceptionHistoryDto>> GetClockEmployeeExceptionsByEmployeeId(int employeeId);
        IOpResult<ClockEmployeeExceptionHistoryDto> AddClockEmployeeException(ClockEmployeeExceptionHistoryDto exception);
        IOpResult<IEnumerable<ClockEmployeeExceptionHistoryDto>> GetEmployeePunchExceptionsByPunchIds(IEnumerable<int> punchIds);
        IOpResult<bool> DeleteClockEmployeeException(IEnumerable<ClockExceptionType> clockExceptionIds, int punchId);
    }
}
