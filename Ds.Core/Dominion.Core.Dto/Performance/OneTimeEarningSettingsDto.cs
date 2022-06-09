using Dominion.Core.Dto.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Performance
{
    public class OneTimeEarningSettingsDto
    {
        public int          OneTimeEarningSettingsId { get; set; }
        public int          EmployeeId { get; set; }
        public string       Name { get; set; }
        public IncreaseType IncreaseType { get; set; }
        public decimal       IncreaseAmount { get; set; }
        public BasedOn      BasedOn { get; set; }
        public Measurement  Measurement { get; set; }
        public bool         DisplayInEss { get; set; }
        public bool         IsArchived { get; set; }
        
    }
}
