using System.Collections.Generic;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Entities.TimeClock;

namespace Dominion.Domain.Entities.Misc
{
    public class TimeZone : Entity<TimeZone>
    {
        public virtual int TimeZoneId { get; set; }
        public virtual string Description { get; set; }
        public virtual int? Utc { get; set; }
        public virtual string FriendlyDesc { get; set; }
        public TimeZones TimeZoneName => (TimeZones)TimeZoneId;

        //Reference Entities
        public virtual ICollection<ClockClientTimePolicy> TimePolicies { get; set; }
    }

    public enum TimeZones : int
    {
        Eastern = 1,
        Alaska = 2,
        Hawaii = 3,
        Pacific = 4, 
        Mountain = 5,
        Central = 6,
        Atlantic = 7
    }
}