using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Performance;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.PerformanceReviews
{
    public partial class ReviewProfileEvaluation : Entity<ReviewProfileEvaluation>, IHasModifiedData
    {
        public virtual int                ReviewProfileEvaluationId { get; set; } 
        public virtual int                ReviewProfileId           { get; set; } 
        public virtual EvaluationRoleType EvaluationRoleTypeId      { get; set; } 
        public virtual string             Instructions              { get; set; } 
        public virtual bool               IncludeCompetencies       { get; set; } 
        public virtual bool               IncludeGoals              { get; set; } 
        public virtual bool               IncludeFeedback           { get; set; } 
        public virtual bool               IsGoalsWeighted           { get; set; }
        public virtual bool               IsSignatureRequired       { get; set; } 
        public virtual bool               IsDisclaimerRequired      { get; set; } 
        public virtual bool               IsApprovalProcess         { get; set; }
        public virtual string             DisclaimerText            { get; set; } 
        public virtual bool               IsActive                  { get; set; }
        public virtual DateTime           Modified                  { get; set; } 
        public virtual int                ModifiedBy                { get; set; }
        public virtual int?               CompentencyOptionId       { get; set; }
        public virtual int?               GoalOptionId              { get; set; }

        //REVERSE NAVIGATION
        public virtual ICollection<ReviewProfileEvaluationFeedback> ReviewProfileEvaluationFeedback { get; set; } // many-to-many ReviewProfileEvaluationFeedback.FK_ReviewProfileEvaluationFeedback_ReviewProfileEvaluation;
        public virtual ICollection<EvaluationTemplate> EvaluationTemplates { get; set; }

        //FOREIGN KEYS
        public virtual ReviewProfile          ReviewProfile          { get; set; } 
        public virtual EvaluationRoleTypeInfo EvaluationRoleTypeInfo { get; set; }
        public virtual CompetencyOptions      CompetencyOptions      { get; set; }
        public virtual GoalOptions            GoalOptions            { get; set; }
    }
}