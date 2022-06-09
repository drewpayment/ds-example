using System;
using System.Collections.Generic;

namespace Dominion.LaborManagement.Dto.Scheduling
{
    public class EmployeeSchedulesDto : SchedulesEmployeeDataDto
    {
        public List<ScheduleShiftDto> EmployeeScheduleShifts { get; set; }
        public List<ScheduleShiftDto> EmployeeRecurringShifts { get; set; }
        public List<ScheduledBenefitDto> EmployeeScheduledBenefits { get; set; } 
    }

    public class SchedulesEmployeeDataDto
    {
        public int      EmployeeId            { get; set; }
        public string   FirstName             { get; set; }
        public string   LastName              { get; set; }
        public int?     JobProfileId            { get; set; }
        public string   JobTitle              { get; set; }
        public bool     IsTerminated          { get; set; }
        public bool     IsNotAssignedToSource { get; set; }
        public string   RateDescription       { get; set; }
        public decimal? Rate                  { get; set; }
        public bool?    IsHourly              { get; set; }
    }

    public class ScheduleShiftDto
    {
        public SchedulesEmployeeDataDto EmployeeDataDto { get; set; }

        public int        EmployeeId                        { get; set; }
        public int        ShiftId                           { get; set; }
        public int        GroupScheduleShiftId              { get; set; }
        public int        ScheduleGroupId                   { get; set; }
        public string     ScheduleGroupDescription          { get; set; }
        public int        ScheduleGroupSourceId             { get; set; }
        public DateTime   EventDate                         { get; set; }
        public TimeSpan?  StartTime                         { get; set; }
        public TimeSpan?  EndTime                           { get; set; }
        public DateTime?  StartTimeDate                     { get; set; }
        public DateTime?  EndTimeDate                       { get; set; }
        public DayOfWeek? DayOfWeek                         { get; set; } //only used for default shifts; used to determine event date
        public bool       IsDeleted                         { get; set; }
        public bool       IsAdded                           { get; set; } //for adding another shift to the same day for published but will be set for either
        public bool       IsPreview                         { get; set; }
        public int?       Override_ScheduleGroupSourceId    { get; set; }
        public int?       Override_ScheduleGroupId          { get; set; }
        public bool       IsOverridden                      { get; set; }
        public string     Override_Description              { get; set; }
    }

    /// <summary>
    /// Represents time an employee has scheduled off due to either a vacation request or holiday. 
    /// </summary>
    public class ScheduledBenefitDto
    {
        public int        EmployeeId    { get; set; }
        public string     Description   { get; set; }
        public DateTime   EventDate     { get; set; }
        public TimeSpan?  StartTime     { get; set; }
        public TimeSpan?  EndTime       { get; set; }
        public DateTime?  StartTimeDate { get; set; }
        public DateTime?  EndTimeDate   { get; set; }
        public DayOfWeek? DayOfWeek     { get; set; }
        public double     TotalHours    { get; set; }
        public bool       IsApproved    { get; set; }
        public bool       IsHoliday     { get; set; }
    }

}
