using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.User
{
    public class UserActionTypeCliamTypeDto : ClaimTypeDto
    {
        public override ClaimSource Source => ClaimSource.UserActionType;
        public int? UserActionTypeId { get; set; }
        public string Designation { get; set; }
        public string Description { get; set; }
        public override bool IsClaimMatch(ClaimTypeDto claim)
        {
            return claim is UserActionTypeCliamTypeDto typed && typed.Designation == Designation;
        }
    }
}
