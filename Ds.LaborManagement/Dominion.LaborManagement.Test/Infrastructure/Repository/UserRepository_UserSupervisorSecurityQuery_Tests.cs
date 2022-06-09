using System.Linq;
using Dominion.Core.Dto.Security;
using Dominion.Core.EF.Repositories;
using Dominion.Domain.Entities.Security;
using Dominion.Domain.Interfaces.Repositories;
using Dominion.Testing.Util.Helpers.Repo;
using NUnit.Framework;

namespace Dominion.LaborManagement.Test.Infrastructure.Repository
{
    [TestFixture]
    public class UserRepository_UserSupervisorSecurityQuery_Tests : BaseRepositoryTest
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
        public void Query_Success()
        {
            //ARRANGE
            var repo = new UserRepository(_data.DbContext) as IUserRepository;
            var supervisor = _dataLookup.UserLookup.Supervisor1;
            var expected = _glu.Qry<UserSupervisorSecurityGroupAccess>(x => x.UserId == supervisor.UserId);

            //ACT
            var result = repo
                .QuerySupervisorSecurityGroupAccess()
                .ByUserId(supervisor.UserId)
                .ByUserSecurityGroupType(UserSecurityGroupType.ClientCostCenter)
                .ExecuteQuery()
                .ToList();

            //ASSERT
            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void Query_No_UserSecurityGroupType_Match_EmptyResults()
        {
            //ARRANGE
            var repo = new UserRepository(_data.DbContext) as IUserRepository;
            var supervisor = _dataLookup.UserLookup.Supervisor1;

            //ACT
            var result = repo
                .QuerySupervisorSecurityGroupAccess()
                .ByUserId(supervisor.UserId)
                .ByUserSecurityGroupType(UserSecurityGroupType.None) //fails because of .None
                .ExecuteQuery()
                .ToList();

            //ASSERT
            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public void Query_No_UserId_Match_EmptyResults()
        {
            //ARRANGE
            var repo = new UserRepository(_data.DbContext) as IUserRepository;

            //ACT
            var result = repo
                .QuerySupervisorSecurityGroupAccess()
                .ByUserId(int.MinValue) //fails because no user id match
                .ByUserSecurityGroupType(UserSecurityGroupType.ClientCostCenter)
                .ExecuteQuery()
                .ToList();

            //ASSERT
            CollectionAssert.IsEmpty(result);
        }

    }

}
