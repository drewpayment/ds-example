using Dominion.Domain.Entities.TimeClock;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClockClientLunchQuery : IQuery<ClockClientLunch, IClockClientLunchQuery>
    {
        /// <summary>
        /// Filters entities by client id.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IClockClientLunchQuery ByClient(int clientId);

        /// <summary>
        /// Filter entities by clock client lunch id.
        /// </summary>
        /// <param name="clockClientLunchId"></param>
        /// <returns></returns>
        IClockClientLunchQuery ByClockClientLunchId(int clockClientLunchId);

        /// <summary>
        /// Filters ClockClientLunches based on whether or not they 
        /// have a ClockClientTimePolicy associated with them.
        /// By default, it will return lunches with a policy
        /// </summary>
        /// <param name="hasTimePolicies"></param>
        /// <returns></returns>
        IClockClientLunchQuery HasTimePolicies(bool hasTimePolicies = true);

        /// <summary>
        /// Filters by lunch rules associated with the particular client cost center.
        /// </summary>
        /// <param name="costCenterId"></param>
        /// <returns></returns>
        IClockClientLunchQuery ByCostCenter(int? costCenterId);

        /// <summary>
        /// Attempts to filter ClockClientLunches by name, normalizes the string param,
        /// removes whitespace and mutates to lowercase for best possible comparison result.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IClockClientLunchQuery ByClockClientLunchName(string name);

        /// <summary>
        /// Filters ClockClientLunches to only those on time policies and then returns the 
        /// lunches related to current time policy.
        /// </summary>
        /// <param name="clockClientTimePolicyId"></param>
        /// <returns></returns>
        IClockClientLunchQuery ByClockClientTimePolicy(int clockClientTimePolicyId);
    }
}