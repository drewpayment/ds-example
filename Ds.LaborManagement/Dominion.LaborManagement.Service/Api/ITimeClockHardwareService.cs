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
using Dominion.LaborManagement.Dto.TimeClockHardware;

namespace Dominion.LaborManagement.Service.Api
{
    public interface ITimeClockHardwareService
    {
        IOpResult<IEnumerable<ClockClientHardwareDto>> GetClockClientHardwares(int clientId);
        IOpResult<ClockClientHardwareDto> UpdateClockClientHardware(int clientId, ClockClientHardwareDto dto);
        IOpResult<ClockClientHardwareDto> DeleteClockClientHardware(int clockClientHardwareId);
    }
}
