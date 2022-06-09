using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Dominion.Utility.ExtensionMethods;
using Dominion.Utility.Msg.Specific;
using Dominion.Utility.OpResult;
using Dominion.Utility.Security.Msg;

namespace Dominion.Utility.Security
{
    public class ResourceAccessRules
    {
        /// <summary>
        /// Criteria for checking against an employee id.
        /// </summary>
        public static Func<IUser, int, bool> EmployeeResourceRule
        {
            get { return (u, i) => u.EmployeeId == i || u.UserEmployeeId == i || u.LastEmployeeId == i; }
        }

        /// <summary>
        /// Criteria for checking against an client id.
        /// </summary>
        public static Func<IUser, int, bool> ClientResourceRule
        {
            get
            {
                return (u, i) => u.ClientId == i || u.AccessibleClientIds.Contains(i) || u.IsSystemAdmin;
            }
        }

        public static Func<IEnumerable<int>, int, bool> MultipleUserClientRule => (ints, i) => ints.Contains(i);

        /// <summary>
        /// Criteria for checking against an user id.
        /// </summary>
        public Func<IUser, int, bool> UserResourceRule
        {
            get
            {
                return (u, i) =>
                {
                    if (u.IsSystemAdmin || u.UserId == i)
                        return true;

                    if (_userResourceGetter == null)
                        return false;

                    if (_accessibleUserIds == null)
                        _accessibleUserIds = _userResourceGetter(_userInfo).ToOrNewList();

                    return _accessibleUserIds.Contains(i);
                };
            }
        }

        private Func<IUser, int, bool> ApplicantResourceRule
        {
            get
            {
                return (u, i) =>
                {
                    //check if we have a way to lookup applicants
                    //if not, just assume user does NOT have access to an applicant
                    if (_applicantResourceGetter == null)
                        return false;

                    //if we haven't loaded what applicants they have access to, do so now
                    if (_accessibleApplicantIds == null)
                        _accessibleApplicantIds = _applicantResourceGetter(_userInfo).ToOrNewList();

                    return _accessibleApplicantIds.Contains(i);
                };
            }
        }

        /// <summary>
        /// IUser data should come from the principal.
        /// Actually the principal is an IUser.
        /// </summary>
        private readonly IUser _userInfo;

        private readonly Func<IUser, IEnumerable<int>> _applicantResourceGetter;

        private Func<IUser, IEnumerable<int>> _userResourceGetter;

        private IEnumerable<int> _accessibleApplicantIds;
        private IEnumerable<int> _accessibleUserIds;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <remarks>
        /// IUser data should come from the principal.
        /// Actually the principal is an IUser.
        /// </remarks>
        /// <param name="userInfo">The current user.</param>
        /// <param name="applicantResourceGetter">A "lazy" loading function used to load accessible 
        /// applicants the user has access to. Will only be used if <see cref="CheckAccessById"/>
        /// is called for <see cref="ResourceOwnership.Applicant"/>(s).</param>
        public ResourceAccessRules(IUser userInfo, Func<IUser, IEnumerable<int>> applicantResourceGetter = null)
        {
            _userInfo = userInfo;
            _applicantResourceGetter = applicantResourceGetter;
        }

        public void SetUserResourceGetter(Func<IUser, IEnumerable<int>> userResourceGetter)
        {
            _userResourceGetter = userResourceGetter;
        }

        /// <summary>
        /// Check for a value against the internal <see cref="IUser"/> with the rule that corresponds with the <see cref="ResourceAccessType"/>.
        /// </summary>
        /// <param name="accessType">Accesstype to check.</param>
        /// <param name="value">
        /// An integer that represent an ID from the <see cref="IUser"/>< data.
        /// </param>
        /// <returns>Opresult.</returns>
        public IOpResult CheckAccessById(ResourceOwnership accessType, int value)
        {
            var opr = new OpResult.OpResult();

            switch (accessType)
            {
                case ResourceOwnership.Employee:
                    opr.EvaluateSuccess(EmployeeResourceRule(_userInfo, value));
                    break;
                case ResourceOwnership.Client:
                    opr.EvaluateSuccess(ClientResourceRule(_userInfo, value));
                    break;
                case ResourceOwnership.User:
                    opr.EvaluateSuccess(UserResourceRule(_userInfo, value));
                    break;
                case ResourceOwnership.Applicant:
                    opr.EvaluateSuccess(ApplicantResourceRule(_userInfo, value));
                    break;
                default:
                    opr.AddExceptionMessage(new ArgumentException());
                    break;
            }

            return AddMessageIfFailed(opr);
        }

        public IOpResult CheckAccessByAccessibleClientIds(int id)
        {
            var opr = new OpResult.OpResult();

            opr.EvaluateSuccess(_userInfo.IsSystemAdmin || MultipleUserClientRule(_userInfo.AccessibleClientIds, id));

            return AddMessageIfFailed(opr);

        }

        /// <summary>
        /// Checks if the current user has access to a given applicant.
        /// </summary>
        /// <param name="applicantId">ID of the applicant to check access to.</param>
        /// <returns></returns>
        public IOpResult HasAccessToApplicant(int applicantId)
        {
            return CheckAccessById(ResourceOwnership.Applicant, applicantId);
        }

        /// <summary>
        /// Helper method to reduce duplicating code.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private IOpResult AddMessageIfFailed(IOpResult result)
        {
            if (result.HasError)
                result.AddMessage(new NotYourResourceMsg());

            return result;
        }

    }
}
