using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Labor;
using Dominion.Core.Services.Dto.Employee;
using Dominion.Core.Services.Interfaces;
using Dominion.Core.Services.Internal.Providers;
using Dominion.Domain.Entities.Employee;
using Dominion.Domain.Entities.TimeClock;
using Dominion.LaborManagement.Dto.Clock;
using Dominion.LaborManagement.Dto.Clock.Misc;
using Dominion.LaborManagement.Dto.JobCosting;
using Dominion.LaborManagement.Dto.Sproc;
using Dominion.LaborManagement.Service.Api.DataServicesInjectors;
using Dominion.LaborManagement.Service.Internal.Providers;
using Moq;
using NUnit.Framework;

namespace Dominion.LaborManagement.Test.Service
{
    [TestFixture]
    public class EmployeePunchProviderTests
    {
        public EmployeePunchProvider Subject { get; set; }

        public Mock<IBusinessApiSession> Session { get; set; }

        public Mock<IJobCostingProvider> JobCosting { get; set; }

        public Mock<IDsDataServicesClockService> DataServices { get; set; }
        public Mock<IClientSettingProvider> SettingsProvider { get; set; }

        [SetUp]
        public void SetUp()
        {
            Session = new Mock<IBusinessApiSession>();
            JobCosting = new Mock<IJobCostingProvider>();
            DataServices = new Mock<IDsDataServicesClockService>();
            SettingsProvider = new Mock<IClientSettingProvider>();
            Subject = new EmployeePunchProvider(Session.Object, JobCosting.Object, DataServices.Object, null, SettingsProvider.Object, null, null, null, null, null, null) ;
        }


        [TestCase(1, null, null, 1)]
        [TestCase(null, 2, null, 2)]
        [TestCase(null, null, 3, 3)]
        [TestCase(1, 2, null, 1)]
        [TestCase(null, 2, 3, 2)]
        [TestCase(1, 2, 3, 1)]
        [TestCase(null, null, null, null)]
        public void Test_CalculateDefaultCostCenterId_Returns_Correct_ID_From_Given_Set(int? costCenter, int? jobCostCostCenter, int? employeeCostCenter, int? expected)
        {
            var jobCosting = new EmployeeJobCostingDto()
            {
                 ClientCostCenterId= jobCostCostCenter.GetValueOrDefault(0)
                
            };
            var employee = new EmployeeDto()
            {
                ClientCostCenterId = employeeCostCenter
            };
            var result = EmployeePunchProvider.CalculateDefaultCostCenterId(costCenter, jobCosting, employee);

            Assert.AreEqual(expected, result);
        }

        [TestCase(1, null, null, 1)]
        [TestCase(null, 2, null, 2)]
        [TestCase(null, null, 3, 3)]
        [TestCase(1, 2, null, 1)]
        [TestCase(null, 2, 3, 2)]
        [TestCase(1, 2, 3, 1)]
        [TestCase(null, null, null, null)]
        public void Test_CalculateDefaultDepartmentId_Returns_Correct_ID_From_Given_Set(int? departmentId, int? jobDepartmentId, int? employeeDepartment, int? expected)
        {
            var jobCosting = new EmployeeJobCostingDto()
            {
                ClientDepartmentId = jobDepartmentId.GetValueOrDefault(0)
            };
            var employee = new EmployeeDto()
            {
                ClientDepartmentId = employeeDepartment
            };
            var result = EmployeePunchProvider.CalculateDefaultDepartmentId(departmentId, jobCosting, employee);

            Assert.AreEqual(expected, result);
        }

