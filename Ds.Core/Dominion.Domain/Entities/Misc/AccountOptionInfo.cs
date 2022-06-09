using System.Collections.Generic;
using Dominion.Core.Dto.Misc;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Misc
{
    /// <summary>
    /// Account Option entity providing additional information about a specific AccountOption.
    /// </summary>
    public class AccountOptionInfo : Entity<AccountOptionInfo>
    {
        public virtual AccountOption AccountOption { get; set; }
        public virtual string Description { get; set; }
        public virtual AccountOptionDataType DataType { get; set; }
        public virtual AccountOptionCategory Category { get; set; }
        public virtual bool? IsEnabledByDefault { get; set; }

        /// <summary>
        /// This value is always null.
        /// </summary>
        public virtual int? ClientId { get; set; }

        public virtual ICollection<AccountOptionItem> AccountOptionItems { get; set; }
        public byte IsSecurityOption { get; set; }

        public AccountOptionInfo()
        {
        }
    }
}