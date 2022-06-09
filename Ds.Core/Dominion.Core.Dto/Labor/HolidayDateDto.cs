using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Labor
{
    public class HolidayDateDto
    {
        public int HolidayDateId { get; set; }
        public int HolidayId { get; set; }
        public DateTime DateObserved { get; set; }

        public virtual HolidayDto Holiday { get; set; }
    }
}
