using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Labor.Enum;
using Dominion.Core.Services.Api;
using Dominion.Core.Services.Api.Notification;
using Dominion.Core.Services.Emails;
using Dominion.Core.Services.Interfaces;
using Dominion.Core.Services.Internal.Providers;
using Dominion.Core.Services.Internal.Providers.Notifications;
using Dominion.Core.Services.Internal.Providers.Resources;
using Dominion.Core.Test.Helpers.TestObjects.Mocks;
using Dominion.LaborManagement.Dto.ApplicantTracking.IndeedApply;
using Dominion.LaborManagement.Dto.Enums;
using Dominion.LaborManagement.Service.Api;
using Dominion.LaborManagement.Service.Api.Notification;
using Dominion.LaborManagement.Service.Internal.Providers;
using Dominion.Testing.Util.Common;
using Dominion.Testing.Util.Helpers.Prototyping;
using Moq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Dominion.Authentication.Interface.Api;
using Dominion.Utility.OpResult;

namespace Dominion.LaborManagement.Test.Internal.Providers
{
    public class IndeedApplicationProviderIntegrationTests
    {
        private const int _california = 7;
        private IndeedApplication _app;
        private IIndeedApplicationProvider _provider;
        private IApplicationDisclaimerService _disclaimerService;
        private Mock<IApplicantTrackingService> _applicantTrackingService;
        private Mock<ISecurityManager> _securityManager;
        private Mock<IApplicantTrackingEmail> _applicantTrackingEmail;
        private Mock<IApplicantTrackingNotificationService> _notificationService;
        private Mock<IAzureResourceProvider> _azureResourceProvider;

        [SetUp]
        public void SetUp()
        {
            var fundamentals = new ProtoFundamentals(ConnStr.ConnectionString);
            var settings = PreConfiguredProtoApiSettings.PioneerResources_JayJohnson_Sys();
            var session = new ProtoApiSession(fundamentals, settings);
            var mock = new ApiSessionMockBase();

            _applicantTrackingService = new Mock<IApplicantTrackingService>();
            _securityManager = new Mock<ISecurityManager>();
            _applicantTrackingEmail = new Mock<IApplicantTrackingEmail>();
            _notificationService = new Mock<IApplicantTrackingNotificationService>();
            _azureResourceProvider = new Mock<IAzureResourceProvider>();
            var loginServiceMock = new Mock<ILoginService>();
            loginServiceMock.Setup(x => x.CheckUserName(It.IsAny<string>())).Returns(() => new OpResult());

            _disclaimerService = new ApplicationDisclaimerService(
                session.BusinessApiSession,
                _applicantTrackingService.Object,
                _securityManager.Object,
                _applicantTrackingEmail.Object,
                _notificationService.Object,
                _azureResourceProvider.Object,
                null,
                null);
            _provider = new IndeedApplicationProvider(loginServiceMock.Object, session.BusinessApiSession, null,_azureResourceProvider.Object);

            JObject o = JObject.Parse(File.ReadAllText("..\\..\\Dto\\Indeed\\TestIndeedPost.json"));

            _app = o.ToObject<IndeedApplication>();
        }

