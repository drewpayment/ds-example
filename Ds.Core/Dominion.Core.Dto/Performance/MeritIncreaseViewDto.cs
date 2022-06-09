using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Employee;
using Dominion.Core.Dto.Payroll;

namespace Dominion.Core.Dto.Performance
{
    public class MeritIncreaseViewDto
    {
        public DateTime? PayrollRequestEffectiveDate { get; set; }
        public decimal? RecommendedMeritPercent { get; set; }
        public bool CanViewRates { get; set; }
        public ICollection<MeritIncreaseCurrentPayAndRatesDto> CurrentPayInfo { get; set; }
        public IEnumerable<MeritLimitDto> MeritRecommendations { get; set; }
        public OneTimeEarningDto OneTimeEarning { get; set; }
        public int? ReviewId { get; set; }

    }
}
