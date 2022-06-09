using System.Linq;

using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Interfaces.Repositories;
using Dominion.LaborManagement.EF.Repository;
using Dominion.LaborManagement.Service.Mapping;
using Dominion.Testing.Util.Helpers.Mapping;
using Dominion.Testing.Util.Helpers.Repo;

using NUnit.Framework;

namespace Dominion.LaborManagement.Test.Infrastructure.Repository
{
    [TestFixture]
    public class LaborManagementRepository_EmployeeDefaultShift_Tests : BaseRepositoryTest
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
        public void By_EmployeeIds()
        {
            //ARRANGE
            var repo = new LaborManagementRepository(_data.DbContext) as ILaborManagementRepository;
            var emps = _dataLookup.ClientLookup.GetTestClient1().Employees.Select(x => x.EmployeeId);

            var expected =
                _dataLookup.GenericLookup
                    .Qry<EmployeeDefaultShift>(x =>emps.Contains(x.EmployeeId))
                    .Select(x => x.EmployeeDefaultShiftId);

            //ACT
            var result = repo
                .EmployeeDefaultShiftQuery()
                .ByEmployeesIds(emps)
                .ExecuteQueryAs(x => x.EmployeeDefaultShiftId);

            //Json.I.SN("EmployeeDefaultShift").TDTS().GenFile(result);

            //ASSERT
            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void By_ScheduleGroupId()
        {
            //ARRANGE
            var repo = new LaborManagementRepository(_data.DbContext) as ILaborManagementRepository;
            var client = _dataLookup.ClientLookup.GetTestClient1();
            var schedId = _dataLookup.GenericLookup
                .Qry<ScheduleGroup>(x => x.ClientCostCenter.ClientId == client.ClientId)
                .Select(x => x.ScheduleGroupId)
                .OrderBy(x => x)
                .Take(1)
                .FirstOrDefault();

            var expected =
                _dataLookup.GenericLookup
                    .Qry<EmployeeDefaultShift>(x => x.GroupScheduleShift.ScheduleGroupId == schedId)
                    .Select(x => x.EmployeeDefaultShiftId);

            //ACT
            var result = repo
                .EmployeeDefaultShiftQuery()
                .ByScheduleGroupId(schedId)
                .ExecuteQueryAs(x => x.EmployeeDefaultShiftId);

            //Json.I.SN("EmployeeDefaultShift_BySchedID").TDTS().GenFile(result);

            //ASSERT
            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void By_GroupScheduleId()
        {
            //ARRANGE
            var repo = new LaborManagementRepository(_data.DbContext) as ILaborManagementRepository;
            var client = _dataLookup.ClientLookup.GetTestClient1();
            var groupSchedId = _dataLookup.GenericLookup
                .Qry<GroupSchedule>(x =>
                    x.ClientId == client.ClientId &&
                    x.GroupScheduleShifts.Any(y => y.EmployeeDefaultShifts.Any()))
                .Select(x => x.GroupScheduleId)
                .OrderBy(x => x)
                .FirstOrDefault();

            var expected =
                _dataLookup.GenericLookup
                    .Qry<EmployeeDefaultShift>(x => x.GroupScheduleShift.GroupScheduleId == groupSchedId)
                    .Select(x => x.EmployeeDefaultShiftId)
                    .OrderBy(x => x)
                    .ToList();

            //ACT
            var result = repo
                .EmployeeDefaultShiftQuery()
                .ByGroupScheduleId(groupSchedId)
                .ExecuteQueryAs(x => x.EmployeeDefaultShiftId)
                .OrderBy(x => x)
                .ToList();

            //Json.I.SN("EmployeeDefaultShift_BySchedID").TDTS().GenFile(result);

            //ASSERT
            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void SchedulingMaps_ToEmployeeDefaultShift()
        {
            //ARRANGE
            var repo = new LaborManagementRepository(_data.DbContext) as ILaborManagementRepository;
            var emps = _dataLookup.ClientLookup.GetTestClient1().Employees.Take(1).Select(x => x.EmployeeId);
            var obj = _dataLookup.GenericLookup
                .Qry<EmployeeDefaultShift>(x => emps.Contains(x.EmployeeId))
                .FirstOrDefault();

            //ACT
            var result = repo
                .EmployeeDefaultShiftQuery()
                .ByEmployeesIds(emps)
                .ExecuteQueryAs(new SchedulingMaps.ToEmployeeDefaultShift())
                .FirstOrDefault();

            //Json.I.SN("EmployeeDefaultShift2").TDTS().GenFile(result);

            //ASSERT
            //Assert.IsNotNull(result);
            Assert.IsTrue(CompareObjects.AreObjectsEqual(result, obj), "object values didn't match");
        }

    }

}
