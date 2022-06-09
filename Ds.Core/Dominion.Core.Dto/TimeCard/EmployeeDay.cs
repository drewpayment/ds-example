using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.TimeCard
{
    public class EmployeeDay
    {
        public DateTime Day { get; set; }
        public string NameOfDay { get; set; }
        public virtual IEnumerable<EmployeeDayPunch> Punches { get; set; } = Enumerable.Empty<EmployeeDayPunch>();
        public IEnumerable<EmployeeDayEarning> Earnings { get; set; } = Enumerable.Empty<EmployeeDayEarning>();
        /// <summary>
        ///         ''' The array of exceptions that exist for the day.
        ///         ''' </summary>
        ///         ''' <returns></returns>
        public IEnumerable<EmployeeDayException> Exceptions { get; set; } = Enumerable.Empty<EmployeeDayException>();
        public EmployeeDayNotes Notes { get; set; }
        /// <summary>
        ///         ''' The schedule that is being used for this day.
        ///         ''' If an override schedule is being used, this will equal <see cref="OverrideSchedule"/>.
        ///         ''' </summary>
        ///         ''' <returns></returns>
        public EmployeeDaySchedule Schedule
        {
            get
            {
                if ((OverrideSchedule == null))
                    return RegularSchedule;
                else if (OverrideSchedule.Schedule1 == null)
                    return null/* TODO Change to default(_) if this is not a reference type */;
                else
                    return OverrideSchedule;
            }
        }

        /// <summary>
        ///         ''' The schedule that the employee is "normally" scheduled to.
        ///         ''' Schedules assigned to this property are usually taken from the <see cref="EmployeeWeeklySchedule"/> that
        ///         ''' is stored in <see cref="EmployeeTotals"/> objects. It was denormalized to help with transferal over to a
        ///         ''' traditionally denormalized format such as a data grid.
        ///         ''' </summary>
        ///         ''' <returns></returns>
        public EmployeeDaySchedule RegularSchedule { get; set; }

        /// <summary>
        ///         ''' The "override" schedule that has been set for the day.
        ///         ''' If null, no override schedule is being used.
        ///         ''' </summary>
        ///         ''' <returns></returns>
        public EmployeeDaySchedule OverrideSchedule { get; set; }
        public bool IsMissingPunch { get; set; }
        public bool IsNoPunchesOnScheduledDay { get; set; }

        /// <summary>
        ///         ''' The number of hours that an employee worked (Non benefit hours) in a day.
        ///         ''' </summary>
        ///         ''' <returns></returns>
        public double? WorkedHours { get; set; }

        // TODO: Replace with EmployeeDayBenefit class
        public virtual IEnumerable<BenefitListByDate.Benefit> Benefits { get; set; } = Enumerable.Empty<BenefitListByDate.Benefit>();

        public EmployeeDayApprovalStatus ApprovalStatus { get; set; } = new EmployeeDayApprovalStatus();

        public PayPeriodEnded.Period PayPeriod { get; set; }

        public double Points { get; set; }

        public double TotalHours { get; set; }

        public virtual IEnumerable<ApprovedDates.Day> Approvals { get; set; } = Enumerable.Empty<ApprovedDates.Day>();

        /// <summary>
        ///         ''' Whether the day has benefits that haven't been removed.
        ///         ''' </summary>
        ///         ''' <returns></returns>
        public bool IsPendingBenefits { get; set; }

        public virtual IEnumerable<EmployeeDayPendingBenefit> PendingBenefits { get; set; } = Enumerable.Empty<EmployeeDayPendingBenefit>();

        /// <summary>
        ///         ''' Whether the user should be able to see the day's records.
        ///         ''' Usually based on the list of employees the user has access to,
        ///         ''' or in the case of the cost center approval option, which cost centers the user 
        ///         ''' has access to.
        ///         ''' </summary>
        ///         ''' <returns></returns>
        public bool HasAccess { get; set; }
        public bool AllowSuperAddBen { get; set; } = true;
        public bool AllowSuperAddPunches { get; set; } = true;
    }
}
