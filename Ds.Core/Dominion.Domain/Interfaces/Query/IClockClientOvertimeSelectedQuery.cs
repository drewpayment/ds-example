using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.TimeClock;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClockClientOvertimeSelectedQuery : 
        IQuery<ClockClientOvertimeSelected, IClockClientOvertimeSelectedQuery>
    {
        IClockClientOvertimeSelectedQuery ByClockClientOvertime(int clockClientOvertimeId);

        /// <summary>
        /// Filter selected overtime records by time policy id
        /// </summary>
        /// <param name="clockClientTimePolicyId"></param>
        /// <returns></returns>
        IClockClientOvertimeSelectedQuery ByClockClientTimePolicy(int clockClientTimePolicyId);
        IClockClientOvertimeSelectedQuery ByClockClientTimePolicyList(int[] clockClientTimePolicyIds);

    }
}
