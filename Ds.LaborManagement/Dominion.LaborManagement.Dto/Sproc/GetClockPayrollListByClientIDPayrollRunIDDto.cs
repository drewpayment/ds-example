using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.Sproc
{
    public class GetClockPayrollListByClientIDPayrollRunIDDto
    {
        public int Displayorder { get; set; }

        public string checkdate { get; set; }

        public string checkdateorder { get; set; }
        public string PayPeriod { get; set; }
        public int PayrollId { get; set; }
    }
}