        [TestCase(1, null, 1)]
        [TestCase(null, 2, 2)]
        [TestCase(1, 2, 1)]
        [TestCase(null, null, null)]
        public void Test_CalculateDefaultDivisionId_Returns_Correct_ID_From_Given_Set(int? jobDivision, int? employeeDivision, int? expected)
        {
            var jobCosting = new EmployeeJobCostingDto() 
            {
                ClientDivisionId = jobDivision.GetValueOrDefault(0)
            };
            var employee = new EmployeeDto()
            {
                ClientDivisionId = employeeDivision
            };
            var result = EmployeePunchProvider.CalculateDefaultDivisionId(employeeDivision, jobCosting, employee);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Test_CalculatePunchCostCenterId_Handles_Null_Last_Punch()
        {
            var result = EmployeePunchProvider.CalculatePunchCostCenterId(true, null, 1);

            Assert.AreEqual(1, result);
        }

        [Test]
        public void Test_CalculatePunchDepartmentId_Handles_Null_Last_Punch()
        {
            var result = EmployeePunchProvider.CalculatePunchDepartmentId(true, null, 1);

            Assert.AreEqual(1, result);
        }

        [Test]
        public void Test_CalculatePunchDivisionId_Handles_Null_Last_Punch()
        {
            var result = EmployeePunchProvider.CalculatePunchDivisionId(true, null, 1);

            Assert.AreEqual(1, result);
        }

        [Test]
        public void Test_CalculateAttributePunchId_Returns_Min_Int_Value_When_Not_An_Out_Punch_And_Given_Null_Default_Id()
        {
            var result = EmployeePunchProvider.CalculateAttributePunchId(false, null, null, 1);

            Assert.AreEqual(int.MinValue, result);
        }

        [Test]
        public void Test_CalculateAttributePunchId_Returns_LastPunchRelationId_When_Is_An_Out_Punch_With_Last_Punch()
        {
            var result = EmployeePunchProvider.CalculateAttributePunchId(true, new ClockEmployeeLastPunchDto(), null, 1);

            Assert.AreEqual(1, result);
        }

        //[Test]
        //public void Test_ShouldAddTransferPunch_Returns_True_When_LastPunch_Is_Transfer_And_Is_Out_Punch_And_Has_More_Than_One_Punch_In_The_Shift()
        //{
        //    var result = EmployeePunchProvider.ShouldAddTransferPunch(
        //        isOutPunch: true,
        //        punchTypeId: null,
        //        lastPunch: new ClockEmployeeLastPunchDto() { IsTransferPunch = true, ClientCostCenterId = 0 },
        //        hasMoreThanOnePunchThisShift: true,
        //        rules: new ClockClientRulesDto()),
        //        clientCostCenter: 0;

        //    Assert.AreEqual(true, result);
        //}

        //[Test]
        //public void Test_ShouldAddTransferPunch_Returns_False_When_LastPunch_Is_Not_Transfer_And_Is_Out_Punch_And_Has_More_Than_One_Punch_In_The_Shift()
        //{
        //    var result = EmployeePunchProvider.ShouldAddTransferPunch(
        //        isOutPunch: true,
        //        punchTypeId: null,
        //        lastPunch: new ClockEmployeeLastPunchDto() { IsTransferPunch = false, ClientCostCenterId = 0},
        //        hasMoreThanOnePunchThisShift: true,
        //        rules: new ClockClientRulesDto()),
        //        clientCostCenter: 0;

        //    Assert.AreEqual(false, result);
        //}

        //[Test]
        //public void Test_ShouldAddTransferPunch_Returns_False_When_Rules_Are_Using_InputPunches()
        //{
        //    var result = EmployeePunchProvider.ShouldAddTransferPunch(
        //        isOutPunch: true,
        //        punchTypeId: null,
        //        lastPunch: new ClockEmployeeLastPunchDto() { IsTransferPunch = true },
        //        hasMoreThanOnePunchThisShift: true,
        //        rules: new ClockClientRulesDto() { AllowInputPunches = true });

        //    Assert.AreEqual(false, result);
        //}

        [Test]
        public void Test_CleanPunch_Returns_A_New_Punch()
        {
            var first = new RealTimePunchRequest();
            var second = new RealTimePunchRequest();

            var result = EmployeePunchProvider.CleanPunch(first, second);

            Assert.AreNotSame(first, result);
            Assert.AreNotSame(second, result);
        }

        [Test]
        public void Test_CleanPunch_Returns_A_Punch_With_The_Same_IDs_As_The_First_Punch()
        {
            var first = new RealTimePunchRequest()
            {
                EmployeeId = 1,
                ClientId = 2,
                CostCenterId = 3,
                DepartmentId = 4,
                JobCostingAssignmentId1 = 5,
                JobCostingAssignmentId2 = 6,
                JobCostingAssignmentId3 = 7,
                JobCostingAssignmentId4 = 8,
                JobCostingAssignmentId5 = 9,
                JobCostingAssignmentId6 = 10
            };
            var second = new RealTimePunchRequest();

            var result = EmployeePunchProvider.CleanPunch(first, second);

            Assert.AreEqual(1, result.EmployeeId);
            Assert.AreEqual(2, result.ClientId);
            Assert.AreEqual(3, result.CostCenterId);
            Assert.AreEqual(4, result.DepartmentId);
            Assert.AreEqual(5, result.JobCostingAssignmentId1);
            Assert.AreEqual(6, result.JobCostingAssignmentId2);
            Assert.AreEqual(7, result.JobCostingAssignmentId3);
            Assert.AreEqual(8, result.JobCostingAssignmentId4);
            Assert.AreEqual(9, result.JobCostingAssignmentId5);
            Assert.AreEqual(10, result.JobCostingAssignmentId6);
        }

        [Test]
        public void Test_CleanPunch_Returns_A_Punch_With_The_Times_And_Comments_From_The_Second_Punch()
        {
            var first = new RealTimePunchRequest();
            var second = new RealTimePunchRequest()
            {
                OverridePunchTime = new DateTime(2016, 1, 1, 9, 0, 0),
                EmployeeComment = "Test"
            };

            var result = EmployeePunchProvider.CleanPunch(first, second);

            Assert.AreEqual(new DateTime(2016, 1, 1, 9, 0, 0), result.OverridePunchTime);
            Assert.AreEqual("Test", result.EmployeeComment);
        }

    }
}
