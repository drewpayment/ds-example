using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.Sproc
{
    public class GetClockEmployeeAllocatedHoursDifferenceDto
    {
        public int EmployeeId { get; set; }
        public DateTime EventDate { get; set; }
        public int AllocatedHoursDifference { get; set; }
    }
}
