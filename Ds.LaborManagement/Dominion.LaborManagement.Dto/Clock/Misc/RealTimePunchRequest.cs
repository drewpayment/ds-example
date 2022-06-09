using Dominion.Core.Dto.Labor;
using System;

namespace Dominion.LaborManagement.Dto.Clock.Misc
{
    /// <summary>
    /// Class used to request a real time punch
    /// </summary>
    public class RealTimePunchRequest
    {
        /// <summary>
        /// Client Id associated with the employee in the request
        /// </summary>
        public int ClientId { get; set; }

        /// <summary>
        /// Id for the employee 
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Cost Center Id for the punch
        /// </summary>
        public int? CostCenterId { get; set; }

        /// <summary>
        /// Department Id for the punch.
        /// </summary>
        public int? DepartmentId { get; set; }

        /// <summary>
        /// Division Id for the punch.
        /// </summary>
        public int? DivisionId { get; set; }

        /// <summary>
        /// Used to determine if the punch is an in or out punch
        /// </summary>
        public bool IsOutPunch { get; set; }

        /// <summary>
        /// Id for the type if punch, if left null the punch will be a normal punch 
        /// </summary>
        public int? PunchTypeId { get; set; }

        /// <summary>
        /// The time that the punch should be processed at.
        /// </summary>
        public DateTime? OverridePunchTime { get; set; }

        /// <summary>
        /// Whether or not to override the punch as a break or lunch
        /// </summary>
        public int? OverrideLunchBreak { get; set; }

        /// <summary>
        /// If using input-hours, the number of hours being input.
        /// </summary>
        public decimal? InputHours { get; set; }

        /// <summary>
        /// If using input-hours, the date the input hours should be applied.
        /// </summary>
        public DateTime? InputHoursDate { get; set; }

        /// <summary>
        /// Field for system provided comments
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// The comment the employee provided.
        /// </summary>
        public string EmployeeComment { get; set; }

        /// <summary>
        /// The Clock Name
        /// </summary>
        public string ClockName { get; set; }
        /// <summary>
        /// The hardware Id for the physical timeclock
        /// </summary>
        public int? ClockHardwareId { get; set; }

        /// <summary>
        /// The ID of the first job costing assignment.
        /// </summary>
        /// <remarks>
        /// Because there are multiple of these, we could represent it as 
        /// an array, but I think that representing job costing as 6 distinct
        /// fields is important, because there is an upper limit to the number
        /// of job costing selections.
        /// </remarks>
        public int? JobCostingAssignmentId1 { get; set; }

        /// <summary>
        /// The ID of the second job costing assignment.
        /// </summary>
        public int? JobCostingAssignmentId2 { get; set; }

        /// <summary>
        /// The ID of the third job costing assignment.
        /// </summary>
        public int? JobCostingAssignmentId3 { get; set; }

        /// <summary>
        /// The ID of the fourth job costing assignment.
        /// </summary>
        public int? JobCostingAssignmentId4 { get; set; }

        /// <summary>
        /// The ID of the fifth job costing assignment.
        /// </summary>
        public int? JobCostingAssignmentId5 { get; set; }

        /// <summary>
        /// The ID of the sixth job costing assignment.
        /// </summary>
        public int? JobCostingAssignmentId6 { get; set; }

        public RealTimePunchLocation PunchLocation { get; set; }

        /// <summary>
        /// The client machine Id for the physical timeclock
        /// </summary>
        public int? ClientMachineId { get; set; }

    }

    public class AppPunchRequest : RealTimePunchRequest
    {
        public string IpAddress { get; set; }
        public bool IpCheck { get; set; }
        public bool IncludeConfig { get; set; }
        public DateTime? Punch { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public int ClientEarningId { get; set; }
        public PunchOptionType? PunchOptionType { get; set; }
    }
}