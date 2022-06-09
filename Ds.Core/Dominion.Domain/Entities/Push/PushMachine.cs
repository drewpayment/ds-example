using System;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Push
{
    public class PushMachine: Entity<PushMachine>
    {
        public virtual int Id { get; set; }
        public virtual string DevSn { get; set; }
        public virtual string DevName { get; set; }
        public virtual string Attlogstamp { get; set; }
        public virtual string Operlogstamp { get; set; }
        public virtual string Attphotostamp { get; set; }
        public virtual string ErrorDelay { get; set; }
        public virtual string Delay { get; set; }
        public virtual string TransFlag { get; set; }
        public virtual string Realtime { get; set; }
        public virtual string TransInterval { get; set; }
        public virtual string TransTimes { get; set; }
        public virtual string Encrypt { get; set; }
        public virtual DateTime? LastRequestTime { get; set; }
        public virtual string DevIp { get; set; }
        public virtual string DevMac { get; set; }
        public virtual string DevFpversion { get; set; }
        public virtual string DevFirmwareVersion { get; set; }
        public virtual int? UserCount { get; set; }
        public virtual int? AttCount { get; set; }
        public virtual int? FpCount { get; set; }
        public virtual string TimeZone { get; set; }
        public virtual int? Timeout { get; set; }
        public virtual int? SyncTime { get; set; }
    }
}
