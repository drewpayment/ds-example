using Dominion.Core.Dto.Labor;
using System.Collections.Generic;

namespace Dominion.LaborManagement.Dto.EmployeeLaborManagement
{
    public class ClockEmployeeSetupDto
    {
        public int EmployeeId { get; set; }
        public int ClientId { get; set; }
        public string BadgeNumber { get; set; }
        public string Pin { get; set; }
        public bool AllowGroupScheduling { get; set; }

        public bool AllowEditEmployeeSetup { get; set; }

        public bool existsPrior { get; set; }
        public PunchOptionType? PunchOption { get; set; }

        public EmployeeTimePolicyInfoDto SelectedTimePolicy { get; set; }
        public IEnumerable<EmployeeScheduleSetupDto> SelectedSchedules { get; set; }
        public IEnumerable<EmployeeCostCenterSetupDto> SelectedCostCenters { get; set; }
        public bool? GeofenceEnabled { get; set; }
    }
}
