using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public class ApplicantPostingSchoolRequirementDto
    {
        public int? MinSchooling { get; set; }
        public bool IsForceMinSchoolingMatch { get; set; }
        public int Order { get; set; }
    }
}
