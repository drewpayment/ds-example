using System.Collections.Generic;

using Dominion.Utility.Security;

namespace Dominion.LaborManagement.Service.Internal.Security
{
    internal class BudgetingActionType : ActionType
    {
        private const string BASE_DESIGNATION = "Budgeting";

        /// <summary>
        /// Initializing constructor, intended to be called from w/in static "factory" properties.
        /// </summary>
        /// <param name="localDesignation">Unique identifier for the action relative to this class.
        /// Does not include the base designation.</param>
        /// <param name="label">User-friendly label that describes the action.</param>
        private BudgetingActionType(string localDesignation, string label, IEnumerable<LegacyRole> roles)
            : base(BASE_DESIGNATION + "." + localDesignation, label, roles)
        {
        }

        /// <summary>
        /// Permissions required to budget hours and rates.
        /// </summary>
        internal static BudgetingActionType BudgetingAdministrator
        {
            get
            {
                var roles = new List<LegacyRole>
                {
                    LegacyRole.SystemAdmin, 
                    LegacyRole.CompanyAdmin,
                    LegacyRole.Supervisor
                };
                return new BudgetingActionType("BudgetingAdministrator", "Permissions required to create and modify a budget.", roles);
            }
        }
    }
}