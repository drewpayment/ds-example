using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public class TurnoverRateDto
    {
        public int StartCount { get; set; }
        public int EndCount { get; set; }
        public int TermedCount { get; set; }
        public int NewCount { get; set; }
        public int HiredAndTermedCount { get; set; }
    }
}
