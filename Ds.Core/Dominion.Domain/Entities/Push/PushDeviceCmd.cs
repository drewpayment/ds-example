using System;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Push
{
    public class PushDeviceCmd : Entity<PushDeviceCmd>
    {
        public virtual int      Id           { get; set; }
        public virtual string   DevSn        { get; set; }
        public virtual string   Content      { get; set; }
        public virtual DateTime? CommitTime   { get; set; }
        public virtual DateTime? TransTime    { get; set; }
        public virtual DateTime? ResponseTime { get; set; }
        public virtual string   ReturnValue  { get; set; }
    }
}
