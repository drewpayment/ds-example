using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.TimeCard
{
    public enum DaysFilterType
    {
        /// <summary>
        ///         ''' Defines that every single day in the date range should appear. ("Filler")
        ///         ''' </summary>
        AllDays = 1,
        /// <summary>
        ///         ''' Defines that every week day in the date range should appear. ("Filler")
        ///         ''' </summary>
        AllWeekdays = 2,
        /// <summary>
        ///         ''' Defines that all days in the date range that have a schedule should appear. ("Filler"/"Remover" - Kinda is default)
        ///         ''' </summary>
        AllScheduledDays = 3,
        /// <summary>
        ///         ''' Defines that all days in the date range that have "activity" should appear. ("Remover")
        ///         ''' <para>
        ///         ''' Activity is defined as days with:
        ///         ''' - One or more punches/benefits/pending benefits
        ///         ''' - or one or more exceptions
        ///         ''' </para>
        ///         ''' </summary>
        DatesWithActivity = 4,
        /// <summary>
        ///         ''' Defines that only dates that have one or more exceptions in the date range should appear. ("Remover")
        ///         ''' </summary>
        DatesWithExceptions = 5,
        /// <summary>
        ///         ''' Defines the ID for the days filter control.
        ///         ''' </summary>
        ControlID = 1
    }

}
