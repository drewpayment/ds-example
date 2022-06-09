using Dominion.Core.Dto.Misc;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Misc;

namespace Dominion.Domain.Entities.Clients
{
    /// <summary>
    /// Account Option configuration for a given client.
    /// </summary>
    /// <remarks>
    /// See also: <seealso cref="IAccountOptionUniqueConstraintDto"/>.
    /// </remarks>
    public class ClientAccountOption : Entity<ClientAccountOption>
    {
        public virtual int ClientAccountOptionId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual Client Client { get; set; }
        public virtual AccountOption AccountOption { get; set; }
        public virtual AccountOptionInfo AccountOptionInfo { get; set; }
        public virtual string Value { get; set; }
    }
}