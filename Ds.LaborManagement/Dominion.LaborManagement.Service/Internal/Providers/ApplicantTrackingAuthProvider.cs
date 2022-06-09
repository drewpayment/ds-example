using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Authentication.Intermediate.Util;
using Dominion.Core.Services.Interfaces;
using Dominion.LaborManagement.Service.Api;
using Dominion.LaborManagement.Service.Internal.Security;
using Dominion.Utility.OpResult;

namespace Dominion.LaborManagement.Service.Internal.Providers
{
    /// <summary>
    /// This provider provides the different authorization implementations we might need in a service.
    /// This could potentially define all of the different ways we would need to authorize users in ApplicantTracking before invoking any business logic.
    /// Handling authorization this way almost completely decouples us from the business logic making authorization easy to manage and test.
    /// </summary>
    public class ApplicantTrackingAuthProvider : IApplicantTrackingAuthProvider
    {
        private readonly IBusinessApiSession _session;
        private readonly ISecurityManager _securityManager;
        public ApplicantTrackingAuthProvider(IBusinessApiSession session, ISecurityManager securityManager)
        {
            _session = session;
            _securityManager = securityManager;
        }

        public IOpResult<T> AuthorizeByClientIdFn<T>(Func<IOpResult<T>> functionClass, int clientId)
        //public IOpResult<T> AuthorizeByClientIdFn<T, U>(MethodClass<T, U> functionClass, int clientId)
        {
            var opResult = new OpResult<T>();

            var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
            var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;

            _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
            _session.ResourceAccessChecks.CheckAccessByAccessibleClientIds(clientId).MergeInto(opResult);

            if (!opResult.Success) return opResult;
            if (!isApplicantTrackingEnabled)
            {
                opResult.SetToFail().AddMessage(new GenericMsg("Applicant Tracking is not enabled."));
                return opResult;
            }

            return functionClass.Invoke();
        }
    }
}
