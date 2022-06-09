using Dominion.Core.Dto.Labor;

namespace Dominion.LaborManagement.Dto.Clock
{
    public class EmployeeTimePolicyRuleConfiguration : IHasTimePolicyRuleShiftSettings
    {
        public int              ClockClientRulesId  { get; set; }
        public string           Name                { get; set; }
        public PunchOptionType? PunchOption         { get; set; }
        public decimal?         MaxShift            { get; set; }
        public byte             ApplyHoursOption    { get; set; }
        public bool             AllowInputPunches   { get; set; }
        public bool             AllowMobilePunching { get; set; }
        public bool             IsHideCostCenter    { get; set; }
        public bool             IsHideDepartment    { get; set; }
        public bool             IsHideEmployeeNotes { get; set; }
        public bool             IsHideJobCosting    { get; set; }
        public bool             IsHidePunchType     { get; set; }
        public bool             IsHideShift         { get; set; }
        public byte?            StartDayOfWeek      { get; set; }
        public virtual short InEarlyGraceTime { get; set; }
        public virtual short OutLateGraceTime { get; set; }
    }
}