using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Utility.Security
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUser
    {
        int    AuthUserId     { get; }
        int    UserId     { get; }
        string UserName   { get; }
        int?   EmployeeId { get; }
        int?   ClientId   { get; }
        int? UserEmployeeId { get; }
        int? LastEmployeeId { get; }
        bool IsAnonymous { get; }
        bool IsSystemAdmin { get; }
        bool IsCompanyAdmin { get; }
        bool IsSupervisor { get; }

        IEnumerable<int> AccessibleClientIds { get; set; }
    }
}
