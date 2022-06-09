using System;

namespace Dominion.LaborManagement.Dto.DataServiceObjects
{
    /// <summary>
    /// Args classed used for making calculate weekly activity requests using the 
    /// <see cref="IDsDataServicesClockService"/>
    /// </summary>
    public class CalculateWeeklyActivityRequestArgs
    {
        public bool ShouldExitFunction { get; set; }
        public bool ShouldIncludeAutoLunch { get; set; }
        public bool ShouldRecalculateDate { get; set; } 
        public bool ShouldRoundPunch { get; set; }
        public int ClientId { get; set; }
        public int ClockEmployeePunchId { get; set; } = int.MinValue;
        public int EmployeeId { get; set; }
        public DateTime DatePunch { get; set; }
    }
}