using System;
using Dominion.Core.Dto.Labor;
using Dominion.LaborManagement.Dto.Sproc;

namespace Dominion.LaborManagement.Dto.Clock
{
    /// <summary>
    /// result class providing information as to the previous punch information
    /// and if user controls should be enabled
    /// </summary>
    public class CheckPunchTypeResultDto
    {
        /// <summary>
        /// Time of last punch, if applicable
        /// </summary>
        public DateTime? LastPunchTime { get; set; }

        /// <summary>
        /// true if the user has no punches for the current day of shift (depends on their shift setup)
        /// </summary>
        public bool IsFirstPunchOfDay { get; set; }

        /// <summary>
        /// true is the user will be punching out with there next / current punch
        /// </summary>
        public bool IsOutPunch { get; set; }

        /// <summary>
        /// property tht indicates if the punch type selection control should be disabled
        /// </summary>
        public bool ShouldDisablePunchTypeSelection { get; set; }

        /// <summary>
        /// property that indicates whether the cost center selection control should be hidden.
        /// </summary>
        public bool ShouldHideCostCenter { get; set; }

        /// <summary>
        /// property that indicates whether the department selection controls should be hidden.
        /// </summary>
        public bool ShouldHideDepartment { get; set; }

        /// <summary>
        /// property that indicates whether the job costing selection controls should be hidden.
        /// </summary>
        public bool ShouldHideJobCosting { get; set; }

        /// <summary>
        /// property that indicates whether employee note input controls should be hidden.
        /// </summary>
        public bool ShouldHideEmployeeNotes { get; set; }

        /// <summary>
        /// Property that indicate whether a cost center must be selected.  Based on
        /// the <see cref="ClockCostCenterRequirementType"/> company option.
        /// </summary>
        public bool IsCostCenterSelectionRequired { get; set; }

        /// <summary>
        /// if not null, indicates the cost center used in the last punch.  If using a 
        /// cost center selection control, this should be the default selection.  
        /// </summary>
        public int? CostCenterId { get; set; }

        /// <summary>
        /// Cost center of previous punch
        /// </summary>
        public int? LastOutCostCenterId { get; set; }

        /// <summary>
        /// indicates the lunch cost center id, if applicable
        /// </summary>
        public int? LunchCostCenterId { get; set; }

        /// <summary>
        /// department id from last punch
        /// </summary>
        public int? DepartmentId { get; set; }

        /// <summary>
        /// division id from last punch
        /// </summary>
        public int? DivisionId { get; set; }

        /// <summary>
        /// Id for the type of punch, normal versus lunch, etc
        /// </summary>
        public int? PunchTypeId { get; set; }

        /// <summary>
        /// What type of punch (e.g. Normal, Input Hours, etc)
        /// </summary>
        public PunchOptionType? PunchOption { get; set; }

        /// <summary>
        /// If input punches are allowed.  This overrides the <see cref="PunchOption"/> setting.
        /// </summary>
        public bool AllowInputPunches { get; set; }

        /// <summary>
        /// The home cost center for the employee. If applicable.
        /// </summary>
        public int? HomeCostCenterId { get; set; }

        /// <summary>
        /// The home department for the employee. If applicable.
        /// </summary>
        public int? HomeDepartmentId { get; set; }

        /// <summary>
        /// id for first job costing assignment selected on previous punch
        /// </summary>
        public virtual int? ClientJobCostingAssignmentId1 { get; set; }

        /// <summary>
        /// id for second job costing assignment selected on previous punch
        /// </summary>
        public virtual int? ClientJobCostingAssignmentId2 { get; set; }

        /// <summary>
        /// id for third job costing assignment selected on previous punch
        /// </summary>
        public virtual int? ClientJobCostingAssignmentId3 { get; set; }

        /// <summary>
        /// id for fourth job costing assignment selected on previous punch
        /// </summary>
        public virtual int? ClientJobCostingAssignmentId4 { get; set; }

        /// <summary>
        /// id for fifth job costing assignment selected on previous punch
        /// </summary>
        public virtual int? ClientJobCostingAssignmentId5 { get; set; }

        /// <summary>
        /// id for sixth job costing assignment selected on previous punch
        /// </summary>
        public virtual int? ClientJobCostingAssignmentId6 { get; set; }

        /// <summary>
        /// The DTO that represents the period ending information for the employee.
        /// </summary>
        public ClockEmployeePayPeriodEndedDto PayPeriodEnded { get; set; }

        /// <summary>
        /// IP Address punch is requested from.
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// Indication if the user can punch from their current IP. (see <see cref="IpAddress"/>).
        /// If null, IP check was not performed.
        /// </summary>
        public bool?  CanPunchFromIp { get; set; }

        /// <summary>
        /// Indication if the user has mobile punching option on the assigned punch rules.
        /// </summary>
        public bool HasMobilePunching { get; set; }

        /// <summary>
        /// Additional details about the employee and their clock setup.
        /// </summary>
        public EmployeeClockConfiguration EmployeeClockConfiguration { get; set; }
    }
}