using System.Collections.Generic;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Interfaces.Query.Labor;
using Dominion.Utility.Query;

namespace Dominion.LaborManagement.EF.Query
{
    public class ClockEmployeeAllocateHoursDetailQuery : Query<ClockEmployeeAllocateHoursDetail, IClockEmployeeAllocateHoursDetailQuery>, IClockEmployeeAllocateHoursDetailQuery
    {
        #region Constructor

        public ClockEmployeeAllocateHoursDetailQuery(IEnumerable<ClockEmployeeAllocateHoursDetail> data, IQueryResultFactory resultFactory = null) : base(data, resultFactory)
        {
        }

        #endregion

        /// <summary>
        /// Filters by a particular client.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IClockEmployeeAllocateHoursDetailQuery IClockEmployeeAllocateHoursDetailQuery.ByClientId(int clientId)
        {
            FilterBy(x => x.ClockEmployeeAllocateHours.ClientId == clientId);
            return this;
        }

        /// <summary>
        /// Filters by details associated with a particular cost center.
        /// </summary>
        /// <param name="costCenterId"></param>
        /// <returns></returns>
        IClockEmployeeAllocateHoursDetailQuery IClockEmployeeAllocateHoursDetailQuery.ByCostCenterId(int costCenterId)
        {
            FilterBy(x => x.ClientCostCenterId == costCenterId);
            return this;
        }
    }
}