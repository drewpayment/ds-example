using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Core;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Onboarding;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Core
{
    public partial class Task : Entity<Task>, IHasModifiedOptionalData
    {
        public Task()
        {
            IsArchived = false;
        }

        public int TaskId { get; set; }
        public int? ParentId { get; set; }
        public string Description { get; set; }
        public decimal Progress { get; set; }
        public CompletionStatusType? CompletionStatus { get; set; }
        public DateTime? CompletionDate { get; set; }
        public int? CompletedBy { get; set; }
        public int? AssignedTo { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsArchived { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? Modified { get; set; }
        public GoalPriority? Priority { get; set; }

        // RELATIONSHIPS
        public virtual Goal Goal { get; set; }
        public virtual Task ParentTask { get; set; }
        public virtual ICollection<Task> ChildrenTasks { get; set; }
        public virtual User.User AssignedToUser { get; set; }
    }
}
