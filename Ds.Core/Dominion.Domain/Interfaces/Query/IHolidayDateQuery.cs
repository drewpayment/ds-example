﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Labor;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IHolidayDateQuery : IQuery<HolidayDate, IHolidayDateQuery>
    {
        IHolidayDateQuery ByYearObserved(int year);

        IHolidayDateQuery ByYearsGreaterObserved(int year);
    }
}