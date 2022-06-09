using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Core;

namespace Dominion.Core.Dto.Performance
{
    public class CompetencyEvaluationDto
    {
        public int       CompetencyId    { get; set; }
        public int       EvaluationId    { get; set; }
        public string    GroupName       { get; set; }
        public int?       GroupId         { get; set; }
        public string    Name            { get; set; }
        public string    Description     { get; set; }
        public int?      DifficultyLevel { get; set; }
        public decimal?  RatingValue     { get; set; }
        public RemarkDto Comment         { get; set; }
        public bool      IsCore          { get; set; }
        public DateTime  Modified        { get; set; }
        public int      ModifiedBy   { get; set; }
        public string   JsonData     { get; set; } 
        public bool IsCommentRequired { get; set; }
        public ApprovalProcessStatus? ApprovalProcessStatusId { get; set; }
        public bool IsEditedByApprover { get; set; }

        public IEnumerable<RemarkDto> ActivityFeed { get; set; }
    }
}