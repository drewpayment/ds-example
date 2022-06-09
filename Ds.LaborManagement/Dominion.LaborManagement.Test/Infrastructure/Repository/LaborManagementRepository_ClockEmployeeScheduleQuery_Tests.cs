using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Security;
using Dominion.Domain.Entities.Employee;
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
    public class LaborManagementRepository_ClockEmployeeScheduleQuery_Tests : BaseRepositoryTest
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
        public void ByEventDateRange()
        {
            //ARRANGE
            var repo = new LaborManagementRepository(_data.DbContext) as ILaborManagementRepository;
            var weekStart = new DateTime(2015, 3, 1);
            var weekEnd = weekStart.AddDays(7);
            var expected = _glu.Qry<ClockEmployeeSchedule>(x =>
                x.EventDate >= weekStart && x.EventDate <= weekEnd);

            //ACT
            var result = repo
                .ClockEmployeeScheduleQuery()
                .ByEventDateRange(weekStart, weekEnd)
                .ExecuteQuery()
                .ToList();

            //ASSERT
            base.AssertResultsAreSame(
                expected.OrderBy(x => x.ClockEmployeeScheduleId), 
                result.OrderBy(x => x.ClockEmployeeScheduleId));
        }

        [Test]
        public void ByEmployeeIds()
        {
            //ARRANGE
            var repo = new LaborManagementRepository(_data.DbContext) as ILaborManagementRepository;
            var client = _dataLookup.ClientLookup.GetTestClient1();

            if(client == null)
                Assert.Inconclusive("Test schema client was not found.");

            var clientId = client.ClientId;
            var employeeIds = _glu.Qry<Employee>(x => x.ClientId == clientId)
                .Select(x => x.EmployeeId)
                .ToList();

            var expected = _glu.Qry<ClockEmployeeSchedule>(x =>
                employeeIds.Contains(x.EmployeeId));

            if(expected == null || !expected.Any())
                Assert.Inconclusive("No mock test data found.");

            //ACT
            var result = repo
                .ClockEmployeeScheduleQuery()
                .ByEmployeeIds(employeeIds)
                .ExecuteQuery()
                .ToList();

            //ASSERT            
            base.AssertResultsAreSame(
                expected.OrderBy(x => x.ClockEmployeeScheduleId), 
                result.OrderBy(x => x.ClockEmployeeScheduleId));
        }


    }
}
