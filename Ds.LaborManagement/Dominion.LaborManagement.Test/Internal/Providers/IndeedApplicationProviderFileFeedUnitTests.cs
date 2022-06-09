using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Dominion.Authentication.Intermediate.Util;
using Dominion.Core.Dto.Labor;
using Dominion.Core.Dto.Labor.Enum;
using Dominion.Core.Services.Api.Dto;
using Dominion.Core.Services.Interfaces;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Misc;
using Dominion.Domain.Interfaces.Entities;
using Dominion.LaborManagement.Dto.ApplicantTracking;
using Dominion.LaborManagement.Dto.ApplicantTracking.IndeedApply;
using Dominion.LaborManagement.Dto.Enums;
using Dominion.LaborManagement.EF.Query;
using Dominion.LaborManagement.Service.Internal.Providers;
using Dominion.LaborManagement.Service.Mapping;
using Dominion.Testing.Util.Common;
using Dominion.Testing.Util.Helpers.Prototyping;
using Dominion.Utility.Mapping;
using Dominion.Utility.OpResult;
using Dominion.Utility.Query;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using ApplicantStatusType = Dominion.LaborManagement.Dto.ApplicantTracking.ApplicantStatusType;
using Dominion.Core.Services.Api;
using Dominion.Authentication.Api;
using Dominion.Authentication.Interface.Api.Providers;
using Dominion.Authentication.Api.Internal.Providers;
using Dominion.Core.Services;
using Dominion.Core.Services.Internal.Providers.Resources;
using Dominion.Authentication.Interface.Api;
using Dominion.Core.Dto.Core;
using Dominion.Utility.Security;

namespace Dominion.LaborManagement.Test.Internal.Providers
{
    [TestFixture]
    public class IndeedApplicationProviderFileFeedUnitTests
    {
        private const int UnitedStatesId = 1; // this is the id of the US in justinDev's db
        private const bool MockSessionEnabled = true; 
        private readonly string _baseFolder = "..\\..\\Dto\\Indeed\\";

        protected Mock<IApplicantTrackingProvider> _mockATProvider { get; set; }
        protected Mock<IAzureResourceProvider> _mockAzureProvider { get; set; }
        protected Mock<IBusinessApiSession> _mockSession { get; set; }
        protected IBusinessApiSession _dbSession { get; set; }

        [SetUp]
        public virtual void SetUp()
        {
            var fundamentals = new ProtoFundamentals(ConnStr.ConnectionString);
            var session = new ProtoApiSession(fundamentals, PreConfiguredProtoApiSettings.AmericanHouse_JayJohnson_Sys());
            _dbSession = session.BusinessApiSession;

            _mockSession = new Mock<IBusinessApiSession>();
            _mockSession.Setup(x => x.UnitOfWork);

            _mockATProvider = new Mock<IApplicantTrackingProvider>();
            _mockAzureProvider = new Mock<IAzureResourceProvider>();
        }

