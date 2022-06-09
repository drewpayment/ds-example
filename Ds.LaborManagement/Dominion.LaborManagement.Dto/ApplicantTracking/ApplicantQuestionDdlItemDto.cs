namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public class ApplicantQuestionDdlItemDto
    {
        public int ApplicantQuestionDdlItemId { get; set; }
        public int QuestionId { get; set; }
        public string Description { get; set; }
        public bool IsDefault { get; set; }
        public string Value { get; set; }
        public int? FlaggedResponse { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsEditing { get; set; } = false;
        public string OldDescription { get; set; }
    }
}