using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Contact;
using Dominion.Core.Dto.Core;
using Dominion.Core.Dto.Employee;

namespace Dominion.Core.Dto.Onboarding
{
    [Serializable]
    public class EmployeeOnboardingTasksDto
    {
        public int EmployeeOnboardingTaskId { get; set; }
        public int EmployeeId { get; set; }
        public string Description { get; set; }
        public string OldDescription { get; set; }
        public int? ParentTaskId { get; set; }
        public CompletionStatusType CompletionStatus { get; set; }
        public DateTime? DateCompleted { get; set; }
        public int? CompletedBy { get; set; }
        public int Sequence { get; set; }
        public DateTime? DueDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime Modified { get; set; }
        public int TaskId { get; set; } // Core.Task.TaskId
        public Task Task { get; set; }
        //REVERSE NAVIGATION
        //public ICollection<EmployeeOnboardingTasksDto> ChildTasks { get; set; } // many-to-one;
        //Foreign Keys
        //public EmployeeOnboardingTasksDto ParentTask { get; set; } // many-to-one;


    }
}
