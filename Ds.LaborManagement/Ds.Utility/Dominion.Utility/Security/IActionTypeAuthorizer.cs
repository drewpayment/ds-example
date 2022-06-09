using System.Collections.Generic;
using System.Security.Claims;

namespace Dominion.Utility.Security
{
    /// <summary>
    /// Interface for an action provider that is used by the PrincipalDominion class to determine
    /// whether the user is allowed to perform a particular action.
    /// </summary>
    public interface IActionTypeAuthorizer
    {
        /// <summary>
        /// Determine whether the given user is allowed to perform the requested action.
        /// </summary>
        /// <param name="identity">Identity object assocated with a particular principal.</param>
        /// <param name="actionTypeDesignation">Designation ID of the action being evaluated.</param>
        /// <returns>True if the given user is allowed to perform the requested action.</returns>
        /// <remarks>
        /// It is expected that the production implementation of this method queries the database
        /// to get the actions allowed by the given identity.
        /// </remarks>
        bool IsActionAllowed(ClaimsIdentity identity, string actionTypeDesignation);

        /// <summary>
        /// This will return a list of allowed actions for the current user.
        /// Added to setup conditional/dynamic menu lists and routes.
        /// </summary>
        /// <returns>List of allowed actions.</returns>
        IEnumerable<string> GetAllowedActions();

    }
}