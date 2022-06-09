using System;
using Dominion.Core.Dto.Labor;

namespace Dominion.Core.Dto.LeaveManagement
{
    public class TimeOffEventDto
    {
        /// <summary>
        /// Request Id from the database for the event.
        /// This will -1 for non employe requested events.
        /// </summary>
        public int? RequestTimeOffId { get; set; }

        /// <summary>
        /// The date the employee request or award <see cref="TimeOffAwardType"/> took place.
        /// </summary>
        public DateTime? RequestDate { get; set; }

        /// <summary>
        /// The starting date of the time off.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// The ending date of the time off.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Represents the type of award this event defines.
        /// Zero is undefined.
        /// </summary>
        public TimeOffAwardType? TimeOffAward { get; set; }

        /// <summary>
        /// Represents the status of the time off event.
        /// Zero is undefined.
        /// </summary>
        public TimeOffStatusType? TimeOffStatus { get; set; }

        /// <summary>
        /// The number of hours.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// The balance of time off hours after the amount is applied.
        /// </summary>
        public decimal BalanceAfter { get; set; }

        /// <summary>
        /// For items with a events request id there may be notes associated with it. 
        /// </summary>
        public TimeOffNotesDto Notes { get; set; }

        /// <summary>
        /// For items with a events request id, there will be a time off request with time off request details.
        /// </summary>
        public TimeOffRequestDto TimeOffRequest { get; set; }
    }
}
