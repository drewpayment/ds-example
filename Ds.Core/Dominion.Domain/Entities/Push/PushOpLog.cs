using System;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Push
{
    public class PushOpLog : Entity<PushOpLog>
    {
        public virtual int       Id         { get; set; }
        public virtual string    OpId       { get; set; }
        public virtual string    AdminId    { get; set; }
        public virtual DateTime? HappenTime { get; set; }
        public virtual string    Object1    { get; set; }
        public virtual string    Object2    { get; set; }
        public virtual string    Object3    { get; set; }
        public virtual string    Object4    { get; set; }
    }
}
