using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.LeaveManagement
{
    public class TimeOffDto
    {
        /// <summary>
        /// The ID of the policy.
        /// Policy is the type of time off: PTO, Vacation, Sick, etc.
        /// </summary>
        /// <remarks>
        /// NOTE:
        ///     We use the term 'policy' but other places it may be termed differently (db, vb code)
        ///     Other known terms used to describe the same thing:
        ///         -Accrual
        ///         -Plan
        /// </remarks>
        public int PolicyId { get; set; }

        /// <summary>
        /// The name of policy.
        /// Policy is the type of time off: PTO, Vacation, Sick, etc.
        /// </summary>
        /// <remarks>
        /// NOTE:
        ///     We use the term 'policy' but other places it may be termed differently (db, vb code)
        ///     Other known terms used to describe the same thing:
        ///         -Accrual
        ///         -Plan
        /// </remarks>/// 
        public string PolicyName { get; set; }

        /// <summary>
        /// How many hours the individual started with before any events are contemplated.
        /// </summary>
        public decimal StartingUnits { get; set; }

        /// <summary>
        /// The number of units in a single day. (eg: 10.0 hrs / day)
        /// </summary>
        public decimal? UnitsPerDay { get; set; }

        /// <summary>
        /// A day after the last payroll processed which is the date when the starting hours are defined.
        /// </summary>
        public DateTime StartingUnitsAsOf { get; set; }

        /// <summary>
        /// How many hours are available after any policy event is evaluated; including pending.
        /// </summary>
        public decimal UnitsAvailable { get; set; }

        /// <summary>
        /// The number of hours pending request.
        /// </summary>
        public decimal PendingUnits { get; set; }

        /// <summary>
        /// How many pending requests were made.
        /// </summary>
        public int PendingRequest { get; set; }

        /// <summary>
        /// The date of the next award; if that data is pertinent.
        /// </summary>
        public DateTime? NextAwardDate { get; set; }

        /// <summary>
        /// Defines what the units represet.
        /// Days or Hours.
        /// </summary>
        public TimeOffUnitType TimeOffUnitType { get; set; }

        /// <summary>
        /// The event activity for the policy associated with this data.
        /// </summary>
        public IEnumerable<TimeOffEventDto> Activity { get; set; }

        /// <summary>
        /// Gets the display 4 decimal option value
        /// </summary>
        /// <value></value>
        public bool Display4Decimals { get; set; }
    }
}
