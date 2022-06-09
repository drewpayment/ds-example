using System;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Clients
{
    public class ClientWorkNumberPolicy : Entity<ClientWorkNumberPolicy>
    {
        public virtual int ClientWorkNumberPolicyId { get; set; }
        public virtual int ClientID { get; set; }
        public virtual int AcceptedBy { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual Boolean IsAccepted { get; set; }

        public User.User User { get; set; }
    }
}