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
using Dominion.Domain.Entities.ApplicantTracking;
using NUnit.Framework;

namespace Dominion.LaborManagement.Test.Infrastructure.Repository
{
    [TestFixture]
    public class LaborManagementRepository_ApplicantApplicationEmailHistoryQuery_Tests : BaseRepositoryTest
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
        public void ApplicantApplicationEmailHistory_Test()
        {
            //ARRANGE
            var repo = new ApplicantTrackingRepository(_data.DbContext) as IApplicantTrackingRepository;
            int headerId = 1134;

            //ACT
            var result = repo
                .ApplicantApplicationEmailHistoryQuery()
                .ByApplicationHeaderId(headerId)
                .ExecuteQuery()
                .ToList()
                .FirstOrDefault();

            //ASSERT
            //Assert.AreNotEqual(null, result);
            Assert.AreEqual(headerId, result.ApplicationHeaderId);
            Assert.AreEqual("01/29/2016", result.SentDate?.ToString("MM/dd/yyyy"));
        }

        [Test]
        public void ApplicantApplicationHeader_Test()
        {
            //ARRANGE
            var repo = new ApplicantTrackingRepository(_data.DbContext) as IApplicantTrackingRepository;
            int headerId = 1134;

            var result = repo
                .ApplicantApplicationHeaderQuery()
                .ByApplicationHeaderId(headerId)
                .ExecuteQueryAs(new ApplicantApplicationHeaderMaps.ToApplicantApplicationHeaderDto())
                .FirstOrDefault();

            //ACT
            /*
            var result = repo
                .ApplicantApplicationHeaderQuery()
                .ByApplicationHeaderId(headerId)
                .ExecuteQuery()
                .ToList()
                .FirstOrDefault();*/

            //ASSERT
            Assert.AreEqual(14, result.ApplicantCompanyCorrespondence.ApplicantCompanyCorrespondenceId);
        }
        [Test]
        public void ApplicantCorrespondence_Test()
        {
            //ARRANGE
            var repo = new ApplicantTrackingRepository(_data.DbContext) as IApplicantTrackingRepository;
            int corresId = 10;

            //ACT
            var result = repo
                .ApplicantCompanyCorrespondenceQuery()
                .ByCorrespondenceId(corresId)
                .ExecuteQuery()
                .ToList()
                .FirstOrDefault();

            //ASSERT
            Assert.AreEqual(10, result.ApplicantCompanyCorrespondenceId);
        }
    }
}