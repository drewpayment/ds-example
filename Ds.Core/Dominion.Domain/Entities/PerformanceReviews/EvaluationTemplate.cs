using System;
using Dominion.Core.Dto.Misc;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.PerformanceReviews
{
    public partial class EvaluationTemplate : Entity<EvaluationTemplate>, IHasModifiedData
    {
        public virtual int       ReviewTemplateId       { get; set; } 
        public virtual int       ReviewProfileEvaluationId { get; set; } 
        public virtual DateTime? StartDate                 { get; set; } 
        public virtual DateTime? DueDate                   { get; set; }
        public virtual int? EvaluationDuration { get; set; }
        public virtual DateUnit? EvaluationDurationUnitTypeId { get; set; }
        public virtual int? ConductedBy { get; set; }

        //FOREIGN KEYS
        public virtual ReviewTemplate ReviewTemplate { get; set; } 
        public virtual ReviewProfileEvaluation ReviewProfileEvaluation { get; set; } 
        public virtual User.User ConductedByUser { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime Modified { get; set; }
    }
}