using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.TimeClock
{
    public class ClockTimeFormat : Entity<ClockTimeFormat>
    {
        public int ClockTimeFormatId { get; set; }
        public string TimeFormat { get; set; }
    }
}
