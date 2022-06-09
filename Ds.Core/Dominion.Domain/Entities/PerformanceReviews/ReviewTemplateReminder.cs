using Dominion.Core.Dto.Misc;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.PerformanceReviews
{
    /// <summary>
    /// Determines when Evaluators should be notified that they have an evaluation to complete.
    /// </summary>
    public class ReviewTemplateReminder : Entity<ReviewTemplateReminder>, IHasModifiedData
    {
        public int ReviewTemplateReminderID { get; set; }
        public int ReviewTemplateId { get; set; }
        public int DurationPrior { get; set; }
        public DateUnit DurationPriorUnitTypeId { get; set; }

        public virtual ReviewTemplate ReviewTemplate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime Modified { get; set; }
    }
}
