using System.Collections.Generic;

using Dominion.Utility.Security;

namespace Dominion.LaborManagement.Service.Internal.Security
{
    internal class LaborManagementActionType : ActionType
    {
        private const string BASE_DESIGNATION = "LaborManagement";

        /// <summary>
        /// Initializing constructor, intended to be called from w/in static "factory" properties.
        /// </summary>
        /// <param name="localDesignation">Unique identifier for the action relative to this class.
        /// Does not include the base designation.</param>
        /// <param name="label">User-friendly label that describes the action.</param>
        private LaborManagementActionType(string localDesignation, string label, IEnumerable<LegacyRole> roles)
            : base(BASE_DESIGNATION + "." + localDesignation, label, roles)
        {
        }

        /// <summary>
        /// Permissions given to supervisor-level group-schedule planners.
        /// </summary>
        internal static LaborManagementActionType LaborPlanSupervisor
        {
            get
            {
                var roles = new List<LegacyRole>
                {
                    LegacyRole.Supervisor
                };
                return new LaborManagementActionType("LaborPlanSupervisor", "Permissions given to supervisor-level group-schedule planners.", roles);
            }
        }

        /// <summary>
        /// Permission for reading auto points
        /// </summary>
        internal static LaborManagementActionType ReadAutoPoints
        {
            get
            {
                var roles = new List<LegacyRole>
                {
                    LegacyRole.SystemAdmin,
                    LegacyRole.CompanyAdmin
                };
                return new LaborManagementActionType(nameof(ReadAutoPoints), "Permission for reading auto points.", roles);
            }
        }

        /// <summary>
        /// Permission for enabling auto points
        /// </summary>
        internal static LaborManagementActionType WriteAutoPoints
        {
            get
            {
                var roles = new List<LegacyRole>
                {
                    LegacyRole.SystemAdmin
                };
                return new LaborManagementActionType(nameof(WriteAutoPoints), "Permission for enabling auto points.", roles);
            }
        }

        /// <summary>
        /// Permissions required to create and modify a labor plan.
        /// </summary>
        internal static LaborManagementActionType LaborPlanAdministrator
        {
            get
            {
                var roles = new List<LegacyRole>
                {
                    LegacyRole.SystemAdmin, 
                    LegacyRole.CompanyAdmin,
                    LegacyRole.Supervisor
                };
                return new LaborManagementActionType("LaborPlanAdministrator", "Permissions required to create and modify a labor plan.", roles);
            }
        }

        /// <summary>
        /// Permissions required to employees against a labor plan.
        /// </summary>
        internal static LaborManagementActionType LaborScheduleAdministrator
        {
            get
            {
                var roles = new List<LegacyRole>
                {
                    LegacyRole.SystemAdmin, 
                    LegacyRole.CompanyAdmin,
                    LegacyRole.Supervisor
                };
                return new LaborManagementActionType("LaborScheduleAdministrator", "Permissions required to schedule employees against a labor plan.", roles);
            }
        }

        internal static LaborManagementActionType ReadTimePolicy
        {
            get
            {
                var roles = new List<LegacyRole>
                {
                    LegacyRole.SystemAdmin,
                    LegacyRole.CompanyAdmin,
                    LegacyRole.Supervisor
                };
                return new LaborManagementActionType("ReadTimePolicy", "Permissions require to read time policies.", roles);
            }
        }

        internal static LaborManagementActionType WriteTimePolicy
        {
            get
            {
                var roles = new List<LegacyRole>
                {
                    LegacyRole.SystemAdmin
                };
                return new LaborManagementActionType("WriteTimePolicy", "Permissions required to write time policies.", roles);
            }
        }

        internal static LaborManagementActionType EditTimePolicy
        {
            get
            {
                var roles = new List<LegacyRole>
                {
                    LegacyRole.SystemAdmin,
                    LegacyRole.CompanyAdmin
                };
                return new LaborManagementActionType("EditTimePolicy", "Permissions required to edit time policies.", roles);
            }
        }

        internal static LaborManagementActionType ReadTimeCardAuthorization
        {
            get
            {
                var roles = new List<LegacyRole>
                {
                    LegacyRole.SystemAdmin,
                    LegacyRole.CompanyAdmin,
                    LegacyRole.Supervisor
                };
                return new LaborManagementActionType("ReadTimeCardAuthorization", "Permissions required to read time card authorization policies.", roles);
            }
        }

        internal static LaborManagementActionType UnassignedFilterOptionIsVisible
        {
            get
            {
                var roles = new List<LegacyRole>
                {
                    LegacyRole.SystemAdmin,
                    LegacyRole.CompanyAdmin
                };
                return new LaborManagementActionType("CanSelectUnassigned", "Permissions required to view and select unassigned filter option.", roles);
            }
        }

        internal static LaborManagementActionType CanImportPunches
        {
            get
            {
                var roles = new List<LegacyRole>
                {
                    LegacyRole.SystemAdmin,
                    LegacyRole.CompanyAdmin
                };
                return new LaborManagementActionType("CanImportPunches", "Permissions required to import punches", roles);
            }
        }
    }
}