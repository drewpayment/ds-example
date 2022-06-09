using Dominion.Core.Dto.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Core;

namespace Dominion.Core.Dto.Misc
{
    public class TimeZoneDto
    {
        public int TimeZoneId { get; set; }
        public string Description { get; set; }
        public int? Utc { get; set; }
        public string FriendlyDescription { get; set; }
        public TimeZoneType TimeZoneName => (TimeZoneType)TimeZoneId;
    }
}
