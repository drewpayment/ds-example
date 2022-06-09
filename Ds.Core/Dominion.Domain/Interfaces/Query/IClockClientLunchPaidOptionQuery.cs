using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Labor;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClockClientLunchPaidOptionQuery : IQuery<ClockClientLunchPaidOption, IClockClientLunchPaidOptionQuery>
    {
        /// <summary>
        /// Filter table results by client id.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IClockClientLunchPaidOptionQuery ByClientId(int clientId);

        /// <summary>
        /// Filter table results by clock client lunch id.
        /// </summary>
        /// <param name="clockClientLunchId"></param>
        /// <returns></returns>
        IClockClientLunchPaidOptionQuery ByClockClientLunchId(int clockClientLunchId);

        /// <summary>
        /// Delete an existing lunch paid option entity.
        /// </summary>
        /// <param name="clockClientLunchPaidOptionId"></param>
        /// <returns></returns>
        IClockClientLunchPaidOptionQuery ByClockClientLunchPaidOptionId(int clockClientLunchPaidOptionId);
    }
}
