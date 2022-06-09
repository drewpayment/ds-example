using System;
using System.Collections.Generic;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    [Serializable]
    public class ApplicantCompanyApplicationDto
    {
        public int CompanyApplicationId { get; set; }
        public string Description { get; set; }
        public int ClientId { get; set; }
        public bool IsEnabled { get; set; }
        public int Education { get; set; }
        public int ReferenceNo { get; set; }
        public bool IsCurrentEmpApp { get; set; }
        public bool IsExcludeHistory { get; set; }
        public bool IsExcludeReferences { get; set; }
        public bool IsFlagVolResign { get; set; }
        public bool IsFlagNoEmail { get; set; }
        public bool IsFlagReferenceCheck { get; set; }
        public int YearsOfEmployment { get; set; }
        public bool? IsExperience { get; set; }

        public List<ApplicationQuestionSectionWithQuestionDetailDto> QuestionSections { get; set; }
        public List<ApplicantQuestionSetsDto> ApplicantQuestionSets { get; set; }
    }
}