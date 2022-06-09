using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.TimeClock;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClockClientLunchSelectedQuery : IQuery<ClockClientLunchSelected, IClockClientLunchSelectedQuery>
    {

        /// <summary>
        /// Filter by clock client lunch id.
        /// </summary>
        /// <param name="clockClientLunchId"></param>
        /// <returns></returns>
        IClockClientLunchSelectedQuery ByClockClientLunchId(int clockClientLunchId);

    }
}
