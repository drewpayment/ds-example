using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.TimeCard
{
    public class TimeCardPunchRow
    {
        public string EventDate { get; set; }
        public string Hours { get; set; }
        public ICollection<TimeCardPunch> Punches { get; set; }

    }
}
