using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Notification
{
    public class EvaluationSharedWithEmployeeNotificationDetailDto
    {
        public int EvaluationId { get; set; }
        public int ReviewId { get; set; }
        public string ReviewTitle { get; set; }
        public int? EmployeeId { get; set; }
        public string EmployeeFirstName { get; set; }
        public string EmployeeLastName { get; set; }
        public DateTime? CompletedDate { get; set; }
    }
}
