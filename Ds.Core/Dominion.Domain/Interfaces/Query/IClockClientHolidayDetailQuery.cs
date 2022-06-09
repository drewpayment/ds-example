using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Labor;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClockClientHolidayDetailQuery : IQuery<ClockClientHolidayDetail, IClockClientHolidayDetailQuery>
    {
        IClockClientHolidayDetailQuery ByClockClientHolidayDetailId(int clockClientHolidayDetailId);

        IClockClientHolidayDetailQuery ByClockClientHolidayId(int clockClientHolidayId);

        IClockClientHolidayDetailQuery ByEventDateOnOrAfter(DateTime startDate);
        IClockClientHolidayDetailQuery ByIsPaid(bool isPaid = true);
    }
}
