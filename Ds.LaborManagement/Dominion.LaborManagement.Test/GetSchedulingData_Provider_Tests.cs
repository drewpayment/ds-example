using System;
using System.Collections.Generic;
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
    public class GetSchedulingData_Provider_Tests
    {
        [Test, Explicit]
        public void GetSchedulesPublished_Succeed()
        {
            //ARRANGE
            var fundamentals = new ProtoFundamentals(ConnStr.ConnectionString);
            var settings = PreConfiguredProtoApiSettings.AmericanHouse_JayJohnson_Sys();
            var session = new ProtoApiSession(fundamentals, settings);
            var groupProvider = new ScheduleGroupProvider(session.BusinessApiSession) as IScheduleGroupProvider;
            var provider = new SchedulingProvider(session.BusinessApiSession, groupProvider ) as ISchedulingProvider;

            var startDate = DateTime.Parse("2015-03-08");
            var endDate = DateTime.Parse("2015-03-14");

            var result = provider.GetEmployeeScheduleShifts(settings.ClientId.Value, startDate, endDate, 13, 9);

            result.Data.SerializeJson(@"c:\+export\GetSchedulesPublished_ProviderTest.json");

            Assert.IsTrue(result.Success);
        }

        [Test, Explicit]
        public void GetSchedulesPreview_Succeed()
        {
            //ARRANGE
            var fundamentals = new ProtoFundamentals(ConnStr.ConnectionString);
            var settings = PreConfiguredProtoApiSettings.AmericanHouse_JayJohnson_Sys();
            var session = new ProtoApiSession(fundamentals, settings);
            var groupProvider = new ScheduleGroupProvider(session.BusinessApiSession) as IScheduleGroupProvider;
            var provider = new SchedulingProvider(session.BusinessApiSession, groupProvider) as ISchedulingProvider;

            var startDate = DateTime.Parse("2015-03-01");
            var endDate = DateTime.Parse("2015-03-07");

            var result = provider.GetEmployeeScheduleShifts(settings.ClientId.Value, startDate, endDate, 13, 9);

            result.Data.SerializeJson(@"c:\+export\GetSchedulesPreview_ProviderTest.json");

            Assert.IsTrue(result.Success);
        }

    }
}
