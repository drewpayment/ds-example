using Dominion.Core.Dto.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Misc
{
    public class AccountOptionInfoDto
    {
        public AccountOption AccountOption    { get; set; }
        public string Description             { get; set; }
        public AccountOptionDataType DataType { get; set; }
        public AccountOptionCategory Category { get; set; }
        public bool? IsEnabledByDefault       { get; set; }

        /// <summary>
        /// This value is always null.
        /// </summary>
        public virtual int? ClientId { get; set; }

        public virtual IEnumerable<AccountOptionItemDto> AccountOptionItems { get; set; }
        public byte IsSecurityOption { get; set; }

    }

    public class AccountOptionInfoWithClientSelectionDto : AccountOptionInfoDto
    {
        public ClientAccountOptionSelectionDto ClientSelection { get; set; }
    }

}
