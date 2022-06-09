using System;

namespace Dominion.LaborManagement.Dto.Scheduling
{
    public class ClockEmployeeScheduleShiftDto 
    {
        public SchedulesEmployeeDataDto EmployeeDataDto { get; set; }

        public int        ClientId                  { get; set; }
        public int        ClockEmployeeScheduleId   { get; set; }
        public int        EmployeeId                { get; set; }
        public DateTime   EventDate                 { get; set; }

        public int?       GroupScheduleShiftId1      { get; set; }
        public int?       ScheduleGroupId1           { get; set; }
        public string     ScheduleGroupDescription1  { get; set; }
        public int?       ScheduleGroupSourceId1     { get; set; }
        public DateTime?  StartTimeDate1             { get; set; }
        public DateTime?  EndTimeDate1               { get; set; }
        public int?       EmployeeDefaultShiftId1    { get; set; }
        public int?       ClientCostCenterId1        { get; set; }

        public int?       GroupScheduleShiftId2      { get; set; }
        public int?       ScheduleGroupId2           { get; set; }
        public string     ScheduleGroupDescription2  { get; set; }
        public int?       ScheduleGroupSourceId2     { get; set; }
        public DateTime?  StartTimeDate2             { get; set; }
        public DateTime?  EndTimeDate2               { get; set; }
        public int?       EmployeeDefaultShiftId2    { get; set; }
        public int?       ClientCostCenterId2        { get; set; }

        public int?       GroupScheduleShiftId3      { get; set; }
        public int?       ScheduleGroupId3           { get; set; }
        public string     ScheduleGroupDescription3  { get; set; }
        public int?       ScheduleGroupSourceId3     { get; set; }
        public DateTime?  StartTimeDate3             { get; set; }
        public DateTime?  EndTimeDate3               { get; set; }
        public int?       EmployeeDefaultShiftId3    { get; set; }
        public int?       ClientCostCenterId3        { get; set; }
    }
}