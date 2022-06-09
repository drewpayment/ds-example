using System.Collections.Generic;

using Dominion.Utility.Dto;

namespace Dominion.Core.Dto.Security
{
    /// <summary>
    /// Container class for information about records that were added, removed and updated during a user action
    /// synchronization call.
    /// </summary>
    public class UserActionSyncResultsDto : DtoObject
    {
        public List<string> AddedSytemUserGroups { get; set; }
        public List<string> RemovedSystemUserGroups { get; set; }
        public List<string> AddedActions { get; set; }
        public List<string> UpdatedActions { get; set; }
        public List<string> RemovedActions { get; set; }

        /// <summary>
        /// Get or set the user names that were associated with a deleted user group.
        /// </summary>
        public List<string> AffectedUserNames { get; set; }
    } // class UserActionSyncResultsDto
}