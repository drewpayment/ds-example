using Dominion.Core.Dto.Labor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.Clock.Misc
{
    public class PunchResult
    {
        public PunchOptionType RequestType { get; set; }
        public bool Success { get; set; }
        public virtual string Message { get; set; }
        public virtual int? PunchId { get; set; }
    }
}
