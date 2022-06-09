using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Labor;

namespace Dominion.Core.Dto.TimeClock
{
    public class ClockRoundingTypeDto
    {
        public PunchRoundingType ClockRoundingTypeId { get; set; }
        public string Description { get; set; }
        public double? Minutes { get; set; }
        public byte? RoundDirection { get; set; }
    }
}
