using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Contact.Search;
using Dominion.Core.Dto.Core;

namespace Dominion.Core.Dto.Performance
{
    public class EvaluationDetailDto : EvaluationDto
    {
        public IEnumerable<ReviewRatingDto>           Ratings                    { get; set; }
        public ContactSearchDto                       ReviewedEmployeeContact    { get; set; }
        public ContactSearchDto                       CurrentEmployeeContact     { get; set; }
        public IEnumerable<CompetencyEvaluationDto>   CompetencyEvaluations      { get; set; }
        public IEnumerable<GoalEvaluationDto>         GoalEvaluations            { get; set; }
        public IEnumerable<IHasFeedbackResponseData>  FeedbackResponses          { get; set; }
        public MeritIncreaseViewDto                   MeritIncreaseInfo          { get; set; }
        public bool                                   IsApprovalProcess          { get; set; }
        public ApprovalProcess                        ApprovalProcessAction      { get; set; }
        public IEnumerable<ApprovalProcessHistoryDto> ApprovalProcessHistory     { get; set; }

        // these probably dont belong here but are convenient 
        public IEnumerable<GoalRateCommentRequiredDto>       GoalRateCommentRequired       { get; set; }
        public IEnumerable<CompetencyRateCommentRequiredDto> CompetencyRateCommentRequired { get; set; }
        public IEnumerable<EvaluationGroupEvaluationDto> EvaluationGroupEvaluations { get; set; }
    }
}