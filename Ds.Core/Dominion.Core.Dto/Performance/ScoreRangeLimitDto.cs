using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Performance
{
    public class ScoreRangeLimitDto
    {
        public int ScoreModelRangeId { get; set; }
        public int ScoreModelId { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public decimal? MaxScore { get; set; }
        public decimal? MeritPercent { get; set; }
        public decimal? MinScore { get; set; } // hard coding
    }
}
