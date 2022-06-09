using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Client;
using Dominion.Core.Dto.Contact.Search;
using Dominion.Core.Dto.Employee;
using Dominion.Core.Dto.Performance;

namespace Dominion.Core.Dto.Core
{
    public class GoalDto : TaskDto
    {
        public int GoalId { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public bool IncludeReview { get; set; }
        public bool IsCompanyGoal { get; set; }
        public bool IsAlignedToCompanyGoal { get; set; }
        public string AlignedCompanyGoalName { get; set; }

        // RELATIONSHIPS

        public virtual EmployeeBasicDto Employee { get; set; }
        public virtual ClientDto Client { get; set; }
        public virtual IEnumerable<TaskDto> Tasks { get; set; }
        public virtual IEnumerable<RemarkDto> Remarks { get; set; }
        public virtual ContactSearchDto GoalOwner { get; set; }
        public virtual OneTimeEarningSettingsDto OneTimeEarningSettings { get; set; }
        public GoalPriority? Priority { get; set; }

    }

    public class GoalWithSubGoalDto : GoalDto
    {
        public virtual IEnumerable<GoalDto> SubGoals { get; set; }
    }

    public class GoalSavingDto : GoalDto
    {
        public bool IsAligned { get; set; }
        public GoalWithSubGoalDto AlignedGoal { get; set; }
    }
    public class EmployeeGoalToParentGoalDto
    {
        // The parent can be either Employee or Company Goal
        public int EmployeeGoalId { get; set; }
        public int ParentEmployeeId { get; set; }
        public int ParentEmployeeGoalId { get; set; }
        public string ParentEmployeeGoalName { get; set; }
        public int CompanyGoalId { get; set; }
        public int CompanyId { get; set; }
        public string CompanyGoalName { get; set; }
    }
}
