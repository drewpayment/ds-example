using Dominion.Domain.Entities.Labor;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClockClientAddHoursQuery : IQuery<ClockClientAddHours, IClockClientAddHoursQuery>
    {
        IClockClientAddHoursQuery ByClockClientAddHoursId(int clockClientAddHoursId);
        IClockClientAddHoursQuery ByClient(int clientId);
        IClockClientAddHoursQuery HasTimePolicies(bool hasTimePolicies);
    }
}
