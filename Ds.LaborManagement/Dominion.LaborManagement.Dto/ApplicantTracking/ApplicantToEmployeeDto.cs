using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public class ApplicantToEmployeeDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? JobProfileId { get; set; }
        public int JobTypeId { get; set; }
        public int? ClientDivisionId { get; set; }
        public int? ClientDepartmentId { get; set; }

    }
}
