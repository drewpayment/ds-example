using System;

namespace Dominion.LaborManagement.Dto.DataServiceObjects
{
    /// <summary>
    /// Object returned when making a Schedule request from the 
    /// <see cref="IDsDataServicesClockService"/>
    /// </summary>
    public class DataServicesGetScheduleResult
    {
        public int ChangeHistoryId { get; set; }
        public int CostCenterId { get; set; }
        public int DepartmentId { get; set; }
        public int ScheduleId { get; set; }
        public int ScheduleNumber { get; set; }
        public string DepartmentName { get; set; }
        public string CostCenterName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime StopTime { get; set; }
    }
}