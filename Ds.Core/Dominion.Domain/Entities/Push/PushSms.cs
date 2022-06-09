using System;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Push
{
    public class PushSms : Entity<PushSms>
    {
        public virtual int       Id        { get; set; }
        public virtual int?      Type      { get; set; }
        public virtual DateTime? BeginTime { get; set; }
        public virtual DateTime? EndTime   { get; set; }
        public virtual string    Content   { get; set; }
    }
}
