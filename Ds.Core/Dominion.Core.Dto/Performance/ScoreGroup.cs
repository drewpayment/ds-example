using System.Collections.Generic;

namespace Dominion.Core.Dto.Performance
{
    public class ScoreGroup : WeightedScoreItem
    {
        public int? EvaluationGroupId { get; set; }
        public int? ParentId          { get; set; }
        public int? CompetencyGroupId { get; set; }
        public bool IsGoals           { get; set; }
        public bool IsCompetencies    { get; set; }

        public IEnumerable<WeightedScoreItem> Items { get; set; }
    }
}