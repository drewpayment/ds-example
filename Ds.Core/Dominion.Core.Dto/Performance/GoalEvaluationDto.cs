using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Core;

namespace Dominion.Core.Dto.Performance
{
    public class GoalEvaluationDto
    {
        public int       GoalId           { get; set; }
        public int       EvaluationId     { get; set; }
        public string    Title            { get; set; }
        public string    Description      { get; set; }
        public DateTime? StartDate        { get; set; }
        public DateTime? DueDate          { get; set; }
        public DateTime? CompletionDate   { get; set; }
        public decimal   Progress         { get; set; }
        public decimal?  RatingValue      { get; set; }
        public RemarkDto Comment          { get; set; }
        public DateTime  Modified         { get; set; }
        public decimal?  Weight           { get; set; }
        public string   JsonData          { get; set; }
        public int?     RemarkId          { get; set; }
        public bool     IsCommentRequired { get; set; }
        public string   OneTimeEarningName { get; set; }
        public ApprovalProcessStatus? ApprovalProcessStatusId { get; set; }
        public bool IsEditedByApprover { get; set; }

        //FOREIGN KEYS
        public virtual GoalDto       Goal       { get; set; } 
        public virtual EvaluationDto Evaluation { get; set; } 
        public virtual RemarkDto     Remark     { get; set; }
        public virtual IEnumerable<RemarkDto> ActivityFeed { get; set; }
        public virtual IEnumerable<TaskDto> Tasks { get; set; }
    }
}