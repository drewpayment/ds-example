using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Employee
{
    public class EmployeeDemographicDto : EmployeeHireDto
    {
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Status { get; set; }
        public virtual string PayType { get; set; }
        public int? LengthOfService {get; set; }
        public int? Age { get; set; }
        public virtual string Gender { get; set; }
        public virtual string Ethnicity { get; set; }
        public virtual bool IsActive { get; set; }
    }
}
