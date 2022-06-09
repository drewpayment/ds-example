using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public partial class ApplicantApplicationHeaderDto
    {
        public int ApplicationHeaderId { get; set; }
        public int ApplicantId { get; set; }
        public int PostingId { get; set; }
        public string PostingDescription { get; set; }
        public bool IsApplicationCompleted { get; set; }
        public bool IsRecommendInterview { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public int ApplicantResumeId { get; set; }
        public int? ApplicantRejectionReasonId { get; set; }
        public int OrigPostingId { get; set; }
        public int? CoverLetterId { get; set; }
        public string CoverLetter { get; set; }
        public ApplicantStatusType? ApplicantStatusTypeId { get; set; }
        public IEnumerable<ApplicantApplicationDetailDto> Resources { get; set; }
        public IEnumerable<ApplicantDocumentDto> ApplicantDocuments { get; set; }
        public bool AddedByAdmin { get; set; }		
        public string AddedByUserName { get; set; }
        public bool IsExternalApplicant { get; set; }		
        public string JobSiteName { get; set; }
        public int? Score { get; set; }
        public int? DisclaimerId { get; set; }

        public virtual ApplicantPostingDto ApplicantPosting { get; set; }
        public virtual ApplicantCompanyCorrespondenceDto ApplicantCompanyCorrespondence { get; set; }
        public virtual ApplicantDto Applicant { get; set; }
    }
}