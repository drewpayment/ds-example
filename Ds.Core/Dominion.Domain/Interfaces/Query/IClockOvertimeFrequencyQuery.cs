using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Entities.TimeClock;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClockOvertimeFrequencyQuery : IQuery<ClockOvertimeFrequency, IClockOvertimeFrequencyQuery>
    {
        IClockOvertimeFrequencyQuery ByCollectionOfFrequencyIds(IEnumerable<int> frequencyIds);
    }
}