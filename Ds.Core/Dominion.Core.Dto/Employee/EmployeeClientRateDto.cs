using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Employee
{
    [Serializable]
    public class EmployeeClientRateDto
    {
        public int EmployeeClientRateId { get; set; }
        public int EmployeeId { get; set; }
        public int ClientId { get; set; }
        public int ClientRateId { get; set; }
        public double Rate { get; set; }
        public bool IsDefaultRate { get; set; }
        public DateTime? RateEffectiveDate { get; set; }
        public string Notes { get; set; }
    }
}
