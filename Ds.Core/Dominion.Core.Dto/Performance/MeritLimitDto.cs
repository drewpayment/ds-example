using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Performance
{
    public class MeritLimitDto
    {
        public string Label { get; set; }
        public decimal? MinScore { get; set; }
        public decimal? MaxScore { get; set; }
        public decimal? MeritPercent { get; set; }
    }
}
