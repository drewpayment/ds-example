using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Employee;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Entities.Payroll;
using Dominion.Domain.Entities.TimeClock;
using Dominion.LaborManagement.Service.Mapping.Clock;
using NUnit.Framework;

namespace Dominion.LaborManagement.Test.Service.Mapping
{
    [TestFixture]
    public class ClockClientRulesMapsTests
    {
        public ClockClientRulesMaps Subject { get; private set; }

        [SetUp]
        public void SetUp()
        {
            Subject = new ClockClientRulesMaps();
        }

        


    }
    [TestFixture]
    public class ToClockClientRulesSummaryPartialTests
    {
        public ClockClientRulesMaps.ToClockClientRulesSummaryPartial Subject { get; private set; }

        [SetUp]
        public void SetUp()
        {
            Subject = new ClockClientRulesMaps.ToClockClientRulesSummaryPartial();
        }

        [Test]
        public void Test_Map_Handles_Object_With_Null_Holiday()
        {
            var client = new Client()
            {
                AccountOptions = new List<ClientAccountOption>(),
                Earnings = new List<ClientEarning>()
            };
            var employeePay = new EmployeePay()
            {
                ClockEmployee = new ClockEmployee()
                {
                    TimePolicy = new ClockClientTimePolicy()
                    {
                        Rules = new ClockClientRules(),
                    },
                    Client = client
                },
                Employee = new Employee()
                {
                    Client = client
                }
            };
            var mapped = Subject.Map(employeePay);

            Assert.NotNull(mapped);
            Assert.AreEqual(0, mapped.HolidayClientEarningId);
            Assert.AreEqual(0, mapped.HolidayWorkedClientEarningId);
        }

    }
}
