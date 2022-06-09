using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Entities.TimeClock;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClockClientHolidayQuery : IQuery<ClockClientHoliday, IClockClientHolidayQuery>
    {
        IClockClientHolidayQuery ByClient(int clientId);

        IClockClientHolidayQuery ByClockClientHolidayId(int clockClientHolidayId);
    }
}