using System.Collections.Generic;
using System.Linq;
using Dominion.Domain.Entities.TimeClock;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;

namespace Dominion.LaborManagement.EF.Query
{
    public class ClockClientLunchQuery : Query<ClockClientLunch, IClockClientLunchQuery>, IClockClientLunchQuery
    {
        public ClockClientLunchQuery(IEnumerable<ClockClientLunch> data, IQueryResultFactory resultFactory = null) : base(data, resultFactory)
        {
        }

        public IClockClientLunchQuery ByClient(int clientId)
        {
            FilterBy(x => x.ClientId.Equals(clientId));
            return this;
        }

        public IClockClientLunchQuery ByClockClientLunchId(int clockClientLunchId)
        {
            FilterBy(x => x.ClockClientLunchId == clockClientLunchId);
            return this;
        }

        public IClockClientLunchQuery ByClockClientLunchName(string name)
        {
            FilterBy(x => x.Name.ToUpper() == name.ToUpper());
            return this;
        }

        public IClockClientLunchQuery ByClockClientTimePolicy(int clockClientTimePolicyId)
        {
            FilterBy(x => x.TimePolicies.Select(y => y.ClockClientTimePolicyId == clockClientTimePolicyId).Any());
            return this;
        }

        /// <summary>
        /// Filters ClockClientLunches based on whether or not they 
        /// have a ClockClientTimePolicy associated with them.
        /// By default, it will return lunches with a policy
        /// </summary>
        /// <param name="hasTimePolicies"></param>
        /// <returns></returns>
        public IClockClientLunchQuery HasTimePolicies(bool hasTimePolicies = true)
        {
            FilterBy(x => hasTimePolicies == x.TimePolicies.Any());
            return this;
        }

        /// <summary>
        /// Filters by lunch rules associated with the particular client cost center.
        /// </summary>
        /// <param name="costCenterId"></param>
        /// <returns></returns>
        IClockClientLunchQuery IClockClientLunchQuery.ByCostCenter(int? costCenterId)
        {
            FilterBy(x => x.ClientCostCenterId == costCenterId);
            return this;
        }
    }
}