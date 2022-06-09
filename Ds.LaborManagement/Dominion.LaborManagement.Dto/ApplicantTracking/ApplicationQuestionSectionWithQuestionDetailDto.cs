using System.Collections.Generic;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{

    public abstract class QuestionSectionDto
    {
        public int SectionId { get; set; }
        public string Description { get; set; }
        public string Instruction { get; set; }
        public int DisplayOrder { get; set; }
        public int ClientId { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsSelected { get; set; } = false;
        public int TotalQuestions { get; set; }
        public int TotalSelctedQuestions { get; set; }
        public bool IsAlreadyUsed { get; set; }
    }
    public class ApplicationQuestionSectionWithQuestionDetailDto : QuestionSectionDto
    {
        public ICollection<ApplicantQuestionControlWithDetailDto> Questions { get; set; }
    }

    public class ApplicationQuestionSectionDto : QuestionSectionDto
    {
        public IEnumerable<ApplicantQuestionControlDto> Questions { get; set; }
    }
}