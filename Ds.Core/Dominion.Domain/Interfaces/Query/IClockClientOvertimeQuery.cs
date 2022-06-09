using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Entities.TimeClock;
using Dominion.Utility.Query;

namespace Dominion.LaborManagement.EF.Query
{
    public interface IClockClientOvertimeQuery : IQuery<ClockClientOvertime, IClockClientOvertimeQuery>
    {
        IClockClientOvertimeQuery ByClockClientOvertimeId(int clockClientOvertimeId);
        IClockClientOvertimeQuery ByClientEarning(int clientEarningId);
        IClockClientOvertimeQuery ByClientId(int clientId);

        /// <summary>
        /// Filters list of overtime entities down by a list of ids.
        /// </summary>
        /// <param name="overtimeIds"></param>
        /// <returns></returns>
        IClockClientOvertimeQuery ByClockClientOvertimeIdList(List<int> overtimeIds);

        /// <summary>
        /// Filters lists of overtime entities by the name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IClockClientOvertimeQuery ByClockClientOvertimeName(string name);
    }
}