using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Performance;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.PerformanceReviews
{
    /// <summary>
    /// Reflects the transition of an evaluation through an Approval Process
    /// </summary>
    public class ApprovalProcessHistory : Entity<ApprovalProcessHistory>
    {
        public virtual int             ApprovalProcessHistoryId { get; set; }
        /// <summary>
        /// The next person to handle the evaluation
        /// </summary>
        public virtual int             ToUserId            { get; set; } 
        /// <summary>
        /// Ther person who did the <see cref="Action"/>
        /// </summary>
        public virtual int?            FromUserId          { get; set; }
        public virtual int             EvaluationId        { get; set; }
        public virtual ApprovalProcess Action              { get; set; }


        public virtual Dominion.Domain.Entities.User.User ToUser { get; set; }
        public virtual Dominion.Domain.Entities.User.User FromUser { get; set; }
        public virtual Evaluation Evaluation { get; set; }


    }

    
}
