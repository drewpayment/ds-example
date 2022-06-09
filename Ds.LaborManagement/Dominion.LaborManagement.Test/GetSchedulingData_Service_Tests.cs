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
    public class GetSchedulingData_Service_Tests
    {
        #region Employee Scheduling Viewing

        [Test, Explicit]
        public void GetGroupSchedule_SchedulingData_Succeed()
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

            var result = service.GetGroupScheduleForScheduling(1);

            result.Data.SerializeJson(@"c:\+export\ServiceTest_GetGroupScheduleForScheduling.json");

            //-------------------------------------
            //Testing
            //-------------------------------------
            foreach(var msgObject in result.MsgObjects)
                Console.WriteLine(msgObject.Msg);

            Assert.IsTrue(result.Success);
        }

        [Test, Explicit]
        public void GetSchedulesPublished_Succeed()
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

            var startDate = DateTime.Parse("2015-03-08");
            var endDate = DateTime.Parse("2015-03-14");

            var result = service.GetEmployeeScheduleShifts(startDate, endDate, 1, 23621);

            result.Data.SerializeJson(@"c:\+export\ServiceTest_GetEmployeeScheduleShifts_Published.json");

            //-------------------------------------
            //Testing
            //-------------------------------------
            foreach(var msgObject in result.MsgObjects)
                Console.WriteLine(msgObject.Msg);

            Assert.IsTrue(result.Success);
        }

        [Test, Explicit]
        public void GetSchedulesPreview_Succeed()
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

            var startDate = DateTime.Parse("2015-03-01");
            var endDate = DateTime.Parse("2015-03-07");

            var result = service.GetEmployeeScheduleShifts(startDate, endDate, 1, 23621);

            result.Data.SerializeJson(@"c:\+export\ServiceTest_GetEmployeeScheduleShifts_Preview.json");

            //-------------------------------------
            //Testing
            //-------------------------------------
            foreach(var msgObject in result.MsgObjects)
                Console.WriteLine(msgObject.Msg);

            Assert.IsTrue(result.Success);
        }

        #endregion

        [Test, Explicit]
        public void SaveEmployeeSchedule_Preview1_Succeed()
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

            var data = new EmployeeSchedulesPersistDto()
            {
                StartDate = DateTime.Parse("2015-03-08"),
                EndDate = DateTime.Parse("2015-03-14"),
                ScheduleGroupId = 1,
                ScheduleSourceId = 23621,
                EmployeeSchedulesDtos 
                    = @"C:\+curwork\~5\TestSave-Json-JayDev\Save_Preview1.json".DeserializeJson<IEnumerable<EmployeeSchedulesDto>>(),
            };

            var result = service.SaveOrUpdateEmployeeSchedule(data);

            result.Data.SerializeJson(@"c:\+export\ServiceTest_SaveOrUpdateEmployeeSchedule_Preview1.json");

            //-------------------------------------
            //Testing
            //-------------------------------------
            foreach(var msgObject in result.MsgObjects)
                Console.WriteLine(msgObject.Msg);

            Assert.IsTrue(result.Success);
        }

        [Test, Explicit]
        public void SaveEmployeeSchedule_Publish3_Succeed()
        {
            //ARRANGE
            var fundamentals = new ProtoFundamentals(ConnStr.ConnectionString);
            var session = new ProtoApiSession(fundamentals, PreConfiguredProtoApiSettings.AmericanHouse_JayJohnson_Sys());
            var providerSG = new ScheduleGroupProvider(session.BusinessApiSession) as IScheduleGroupProvider;
            var providerGS = new GroupScheduleProvider(session.BusinessApiSession) as IGroupScheduleProvider;
            var providerSched = new SchedulingProvider(session.BusinessApiSession,providerSG) as ISchedulingProvider;
            var service =
                new GroupScheduleService(session.BusinessApiSession, providerGS, providerSG, providerSched) as
                    IGroupScheduleService;

            var data = new EmployeeSchedulesPersistDto()
            {
                StartDate = DateTime.Parse("2015-03-08"),
                EndDate = DateTime.Parse("2015-03-14"),
                ScheduleGroupId = 1,
                ScheduleSourceId = 23621,
                EmployeeSchedulesDtos 
                    = @"C:\+curwork\~5\TestSave-Json-JayDev\Save_Publish3.json".DeserializeJson<IEnumerable<EmployeeSchedulesDto>>(),
            };

            var result = service.SaveOrUpdateEmployeeSchedule(data);

            result.Data.SerializeJson(@"c:\+export\ServiceTest_SaveOrUpdateEmployeeSchedule_Preview3.json");

            //-------------------------------------
            //Testing
            //-------------------------------------
            foreach(var msgObject in result.MsgObjects)
                Console.WriteLine(msgObject.Msg);

            Assert.IsTrue(result.Success);
        }

        [Test, Explicit]
        public void SaveEmployeeSchedule_Preview_To_Publish1_Succeed()
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

            var data = new EmployeeSchedulesPersistDto()
            {
                StartDate = DateTime.Parse("2015-03-01"),
                EndDate = DateTime.Parse("2015-03-07"),
                ScheduleGroupId = 1,
                ScheduleSourceId = 23621,
                EmployeeSchedulesDtos 
                    = @"C:\+curwork\~5\TestSave-Json-JayDev\Save_Preview_To_Publish_1.json".DeserializeJson<IEnumerable<EmployeeSchedulesDto>>(),
            };

            var result = service.SaveOrUpdateEmployeeSchedule(data);

            result.Data.SerializeJson(@"c:\+export\Save_Preview_To_Publish_1_RESULT.json");

            //-------------------------------------
            //Testing
            //-------------------------------------
            foreach(var msgObject in result.MsgObjects)
                Console.WriteLine(msgObject.Msg);

            Assert.IsTrue(result.Success);
        }

    }
}
