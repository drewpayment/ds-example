using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Notification
{
    public class ApprovalProcessNotificationDetailDto
    {
        public int       EvaluationId       { get; set; }
        public int       ReviewId           { get; set; }
        public string    ReviewTitle        { get; set; }
        public int?      EmployeeId         { get; set; }
        public int?      EvaluatorId        { get; set; }
        public int?      ApproverId         { get; set; }
        public string    EmployeeFirstName  { get; set; }
        public string    EmployeeLastName   { get; set; }
        public string    EvaluatorFirstName { get; set; }
        public string    EvaluatorLastName  { get; set; }
        public string    ApproverFirstName  { get; set; }
        public string    ApproverLastName   { get; set; }
        public DateTime? CompletedDate      { get; set; }
    }
}
