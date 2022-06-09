using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Core
{
    public class GoalRemark : Entity<GoalRemark>
    {
        public int GoalId { get; set; }
        public int RemarkId { get; set; }

        // RELATIONSHIPS

        public virtual ICollection<Goal> Goals { get; set; }
        public virtual ICollection<Remark> Remarks { get; set; }
    }
}
