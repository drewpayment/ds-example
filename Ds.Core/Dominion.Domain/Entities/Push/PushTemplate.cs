using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Push
{
    public class PushTemplate : Entity<PushTemplate>
    {
        public virtual int    Id      { get; set; }
        public virtual string DevSn   { get; set; }
        public virtual string Pin     { get; set; }
        public virtual string Fid     { get; set; }
        public virtual short? Size    { get; set; }
        public virtual string Valid   { get; set; }
        public virtual string TmpStr  { get; set; }
        public virtual string TmpType { get; set; }
    }
}
