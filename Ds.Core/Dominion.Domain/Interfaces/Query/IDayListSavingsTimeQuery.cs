using System;
using Dominion.Domain.Entities.Misc;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Query that pulls information from the TimeDst table and provides a location for common queries
    /// of this dataset.
    /// </summary>
    public interface IDayListSavingsTimeQuery : IQuery<DayLightSavingsTime, IDayListSavingsTimeQuery>
    {
        /// <summary>
        ///  Filters results to the year of the date passed in.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        IDayListSavingsTimeQuery ForYear(DateTime date);
    }
}