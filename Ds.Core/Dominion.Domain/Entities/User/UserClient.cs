using Dominion.Core.Dto.User;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;

namespace Dominion.Domain.Entities.User
{
    /// <summary>
    /// Container for UserClient.
    /// </summary>
    /// <remarks>
    /// (Per Ron): Intended for <see cref="UserType.CompanyAdmin"/>'s only that have access to multiple clients.
    /// </remarks>
    public class UserClient : Entity<UserClient>
    {
        public virtual int  UserId         { get; set; }
        public virtual int  ClientId       { get; set; }
        public virtual bool IsClientAdmin  { get; set; }
        public virtual bool IsBenefitAdmin { get; set; }

        public virtual Client Client { get; set; }
        public virtual User   User   { get; set; }

    }
}