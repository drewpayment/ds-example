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
using Dominion.LaborManagement.Dto.Scheduling;
using Dominion.LaborManagement.Service.Api;
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
    public class CoAdmin_GroupSchedule_Service_Tests
    {
        [Test, Explicit]
        public void CoAdmin_Get_Config_Succeed()
        {
            //ARRANGE
            var fundamentals = new ProtoFundamentals(ConnStr.ConnectionString);
            var session = new ProtoApiSession(fundamentals, PreConfiguredProtoApiSettings.AmericanHouse_JayJohnson_Sys());
            var providerSG = new ScheduleGroupProvider(session.BusinessApiSession) as IScheduleGroupProvider;
            var providerGS = new GroupScheduleProvider(session.BusinessApiSession) as IGroupScheduleProvider;
            var providerSched = new SchedulingProvider(session.BusinessApiSession, providerSG) as ISchedulingProvider;
            var service =
                new GroupScheduleService(session.BusinessApiSession, providerGS, providerSG, providerSched) as
                    IGroupScheduleService;

            var result = service.GetGroupSchedule(1);

            result.Data.SerializeJson(@"c:\+export\CoAdmin_Get_Config_Succeed.json");

            //-------------------------------------
            //Testing
            //-------------------------------------
            foreach(var msgObject in result.MsgObjects)
                Console.WriteLine(msgObject.Msg);

            Assert.IsTrue(result.Success);
        }

        [Test, Explicit]
        public void CoAdmin_Change_Save_1_Update_Config_Succeed()
        {
            //ARRANGE
            var fundamentals = new ProtoFundamentals(ConnStr.ConnectionString);
            var session = new ProtoApiSession(fundamentals, PreConfiguredProtoApiSettings.AmericanHouse_JayJohnson_Sys());
            var providerSG = new ScheduleGroupProvider(session.BusinessApiSession) as IScheduleGroupProvider;
            var providerGS = new GroupScheduleProvider(session.BusinessApiSession) as IGroupScheduleProvider;
            var providerSched = new SchedulingProvider(session.BusinessApiSession, providerSG) as ISchedulingProvider;
            var service =
                new GroupScheduleService(session.BusinessApiSession, providerGS, providerSG, providerSched) as
                    IGroupScheduleService;

            var incoming =
                @"C:\+curwork\~5\TestSave-Json-JayDev\Save_CoAdmin1.json"
                    .DeserializeJson<GroupScheduleDtos.GroupScheduleWithScheduleGroupsDto>();

            var result = service.SaveOrUpdateGroupSchedule(incoming);

            result.Data.SerializeJson(@"c:\+export\CoAdmin_Change_Save_1_Update_Config_Succeed.json");

            //-------------------------------------
            //Testing
            //-------------------------------------
            foreach(var msgObject in result.MsgObjects)
                Console.WriteLine(msgObject.Msg);

            Assert.IsTrue(result.Success);
        }

        [Test, Explicit]
        public void CoAdmin_Change_Save_2_Update_Config_Succeed()
        {
            //ARRANGE
            var fundamentals = new ProtoFundamentals(ConnStr.ConnectionString);
            var session = new ProtoApiSession(fundamentals, PreConfiguredProtoApiSettings.AmericanHouse_JayJohnson_Sys());
            var providerSG = new ScheduleGroupProvider(session.BusinessApiSession) as IScheduleGroupProvider;
            var providerGS = new GroupScheduleProvider(session.BusinessApiSession) as IGroupScheduleProvider;
            var providerSched = new SchedulingProvider(session.BusinessApiSession, providerSG) as ISchedulingProvider;
            var service =
                new GroupScheduleService(session.BusinessApiSession, providerGS, providerSG, providerSched) as
                    IGroupScheduleService;

            var incoming =
                @"C:\+curwork\~5\TestSave-Json-JayDev\Save_CoAdmin2.json"
                    .DeserializeJson<GroupScheduleDtos.GroupScheduleWithScheduleGroupsDto>();

            var result = service.SaveOrUpdateGroupSchedule(incoming);

            result.Data.SerializeJson(@"c:\+export\CoAdmin_Change_Save_2_Update_Config_Succeed.json");

            //-------------------------------------
            //Testing
            //-------------------------------------
            foreach(var msgObject in result.MsgObjects)
                Console.WriteLine(msgObject.Msg);

            Assert.IsTrue(result.Success);
        }

        [Test, Explicit]
        public void CoAdmin_Adhoc_2015_03_17()
        {
            //ARRANGE
            var fundamentals = new ProtoFundamentals(ConnStr.ConnectionString);
            var session = new ProtoApiSession(fundamentals, PreConfiguredProtoApiSettings.AmericanHouse_JayJohnson_Sys());
            var providerSG = new ScheduleGroupProvider(session.BusinessApiSession) as IScheduleGroupProvider;
            var providerGS = new GroupScheduleProvider(session.BusinessApiSession) as IGroupScheduleProvider;
            var providerSched = new SchedulingProvider(session.BusinessApiSession, providerSG) as ISchedulingProvider;
            var service =
                new GroupScheduleService(session.BusinessApiSession, providerGS, providerSG, providerSched) as
                    IGroupScheduleService;

            var incoming =
                @"C:\+curwork\~5\TestSave-Json-JayDev\Save_Error_From_Josh2.json"
                    .DeserializeJson<GroupScheduleDtos.GroupScheduleWithScheduleGroupsDto>();

            var result = service.SaveOrUpdateGroupSchedule(incoming);

            result.Data.SerializeJson(@"c:\+export\CoAdmin_Adhoc_2015_03_17.json");

            //-------------------------------------
            //Testing
            //-------------------------------------
            foreach(var msgObject in result.MsgObjects)
                Console.WriteLine(msgObject.Msg);

            Assert.IsTrue(result.Success);
        }

    }
}
