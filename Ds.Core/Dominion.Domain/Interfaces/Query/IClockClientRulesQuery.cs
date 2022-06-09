using Dominion.Domain.Entities.TimeClock;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClockClientRulesQuery : IQuery<ClockClientRules, IClockClientRulesQuery>
    {
        IClockClientRulesQuery ByClient(int clientId);

        /// <summary>
        /// Filter current ClockClientRules result set by the specified clockClientRulesId
        /// </summary>
        /// <param name="clockClientRulesId"></param>
        /// <returns></returns>
        IClockClientRulesQuery ByClockClientRulesId(int clockClientRulesId);
    }
}