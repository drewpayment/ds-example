using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.PerformanceReviews
{
    public partial class ReviewProfileEvaluationFeedback : Entity<ReviewProfileEvaluationFeedback>
    {
        public virtual int   ReviewProfileEvaluationId { get; set; } 
        public virtual int   FeedbackId                { get; set; } 
        public virtual short OrderIndex                { get; set; } 

        //FOREIGN KEYS
        public virtual ReviewProfileEvaluation ReviewProfileEvaluation { get; set; } 
        public virtual Feedback Feedback { get; set; } 
    }
}