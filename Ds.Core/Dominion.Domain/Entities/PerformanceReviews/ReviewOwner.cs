using Dominion.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.PerformanceReviews
{
    public class ReviewOwner : Entity<ReviewOwner>
    {
        public int ReviewTemplateId { get; set; }
        public int UserId { get; set; }
        public virtual User.User User { get; set; }
        public virtual ReviewTemplate ReviewTemplate { get; set; }
    }
}
