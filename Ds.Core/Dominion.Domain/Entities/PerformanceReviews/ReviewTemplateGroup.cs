using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Core;
using Dominion.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.PerformanceReviews
{
    public class ReviewTemplateGroup : Entity<ReviewTemplateGroup>, IHasModifiedData
    {
        public int ReviewTemplateId { get; set; }
        public int GroupId { get; set; }
        public virtual ReviewTemplate ReviewTemplate { get; set; }
        public virtual Group Group { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime Modified { get; set; }
    }
}
