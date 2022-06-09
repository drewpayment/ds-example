namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public class ApplicantApplicationEmailHistoryDto
    {
        public int ApplicantApplicationEmailHistoryId { get; set; }
        public int ApplicationHeaderId { get; set; }
        public bool? isText { get; set; }
        public int? ApplicantCompanyCorrespondenceId { get; set; }
        public string CorrespondenceSubject { get; set; }
        public string CorrespondenceTemplateName { get; set; }
        public ApplicantStatusType? ApplicantStatusTypeId { get; set; }
        public string SenderName { get; set; }
        public System.DateTime SentDate { get; set; }
        public string SenderEmail { get; set; }
    }
}