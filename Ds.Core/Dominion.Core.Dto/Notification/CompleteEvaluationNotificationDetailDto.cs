using System;
using Dominion.Core.Dto.Performance;

namespace Dominion.Core.Dto.Notification
{
    public class CompleteEvaluationNotificationDetailDto
    {
        public int                ReviewId            { get; set; }
        public string             ReviewTitle         { get; set; }
        public int                EvaluationId        { get; set; }
        public int                EvaluatedByUserId   { get; set; }
        public EvaluationRoleType Role                { get; set; }
        public DateTime?          EvaluationStartDate { get; set; }
        public DateTime           EvaluationDueDate   { get; set; }
        public int                ReviewedEmployeeId  { get; set; }
        public string             EmployeeFirstName   { get; set; }
        public string             EmployeeLastName    { get; set; }
        
}
}
