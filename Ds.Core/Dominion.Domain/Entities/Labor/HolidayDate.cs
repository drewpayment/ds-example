using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Labor
{
    public class HolidayDate
    {
        public int HolidayDateId { get; set; }
        public int HolidayId { get; set; }
        public DateTime DateObserved { get; set; }

        public virtual Holiday Holiday { get; set; }
    }
}
