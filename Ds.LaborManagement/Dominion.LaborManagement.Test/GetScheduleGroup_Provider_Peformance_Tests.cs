using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dominion.Core.Services.Interfaces;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Employee;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Entities.User;
using Dominion.LaborManagement.Dto.GroupScheduling;
using Dominion.LaborManagement.Service.Internal;
using Dominion.LaborManagement.Service.Internal.Providers;
using Dominion.LaborManagement.Service.Mapping;
using Dominion.LaborManagement.Test.Properties;
using Dominion.Testing.Util.Common;
using Dominion.Testing.Util.Helpers.Mapping;
using Dominion.Testing.Util.Helpers.Prototyping;
using Dominion.Utility.ExtensionMethods;
using Dominion.Utility.OpResult;
using Dominion.Utility.Query.LinqKit;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Dominion.LaborManagement.Test
{
    [TestFixture, Explicit]
    public class GetSchedule_Provider_Performance_Tests
    {
        [Test]
        public void GetClientCostCenterScheduleGroups_Speed_Test_EF()
        {
            //ARRANGE
            var fundamentals = new ProtoFundamentals(ConnStr.ConnectionString);
            var session = new ProtoApiSession(fundamentals, PreConfiguredProtoApiSettings.DominionTime_JayJohnson_Sys());
            var provider = new ScheduleGroupProvider(session.BusinessApiSession) as IScheduleGroupProvider;

            //make sure the EF machine is warmed up by performing a query that doesn't cache the objects I'm looking for.
            var xxx = new GroupScheduleProvider(session.BusinessApiSession) as IGroupScheduleProvider;
            var yyy = xxx.GetGroupScheduleForScheduling(1);
            Console.WriteLine(yyy.Data.FirstOrDefault().Name);

            var sw = new Stopwatch();
            sw.Start();

            var result = provider.GetClientCostCenterScheduleGroups(
                clientId: session.LoggedInUsersClient.ClientId,
                userId: null,
                scheduleGroupId: null,
                withScheduleGroupShiftNames: false);

            Console.WriteLine("Record Count: " + result.Count());
            result.SerializeJson(@"c:\+export\GetScheduleGroups_SpeedTest_EF_" + result.Count() + ".json");

            sw.Stop();
            
            Console.WriteLine("Elapsed Milliseconds: " + sw.ElapsedMilliseconds);

            Assert.IsNotNull(result);
        }


    }
}
