using System.Linq;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Entities.User;
using Dominion.Domain.Interfaces.Repositories;
using Dominion.LaborManagement.Dto.GroupScheduling;
using Dominion.LaborManagement.EF.Repository;
using Dominion.Testing.Util.Helpers.Repo;
using NUnit.Framework;

namespace Dominion.LaborManagement.Test.Infrastructure.Repository
{
    [TestFixture]
    public class LaborManagementRepository_ClientCostCenterQuery_Tests : BaseRepositoryTest
    {
        #region FIXTURE CONFIGURATION

        [TestFixtureSetUp]
        public override void FixtureSetup()
        {
            base.FixtureSetup();
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            base.FixtureTearDown();
        }

        #endregion

        [Test]
        public void ByIsActive_ActiveOnly_Success()
        {
            //ARRANGE
            var repo = new LaborManagementRepository(_data.DbContext) as ILaborManagementRepository;
            var client2 = _dataLookup.ClientLookup.GetTestClient2();
            var expected = _dataLookup.GenericLookup.Qry<ClientCostCenter>(x =>
                x.IsActive == true &&
                x.ClientId == client2.ClientId);

            //ACT
            var result = repo
                .ClientCostCenterQuery()
                .ByClientId(client2.ClientId)
                .ByIsActive(true)
                .ExecuteQuery()
                .ToList();

            //ASSERT
            CollectionAssert.AreEqual(expected, result);
            Assert.IsTrue(result.All(x=> x.IsActive));
        }

        [Test]
        public void ByScheduleGroupId_Success()
        {
            //ARRANGE
            var repo = new LaborManagementRepository(_data.DbContext) as ILaborManagementRepository;
            var scheduleGroup =
                _glu.QryTop<ScheduleGroup>(1, x => x.ScheduleGroupType == ScheduleGroupType.ClientCostCenter)
                .First();

            var expected = _dataLookup.GenericLookup.Qry<ClientCostCenter>(x =>
                 x.ScheduleGroups.Any(y => y.ScheduleGroupId == scheduleGroup.ScheduleGroupId));

            //ACT
            var result = repo
                .ClientCostCenterQuery()
                .ByScheduleGroupId(scheduleGroup.ScheduleGroupId)
                .ExecuteQuery()
                .ToList();

            //ASSERT
            CollectionAssert.AreEqual(expected, result);
            Assert.IsTrue(result.All(x=> x.ScheduleGroups.All(y => y.ScheduleGroupId == scheduleGroup.ScheduleGroupId)));
        }

        [Test]
        public void ByUserSupervisorSecurity_Success()
        {
            //ARRANGE
            var repo = new LaborManagementRepository(_data.DbContext) as ILaborManagementRepository;
            var clientId = _dataLookup.ClientLookup.GetTestClient1().ClientId;
            var userId = _dataLookup.UserLookup.Supervisor1.UserId;

            var expected = _dataLookup.GenericLookup.Qry<ClientCostCenter>(x =>
                 x.UserSupervisorSecurities.Any(z => z.ClientId == clientId) && 
                 x.UserSupervisorSecurities.Any(y => y.UserId == userId));

            //ACT
            var result = repo
                .ClientCostCenterQuery()
                .ByUserSupervisorSecurity(clientId, userId)
                .ExecuteQuery()
                .ToList();

            //ASSERT
            CollectionAssert.AreEqual(expected, result);
        }

    }
}
