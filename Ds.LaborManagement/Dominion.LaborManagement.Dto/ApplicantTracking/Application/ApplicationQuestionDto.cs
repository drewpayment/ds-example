using System.Collections.Generic;
using Dominion.Core.Dto.Core;

namespace Dominion.LaborManagement.Dto.ApplicantTracking.Application
{
    public class ApplicationQuestionDto
    {
        public int    QuestionId      { get; set; }
        public int    SectionId       { get; set; }
        public int    DisplayOrder    { get; set; }
        public string QuestionText    { get; set; }
        public string ResponseTitle   { get; set; }
        public bool   IsEnabled       { get; set; }
        public bool   IsFlagged       { get; set; }
        public FieldType    FieldTypeId     { get; set; }
        public bool   IsRequired      { get; set; }
        public string FlaggedResponse { get; set; }
        public int? SelectionCount  { get; set; }
        public IEnumerable<QuestionItemOptionDto> Items { get; set; }
    }
}