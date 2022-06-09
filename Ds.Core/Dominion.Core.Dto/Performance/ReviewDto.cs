using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Contact.Search;
using Dominion.Core.Dto.Core;
using Dominion.Core.Dto.Employee;

namespace Dominion.Core.Dto.Performance
{
    public class ReviewDto : BaseReviewDto
    {
        public int       ReviewedEmployeeId       { get; set; }
        public ReviewContactSearchDto ReviewedEmployeeContact { get; set; }
        public virtual IEnumerable<RemarkDto> Remarks { get; set; }
    }

    public class ReviewWithEmployeesDto : BaseReviewDto
    {
        public List<int> ReviewedEmployeeIds { get; set; }
        public List<ReviewContactSearchDto> ReviewedEmployeeContacts { get; set; }
    }

    public class BaseReviewDto
    {
        public int       ReviewId { get; set; }
        public int?      ClientId { get; set; }
        public int?      ReviewOwnerUserId { get; set; }
        public int?      ReviewProfileId { get; set; }
        public int?      ReviewTemplateId { get; set; }
        public string    Title { get; set; }
        public DateTime? EvaluationPeriodFromDate { get; set; }
        public DateTime? EvaluationPeriodToDate { get; set; }
        public DateTime? ReviewProcessStartDate { get; set; }
        public DateTime? ReviewProcessDueDate { get; set; }
        public decimal?  OverallRatingValue { get; set; }
        public string    Instructions { get; set; }

        public DateTime? ReviewCompletedDate { get; set; }
        public bool      IsReviewMeetingRequired { get; set; }
        public bool      IsGoalsWeighted { get; set; }
        public DateTime? PayrollRequestEffectiveDate { get; set; }
        public decimal? OverallScore { get; set; }

        public ContactSearchDto              ReviewOwnerContact { get; set; }
        public IEnumerable<EvaluationDto>    Evaluations { get; set; }
        public IEnumerable<ReviewMeetingDto> Meetings { get; set; }
        public IEnumerable<MeritIncreaseDto> MeritIncreases { get; set; }
        public ProposalDto Proposal { get; set; }
        public int? CompletedGoals { get; set; }
        public int? TotalGoals { get; set; }
        public IEnumerable<EmployeeNotesDto> EmployeeNotes { get; set; }
    }

    public class ReviewDateDto
    {
        public DateTime? EvaluationPeriodFromDate { get; set; }
        public DateTime? EvaluationPeriodToDate { get; set; }
    }
}