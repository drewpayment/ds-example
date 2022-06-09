using System.Collections.Generic;

namespace Dominion.Core.Dto.Performance
{
    public class EvaluationSyncOptions
    {
        public bool             IncludeCompetencies       { get; set; }
        public bool             IncludeGoals              { get; set; }
        public bool             AllowGoalWeightAssignment { get; set; }
        public bool             IncludeFeedback           { get; set; }
        public IEnumerable<FeedbackWithOrder> LimitedFeedback { get; set; }
    }
}