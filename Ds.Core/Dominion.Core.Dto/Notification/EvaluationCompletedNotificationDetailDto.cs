using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Notification
{
    public class EvaluationCompletedNotificationDetailDto
    {
        public int EvaluationId { get; set; }
        public int ReviewId { get; set; }
        public int ReviewedEmployeeId { get; set; }
        public int EvaluatedByUserId { get; set; }
        public string ReviewTitle { get; set; }
        public string EvaluatedEmployeeName { get; set; }
        public string EvaluationCompletedBy { get; set; }
        public DateTime? CompletedDate { get; set; }
    }
}
