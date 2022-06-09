using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.Sproc
{
    public class GetClockClientNoteListResultDto
    {
        public string Note { get; set; }
        public int ClockClientNoteID { get; set; }
        public bool isActive { get; set; }
    }
}
