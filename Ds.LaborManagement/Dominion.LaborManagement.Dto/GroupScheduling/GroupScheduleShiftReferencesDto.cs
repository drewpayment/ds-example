using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.GroupScheduling
{
    public class GroupScheduleShiftReferencesDto
    {
        public int              GroupScheduleShiftId      { get; set; }
        public IEnumerable<int> EmployeeDefaultShiftIds   { get; set; }
        public IEnumerable<int> EmployeSchedulePreviewIds { get; set; }
    }

}
