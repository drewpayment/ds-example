using System.Collections.Generic;

namespace Dominion.Core.Dto.Performance
{
    public class ReviewProfileEvaluationDto
    {
        public int                ReviewProfileEvaluationId   { get; set; }
        public int                ReviewProfileId             { get; set; }
        public EvaluationRoleType Role                        { get; set; }
        public string             Instructions                { get; set; }
        public bool               IncludeCompetencies         { get; set; }
        public bool               IncludeGoals                { get; set; }
        public bool               IncludeFeedback             { get; set; }
        public bool               IsGoalsWeighted             { get; set; }
        public bool               IsDisclaimerRequired        { get; set; }
        public string             DisclaimerText              { get; set; }
        public bool               IsSignatureRequired         { get; set; }
        public bool               IsActive                    { get; set; }
        public bool               IsCompetencyCommentRequired { get; set;}
        public bool               IsGoalCommentRequired       { get; set; }
        public bool               IsApprovalProcess           { get; set; }
        public int?               CompetencyOptionId          { get; set; }
        public int?               GoalOptionId                { get; set; }
        public bool               IncludePayrollRequests      { get; set; }
        public List<int> SelectedCompetenceyRatings { get; set; } = new List<int>();
        public List<int> SelectedGoalRatings { get; set; } = new List<int>();

        public IEnumerable<ReviewProfileEvaluationFeedbackDto> Feedback { get; set; }
    }
}