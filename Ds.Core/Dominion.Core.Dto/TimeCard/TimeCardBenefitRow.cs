using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.TimeCard
{
    public class TimeCardBenefitRow
    {
        public string BenefitDescription { get; set; }
        public string BenefitDate { get; set; }
        public int RequestTimeOffId { get; set; }
    }
}
