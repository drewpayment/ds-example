using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Performance
{
    public class GoalRateCommentRequiredDto
    {
        public int OptionId { get; set; }
        public int ReviewRatingId { get; set; }
        public ReviewRatingDto ReviewRating { get; set; }
    }
}
