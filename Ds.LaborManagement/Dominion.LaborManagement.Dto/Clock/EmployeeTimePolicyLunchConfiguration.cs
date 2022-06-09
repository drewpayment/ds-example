using System;

namespace Dominion.LaborManagement.Dto.Clock
{
    public class EmployeeTimePolicyLunchConfiguration
    {
        public int       ClockClientLunchId { get; set; }
        public string    Name               { get; set; }
        public int?      ClientCostCenterId { get; set; }
        public DateTime? StartTime          { get; set; }
        public DateTime? StopTime           { get; set; }
        public bool      IsSunday           { get; set; }
        public bool      IsMonday           { get; set; }
        public bool      IsTuesday          { get; set; }
        public bool      IsWednesday        { get; set; }
        public bool      IsThursday         { get; set; }
        public bool      IsFriday           { get; set; }
        public bool      IsSaturday         { get; set; }
    }
}