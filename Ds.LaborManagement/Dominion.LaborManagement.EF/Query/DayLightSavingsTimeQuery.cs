using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Misc;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;

namespace Dominion.LaborManagement.EF.Query
{
    public class DayLightSavingsTimeQuery : Query<DayLightSavingsTime, IDayListSavingsTimeQuery>,
        IDayListSavingsTimeQuery
    {
        public DayLightSavingsTimeQuery(IEnumerable<DayLightSavingsTime> data, IQueryResultFactory resultFactory = null) : base(data, resultFactory)
        {
        }

        public IDayListSavingsTimeQuery ForYear(DateTime date)
        {
            FilterBy(x=> x.BeginDate.Year.Equals(date.Year));
            return this;
        }
    }
}