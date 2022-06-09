using System.Collections.Generic;

namespace Dominion.Core.Dto.User.Search
{
    public class UserSearchOptions
    {
        public bool? IsActiveUser { get; set; }
        public IEnumerable<UserType> UserTypes { get; set; }
        public int? SupervisorToEmployeeId { get; set; }
        public bool ExcludeTimeClockOnly { get; set; }
        public bool HaveActiveEmployee { get; set; }
        public bool ifSupervisorGetSubords { get; set; }
    }
}