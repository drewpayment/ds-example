using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.TimeCard
{
    public class EmployeeTotals
    {
        public string EmployeeName { get; set; }
        public int UserTypeID { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeNumber { get; set; }

        public virtual IEnumerable<EmployeeDay> Days { get; set; }

        public int EmployeeActivity { get; set; }

        public int PunchOption { get; set; }

        public RulesByEmployeeID.Rule Rules { get; set; }

        public IEnumerable<EmployeeWeekTotals> WeeklyTotals { get; set; }

        public string TimePolicyName { get; set; }
        public int? TimePolicyID { get; set; }
        public bool HasGeofencing { get; set; }
    }

    public class EmployeeWeekTotals
    {
        public DateTime StartOfWeek { get; set; }
        public virtual IEnumerable<EmployeeWeekTotal> Totals { get; set; }

        /// <summary>
        ///         ''' The sum of all of the totals listed in the totals array.
        ///         ''' </summary>
        ///         ''' <returns></returns>
        public double TotalHours { get; set; }
    }

    public class EmployeeWeekTotal
    {
        /// <summary>
        ///         ''' Gets or sets the "name" of the total. This is what is displayed in the "Date" column.
        ///         ''' Often "Regular" or "Overtime".
        ///         ''' </summary>
        ///         ''' <returns></returns>
        public string Name { get; set; }

        /// <summary>
        ///         ''' Gets or sets the "group" of the total. That is, the key that was calculated to group similar earnings together.
        ///         ''' Can be used to additionally group multiple weekly totals together.
        ///         ''' </summary>
        ///         ''' <returns></returns>
        public string Group { get; set; }

        public double Hours { get; set; }
        /// <summary>
        ///         ''' Gets or sets the array of values that should be displayed in the "exceptions" cell for the total row.
        ///         ''' </summary>
        ///         ''' <returns></returns>
        public virtual IEnumerable<string> Exceptions { get; set; } = Enumerable.Empty<string>();
    }
}
