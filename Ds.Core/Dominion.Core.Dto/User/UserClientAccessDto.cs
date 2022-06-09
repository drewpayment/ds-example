using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.User
{
    public class UserClientAccessDto
    {
        public int      UserId         { get; set; }
        public int      ClientId       { get; set; }
        public string   ClientName     { get; set; }
        public bool     HasAccess      { get; set; }
        public bool     IsClientAdmin  { get; set; }
        public bool     IsBenefitAdmin { get; set; }
    }
}
