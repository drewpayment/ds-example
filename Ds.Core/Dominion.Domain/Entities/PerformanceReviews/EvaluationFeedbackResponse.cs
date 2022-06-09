using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Core;
using System.Collections.Generic;

namespace Dominion.Domain.Entities.PerformanceReviews
{
    public partial class EvaluationFeedbackResponse : Entity<EvaluationFeedbackResponse>
    {
        public int    ResponseId   { get; set; } 
        public int    EvaluationId { get; set; } 

        //FOREIGN KEYS
        public virtual FeedbackResponse FeedbackResponse { get; set; } 
        public virtual Evaluation Evaluation { get; set; }
        public virtual ICollection<Remark> Remarks { get; set; }
    }
}