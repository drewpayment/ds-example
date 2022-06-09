using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Performance;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Core;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.PerformanceReviews
{
    /// <summary>
    /// Connects an instance of an evaluation to a specific competency.  This stores how
    /// well the employee was rated for the competency.
    /// </summary>
    public partial class CompetencyEvaluation : Entity<CompetencyEvaluation>, IHasModifiedData
    {
        public int      CompetencyId { get; set; } 
        public int      EvaluationId { get; set; } 
        public decimal? RatingValue  { get; set; } 
        /// <summary>
        /// The original state of the competency.  We store the original state because
        /// the competency may change after the evaluator has supplied a rating or comment.
        /// </summary>
        public string   JsonData     { get; set; } 
        public int?     RemarkId     { get; set; } 
        public DateTime Modified     { get; set; } 
        public int      ModifiedBy   { get; set; }
        public ApprovalProcessStatus? ApprovalProcessStatusId { get; set; }
        public bool IsEditedByApprover { get; set; }

        //FOREIGN KEYS
        public virtual Competency Competency { get; set; } 
        public virtual Evaluation Evaluation { get; set; }
        /// <summary>
        /// Justification/explanation of the <see cref="RatingValue"/>
        /// </summary>
        public virtual Remark Remark { get; set; }
        public virtual ICollection<Remark> Remarks { get; set; }

    }
}