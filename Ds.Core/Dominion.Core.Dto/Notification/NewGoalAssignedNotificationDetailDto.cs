using System;

namespace Dominion.Core.Dto.Notification
{
    public class NewGoalAssignedNotificationDetailDto
    {
        public int?      UserId         { get; set; }
        public int?      EmployeeId     { get; set; }
        public int       ClientId       { get; set; }
        public int       GoalId         { get; set; }
        public string    Title          { get; set; }
        public DateTime  StartDate      { get; set; }
        public DateTime? DueDate        { get; set; }
        public bool      IsEmployeeGoal { get; set; }
        
    }
}
