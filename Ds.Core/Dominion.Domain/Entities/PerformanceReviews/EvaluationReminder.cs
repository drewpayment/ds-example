using Dominion.Core.Dto.Misc;
using Dominion.Core.Dto.Notification;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.PerformanceReviews
{
    public class EvaluationReminder : Entity<EvaluationReminder>
    {
        public int EvaluationReminderId { get; set; }
        public int EvaluationId { get; set; }
        public int DurationPrior { get; set; }
        public DateUnit DurationPriorUnitTypeId { get; set; }

        /// <summary>
        /// When a user chooses to receive both text and email notifications there may be more than one.
        /// </summary>
        public virtual ICollection<NotificationDelivery> SentNotifications { get; set; }
        public virtual Evaluation Evaluation { get; set; }
    }
}
