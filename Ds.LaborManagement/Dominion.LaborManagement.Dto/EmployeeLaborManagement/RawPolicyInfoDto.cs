using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.EmployeeLaborManagement
{
    public class RawPolicyInfoDto
    {
        public int TimePolicyId { get; set; }
        public int ClientId { get; set; }
        public string Name { get; set; }
        public RawRuleInfoDto Rule { get; set; }
    }
}