        [Test]
        public void Test_Application_Mapped_Properly()
        {
            var currentDateTime = DateTime.Now.Date;
            var result = _provider.SaveApplication(_app, currentDateTime, 273);
            var providedApplicant = _app.Applicant;
            var createdApplicant = result.Data.Applicant;

            Assert.IsTrue(result.Success);

            // root applicant data
            Assert.AreEqual(providedApplicant.FullName.Split(' ').First(), createdApplicant.FirstName);
            Assert.AreEqual(providedApplicant.FullName.Split(' ').Last(), createdApplicant.LastName);
            Assert.AreEqual(providedApplicant.Email, createdApplicant.EmailAddress);
            Assert.AreEqual("(555) 555-5555", createdApplicant.PhoneNumber);
            Assert.AreEqual(713, result.Data.PostingId);
            Assert.AreEqual(273, createdApplicant.ClientId);

            // certifications
            var firstProvidedCertificate = providedApplicant.Resume.Json.Certifications.Values.First();
            var firstCreatedLicense = createdApplicant.ApplicantLicenses.First();
            Assert.AreEqual(firstProvidedCertificate.Description, firstCreatedLicense.Description);
            Assert.AreEqual(firstProvidedCertificate.Title, firstCreatedLicense.Type);
            Assert.AreEqual(new DateTime(firstProvidedCertificate.StartDateYear.Value, firstProvidedCertificate.StartDateMonth.Value, 1), firstCreatedLicense.ValidFrom);
            Assert.Null(firstCreatedLicense.ValidTo);

            // education
            var firstProvidedEducation = providedApplicant.Resume.Json.Educations.Values.First();
            var firstCreatedEducation = createdApplicant.ApplicantEducationHistories.First();
            Assert.AreEqual(firstProvidedEducation.Degree, firstCreatedEducation.ExternalDegree);
            Assert.AreEqual(firstProvidedEducation.Field, firstCreatedEducation.Studied);
            Assert.AreEqual(firstProvidedEducation.School, firstCreatedEducation.Description);

            if (firstProvidedEducation.EndCurrent)
            {
                int yearsCompleted = currentDateTime.Year - firstProvidedEducation.StartDateYear.Value;
                yearsCompleted = currentDateTime.Month < 5 ? yearsCompleted - 1 : yearsCompleted;
                Assert.AreEqual(yearsCompleted, firstCreatedEducation.YearsCompleted);
            } else if(firstProvidedEducation.EndDateYear.HasValue)
            {
                Assert.AreEqual(firstProvidedEducation.EndDateYear - firstProvidedEducation.StartDateYear, firstCreatedEducation.YearsCompleted);
            }

            Assert.AreEqual(HasDegreeType.InProgress, firstCreatedEducation.HasDegree);
            Assert.True(firstCreatedEducation.IsEnabled);
            Assert.Null(firstCreatedEducation.ApplicantSchoolTypeId);
            Assert.AreEqual(3, createdApplicant.ApplicantEducationHistories.Count);

            // work history
            var firstProvidedEmploymentHistory = providedApplicant.Resume.Json.Positions.Values.First(x => x.Title == "tester"); // looks like the positions are swapped around when deserializing into c# object
            var firstCreatedEmploymentHistory = createdApplicant.ApplicantEmploymentHistories.First(x => x.Title == "tester");
            Assert.AreEqual(firstProvidedEmploymentHistory.Title, firstCreatedEmploymentHistory.Title);
            Assert.AreEqual(firstProvidedEmploymentHistory.Company, firstCreatedEmploymentHistory.Company);
            Assert.AreEqual(firstProvidedEmploymentHistory.Description, firstCreatedEmploymentHistory.Responsibilities);
            Assert.AreEqual(firstProvidedEmploymentHistory.Location.Split(',').First().Trim(), firstCreatedEmploymentHistory.City);
            Assert.AreEqual(firstProvidedEmploymentHistory.StartDateMonth.Value.ToString("00") + "-" + firstProvidedEmploymentHistory.StartDateYear, firstCreatedEmploymentHistory.StartDate);
            Assert.AreEqual(firstProvidedEmploymentHistory.EndDateMonth.Value.ToString("00") + "-" + firstProvidedEmploymentHistory.EndDateYear, firstCreatedEmploymentHistory.EndDate);
            Assert.AreEqual(_california, firstCreatedEmploymentHistory.StateId);

            // external application identity
            var providedExternalId = _app.Id;
            var createdExternalIdentity = result.Data.ExternalApplicationIdentity;
            Assert.AreEqual(providedExternalId, createdExternalIdentity.ExternalApplicationId);
            Assert.AreEqual(ApplicantJobSiteEnum.Indeed, createdExternalIdentity.ApplicantJobSiteId);
            Assert.AreEqual(result.Data.ApplicationHeaderId, createdExternalIdentity.ApplicantApplicationHeaderId);

            // answers
            var providedAnswers = _app.Questions.Answers.ToList();
            Assert.AreEqual(90, providedAnswers.Count());
            Assert.AreEqual("1", providedAnswers.First().Id);
            Assert.AreEqual("2600 Esperanza Crossing", providedAnswers.First().GetValues().First());
        }
    }
}
