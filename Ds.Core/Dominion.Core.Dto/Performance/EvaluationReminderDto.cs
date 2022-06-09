using Dominion.Core.Dto.Misc;
using Dominion.Core.Dto.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Performance
{
    public class EvaluationReminderDto
    {
        public int EvaluationReminderId { get; set; }
        public int EvaluationId { get; set; }
        public int DurationPrior { get; set; }
        public DateUnit DurationPriorUnitTypeId { get; set; }

        public IEnumerable<NotificationDeliveryDto> SentNotifications { get; set; }
    }
}
