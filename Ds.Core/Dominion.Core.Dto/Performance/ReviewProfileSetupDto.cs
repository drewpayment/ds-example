using System.Collections.Generic;

namespace Dominion.Core.Dto.Performance
{
    public class ReviewProfileSetupDto : ReviewProfileBasicSetupDto
    {
        public IEnumerable<ReviewProfileEvaluationDto> Evaluations { get; set; }
        public IEnumerable<ReviewRatingDto> ReviewRatings { get; set; }
    }
}