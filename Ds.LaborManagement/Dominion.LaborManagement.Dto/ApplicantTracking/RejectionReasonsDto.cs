using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public class RejectionReasonsDto
    {
        public int RejectionCount { get; set; }
        public int ApplicantRejectionReasonId { get; set; }
        public string Description { get; set; }
    }
}
