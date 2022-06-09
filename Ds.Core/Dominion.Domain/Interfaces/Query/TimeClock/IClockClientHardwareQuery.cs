using Dominion.Domain.Entities.TimeClock;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClockClientHardwareQuery : IQuery<ClockClientHardware, IClockClientHardwareQuery>
    {
        /// <summary>
        /// Filters entities by ClockClientHardwareId.
        /// </summary>
        /// <param name="ClockClientHardwareId"></param>
        /// <returns></returns>
        IClockClientHardwareQuery ByClockClientHardwareId(int clockClientHardwareId);

        /// <summary>
        /// Filters entities by client id.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IClockClientHardwareQuery ByClientId(int clientId);
    }
}