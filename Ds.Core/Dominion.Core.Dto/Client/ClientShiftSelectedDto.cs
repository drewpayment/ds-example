using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Client
{
    public class ClientShiftSelectedDto
    {
        public int ClockClientShiftSelectedId { get; set; }
        public int ClockClientTimePolicyId { get; set; }
        public int ClientShiftId { get; set; }
    }
}
