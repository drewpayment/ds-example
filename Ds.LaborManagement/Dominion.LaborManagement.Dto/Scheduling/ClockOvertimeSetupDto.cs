using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.Scheduling
{
    public class ClockOvertimeSetupDto
    {
        public int employeeId { get; set; }
        public int? clockClientTimePolicyId { get; set; }
        public List<int> clockClientOvertimeId { get; set; }
        public List<int> clockOvertimeFrequencyId { get; set; }
    }
}