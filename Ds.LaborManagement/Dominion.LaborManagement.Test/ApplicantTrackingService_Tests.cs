using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dominion.Core.Services.Interfaces;
using Dominion.Core.Services.Api;
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
using Dominion.LaborManagement.Dto;
using Dominion.LaborManagement.Dto.ApplicantTracking;
using Dominion.Core.Dto.Labor;
using Dominion.Core.Test.Helpers;
using Dominion.Core.Test.Helpers.TestObjects.Mocks;
using Dominion.Utility.ExtensionMethods;
using Dominion.Utility.OpResult;
using Dominion.Utility.Query.LinqKit;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Dominion.Utility.OpResult;
using Dominion.Domain.Entities.Misc;
using Dominion.Core.Dto.Misc;
using Dominion.Utility.Containers;
using Dominion.Utility.Query;
using Dominion.Utility.Security;
using Dominion.LaborManagement.Service.Internal.Security;
using System.Security.Claims;
using Dominion.Core.Dto.Employee;
using Dominion.Core.Dto.Core;
using Dominion.Core.Services.Emails;


namespace Dominion.LaborManagement.Test
{
    [TestFixture]
    public class ApplicantTrackingService_Tests
    {
        public static Func<int, int, bool> SimpleExtensionTest
        {
            get
            {
                return (u, i) => u == i || u < 100;
            }
        }

        [Test]
        public void Test_Environment()
        {
            /*var opr = new Dominion.Utility.OpResult.OpResult();
            opr.EvaluateSuccess(SimpleExtensionTest(1000, 1000));
            Assert.IsTrue(opr.Success);*/

            var fundamentals = new ProtoFundamentals(ConnStr.ConnectionString);
            var session = new ProtoApiSession(fundamentals, PreConfiguredProtoApiSettings.AmericanHouse_JayJohnson_Sys());

            IOpResult res = session.BusinessApiSession.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin);
            Assert.AreEqual(res.Success, true);

