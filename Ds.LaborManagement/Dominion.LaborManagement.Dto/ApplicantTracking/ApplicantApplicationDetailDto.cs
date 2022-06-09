namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public class ApplicantApplicationDetailDto
    {
        public int ApplicationHeaderId { get; set; }
        public int? QuestionId { get; set; }
        public string Response { get; set; }
        public string ResponseTitle { get; set; }
        public string QuestionName { get; set; }		
        public string ResponseFileName { get; set; }
        public bool IsFlagged { get; set; }
        public int SectionId { get; set; }
        public int ApplicationDetailId { get; set; }
    }
}