using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;

namespace Dominion.Domain.Entities.User
{
    public class UserPin : Entity<UserPin>
    {
        public virtual int      UserPinId       { get; set; }
        public virtual int?      UserId          { get; set; }
        public virtual int?      ClientContactId { get; set; }
        public virtual int      ClientId        { get; set; }
        public virtual string   Pin             { get; set; }
        public virtual DateTime Modified        { get; set; }
        public virtual int      ModifiedBy      { get; set; }

        public virtual User User { get; set; }
        public virtual ClientContact ClientContact { get; set; }
    }
}
