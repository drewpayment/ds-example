using Dominion.Domain.Entities.Labor;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Labor
{
    public interface IClockEmployeeAllocateHoursDetailQuery : IQuery<ClockEmployeeAllocateHoursDetail, IClockEmployeeAllocateHoursDetailQuery>
    {
        /// <summary>
        /// Filters by a particular client.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IClockEmployeeAllocateHoursDetailQuery ByClientId(int clientId);

        /// <summary>
        /// Filters by details associated with a particular cost center.
        /// </summary>
        /// <param name="costCenterId"></param>
        /// <returns></returns>
        IClockEmployeeAllocateHoursDetailQuery ByCostCenterId(int costCenterId);
    }
}
