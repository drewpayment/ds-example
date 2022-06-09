using System.Collections.Generic;
using System.Linq;

namespace Dominion.Utility.Security
{
    /// <summary>
    /// Base class for an object that represents a type of action that is used to help determine
    /// whether a given action is allowed to be performed by a specific user.
    /// </summary>
    /// <remarks>
    /// Deriving classes are expected to use "factory" properties that construct the object.
    /// </remarks>
    public class ActionType : IActionType
    {
        private string _localDesignation;
        private string _label;
        private List<LegacyRole> _roles;

        /// <summary>
        /// Initializing constructor, intended to be called from w/in static "factory" properties
        /// and by code instantiates a PrincipalDominion object.
        /// </summary>
        /// <param name="localDesignation">Unique identifier for the action.</param>
        /// <param name="label">User-friendly label that describes the action.</param>
        /// <param name="roles">List of legacy application roles that are allowed to perform this action.</param>
        public ActionType(string localDesignation, string label, IEnumerable<LegacyRole> roles)
        {
            _localDesignation = localDesignation;
            _label = label;

            if (roles != null)
                _roles = roles.ToList();
            else
                _roles = new List<LegacyRole>();
        }

        /// <summary>
        /// Get the unique identifier for this action.
        /// </summary>
        public string Designation
        {
            get { return _localDesignation; }
        }

        /// <summary>
        /// Get the label for this action.
        /// </summary>
        public string Label
        {
            get { return _label; }
        }

        /// <summary>
        /// Get the legacy system roles that are allowed to perform this action.
        /// </summary>
        /// <remarks>This property will become obsolete once the legacy software is no longer in use.</remarks>
        public List<LegacyRole> Roles
        {
            get { return _roles; }
        }



    }
}