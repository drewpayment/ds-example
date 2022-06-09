using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Security;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Interfaces.Repositories;
using Dominion.LaborManagement.Dto.GroupScheduling;
using Dominion.LaborManagement.EF.Repository;
using Dominion.LaborManagement.Service.Mapping;
using Dominion.Testing.Util.Helpers.Dto;
using Dominion.Testing.Util.Helpers.Mapping;
using Dominion.Testing.Util.Helpers.Repo;
using Dominion.Utility.ExtensionMethods;
using NUnit.Framework;

namespace Dominion.LaborManagement.Test.Infrastructure.Repository
{
    [TestFixture]
    public class LaborManagementRepository_GroupScheduleQuery_Tests : BaseRepositoryTest
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

        #region ByClientId

        /// <summary>
        /// EXAMPLE: Repository (test)
        /// - digging into the navigational properties
        /// - always test the full potential of navigation properties.
        /// - lowfix: jay: maybe this type of test is better at the schema test level.
        /// </summary>
        [Test]
        public void ByClientId_Simple()
        {
            //ARRANGE
            var repo = new LaborManagementRepository(_data.DbContext) as ILaborManagementRepository;
            var testClientId = this.GetTestClientId();
            var obj = this.GetTestSchedule(testClientId);

            //ACT
            var result = repo
                .GroupScheduleQuery()
                .ByClientId(testClientId)
                .ExecuteQuery()
                .ToList()
                .FirstOrDefault();

            //ASSERT
            Assert.AreSame(obj, result);
        }

        /// <summary>
        /// EXAMPLE: Expression Map (test)
        /// review: jay: seems to be the fastest way to test expression mappings
        /// </summary>
        [Test]
        public void ByClientId_WithMap_ToGroupScheduleDto()
        {
            //ARRANGE
            var repo = new LaborManagementRepository(_data.DbContext) as ILaborManagementRepository;
            var obj = this.GetTestSchedule();

            //ACT
            var result = repo
                .GroupScheduleQuery()
                .ByGroupScheduleId(obj.GroupScheduleId)
                .ExecuteQueryAs(new GroupScheduleMaps.ToGroupScheduleDto())
                .ToList()
                .FirstOrDefault();

            //ASSERT
            Assert.IsTrue(CompareObjects.AreObjectsEqual(result, obj), "object values didn't match");
        }

        #endregion

        #region ByGroupScheduleId

        /// <summary>
        /// EXAMPLE: Repository (test)
        /// - digging into the navigational properties
        /// - always test the full potential of navigation properties.
        /// - lowfix: jay: maybe this type of test is better at the schema test level.
        /// </summary>
        [Test]
        public void ByGroupScheduleId_Simple()
        {
            //ARRANGE
            var repo = new LaborManagementRepository(_data.DbContext) as ILaborManagementRepository;
            var obj = this.GetTestSchedule();

            //ACT
            var result = repo
                .GroupScheduleQuery()
                .ByGroupScheduleId(obj.GroupScheduleId)
                .ExecuteQuery()
                .ToList()
                .FirstOrDefault();

            //ASSERT
            Assert.AreSame(obj, result);
        }

        /// <summary>
        /// EXAMPLE: Repository (test)
        /// - digging into the navigational properties
        /// - always test the full potential of navigation properties.
        /// - lowfix: jay: maybe this type of test is better at the schema test level.
        /// </summary>
        [Test]
        public void ByGroupScheduleId_All_NavigationProps()
        {
            //ARRANGE
            var repo = new LaborManagementRepository(_data.DbContext) as ILaborManagementRepository;
            var obj = this.GetTestSchedule();

            //ACT
            var result = repo
                .GroupScheduleQuery()
                .ByGroupScheduleId(obj.GroupScheduleId)
                .ExecuteQueryAs(
                    x => new
                    {
                        Client = x.Client,
                        Shifts = x.GroupScheduleShifts.Select(y => new
                        {
                            DayOfWeek = y.DayOfWeek,
                            Start = y.StartTime,
                            ScheduleGroup = y.ScheduleGroup,
                            CostCenterDescription = y.ScheduleGroup.ClientCostCenter.Description,
                        })
                    }
                )
                .ToList()
                .FirstOrDefault();

            //ASSERT
            Assert.IsNotNull(result.Client);
            Assert.IsNotNull(result.Shifts);
            Assert.IsNotNull(result.Shifts.First().Start);
            Assert.Greater(result.Shifts.First().Start.Hours, 0);
            Assert.IsInstanceOf<DayOfWeek>(result.Shifts.First().DayOfWeek);
            Assert.IsNotNull(result.Shifts.First().ScheduleGroup.ClientCostCenter);
            Assert.IsNotNull(result.Shifts.First().CostCenterDescription);
        }

        /// <summary>
        /// EXAMPLE: Expression Map (test)
        /// review: jay: seems to be the fastest way to test expression mappings
        /// </summary>
        [Test]
        public void ByGroupScheduleId_WithMap_ToGroupScheduleConfigRawDto()
        {
            //ARRANGE
            var repo = new LaborManagementRepository(_data.DbContext) as ILaborManagementRepository;
            var obj = this.GetTestSchedule();

            //ACT
            var result = repo
                .GroupScheduleQuery()
                .ByGroupScheduleId(obj.GroupScheduleId)
                .ExecuteQueryAs(new GroupScheduleMaps.ToCostCenterGroupScheduleRawDto())
                .ToList()
                .FirstOrDefault();

            //result.SerializeJson(@"c:\+export\GroupScheduleRawData.json");

            //ASSERT
            Assert.IsNotNull(result);

            var originalGroupScheduleMapped =
                new GroupScheduleMaps.ToCostCenterGroupScheduleRawDto().MapExpression.Compile().Invoke(obj);
            
            var comparisonResult = CompareObjects.AreObjectsEqual(
                result, 
                originalGroupScheduleMapped,
                "SourceId",
                "ScheduleGroupType");
            
            Assert.IsTrue(comparisonResult, "object values didn't match");
        }

        #endregion

        private int GetTestClientId()
        {
            var client = _dataLookup.ClientLookup.GetTestClient1();

            if(client == null)
                Assert.Inconclusive("Test client was not found in the database.");

            return client.ClientId;
        }

        private GroupSchedule GetTestSchedule(int? clientId = null)
        {
            clientId = clientId ?? this.GetTestClientId();

            var testSchedule = _dataLookup.GenericLookup.Qry<GroupSchedule>(x => x.ClientId == clientId).FirstOrDefault();

            if(testSchedule == null)
                Assert.Inconclusive("Mock group schedule was not found in the database for the test client.");

            return testSchedule;
        }

    }
}
