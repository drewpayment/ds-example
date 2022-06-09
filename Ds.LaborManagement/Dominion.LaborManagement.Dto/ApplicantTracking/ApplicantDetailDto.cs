using Dominion.LaborManagement.Dto.EmployeeLaborManagement;
using System;
namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public class ApplicantDetailDto
    {
        public int ApplicantId { get; set; }
        public string ApplicantName { get; set; }
        public string ApplicantNameFlipped { get; set; }
        public string Username { get; set; }
        public int OrigPostingId { get; set; }
        public int? EmployeeId { get; set; }
        public int PostingId { get; set; }
        public int PostingNo { get; set; }
        public string Posting { get; set; }
        public DateTime? PostingStartDate { get; set; }
        public DateTime? ApplicationSubmittedOn { get; set; }
        public bool IsFlagged { get; set; }
        public bool IsApplicantDenied { get; set; }
        public bool IsTextEnabled { get; set; }
        public int ApplicationHeaderId { get; set; }
        public bool IsRecommended { get; set; }
        public int ApplicantResumeId { get; set; }
        public DateTime? FilledOn { get; set; }
        public int ApplicantResumeRequiredId { get; set; }
        public string Note { get; set; }
        public int ApplicantOnBoardingProcessId { get; set; }
        public int? ApplicantRejectionReasonId { get; set; }
        public string RejectionReason { get; set; }
        public bool HasViewed { get; set; }
        public ApplicantStatusType? ApplicantStatusTypeId { get; set; }
        public int NoteCount { get; set; }
        public string ResumeLinkLocation { get; set; }
        public Dto.ApplicantTracking.ApplicantCorrespondenceType? ApplicantCorrespondenceTypeId { get; set; }
        public int SentEmailsCount { get; set; }
        public bool IsExternalApplicant { get; set; }
		public string JobSiteName { get; set; }
        public string CoverLetter { get; set; }		
        public int? CoverLetterId { get; set; }
        public bool AddedByAdmin { get; set; }
        public int? Score { get; set; }
        public int? DisclaimerId { get; set; }
        public string AddedByUserName { get; set; }
        public EmployeeHireInfoDto HireInfo { get; set; }
        public int DocumentCount { get; set; }
    }
}