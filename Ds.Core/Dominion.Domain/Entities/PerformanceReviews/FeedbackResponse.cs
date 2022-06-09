using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Performance;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.PerformanceReviews
{
    public partial class FeedbackResponse : Entity<FeedbackResponse>, IHasModifiedData
    {
        public int      ResponseId           { get; set; } 
        public int      FeedbackId           { get; set; } 
        public int?     ResponseByUserId     { get; set; } 
        public int?     ResponseByEmployeeId { get; set; } 
        public string   JsonData             { get; set; } 
        public short?   OrderIndex           { get; set; }
        public DateTime Modified             { get; set; } 
        public int      ModifiedBy           { get; set; } 
        public ApprovalProcessStatus? ApprovalProcessStatusId { get; set; }
        public bool IsEditedByApprover { get; set; }

        //REVERSE NAVIGATION
        public virtual EvaluationFeedbackResponse        EvaluationFeedbackResponse { get; set; } // one-to-one EvaluationFeedbackResponse.FK_EvaluationFeedbackResponse_FeedbackResponse;
        public virtual ICollection<FeedbackResponseItem> ResponseItems              { get; set; } // many-to-one;

        //FOREIGN KEYS
        public virtual Feedback Feedback { get; set; } 
        public virtual User.User ResponseByUser { get; set; }
        public virtual Employee.Employee ResponseByEmployee { get; set; }
    }
}