        [Test]
        public void Test_Generate_Questions_For_IndeedPost_By_ApplicationId()
        {
            #region "MOCK SETUP"
            OpResult<ICollection<ApplicationQuestionSectionWithQuestionDetailDto>> returnData = new OpResult<ICollection<ApplicationQuestionSectionWithQuestionDetailDto>>()
            {
                Data = GetMockQuestions()
            };
            _mockATProvider.Setup(x => x.GetApplicationSectionsByApplicationId(It.IsAny<int>(), It.IsAny<int?>())).Returns(returnData);
            IApplicantTrackingProvider atProvider = _mockATProvider.Object;
            if (!MockSessionEnabled) atProvider = new ApplicantTrackingProvider(_dbSession, null, null, null, null);
            #endregion

            int applicationId = 9;
            List<IndeedQuestion> list = new List<IndeedQuestion>();
            ICollection<ApplicationQuestionSectionWithQuestionDetailDto> dtos = atProvider.GetApplicationSectionsByApplicationId(applicationId,null).Data;
            foreach(ApplicationQuestionSectionWithQuestionDetailDto dto in dtos)
            {
                list.Add(new IndeedQuestion()
                {
                    Id = "Section_" + dto.SectionId,
                    Text = dto.Description,
                    Type = "information",
                });

                list.AddRange(dto.Questions.Select(IndeedApplicationMaps.IndeedQuestionMap));

                list.Add(new IndeedQuestion()
                {
                    Id = "Pagebreak_" + dto.SectionId,
                    Type = "pagebreak",
                });
            }

            string output = JsonConvert.SerializeObject(list, Formatting.Indented, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            Assert.Greater(dtos.Count, 0);
            File.WriteAllText(string.Format(_baseFolder + "App{0}_{1}.json", applicationId, DateTime.Now.ToString("MM_dd_yyyy_HH_mm")), output);
        }

        [Test]
        public void Test_IndeedPostFile_Deserialize_And_Save()
        {
            #region "MOCK SETUP"
            OpResult<AddApplicantDto> returnData = new OpResult<AddApplicantDto>()
            {
                Data = new AddApplicantDto()
                {
                    FirstName = "Murphy",
                    MiddleInitial = "W",
                    LastName = "Wolf",
                    UserName = "murphy.wolf",
                    Password = "123",
                    Email = "murphy.wolf@gmail.com",
                    DsUserType = 5,
                    ClientId = 1390,
                    DsUserId = 34461,
                    DOB = null,
                    SecretAnswer = "Test",
                    SecretQuestion = 1,
                    Address1 = "77861 Icie Junctions",
                    Address2 = "",
                    City = "Wyandotte",
                    CellPhone = "789-789-8878",
                    WorkPhone = "235-546-4564",
                    Extension = "",
                    ApplicantId = 2065,
                    signInToken = "dfsfsdfsd",
                    PostLink = "sdfsdfsdfd",
                },
                Success = true
            };
            _mockATProvider.Setup(x => x.AddNewUser(It.IsAny<AddApplicantDto>())).Returns(returnData);

            _mockAzureProvider.Setup(x => x.CreateOrUpdateApplicantAzureFile(It.IsAny<int>(), It.IsAny<int>(), 
                    It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(new OpResult<string>() { Data = "This string represents a resource location" });

            _mockAzureProvider.Setup(x => x.GetAzureAccountNum(AzureDirectoryType.ApplicantFile))
                .Returns(0);

            var user = new Mock<IUser>();
            user.Setup(u => u.ClientId).Returns(0);

            _mockSession.Setup(x => x.UnitOfWork.UserRepository.GetUserByUserName(It.IsAny<string>()))
                .Returns(() => null);

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ExternalApplicationIdentityQuery()
                .ByExternalApplicationId(It.IsAny<string>())
                .ExecuteQueryAs<int?>(It.IsAny<Expression<Func<ExternalApplicationIdentity, int?>>>()))
                .Returns(()=>new List<int?>() { });

            _mockSession.Setup(x => x.LoggedInUserInformation).Returns((SessionUserDto)user.Object);

            _mockSession.Setup(x => x.UnitOfWork.MiscRepository.GetCountryList(It.IsAny<QueryBuilderAutoMap<Country, Country>>()))
                .Returns(() => new List<Country>() { new Country() { CountryId = UnitedStatesId } });

            _mockSession.Setup(x => x.UnitOfWork.MiscRepository.GetStateList(It.IsAny<QueryBuilderAutoMap<State, State>>()))
                .Returns(() => new List<State>() { new State() { StateId=1} });

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantsQuery().ByApplicantId(It.IsAny<int>())
                .ExecuteQueryAs(It.IsAny<Expression<Func<Applicant, int>>>()))
                .Returns(() => new List<int>() { 0});

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantsQuery().ByApplicantId(It.IsAny<int>())
                .FirstOrDefault())
                .Returns(new Applicant() {
                    ApplicantId = returnData.Data.ApplicantId, ClientId = returnData.Data.ClientId,
                    FirstName = returnData.Data.FirstName, LastName = returnData.Data.LastName });

            _mockSession.Setup(x => x.UnitOfWork.LocationRepository.StateQuery().ByAbbreviation(It.IsAny<List<string>>())
                .ExecuteQuery())
                .Returns(() => new List<State>() { new State() { StateId = 1, Abbreviation= "MI", Name= "Michigan", Code="MI" } });

            var loginServiceMock = new Mock<ILoginService>();
            loginServiceMock.Setup(x => x.CheckUserName(It.IsAny<string>())).Returns(() => new OpResult());

            ICollection<ApplicationQuestionSectionWithQuestionDetailDto> temp = GetMockQuestions();
            IEnumerable<ApplicantQuestionControl> questions = temp.SelectMany(x => x.Questions).Select(x => new ApplicantQuestionControl()
            {
                QuestionId = x.QuestionId,
                SectionId = x.SectionId,
                IsEnabled = x.IsEnabled,
                IsFlagged = x.IsFlagged,
                FieldTypeId = x.FieldTypeId,
                FlaggedResponse = x.FlaggedResponse,
            });

            _mockSession.Setup(x => x.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                    .ByPostingId(It.IsAny<int>())
                    .ExecuteQueryAs(It.IsAny<Expression<Func<ApplicantPosting, ApplicantQuestionControl>>>())).Returns(questions);

            IApplicantTrackingProvider atProvider = _mockATProvider.Object;
            IIndeedApplicationProvider provider = new IndeedApplicationProvider(
                loginServiceMock.Object,
                MockSessionEnabled ? _mockSession.Object : _dbSession, 
                atProvider, null);
            #endregion


            string jsonPost = File.ReadAllText(_baseFolder + "Applicant_Donald_Watson.json");

            IndeedApplication appl = JsonConvert.DeserializeObject<IndeedApplication>(jsonPost, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            
            IOpResult<ApplicantApplicationHeader> result = provider.CreateUserAndAddApplication(appl, DateTime.Now, returnData.Data.ClientId);
            
            if (result.Success)
            {
                if(MockSessionEnabled)
                    _mockSession.Verify(x => x.UnitOfWork.Commit(), Times.AtLeastOnce);
                else
                    Assert.Greater(result.Data.ApplicationHeaderId, 0);
            }
            else
            {
                string messages = "";
                foreach (Utility.Msg.IMsgSimple x in result.MsgObjects)
                    messages += x.Msg + Environment.NewLine;

                File.WriteAllText(string.Format(_baseFolder + "Error{0}.txt", DateTime.Now.ToString("MM_dd_yyyy_HH_mm")), messages);
                Assert.IsTrue(result.Success);
            }
        }

        private ICollection<ApplicationQuestionSectionWithQuestionDetailDto> GetMockQuestions()
        {
            return new List<ApplicationQuestionSectionWithQuestionDetailDto>()
            {
                new ApplicationQuestionSectionWithQuestionDetailDto()
                {
                    Description="General Information",
                    SectionId=1,
                    Questions = new List<ApplicantQuestionControlWithDetailDto>()
                    {
                        new ApplicantQuestionControlWithDetailDto()
                        {
                            QuestionId=1,
                            IsRequired= false,
                            FieldTypeId= FieldType.Text,
                            Question = "Have you ever worked under another name?  To facilitate reference check, please list:",
                        },
                        new ApplicantQuestionControlWithDetailDto()
                        {
                            QuestionId=2,
                            IsRequired= true,
                            FieldTypeId= FieldType.List,
                            Question = "Are you physically and otherwise able to perform the essential functions of the position(s) with or without an accommodation?",
                            ApplicantQuestionDdlItem = new List<ApplicantQuestionDdlItemDto>()
                            {
                                new ApplicantQuestionDdlItemDto()
                                {
                                    Description = "Yes",
                                    Value="Yes"
                                },
                                new ApplicantQuestionDdlItemDto()
                                {
                                    Description = "No",
                                    Value="No"
                                }
                            }
                        },
                    }
                },
                new ApplicationQuestionSectionWithQuestionDetailDto()
                {
                    Description="Employment Information",
                    SectionId=2,
                    Questions = new List<ApplicantQuestionControlWithDetailDto>()
                    {
                        new ApplicantQuestionControlWithDetailDto()
                        {
                            QuestionId=3,
                            IsRequired= false,
                            FieldTypeId= FieldType.List,
                            Question = "Have you been employed by Summit Pointe or Community Mental Health / Calhoun County?",
                            ApplicantQuestionDdlItem = new List<ApplicantQuestionDdlItemDto>()
                            {
                                new ApplicantQuestionDdlItemDto()
                                {
                                    Description = "Yes",
                                    Value="Yes"
                                },
                                new ApplicantQuestionDdlItemDto()
                                {
                                    Description = "No",
                                    Value="No"
                                }
                            }
                        },
                        new ApplicantQuestionControlWithDetailDto()
                        {
                            QuestionId=4,
                            IsRequired= true,
                            FieldTypeId= FieldType.Boolean,
                            Question = "Have you been convicted of any crime (including a plea aggreement) within the last 7 years?"
                        },
                    }
                }
            };
        }
    }
}
