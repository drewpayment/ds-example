using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Security;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Entities.Security;
using Dominion.Domain.Interfaces.Repositories;
using Dominion.LaborManagement.Dto.GroupScheduling;
using Dominion.LaborManagement.EF.Repository;
using Dominion.LaborManagement.Service.Mapping;
using Dominion.Testing.Util.Helpers.Dto;
using Dominion.Testing.Util.Helpers.Mapping;
using Dominion.Testing.Util.Helpers.Repo;
using Dominion.Utility.ExtensionMethods;
using NUnit.Framework;
using Dominion.Utility.ExtensionMethods;

namespace Dominion.LaborManagement.Test.Infrastructure.Repository
{
    [TestFixture]
    public class LaborManagementRepository_ClockEmployeeCostCenterQuery_Tests : BaseRepositoryTest
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

            //ACT
            var result = repo
                .ClockEmployeeCostCenterQuery()
                .ByCostCenterId(9)
                .ExecuteQuery()
                .ToList();

            //ASSERT
            Assert.NotNull(result);
        }


    }
}
