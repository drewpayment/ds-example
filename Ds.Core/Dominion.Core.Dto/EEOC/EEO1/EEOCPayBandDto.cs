using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.EEOC.EEO1
{
    public class EEOCPayBandDto
    {
        public int PayBand { get; set; }
        public decimal LowerThreshold { get; set; }
        public decimal UpperThreshold { get; set; }
    }
}
