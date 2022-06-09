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
    public class ReviewRemark : Entity<ReviewRemark>
    {
        public int RemarkId { get; set; }
        public int ReviewId { get; set; }
        public virtual Remark Remark { get; set; }
        public virtual Review Review { get; set; }
    }
}
