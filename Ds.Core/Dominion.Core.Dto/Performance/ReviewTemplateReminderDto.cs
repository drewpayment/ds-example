using Dominion.Core.Dto.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Performance
{
    public class ReviewTemplateReminderDto
    {
        public int ReviewReminderID { get; set; }
        public int ReviewTemplateId { get; set; }
        public int DurationPrior { get; set; }
        public DateUnit DurationPriorUnitTypeId { get; set; }
    }
}
