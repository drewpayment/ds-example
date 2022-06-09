using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Push
{
    public class PushUserInfo : Entity<PushUserInfo>
    {
        public virtual int    Id       { get; set; }
        public virtual string DevSn    { get; set; }
        public virtual string Pin      { get; set; }
        public virtual string Username { get; set; }
        public virtual string Password { get; set; }
        public virtual string IdCard   { get; set; }
        public virtual string Group    { get; set; }
        public virtual string TimeZone { get; set; }
        public virtual string Pri      { get; set; }
    }
}
