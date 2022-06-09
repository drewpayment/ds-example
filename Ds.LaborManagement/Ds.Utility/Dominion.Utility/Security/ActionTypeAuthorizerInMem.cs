using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Dominion.Utility.Security
{
    /// <summary>
    /// This class provides a simple, in-memory implementation for IActionTypeAuthorizer that 
    /// can be used in unit tests that don't connect to the database.
    /// </summary>
    public class ActionTypeAuthorizerInMem : SortedList<string, ActionType>, IActionTypeAuthorizer
    {
        /// <summary>
        /// Add the given action to the list.
        /// </summary>
        /// <param name="actionType">The object to add to the list.</param>
        /// <returns>True if the object is added, false if the object is already in the list.</returns>
        public bool Add(ActionType actionType)
        {
            if (! this.ContainsKey(actionType.Designation))
            {
                this.Add(actionType.Designation, actionType);
                return true;
            }
            else
            {
                return false;
            }
        }

        // Add()
        #region implementation of IActionTypeAuthorizer

        /// <summary>
        /// Determine whether the given user is allowed to perform the requested action.
        /// </summary>
        /// <param name="identity">Identity object assocated with a particular principal.</param>
        /// <param name="actionTypeDesignation">Designation ID of the action being evaluated.</param>
        /// <returns>True if the given user is allowed to perform the requested action.</returns>
        public bool IsActionAllowed(ClaimsIdentity identity, string actionTypeDesignation)
        {
            return this.ContainsKey(actionTypeDesignation);
        }

        /// <summary>
        /// This will return a list of allowed actions for the current user.
        /// Added to setup conditional/dynamic menu lists and routes.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetAllowedActions()
        {
            return this.Keys;
        }

        #endregion // implementation of IActionTypeAuthorizer
    }
}