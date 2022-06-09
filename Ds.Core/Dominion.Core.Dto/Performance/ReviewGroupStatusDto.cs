using Dominion.Core.Dto.Misc;
using System;
using System.Collections.Generic;

namespace Dominion.Core.Dto.Performance
{
    public class ReviewGroupStatusDto
    {
        public int?   ReviewTemplateId   { get; set; }
        public string ReviewTemplateName { get; set; }
        public DateTime? ReviewProcessStartDate   { get; set; }
        public DateTime? ReviewProcessDueDate     { get; set; }
        public ReferenceDate ReferenceDateTypeId { get; set; }
        public int? DelayAfterReference { get; set; }
        public DateUnit? DelayAfterReferenceUnitTypeId { get; set; }
        public int? ReviewProcessDuration { get; set; }
        public DateUnit? ReviewProcessDurationUnitTypeId { get; set; }
        public int? SupervisorEvalDuration { get; set; }
        public DateUnit? SupervisorEvalDurationUnitTypeId { get; set; }
        public int? EvaluationPeriodDuration { get; set; }
        public DateTime? PayrollRequestEffectiveDate { get; set; }
        public DateTime? SupervisorEvalDueDate { get; set; }


        public IEnumerable<ReviewStatusDto> Reviews { get; set; }
    }
}