using System.Collections.Generic;
using Dominion.Core.Dto.Core;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public class ApplicantQuestionControlDto
    {
        public int QuestionId { get; set; }
        public string Question { get; set; }
        public string ResponseTitle { get; set; }
        public FieldType FieldTypeId { get; set; }
        public int SectionId { get; set; }
        public int ClientId { get; set; }
        public bool IsFlagged { get; set; }
        public bool IsRequired { get; set; }
        public string FlaggedResponse { get; set; }
        public bool IsEnabled { get; set; }
        public int OrderId { get; set; }
        public bool IsSelected { get; set; } = false; // This field indicates whether the question is shown to the applicant
        public bool IsActivated { get; set; } = false;  // This field indicates whether the question was clicked on?
        public bool IsReferred { get; set; } = false;  // This field indicates whether the question was answered by any applicant
        public int? SelectionCount { get; set; }

        public List<ApplicantQuestionDdlItemDto> ApplicantQuestionDdlItem { get; set; }
    }

    public class ApplicantQuestionControlWithDetailDto : ApplicantQuestionControlDto
    {
        public ApplicantApplicationDetailDto detail { get; set; }
    }
}