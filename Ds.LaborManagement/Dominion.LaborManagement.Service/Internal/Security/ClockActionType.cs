using System.Collections.Generic;
using Dominion.Utility.Security;

namespace Dominion.LaborManagement.Service.Internal.Security
{
    /// <summary>
    /// List of actions performed by the ClockService.
    /// </summary>
    internal class ClockActionType : Utility.Security.ActionType
    {
        private const string BaseDesignation = "Clock";

        private ClockActionType(string designation, string label, IEnumerable<LegacyRole> roles) : base(BaseDesignation + "." + designation, label, roles)
        {
        }

        public static ClockActionType Administrator
        {
            get
            {
                List<LegacyRole> roles = new List<LegacyRole>()
                {
                    LegacyRole.SystemAdmin
                };

                return new ClockActionType(nameof(Administrator), "Role for clock administrator users", roles);
            }
        }

        public static ClockActionType ReadUser
        {
            get
            {
                var roles = new List<LegacyRole>()
                {
                    LegacyRole.SystemAdmin,
                    LegacyRole.CompanyAdmin,
                    LegacyRole.Supervisor,
                    LegacyRole.Employee
                };
                return new ClockActionType(nameof(ReadUser), "Role for normal clock users", roles);
            }
        }

        public static ClockActionType User
        {
            get
            {
                List<LegacyRole> roles = new List<LegacyRole>()
                {
                    LegacyRole.SystemAdmin,
                    LegacyRole.CompanyAdmin,
                    LegacyRole.Supervisor,
                    LegacyRole.Employee
                };

                return new ClockActionType(nameof(User), "Role for normal clock users", roles);
            }
        }

        public static IEnumerable<ClockActionType> GetAll() => new List<ClockActionType>()
        {
            Administrator,
            ReadUser,
            User
        };

        public static ClockActionType ClockEmployeeAdministrator
        {
            get
            {
                List<LegacyRole> roles = new List<LegacyRole>()
                {
                    LegacyRole.SystemAdmin,
                    LegacyRole.CompanyAdmin,
                    LegacyRole.Supervisor,
                    LegacyRole.Employee
                };

                return new ClockActionType(nameof(ClockEmployeeAdministrator), "Role for clock employee administrator users", roles);
            }
        }

        public static ClockActionType MobileTimeClockAccess
        {
            get
            {
                List<LegacyRole> roles = new List<LegacyRole>
                {
                    LegacyRole.SystemAdmin,
                    LegacyRole.CompanyAdmin,
                    LegacyRole.Supervisor,
                    LegacyRole.Employee
                };

                return new ClockActionType(nameof(MobileTimeClockAccess), "Role for users that have access to Time Clock in mobile", roles);
            }
        }

        public static ClockActionType CanEditClockEmployee
        {
            get
            {
                List<LegacyRole> roles = new List<LegacyRole>
                {
                    LegacyRole.SystemAdmin,
                    LegacyRole.CompanyAdmin,
                    LegacyRole.Supervisor
                };

                return new ClockActionType(nameof(CanEditClockEmployee), "Permission that allows the user to edit the ClockEmployee screen and assign Time Policies", roles);
            }
        }

        public static ClockActionType CanOptInToCompanyFeature
        {
            get
            {
                List<LegacyRole> roles = new List<LegacyRole>
                {
                    LegacyRole.SystemAdmin,
                    LegacyRole.CompanyAdmin,
                };

                return new ClockActionType(nameof(CanOptInToCompanyFeature), "Permission that allows the user to edit Company Features.", roles);
            }
        }
    }
}
