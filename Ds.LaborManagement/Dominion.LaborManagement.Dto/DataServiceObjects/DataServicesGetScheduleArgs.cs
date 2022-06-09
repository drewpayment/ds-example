using System;

namespace Dominion.LaborManagement.Dto.DataServiceObjects
{
    /// <summary>
    /// Args classed used for making requests for schedules using the 
    /// <see cref="IDsDataServicesClockService"/>
    /// </summary>
    public class DataServicesGetScheduleArgs
    {
        public bool ShouldHideMultipleSchedules { get; set; }
        public int ApplyHoursOption { get; set; }
        public int ClientId { get; set; }
        public int EmployeeId { get; set; }
        public int ClockClientTimePolicyId { get; set; }
        public DateTime PunchTime { get; set; }
    }
}