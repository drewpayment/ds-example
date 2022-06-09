using System;
using System.Collections.Generic;

namespace Dominion.LaborManagement.Dto.ApplicantTracking.Application
{
    public class ApplicationQuestionResponseDto
    {
        public int    ApplicationDetailId { get; set; }
        public bool   IsFlagged           { get; set; }
        public int?   QuestionId          { get; set; }
        public int    SectionId           { get; set; }
        public string ResponseValue       { get; set; }
    }

    public partial class ApplicationQuestionFilterDto
    {
        public int ClientId { get; set; }
        public int? QuestionId { get; set; }
        public string QuestionText { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? PostingId { get; set; }
    }

    public partial class ApplicationQuestionResponseNumbersDto
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public IDictionary<string, int> Responses { get; set; }
    }
}