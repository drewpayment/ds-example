using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.LeaveManagement
{
    public class JobProfileAccrualsDto
    {
        public int JobProfileId { get; set; }
        public int ClientAccrualId { get; set; }
        public string Description { get; set; }
    }
}
