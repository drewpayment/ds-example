using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Labor;
using Dominion.LaborManagement.Dto.EmployeeLaborManagement;
using Dominion.LaborManagement.Dto.Scheduling;

namespace Dominion.LaborManagement.Dto.Clock
{
    /// <summary>
    /// DTO object created for syncing clock employee data for
    /// mobile. 
    /// </summary>
    public class ClockEmployeeDto
    {
        public int EmployeeId { get; set; }

        public string BadgeNumber { get; set; }

        public string EmployeePin { get; set; }

        public double? AverageWeeklyHours { get; set; }

        public int? TimeZoneId { get; set; }

        public int? ClockClientTimePolicyId { get; set; }

        public bool? IsDayLightSavingsObserved { get; set; }

        public int ClientId { get; set; }
		
        public DateTime? HireDate { get; set; }
		
        public DateTime? RehireDate { get; set; }
		
        public DateTime? AnniversaryDate { get; set; }
		
		public virtual ClockClientTimePolicyDtos.ClockClientTimePolicyDto TimePolicy { get; set; }

        public virtual ICollection<ClockEmployeePunchDto> Punches { get; set; }

        public virtual EmployeeCostCenterSetupDto Employee { get; set; }

        public virtual TimeClockClientDto Client { get; set; }
        public virtual bool GeofenceEnabled { get; set; }
    }
}
