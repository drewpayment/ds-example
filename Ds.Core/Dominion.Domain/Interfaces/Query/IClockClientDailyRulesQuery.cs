using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Labor;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClockClientDailyRulesQuery : IQuery<ClockClientDailyRules, IClockClientDailyRulesQuery>
    {
        /// <summary>
        /// Filters tables results by the ClockClientDailyRulesId column.
        /// </summary>
        /// <param name="clockClientDailyRuleId"></param>
        /// <returns></returns>
        IClockClientDailyRulesQuery ByClockClientDailyRule(int clockClientDailyRuleId);

        /// <summary>
        /// Filters table results by the ClockClientRulesId column.
        /// </summary>
        /// <param name="clockClientRulesId"></param>
        /// <returns></returns>
        IClockClientDailyRulesQuery ByClockClientRule(int clockClientRulesId);
    }
}
