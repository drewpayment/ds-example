
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.PerformanceReviews;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Core
{
    public class Goal : Entity<Goal>, IHasModifiedOptionalData
    {
        public int GoalId { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public bool IncludeReview { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? Modified { get; set; }

        // RELATIONSHIPS
        public virtual ClientGoal ClientGoal { get; set; }
        public virtual EmployeeGoal EmployeeGoal { get; set; }
        public virtual Task Task { get; set; }
        //public virtual ICollection<EmployeeGoal> EmployeeGoals { get; set; }
        //public virtual ICollection<ClientGoal> ClientGoals { get; set; }
        public virtual ICollection<Remark> Remarks { get; set; }
        public virtual ICollection<GoalEvaluation> GoalEvaluations { get; set; }
    }
}
