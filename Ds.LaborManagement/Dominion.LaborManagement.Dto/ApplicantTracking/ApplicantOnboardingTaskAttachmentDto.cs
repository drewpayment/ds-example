using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public partial class ApplicantOnboardingTaskAttachmentDto
    {
        public int ApplicantOnBoardingTaskAttachmentId { get; set; }
        public int ApplicantOnBoardingTaskId { get; set; }
        public int ClientId { get; set; }
        public string Description { get; set; }
        public string FileLocation { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }
        public string FileType { get; set; }
    }
}
