using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.EmployeeLaborManagement
{
    [Serializable]
    class ClockEmployeeDto
    {
        public virtual int EmployeeId { get; set; } 
        public virtual string BadgeNumber { get; set; }
        public virtual double? AverageWeeklyHours { get; set; } 
        public virtual int? TimeZoneId { get; set; } 
        public virtual int? ClockClientTimePolicyId { get; set; } 
        public virtual bool? IsDayLightSavingsObserved { get; set; } 
        public virtual int? ModifiedBy { get; set; } 
        public virtual DateTime? Modified { get; set; } 
        public virtual int ClientId { get; set; } 
    }
}
