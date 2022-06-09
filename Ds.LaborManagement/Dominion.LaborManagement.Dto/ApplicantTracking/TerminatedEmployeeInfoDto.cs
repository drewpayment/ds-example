using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Employee;
using Dominion.Taxes.Dto.Taxes;


namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    [Serializable]
    public class TerminatedEmployeeInfoDto
    {
        public DateTime MonthStartDate { get; set; }
        public int TotalCount { get; set; }
        public float TurnoverRate { get; set; }
        public float RetentionRate { get; set; }
        public float GrowthRate { get; set; }
        public IEnumerable<TerminatedEmployeeDto> TerminatedEmployees { get; set; }
        public IEnumerable<TerminatedEmployeeDto> NewHires { get; set; }
    }
}