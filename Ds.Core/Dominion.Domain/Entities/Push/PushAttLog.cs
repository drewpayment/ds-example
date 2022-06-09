using System;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Push
{
    public class PushAttLog : Entity<PushAttLog>
    {
        public virtual int       Id        { get; set; }
        public virtual string    DevSn     { get; set; }
        public virtual string    Pin       { get; set; }
        public virtual DateTime? AttTime   { get; set; }
        public virtual string    Status    { get; set; }
        public virtual string    Verify    { get; set; }
        public virtual string    WorkCode  { get; set; }
        public virtual string    Reserved1 { get; set; }
        public virtual string    Reserved2 { get; set; }
        public virtual int?      JobCode1  { get; set; }
        public virtual int?      JobCode2  { get; set; }
        public virtual int?      JobCode3  { get; set; }
        public virtual int?      JobCode4  { get; set; }
        public virtual int?      JobCode5  { get; set; }
        public virtual int?      JobCode6  { get; set; }
        public virtual int?      TipCode1  { get; set; }
        public virtual int?      TipCode2  { get; set; }
        public virtual int?      TipCode3  { get; set; }

    }
}
