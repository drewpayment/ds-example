using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Performance
{
    public class RecommendedBonusDto
    {
        /// <summary>
        /// Number of completed goals of a <see cref="Review"/> that are included in one time earnings
        /// </summary>
        public int Complete { get; set; }
        /// <summary>
        /// Number of total goals of a <see cref="Review"/> that are included in one time earnings
        /// </summary>
        public int Total { get; set; }
        public decimal? TargetIncreaseAmount { get; set; }
        public IncreaseType? TargetIncreaseType { get; set; }
    }
}
