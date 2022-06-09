using System;
using System.Collections.Generic;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public class ClosePostingDto
    {
        public int ClientId { get; set; }
        public int PostingId { get; set; }
        public bool IsRejectRemainingApplicants { get; set; }
        public bool IsClosePosting { get; set; }
        public int RejectionReason { get; set; }
        public List<ApplicantFinalCorrespondenceDto> ApplicantFinalCorrespondences { get; set; }
    }
}