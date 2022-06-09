using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Core
{
    public class TaskDto
    {
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
        public GoalPriority? Priority { get; set; }
        public decimal? Weight { get; set; }

    }
}