            session.BusinessApiSession.LoggedInUserInformation.AccessibleClientIds = new List<int> { 1 };
            session.BusinessApiSession.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, 1).MergeInto(res);
            Assert.AreEqual(res.Success, true);
        }

        [Test]
        public void Audit_ApplicantCompanyApplication_Insert()
        {
            //ARRANGE
            var fundamentals = new ProtoFundamentals(ConnStr.ConnectionString);
            var session = new ProtoApiSession(fundamentals, PreConfiguredProtoApiSettings.AmericanHouse_JayJohnson_Sys());

            var service = new ApplicantTrackingService(session.BusinessApiSession, null, null, null, null, null, null,
                    null, null, null, null, null, null, null, null, null, null, null) as
                    IApplicantTrackingService;
            session.BusinessApiSession.LoggedInUserInformation.AccessibleClientIds = new List<int> { 1 };

            ApplicantCompanyApplicationDto dto = new ApplicantCompanyApplicationDto()
            {
                CompanyApplicationId = 0,
                Description = "Audition 123",
                ClientId = 17435,
                IsEnabled = true,
                ReferenceNo = 1,
                YearsOfEmployment = 2,
                Education = 0,
                IsCurrentEmpApp = true,
                IsExcludeHistory = false,
                IsExcludeReferences = false,
                IsFlagVolResign = false,
                IsFlagNoEmail = false,
                IsFlagReferenceCheck = false,
                //QuestionSections = new List<ApplicationQuestionSectionWithQuestionDetailDto>()
            };
            
            // 1. Create New
            var result = service.SaveApplicantCompanyApplication(dto);
            dto = result.Data;

            Assert.IsTrue(result.Success);
            Assert.AreNotEqual(0, dto.CompanyApplicationId);
        }

        [Test]
        public void Audit_ApplicantCompanyApplication_Delete()
        {
            //ARRANGE
            var fundamentals = new ProtoFundamentals(ConnStr.ConnectionString);
            var session = new ProtoApiSession(fundamentals, PreConfiguredProtoApiSettings.Time_JoshDeGram_Sys());

            var service = new ApplicantTrackingService(session.BusinessApiSession, null, null, null, null, null, null,
                    null, null, null, null, null, null, null, null, null, null, null) as
                    IApplicantTrackingService;
            session.BusinessApiSession.LoggedInUserInformation.AccessibleClientIds = new List<int> { 1 };

            // 2. Update/Delete
            var delResult = service.DeleteApplicantCompanyApplication(1031);
            Assert.IsTrue(delResult.Success);
        }

        [Test]
        public void Audit_ApplicantPostingCategory_Succeed()
        {
            //ARRANGE
            var fundamentals = new ProtoFundamentals(ConnStr.ConnectionString);
            var session = new ProtoApiSession(fundamentals, PreConfiguredProtoApiSettings.Time_JoshDeGram_Sys());

            var service =
                new ApplicantTrackingService(session.BusinessApiSession, null, null, null, null, null, null, null, null,
                        null, null, null, null, null, null, null, null, null) as
                    IApplicantTrackingService;

            var result = service.UpdateApplicantPostingCategory(new ApplicantPostingCategoryDto()
            {
                PostingCategoryId = 0,
                Name = "Painter",
                ClientId = 1234,
                IsEnabled = true,
                Description = "Testing unit 124"
            });

            Assert.IsTrue(result.Success);
            List<ApplicantPostingCategoryDto> resDate = result.Data.ToList();
            int pId = result.Data.Where(x => x.Name == "Painter").First().PostingCategoryId;

            result = service.UpdateApplicantPostingCategory(new ApplicantPostingCategoryDto()
            {
                PostingCategoryId = pId,
                Name = "Painter leaeder",
                ClientId = 7004,
                IsEnabled = true,
                Description = "Testing unit"
            });

            Assert.IsTrue(result.Success);

            result = service.DeleteApplicantPostingCategory(new ApplicantPostingCategoryDto()
            {
                PostingCategoryId = pId,
                Name = "Painter leaeder",
                ClientId = 7004,
                IsEnabled = true,
                Description = "Testing unit"
            });

            Assert.IsTrue(result.Success);
        }

        [Test]
        public void Audit_ApplicantCompanyCorrespondence_Insert()
        {
            //ARRANGE
            var fundamentals = new ProtoFundamentals(ConnStr.ConnectionString);
            var session = new ProtoApiSession(fundamentals, PreConfiguredProtoApiSettings.AmericanHouse_JayJohnson_Sys());

            var service =
                new ApplicantTrackingService(session.BusinessApiSession, null, null, null, null, null, null, null, null,
                        null, null, null, null, null, null, null, null, null) as
                    IApplicantTrackingService;

            ApplicantCompanyCorrespondenceDto dto = new ApplicantCompanyCorrespondenceDto()
            {
                ApplicantCompanyCorrespondenceId = 0,
                ClientId = 17435,
                Description = "Testing unit 124",
                IsActive = true,
                ApplicantCorrespondenceTypeId = ApplicantCorrespondenceType.ApplicationCompleted,
                Body = "Dear {Applicant} {body} Thanks {signature}"
            };

            var result = service.SaveApplicantCompanyCorrespondence(dto);

            Assert.IsTrue(result.Success);
            ApplicantCompanyCorrespondenceDto resDate = result.Data;
            int pId = resDate.ApplicantCompanyCorrespondenceId;

            dto.Description = "Testing unit 421";
            result = service.SaveApplicantCompanyCorrespondence(dto);
            Assert.IsTrue(result.Success);

            var result2 = service.DeleteApplicantCompanyCorrespondence(pId, isApplicantAdmin:false);
            Assert.IsTrue(result2.Success);
        }

        [Test]
        public void Audit_ApplicantCompanyCorrespondence_Update()
        {
            //ARRANGE
            var fundamentals = new ProtoFundamentals(ConnStr.ConnectionString);
            var session = new ProtoApiSession(fundamentals, PreConfiguredProtoApiSettings.AmericanHouse_JayJohnson_Sys());

            var service =
                new ApplicantTrackingService(session.BusinessApiSession, null, null, null, null, null, null, null, null,
                        null, null, null, null, null, null, null, null, null) as
                    IApplicantTrackingService;

            ApplicantCompanyCorrespondenceDto dto = new ApplicantCompanyCorrespondenceDto()
            {
                ApplicantCompanyCorrespondenceId = 55,
                ClientId = 17435,
                Description = "Testing unit 421",
                IsActive = true,
                ApplicantCorrespondenceTypeId = ApplicantCorrespondenceType.ApplicationCompleted,
                Body = "Dear {Applicant} {body} Thanks {signature}"
            };

            var result = service.SaveApplicantCompanyCorrespondence(dto);

            Assert.IsTrue(result.Success);
            ApplicantCompanyCorrespondenceDto resDate = result.Data;
            int pId = resDate.ApplicantCompanyCorrespondenceId;
            
            // delete
            var result2 = service.DeleteApplicantCompanyCorrespondence(pId, isApplicantAdmin:false);
            Assert.IsTrue(result2.Success);
        }

        [Test]
        public void Audit_ApplicantCompanyCorrespondence_Delete()
        {
            //ARRANGE
            var fundamentals = new ProtoFundamentals(ConnStr.ConnectionString);
            var session = new ProtoApiSession(fundamentals, PreConfiguredProtoApiSettings.Time_JoshDeGram_Sys());

            var service =
                new ApplicantTrackingService(session.BusinessApiSession, null, null, null, null, null, null, null, null,
                        null, null, null, null, null, null, null, null, null) as
                    IApplicantTrackingService;

            // delete
            var result2 = service.DeleteApplicantCompanyCorrespondence(55, isApplicantAdmin:false);
            Assert.IsTrue(result2.Success);
        }


        [Test]
        public void Audit_ApplicantQuestionControls_Insert()
        {
            //ARRANGE
            var fundamentals = new ProtoFundamentals(ConnStr.ConnectionString);
            var session = new ProtoApiSession(fundamentals, PreConfiguredProtoApiSettings.AmericanHouse_JayJohnson_Sys());

            var service =
                new ApplicantTrackingService(session.BusinessApiSession, null, null, null, null, null, null, null, null,
                        null, null, null, null, null, null, null, null, null) as
                    IApplicantTrackingService;

            ApplicantQuestionControlDto dto = new ApplicantQuestionControlDto()
            {
                QuestionId = 0,
                ClientId = 17435,
                Question = "BioData form 124",
                IsEnabled = true,
                FieldTypeId = FieldType.List,
                SectionId = 1102,
                IsFlagged = true,
                IsRequired = true,
                FlaggedResponse = "Test description"
            };

            var result = service.SaveApplicantQuestionControl(dto);
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void Audit_ApplicantQuestionControls_Update()
        {
            //ARRANGE
            var fundamentals = new ProtoFundamentals(ConnStr.ConnectionString);
            var session = new ProtoApiSession(fundamentals, PreConfiguredProtoApiSettings.AmericanHouse_JayJohnson_Sys());

            var service =
                new ApplicantTrackingService(session.BusinessApiSession, null, null, null, null, null, null, null, null,
                        null, null, null, null, null, null, null, null, null) as
                    IApplicantTrackingService;

            ApplicantQuestionControlDto dto = new ApplicantQuestionControlDto()
            {
                QuestionId = 1064,
                ClientId = 17435,
                Question = "BioData form 421",
                IsEnabled = true,
                FieldTypeId = FieldType.List,
                SectionId = 1102,
                IsFlagged = true,
                IsRequired = true,
                FlaggedResponse = "Test description"
            };

            var result = service.SaveApplicantQuestionControl(dto);
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void Audit_ApplicantQuestionControls_Delete()
        {
            //ARRANGE
            var fundamentals = new ProtoFundamentals(ConnStr.ConnectionString);
            var session = new ProtoApiSession(fundamentals, PreConfiguredProtoApiSettings.Time_JoshDeGram_Sys());

            var service =
                new ApplicantTrackingService(session.BusinessApiSession, null, null, null, null, null, null, null, null,
                        null, null, null, null, null, null, null, null, null) as
                    IApplicantTrackingService;

            // delete
            var result2 = service.DeleteApplicantQuestionControl(1064);
            Assert.IsTrue(result2.Success);
        }

        [Test]
        public void Audit_ApplicantQuestionControls_Get()
        {
            //ARRANGE
            var fundamentals = new ProtoFundamentals(ConnStr.ConnectionString);
            var session = new ProtoApiSession(fundamentals, PreConfiguredProtoApiSettings.Time_JoshDeGram_Sys());

            var service =
                new ApplicantTrackingService(session.BusinessApiSession, null, null, null, null, null, null, null, null,
                        null, null, null, null, null, null, null, null, null) as
                    IApplicantTrackingService;

            // Retrieve
            ApplicantQuestionControlDto dto = session.UnitOfWork.ApplicantTrackingRepository.ApplicantQuestionControlQuery()
                    .ByQuestionId(29)
                    .ExecuteQueryAs(x => new ApplicantQuestionControlDto
                    {
                        QuestionId = x.QuestionId,
                        Question = x.Question,
                        FieldTypeId = x.FieldTypeId,
                        SectionId = x.SectionId,
                        ClientId = x.ClientId,
                        IsFlagged = x.IsFlagged,
                        IsRequired = x.IsRequired,
                        FlaggedResponse = x.FlaggedResponse,
                        IsEnabled = x.IsEnabled,
                        ApplicantQuestionDdlItem = x.ApplicantQuestionDdlItem.Select(y => new ApplicantQuestionDdlItemDto { ApplicantQuestionDdlItemId = y.ApplicantQuestionDdlItemId, Description = y.Description, OldDescription = y.Description, QuestionId = y.QuestionId }).ToList()
                    }).First();

            Assert.AreNotSame(29, dto.QuestionId);
        }

        [Test]
        public void Audit_ApplicantClient_Insert()
        {
            //ARRANGE
            var fundamentals = new ProtoFundamentals(ConnStr.ConnectionString);
            var session = new ProtoApiSession(fundamentals, PreConfiguredProtoApiSettings.AmericanHouse_JayJohnson_Sys());

            var service =
                new ApplicantTrackingService(session.BusinessApiSession, null, null, null, null, null, null, null, null,
                        null, null, null, null, null, null, null, null, null) as
                    IApplicantTrackingService;

            ApplicantClientDto dto = new ApplicantClientDto()
            {
                ClientId = 17435,
                CorrespondenceEmailAddress = "test@gmail.com",
            };

            var result = service.SaveApplicantClient(dto);
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void Audit_ApplicantClient_Update()
        {
            //ARRANGE
            var fundamentals = new ProtoFundamentals(ConnStr.ConnectionString);
            var session = new ProtoApiSession(fundamentals, PreConfiguredProtoApiSettings.Time_JoshDeGram_Sys());

            var service =
                new ApplicantTrackingService(session.BusinessApiSession, null, null, null, null, null, null, null, null,
                        null, null, null, null, null, null, null, null, null) as
                    IApplicantTrackingService;

            ApplicantClientDto dto = new ApplicantClientDto()
            {
                ClientId = 17435,
                CorrespondenceEmailAddress = "test2@gmail.com",
            };

            var result = service.SaveApplicantClient(dto);
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void AccountFeatureService_ClientFeature()
        {
            //ARRANGE
            var fundamentals = new ProtoFundamentals(ConnStr.ConnectionString);
            var session = new ProtoApiSession(fundamentals, PreConfiguredProtoApiSettings.Time_JoshDeGram_Sys());

            var service =
                new AccountFeatureService(session.BusinessApiSession, null, null) as
                    IAccountFeatureService;

            var result = service.GetClientAccountFeature(AccountFeatureEnum.OnBoarding, 1389);
            Assert.IsTrue(result.Success);
            //Assert.IsTrue(result.Data.IsEnabled);
        }

        [Test, Explicit]
        public void EmailTemplates_Test()
        {
            var fundamentals = new ProtoFundamentals(ConnStr.ConnectionString);
            var session = new ProtoApiSession(fundamentals, PreConfiguredProtoApiSettings.AmericanHouse_JayJohnson_Sys());

            var clientAzureMock = new Mock<IClientAzureService>();
            clientAzureMock.Setup(x => x.GetAzureClientResource(ResourceSourceType.AzureClientImage, 1390, "logo"))
                .Returns(new OpResult<AzureViewDto>() { Data = new AzureViewDto() { Source = "This is test image" }, Success = true });

            IApplicantTrackingService atService =
                new ApplicantTrackingService(session.BusinessApiSession, null, null, null, null, null, null, null,
                        clientAzureMock.Object, null, null, null, null, null, null, null, null, null) as
                    IApplicantTrackingService;
            var disclaimerService =
                new ApplicationDisclaimerService(session.BusinessApiSession, atService, null, null, null, null, clientAzureMock.Object, null) as
                    IApplicationDisclaimerService;

            Console.WriteLine("Correspondence EmailTemplates Test");
            IOpResult<EmailBodyDto> companyCorrepondenceBody = atService.GetApplicantCompanyCorrespondenceEmailBodyByCorrespondenceId(17, null, isApplicantAdmin:false);
            Console.WriteLine(companyCorrepondenceBody.Data.Body);
            Assert.IsTrue(companyCorrepondenceBody.Success);

            Console.WriteLine("Rejection EmailTemplates Test");
            IOpResult<string> htmlBody = atService.GetApplicantCorrespondenceEmailByApplicationEmailHistoryId(1018);
            Console.WriteLine(htmlBody.Data);
            Assert.IsTrue(htmlBody.Success);

            Console.WriteLine("Disclaimer Test");
            IOpResult<DisclaimerDetailDto> disclaimerBody = disclaimerService.GetDisclaimer(1018,CorrespondenceType.Disclaimer);

            
            Console.WriteLine(disclaimerBody.Data.Body);
            Assert.IsTrue(htmlBody.Success);
        }

        [Test, Explicit]
        public void ApplicationHeader1134TestCase_Test()
        {
            var fundamentals = new ProtoFundamentals(ConnStr.ConnectionString);
            var session = new ProtoApiSession(fundamentals, PreConfiguredProtoApiSettings.AmericanHouse_JayJohnson_Sys());

            var clientAzureMock = new Mock<IClientAzureService>();
            clientAzureMock.Setup(x => x.GetAzureClientResource(ResourceSourceType.AzureClientImage, 26591, "logo"))
                .Returns(new OpResult<AzureViewDto>() { Data = new AzureViewDto() { Source = "This is test image" }, Success = true });

            IApplicantTrackingService atService =
                new ApplicantTrackingService(session.BusinessApiSession, null, null, null, null, null, null, null,
                        clientAzureMock.Object, null, null, null, null, null, null, null, null, null) as
                    IApplicantTrackingService;
            
            Console.WriteLine("Rejection EmailTemplates Test");
            IOpResult<IEnumerable<ApplicantApplicationEmailHistoryDto>> data = atService.GetApplicantCorrespondencesByApplicationHeaderId(1134);
            Console.WriteLine(data.Data.Count());
            Assert.IsTrue(data.Success);
        }

        [Test, Explicit]
        public void ApplicationHeaderResumeTestCase_Test()
        {
            var fundamentals = new ProtoFundamentals(ConnStr.ConnectionString);
            var session = new ProtoApiSession(fundamentals, PreConfiguredProtoApiSettings.AmericanHouse_JayJohnson_Sys());

            IApplicantTrackingService atService =
                new ApplicantTrackingService(session.BusinessApiSession, null, null, null, null, null, null, null, null,
                        null, null, null, null, null, null, null, null, null) as
                    IApplicantTrackingService;

            IOpResult<IEnumerable<ApplicantApplicationHeaderDto>> data = atService.GetApplicantApplicationHeaders(1012);
            Console.WriteLine(data.Data.Count());
            Assert.IsTrue(data.Success);
        }

        /*[Test]
        public void EmployeeEmailAddressCheck()
        {
            int yr = DateTime.Now.Month < 12 ? 
                DateTime.Now.AddYears(-1).Year : DateTime.Now.Year ;

            //Assert.IsTrue(yr == 2017);

            var fundamentals = new ProtoFundamentals(ConnStr.ConnectionString);
            var session = new ProtoApiSession(fundamentals, PreConfiguredProtoApiSettings.AmericanHouse_JayJohnson_Sys());
            
            var _employeeManager =
                new EmployeeManager(session.ClaimsPrincipal, session.BusinessApiSession.UnitOfWork, session.BusinessApiSession,null,null) as
                    IEmployeeManager;

            IOpResult<EmployeeContactInformationViewDto> contactInfo = _employeeManager.GetEmployeeContact<EmployeeContactInformationViewDto>(7022);
            Assert.IsTrue(contactInfo.Data.EmailAddress == "edison@yahoo.com");
        }

        [Test]
        public void EmployeeEmailComputeHashTest()
        {
            EmployeeW2ConsentEmail e = new EmployeeW2ConsentEmail("", "", "", "", 0);
            string inputValue = "Hello world";
            string hasedValue = e.GetHashCode(inputValue);
            bool transValue = e.VerifyMd5Hash(inputValue, hasedValue);

            Assert.IsTrue(transValue, inputValue);
        }*/
    }
}
