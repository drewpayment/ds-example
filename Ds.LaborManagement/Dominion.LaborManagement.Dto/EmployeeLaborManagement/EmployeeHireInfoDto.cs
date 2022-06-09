using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Employee;

namespace Dominion.LaborManagement.Dto.EmployeeLaborManagement
{
    public class EmployeeHireInfoDto : EmployeeBasicDto
    {
        public int? JobProfileId { get; set; }
        public string JobTitle { get; set; }
        public DateTime? HiredOn { get; set; }
        public int? HiredBy { get; set; }
        public string Hirer { get; set; }
    }
}
