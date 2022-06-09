using Dominion.Core.Services.Api;
using Dominion.Core.Test.App.Core.Api.SecurityManagerTest;
using Dominion.LaborManagement.Service.Internal.Security;

using NUnit.Framework;

namespace Dominion.LaborManagement.Test.Internal.Security
{
    [Category("Security Actions")]
    public class LaborManagementActionTypeSyncTests
    {
        /// <summary>
        /// Utility test used to synchronize the database (per app.config connection) with the currently registered 
        /// <see cref="SystemActionType"/>s.
        /// </summary>
        [Test]
        public void SyncActionTypes()
        {
            var results = ActionTypeSynchronizer.SynchronizeActionTypes<LaborManagementActionType>();
            Assert.IsTrue(results.IsValid, "Synchronization of permissions (action types) was not successful.");
        }
    }
}