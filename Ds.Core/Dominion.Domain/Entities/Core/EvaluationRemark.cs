using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.PerformanceReviews;

namespace Dominion.Domain.Entities.Core
{
    public class EvaluationRemark : Entity<EvaluationRemark>
    {
        public int EvaluationID { get; set; }
        public int RemarkId     { get; set; }

        public virtual Evaluation Evaluation { get; set; }
        public virtual Remark     Remark     { get; set; }

    }
}
