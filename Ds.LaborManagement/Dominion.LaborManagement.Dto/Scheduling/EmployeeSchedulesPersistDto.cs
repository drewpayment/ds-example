using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.Scheduling
{
    public class EmployeeSchedulesPersistDto
    {
        public DateTime     StartDate           { get; set; }
        public DateTime     EndDate             { get; set; }
        public int          ScheduleGroupId     { get; set; }

        /// <summary>
        /// This would be the cost center id for a cost center schedule group.
        /// </summary>
        public int          ScheduleSourceId    { get; set; }

        public IEnumerable<EmployeeSchedulesDto> EmployeeSchedulesDtos { get; set; }
    }
}
