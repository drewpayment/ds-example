using Dominion.Core.Services.Api;
using Dominion.Core.Test.App.Core.Api.SecurityManagerTest;
using Dominion.LaborManagement.Service.Internal.Security;

using NUnit.Framework;

namespace Dominion.LaborManagement.Test.Internal.Security
{
    [TestFixture, Explicit("Run only as needed to push the action types.")]
    [Category("Security Actions")]
    public class AplicantTrackingActionTypeSyncTest
    {
        /// <summary>
        /// Utility test used to synchronize the database (per app.config connection) with the currently registered 
        /// <see cref="SystemActionType"/>s.
        /// </summary>
        [Test]
        public void SyncActionTypes()
        {
            var results = ActionTypeSynchronizer.SynchronizeActionTypes<ApplicantTrackingActionType>();
            Assert.IsTrue(results.IsValid, "Synchronization of permissions (action types) was not successful.");
        }
    }
}