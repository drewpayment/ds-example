using System.Linq;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Interfaces.Query;
using Dominion.LaborManagement.EF.Query;
using NUnit.Framework;

namespace Dominion.LaborManagement.Test.Infrastructure.Query
{
    [TestFixture]
    public class GroupScheduleQueryTests
    {
        [Test]
        public void ByGroupScheduleId()
        {
            //ARRANGE
            var data = new[]
            {
                new GroupSchedule() {GroupScheduleId = 100},
                new GroupSchedule() {GroupScheduleId = 101},
                new GroupSchedule() {GroupScheduleId = 102},
            };

            var qry = new GroupScheduleQuery(data) as IGroupScheduleQuery;

            //ACT
            var result = qry.ByGroupScheduleId(101).ExecuteQuery().FirstOrDefault();

            //ASSERT
            Assert.AreSame(data[1], result, "Wrong object.");
        }

        [Test]
        public void ByClientId()
        {
            //ARRANGE
            var data = new[]
            {
                new GroupSchedule() {ClientId = 100},
                new GroupSchedule() {ClientId = 101},
                new GroupSchedule() {ClientId = 102},
            };

            var qry = new GroupScheduleQuery(data) as IGroupScheduleQuery;

            //ACT
            var result = qry.ByClientId(101).ExecuteQuery().FirstOrDefault();

            //ASSERT
            Assert.AreSame(data[1], result, "Wrong object.");
        }

    }

}
