using System;
using System.Collections.Generic;
using System.Security.Claims;
using Dominion.Utility.Transform;

namespace Dominion.Utility.Security
{
    /// <summary>
    /// This class represents the security context to be used by the application.
    /// lowFix: jay: IUser isn't implemented EXPLICITLY but it should be in the future
    /// </summary>
    public class PrincipalDominion : ClaimsPrincipal, IUser
    {
        private IActionTypeAuthorizer _authorizer;

        /// <summary>
        /// Initializing constructor.
        /// </summary>
        /// <param name="identity">The user identity upon which to base this object.</param>
        /// <param name="authorizer">Object that authorizes a specified action/role for the identity.</param>
        public PrincipalDominion(ClaimsIdentity identity, IActionTypeAuthorizer authorizer)
            : base(identity)
        {
            if (authorizer != null)
                _authorizer = authorizer;
            else
                throw new ArgumentNullException("authorizer");
        }

        #region PUBLIC PROPERTIES

        /// <summary>
        /// Authenticated Unique UserId
        /// </summary>
        public int AuthUserId
        {
            get
            {
                int authUserId;
                if (!this.TryGetClaimValue<int>(DominionClaimTypes.AuthUserId, out authUserId))
                    authUserId = 0;

                return authUserId;
            }
        }


        /// <summary>
        /// Authenticated Unique UserId
        /// </summary>
        public int UserId
        {
            get
            {
                int userId;
                if (! this.TryGetClaimValue<int>(DominionClaimTypes.UserId, out userId))
                    userId = 0;

                return userId;
            }
        }

        /// <summary>
        /// Name that the user authenticated with.
        /// </summary>
        public string UserName
        {
            get
            {
                string name;
                if (! this.TryGetClaimValue<string>(ClaimTypes.Name, out name))
                    name = string.Empty;

                return name;
            }
        }

        /// <summary>
        /// Client ID that the user is tied to.
        /// </summary>
        public int? ClientId
        {
            get
            {
                int clientId;
                if (! this.TryGetClaimValue<int>(DominionClaimTypes.ClientId, out clientId))
                    return null;

                return clientId;
            }
        }

        public int? UserEmployeeId { get; set; }
        public int? LastEmployeeId { get; set; }
        public int? ApplicantId { get; set; }
        public IEnumerable<int> AccessibleClientIds { get; set; }
        public bool IsAnonymous { get; set; }
        public bool IsSystemAdmin { get; set; }
        public bool IsCompanyAdmin { get; set; }
        public bool IsSupervisor { get; set; }
        /// <summary>
        /// Emloyee ID that the user is tied to.
        /// </summary>
        public int? EmployeeId
        {
            get
            {
                int employeeId;
                if (! this.TryGetClaimValue<int>(DominionClaimTypes.EmployeeId, out employeeId))
                    return null;

                return employeeId;
            }
        }

        #endregion

        #region IPrincipal IMPLEMENTATION

        /// <summary>
        /// Determine whether the current principal belongs to the specified action type.
        /// </summary>
        /// <param name="actionTypeDesignation">The action type designation (role) for which to check.</param>
        /// <returns>True if the identity is allowed to perform the specified action.</returns>
        public override bool IsInRole(string actionTypeDesignation)
        {
            return _authorizer.IsActionAllowed((ClaimsIdentity) Identity, actionTypeDesignation);
        }

        #endregion

        /// <summary>
        /// This will return a list of allowed actions for the current user.
        /// Added to setup conditional/dynamic menu lists and routes.
        /// </summary>
        /// <returns>List of allowed actions.</returns>
        public IEnumerable<string> GetAllowedActions()
        {
            return _authorizer.GetAllowedActions();
        }
    }
}