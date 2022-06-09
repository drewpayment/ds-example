using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Sprocs
{
    public class TimeCardAuthorizationDataArgs
    {
        public int clientId { get; set; }
        public int controlId { get; set; }
        public int userId { get; set; }
        public string categoryString { get; set; }
        public int payrollRunId { get; set; }
        public bool hideCustomDateRange { get; set; }
    }
}
