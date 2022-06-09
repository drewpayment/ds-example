using System.Collections.Generic;

namespace Dominion.LaborManagement.Dto.ApplicantTracking.Application
{
    public class ApplicationSectionWithQuestionsDto
    {
        public int    SectionId    { get; set; }
        public string Description  { get; set; }
        public string Instruction { get; set; }
        public int    DisplayOrder { get; set; }
        public bool   IsEnabled    { get; set; }

        public IEnumerable<ApplicationQuestionDto> Questions { get; set; }
    }   
}