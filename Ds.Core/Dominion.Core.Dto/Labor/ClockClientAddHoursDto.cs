using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Payroll;

namespace Dominion.Core.Dto.Labor
{
    public partial class ClockClientAddHoursDto
    {
        public int ClockClientAddHoursId { get; set; }
        public int ClientId { get; set; }
        public string Name { get; set; }
        public int CalculationFrequency { get; set; }
        public double TimeWorkedThreshold { get; set; }
        public double Award { get; set; }
        public int ClientEarningId { get; set; }
        public bool? IsSunday { get; set; }
        public bool? IsMonday { get; set; }
        public bool? IsTuesday { get; set; }
        public bool? IsWednesday { get; set; }
        public bool? IsThursday { get; set; }
        public bool? IsFriday { get; set; }
        public bool? IsSaturday { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }

        public IEnumerable<ClockClientTimePolicyDtos.ClockClientTimePolicyDto> TimePolicies { get; set; }
    }
}
