using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public partial class ApplicantsStatusChangeDto
    {

        public IEnumerable<ApplicantDetailDto> Applicants { get; set; }
        public ApplicantStatusType? ToStatusId { get; set; }
        public bool IsApplicantHiringWorkflowEnabled { get; set; }
        public int ClientId { get; set; }
        public int CurrentUserId { get; set; }
    }
}
