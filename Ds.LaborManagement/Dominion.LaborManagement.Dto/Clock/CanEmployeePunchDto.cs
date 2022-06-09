using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.Clock
{
    public class CanEmployeePunchDto
    {
        public bool CanPunch { get; set; }
        public string IpAddress { get; set; }
    }
}
