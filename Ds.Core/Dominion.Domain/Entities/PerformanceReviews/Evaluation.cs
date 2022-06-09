using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Performance;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Core;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.PerformanceReviews
{
    /// <summary>
    /// This is the core entity that Users interact with when filling out the form to complete an <see cref="Evaluation"/>
    /// </summary>
    public partial class Evaluation : Entity<Evaluation>, IHasModifiedData
    {
        public int                EvaluationId              { get; set; } 
        public int                ReviewId                  { get; set; } 
        /// <summary>
        /// The user with the task of filling out the evaluation.
        /// </summary>
        public int?               EvaluatedByUserId         { get; set; } 
        public int?               CurrentAssignedUserId     { get; set; }
        public EvaluationRoleType EvaluationRoleTypeId      { get; set; } 
        public DateTime?          StartDate                 { get; set; } 
        public DateTime           DueDate                   { get; set; } 
        public decimal?           OverallRatingValue        { get; set; } 
        public DateTime?          CompletedDate             { get; set; } 
        public int?               SignatureId               { get; set; } 
        public bool?              IsViewableByEmployee      { get; set; }
        public bool               AllowGoalWeightAssignment { get; set; }
        public string             JsonData                  { get; set; }
        public DateTime           Modified                  { get; set; } 
        public int                ModifiedBy                { get; set; } 

        public virtual ICollection<CompetencyEvaluation> CompetencyEvaluations { get; set; } 
        public virtual ICollection<GoalEvaluation> GoalEvaluations { get; set; } 
        public virtual ICollection<EvaluationFeedbackResponse> EvaluationFeedbackResponses { get; set; }
        public virtual Review Review { get; set; } 
        public virtual EvaluationRoleTypeInfo EvaluationRoleTypeInfo { get; set; } 
        public virtual User.User EvaluatedByUser { get; set; }
        public virtual User.User CurrentAssignedUser { get; set; }
        public virtual Signature Signature { get; set; }
        public virtual ICollection<ApprovalProcessHistory> ApprovalProcessHistory { get; set; }
        public virtual ICollection<Signature> Signatures { get; set; }
		public virtual ICollection<EvaluationReminder> Reminders { get; set; }
        public virtual ICollection<EvaluationGroupEvaluation> EvaluationGroupEvaluations { get; set; }
    }
}