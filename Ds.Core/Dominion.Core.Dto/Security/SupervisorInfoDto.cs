using System.Collections.Generic;
using Dominion.Core.Dto.User;

namespace Dominion.Core.Dto.Security
{
    public class SupervisorInfoDto
    {
        public int UserId { get; set; }
        public int ClientId { get; set; }
        public UserViewEmployeePayType ViewEmployeePayType { get; set; }

        /// <summary>
        /// Indication if emulated supervisor groups are included in the supervisor's access.
        /// </summary>
        public bool IncludesEmulatedGroups { get; set; }

        /// <summary>
        /// Indication of what group types are included from emulated supervisors. Will only 
        /// be set if <see cref="IncludesEmulatedGroups"/> is true.
        /// </summary>
        public IEnumerable<UserSecurityGroupType> EmulatedGroupTypes { get; set; }

        /// <summary>
        /// All departments the user supervises including emulated departments if requested.
        /// </summary>
        public IDictionary<int, SupervisorGroupDto> SupervisedDepartments => AllGroups[UserSecurityGroupType.ClientDepartment];

        /// <summary>
        /// All employees the user supervises. Will include emulated employees if requested.
        /// </summary>
        public IDictionary<int, SupervisorGroupDto> SupervisedEmployees => AllGroups[UserSecurityGroupType.Employee];

        /// <summary>
        /// All cost centers the user supervises. Will include emulated cost centers if requested.
        /// </summary>
        public IDictionary<int, SupervisorGroupDto> SupervisedCostCenters => AllGroups[UserSecurityGroupType.ClientCostCenter];

        /// <summary>
        /// All reports the user supervises. Will include emualted reports if requested.
        /// </summary>
        public IDictionary<int, SupervisorGroupDto> SupervisedReports => AllGroups[UserSecurityGroupType.Report];

        public IDictionary<UserSecurityGroupType, IDictionary<int, SupervisorGroupDto>> AllGroups { get; set; }

        /// <summary>
        /// Emulated supervisor info.
        /// </summary>
        public IEnumerable<SupervisorInfoDto> EmulatedSupervisors { get; set; }
    }
}