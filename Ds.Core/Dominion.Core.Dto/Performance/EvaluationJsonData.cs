using System.Collections.Generic;

namespace Dominion.Core.Dto.Performance
{
    public class EvaluationJsonData
    {
        public class RatingInfoData
        {
            public IEnumerable<RatingItem> Ratings { get; set; }
        }

        public class RatingItem
        {
               public int    Rating      { get; set; }
               public string Label       { get; set; }
               public string Description { get; set; }
               public bool    CompetencyRequiresComment { get; set; }
               public bool GoalRequiresComment { get; set; }
        }

        public RatingInfoData RatingInfo { get; set; }
    }
}