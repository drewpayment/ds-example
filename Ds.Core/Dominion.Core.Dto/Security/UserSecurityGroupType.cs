using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Security
{
    /// <summary>
    /// There is no entity for this enum.
    /// These values are used in the 'dbo.UserSupervisorSecurity' & 'core.vUserSupervisorSecurity' table. 
    /// The underlying type had to be set to byte since the value for the column is 'tinyint'.
    /// </summary>
    public enum UserSecurityGroupType : byte
    {
        None             = 0,
        Employee         = 1,
        ClientDepartment = 2,
        Report           = 3,
        ClientCostCenter = 4,
        UserEmulation    = 5,
    }
}
