using Dominion.Core.Dto.Performance;

namespace Dominion.Core.Dto.Performance
{
    public class WeightedScoreItem
    {
        public string Name { get; set; }
        public decimal? Score { get; set; }
        public decimal? Weight { get; set; }
        public decimal? WeightPercent { get; set; }
        public decimal? WeightedScore { get; set; }
        public ScoreModelEvalInfoDto evalInfo { get; set;}

        public static WeightedScoreItem ToWeightedScoreItem(GoalEvaluationDto g)
        {
            return new WeightedScoreItem
            {
                Name = g.Title,
                Weight = g.Weight,
                Score = g.RatingValue
            };
        }

        public static WeightedScoreItem ToWeightedScoreItemWithComment(GoalEvaluationDto g)
        {
            return new WeightedScoreItem
            {
                Name = g.Title,
                Weight = g.Weight,
                Score = g.RatingValue,
               evalInfo = new ScoreModelEvalInfoDto
               {
                   Comments = g.Comment,
                   Id = "evalId:" + g.EvaluationId + ",goalId:" + g.GoalId,
                   Description = g.Description,
                   GroupName = "Goals"
               }
            };
        }

        public static WeightedScoreItem ToWeightedScoreItem(CompetencyEvaluationDto c)
        {
            return new WeightedScoreItem
            {
                Name = c.Name,
                Score = c.RatingValue
            };
        }

        public static WeightedScoreItem ToWeightedScoreItemWithComment(CompetencyEvaluationDto c)
        {
            return new WeightedScoreItem
            {
                Name = c.Name,
                Score = c.RatingValue,
                evalInfo = new ScoreModelEvalInfoDto
                {
                    Comments = c.Comment,
                    Id = "evalId:" + c.EvaluationId + ",compId:" + c.CompetencyId,
                    Description = c.Description,
                    GroupName = c.GroupName ?? "Competencies"
                }
            };
        }

    }
}