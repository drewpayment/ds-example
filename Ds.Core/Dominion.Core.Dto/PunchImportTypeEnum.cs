using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto
{
    public enum PunchImportTypeEnum
    {
        //Public Enum PunchImportTypeEnum
        //<Description("Posi Pay")>
        //PosiTouch = 1
        //< Description("Swept Punch") >
        //SweptPunch = 2
        //End Enum
        [Description("Posi Touch")]
        PosiTouch = 0,
        [Description("Swept Punch")]
        SweptPunch = 1
    }

    // All descriptions/comments on individual enums pertain to PosiTouch documentation, not Dominion Systems.
    public class PosiTouchEnums
    {
        public enum RecordType
        {
            Header = 0,
            JobDetail = 1,
            PunchDetail = 2
        }

        public enum PunchType
        {
            // From clock
            Regular = 1,
            // From clock, deleted later
            Deleted = 2,
            // Created through adjustment process. In-punch is always adjustment, but Out-punch may have come from time clock.
            Adjustment = 3,
            // Adds to pay for this period, but does not figure in overtime calculations.
            PreviousPeriodAdjustment = 4,
            // Pay adjustments for working same position on a different shift.
            ShiftDifferential = 5,
            Holidays = 6,
            // Adds to pay for this period, but does not figure in overtime calculations.
            PreviousAdjustments = 7,
            // All break types (Break1, Break2)*. Adjustments and regular break punches will report as a type 8.
            // * As defined in Setup > T&A > Front of the House > Profiles & Breaks
            Breaks = 8,
            // For working a spread of hours
            SpreadPayment = 14
        }

        public enum PayType
        {
            RegularAndSalaried = 1,
            OvertimeDay = 2,
            OvertimeWeek = 3,
            MiscellaneousPay = 4,
            ShiftDifferential = 5,
            HolidayPay = 6,
            PreviousPeriodAdjustment = 7,
            // Used for additional daily overtime: e.g. double pay after 12 hours.
            OvertimeDay2 = 8,
            OvertimeWeek2 = 9,
            // Overtime7thDayOfWeek{,2} - Similar to Overtime-Day, but only for 7 day weeks.
            Overtime7thDayOfWeek = 10,
            Overtime7thDayOfWeek2 = 11,
            Overtime6thDayOfWeek = 12,
            Overtime6thDayOfWeek2 = 13,
            // For working a spread of hours
            SpreadPayment = 13,
            SplitShift = 14
        }
    }

    
}
