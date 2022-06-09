using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Dominion.Authentication.Dto;
using Dominion.Authentication.Interface.Api.Providers;
using Dominion.Core.Dto.Labor;
using Dominion.Core.Services.Api.Auth;
using Dominion.Core.Services.Api.DataServicesInjectors;
using Dominion.Core.Services.Interfaces;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.LaborManagement.Dto.ApplicantTracking;
using Dominion.Utility.Configs;
using Dominion.Utility.Constants;
using Dominion.Utility.ExtensionMethods;
using Dominion.Utility.OpResult;
using Dominion.Utility.Query.LinqKit;
using Dominion.Utility.Sts;
using Dominion.Core.Dto.Core;

namespace Dominion.LaborManagement.Service.Internal.Providers
{
    public class ApplicantTrackingProvider : IApplicantTrackingProvider
    {
        private readonly IBusinessApiSession _session;
        private readonly IApplicantTrackingProvider _applicantTrackingProvider;
        private readonly IUserService _userService;
        private readonly ISecurityService _securityService;
        private readonly IAuthenticationProvider _authenticationProvider;
        private readonly IDsDataServicesApplicantService _objApplicantService;

        public ApplicantTrackingProvider(IBusinessApiSession session, IUserService userService,
            ISecurityService securityService, IAuthenticationProvider authenticationProvider, 
            IDsDataServicesApplicantService objApplicantService)
        {
            _session = session;
            _applicantTrackingProvider = this;
            _userService = userService;
            _securityService = securityService;
            _authenticationProvider = authenticationProvider;
            _objApplicantService = objApplicantService;
        }
        IOpResult<ICollection<ApplicationQuestionSectionWithQuestionDetailDto>> IApplicantTrackingProvider.GetApplicationSectionsByApplicationId(int applicationId, int? applicationHeaderId)
        {
            var result =  new OpResult<ICollection<ApplicationQuestionSectionWithQuestionDetailDto>>();
            var detailDto = new Dictionary<int, ApplicantApplicationDetailDto>();

            var applicantCompanyApplication = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantCompanyApplicationQuery()
                .ByApplicationId(applicationId).ExecuteQuery().FirstOrDefault();

            if (applicationHeaderId.HasValue)
            {
                var detail = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantApplicationDetailQuery()
                    .ByApplicationHeaderId(applicationHeaderId.Value).WhereQuestionIdIsNotNull().ExecuteQuery();
                
                var resources = _applicantTrackingProvider.GetApplicantResourceDetails(applicationHeaderId.Value)
                    .Data.ToDictionary(x=>x.ResourceId.ToString(),y=>y.Name);

                detailDto = detail.Select(x => new ApplicantApplicationDetailDto()
                {
                    SectionId = x.SectionId,
                    ApplicationDetailId = x.ApplicationDetailId,
                    ApplicationHeaderId = x.ApplicationHeaderId,
                    QuestionId = x.QuestionId,
                    IsFlagged = x.IsFlagged,
                    Response = x.Response,
                    ResponseFileName = resources.ContainsKey(x.Response) ? resources[x.Response] : "",
                }).ToDictionary(x => x.QuestionId.Value, y => y); //QuestionId will never be null because null values are filtered in the db query.
            }

            var questionSetQuery = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantQuestionSetQuery()
                .ByApplicationId(applicationId);

            var questionSets = _session.UnitOfWork.ApplicantTrackingRepository
                .ApplicantQuestionControlQuery().getApplicationQuestionSets(questionSetQuery).Execute();
            var questionSetsDto = new List<ApplicationQuestionSectionWithQuestionDetailDto>();

            var list = (questionSets as List<ApplicantQuestionControlAndQuestionSets>).OrderBy(x => x.SectionId).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                var itemSection = item.ApplicationQuestionSection;
                var questionSetLast = questionSetsDto.Count - 1;
                if (questionSetsDto.Count == 0 ||
                    (questionSetsDto[questionSetLast].SectionId != item.SectionId)) // Create the section if it is not already in the collection.
                {
                    questionSetsDto.Add(new ApplicationQuestionSectionWithQuestionDetailDto()
                    {
                        SectionId = itemSection.SectionId,
                        Description = itemSection.Description,
                        Instruction = itemSection.ApplicationSectionInstruction.Where(y => y.ClientId == applicantCompanyApplication.ClientId).Select(z => z.Instruction).FirstOrDefault(),
                        DisplayOrder = itemSection.DisplayOrder,
                        ClientId = itemSection.ClientId,
                        IsEnabled = itemSection.IsEnabled,
                        Questions = new List<ApplicantQuestionControlWithDetailDto>()
                    });
                }
                questionSetLast = questionSetsDto.Count - 1;
                var ddlList = new List<ApplicantQuestionDdlItemDto>(); //From here to the end of the loop we are creating the questionDto and dropping it on the current sectionDto.
                item.ApplicantQuestionDdlItem.ForEach(x =>
                {
                    ddlList.Add(new ApplicantQuestionDdlItemDto()
                    {
                        ApplicantQuestionDdlItemId = x.ApplicantQuestionDdlItemId,
                        Description = x.Description,
                        FlaggedResponse = x.FlaggedResponse,
                        IsDefault = x.IsDefault,
                        IsEnabled = x.IsEnabled,
                        QuestionId = x.QuestionId,
                        Value = x.Value
                    });
                });

                var createdQuestionControl = new ApplicantQuestionControlWithDetailDto()
                {
                    ClientId = item.ClientId,
                    FieldTypeId = item.FieldTypeId,
                    FlaggedResponse = item.FlaggedResponse,
                    IsEnabled = item.IsEnabled,
                    IsFlagged = item.IsFlagged,
                    IsRequired = item.IsRequired,
                    Question = item.Question,
                    QuestionId = item.QuestionId,
                    SectionId = item.SectionId,
                    ApplicantQuestionDdlItem = ddlList,
                    OrderId = item.OrderId,
                    SelectionCount = item.SelectionCount,
                };

                if (applicationHeaderId.HasValue)
                {
                    createdQuestionControl.detail = detailDto.ContainsKey(item.QuestionId)
                        ? detailDto[item.QuestionId]
                        : new ApplicantApplicationDetailDto()
                        {
                            ApplicationHeaderId = applicationHeaderId.Value,
                            SectionId = item.SectionId,
                            QuestionId = item.QuestionId,
                            Response = ""
                        };
                }

                questionSetsDto[questionSetLast].Questions.Add(createdQuestionControl);
            }

            questionSetsDto.Sort((x, y) =>
            {
                if (x.DisplayOrder > y.DisplayOrder)
                {
                    return 1;
                }
                else if (x.DisplayOrder < y.DisplayOrder)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            });

            foreach (var section in questionSetsDto)
            {
                var questionList = section.Questions.ToList();
                questionList.Sort((x, y) =>
                {
                    if (x.OrderId > y.OrderId)
                    {
                        return 1;
                    }
                    else if (x.OrderId < y.OrderId)
                    {
                        return -1;
                    }
                    else
                    {
                        return 0;
                    }
                });

                section.Questions = questionList;
            }

            result.Data = questionSetsDto;

            return result;
        }

        IOpResult<IEnumerable<ResourceDto>> IApplicantTrackingProvider.GetApplicantResourceDetails(int applicationHeaderId)
        {
            var result = new OpResult<IEnumerable<ResourceDto>>();

            IEnumerable<int> resources = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantApplicationDetailQuery()
                    .ByApplicationHeaderId(applicationHeaderId)
                    .ByQuestionFieldType(FieldType.File)
                    .WhereQuestionIdIsNotNull()
                    .ExecuteQueryAs(x=> !string.IsNullOrEmpty(x.Response) ? x.Response : "0" ).Select(y => int.Parse(y)).ToList();

            result.Data = _session.UnitOfWork.CoreRepository.QueryResources().ByResources(resources).ExecuteQueryAs(r => new ResourceDto
            {
                ResourceId = r.ResourceId,
                Name = r.Name,
                Source = r.Source,
                SourceTypeId = r.SourceType,
                EmployeeId = null,
                AddedDate = r.AddedDate,
                AddedBy = r.AddedById,
            });

            return result;
        }

        /// <summary>
        /// Starts a new Application for an applicant
        /// </summary>
        /// <param name="postingId">The post the applicant wants to apply</param>
        /// <param name="applicantId">The id of the applicant</param>
        /// <returns>url to redirect to based on resume requirement</returns>
        IOpResult<string> IApplicantTrackingProvider.StartApplication(int postingId, int applicantId)
        {
            return _applicantTrackingProvider.StartApplication(0, postingId, applicantId);
        }

        /// <summary>
        /// Starts an Application for an applicant
        /// </summary>
        /// <param name="applicationHeaderId">Id of an existing applicantion; use 0 for a new application</param>
        /// <param name="postingId">The post the applicant wants to apply</param>
        /// <param name="applicantId">The id of the applicant</param>
        /// <returns></returns>
        IOpResult<string> IApplicantTrackingProvider.StartApplication(int applicationHeaderId,
            int postingId, int applicantId)
        {
            var result = new OpResult<string>();

            result.TryCatch(() =>
            {
                //If no application Header Id is given; Create New Application
                if (applicationHeaderId == 0)
                {
                    var addResults = _applicantTrackingProvider.AddApplicantApplicationHeader(postingId, applicantId);
                    if (addResults.HasNoErrorAndHasData)
                    {
                        applicationHeaderId = addResults.Data;
                    }
                }

                //We attempt to get Application Data using the postingId and applicantId
                var getResults = _applicantTrackingProvider.GetApplicantApplicationHeader(postingId, applicantId);
                if (getResults.HasNoErrorAndHasData)
                {
                    var applicationHeaderDto = getResults.Data;
                    if (applicationHeaderId <= 0)
                    {
                        //set the application Header Id from the results
                        applicationHeaderId = applicationHeaderDto.ApplicationHeaderId;
                    }

                    if (applicationHeaderId > 0 && !applicationHeaderDto.IsApplicationCompleted)
                    {
                        var resumeResults = _applicantTrackingProvider.GetPostingResumeOption(applicationHeaderId);
                        var applicationId = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                            .ByPostingId(postingId).ExecuteQueryAs(x => x.ApplicationId).FirstOrDefault();
                        if (resumeResults.HasNoErrorAndHasData && applicationId > 0)
                        {
                            result.Data = $"ApplicantJobBoard.aspx?Application={applicationId}&Section=1&Head={applicationHeaderId}";
                        }
                    }
                }
            });

            return result;
        }

        /// <summary>
        /// Creates a new Application for an Application
        /// </summary>
        /// <param name="postingId">The post the applicant wants to apply</param>
        /// <param name="applicantId">The id of the applicant</param>
        /// <returns>ApplicationHeaderId</returns>
        IOpResult<int> IApplicantTrackingProvider.AddApplicantApplicationHeader(int postingId, int applicantId)
        {
            var opResult = new OpResult<int>();
            var applicantApplicationHeader = new ApplicantApplicationHeader()
            {
                ApplicantId = applicantId,
                PostingId = postingId,
                DateSubmitted = DateTime.Now,
                OrigPostingId = postingId
            };

            _session.UnitOfWork.RegisterNew(applicantApplicationHeader);
            _session.UnitOfWork.Commit().MergeInto(opResult);

            opResult.SetDataOnSuccess(applicantApplicationHeader.ApplicationHeaderId);
            return opResult;
        }

        /// <summary>
        /// Retrieves an existing Application
        /// </summary>
        /// <param name="postingId">The post the applicant wants to apply</param>
        /// <param name="applicantId">The id of the applicant</param>
        /// <returns>returns ApplicationHeaderDto instance</returns>
        IOpResult<ApplicantApplicationHeaderDto> IApplicantTrackingProvider.GetApplicantApplicationHeader(int postingId, int applicantId)
        {
            var opResult = new OpResult<ApplicantApplicationHeaderDto>();
           
            ApplicantApplicationHeaderDto header = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantApplicationHeaderQuery()
                .ByApplicantId(applicantId)
                .ByPostingId(postingId)
                .ExecuteQueryAs(x => new ApplicantApplicationHeaderDto
                {
                    Applicant = new ApplicantDto { FirstName = x.Applicant.FirstName, LastName = x.Applicant.LastName},
                    ApplicationHeaderId = x.ApplicationHeaderId,
                    ApplicantId = x.ApplicantId,
                    PostingId = x.PostingId,
                    PostingDescription = x.ApplicantPosting.Description,
                    IsApplicationCompleted = x.IsApplicationCompleted,
                    IsRecommendInterview = x.IsRecommendInterview,
                    DateSubmitted = x.DateSubmitted,
                    ApplicantResumeId = x.ApplicantResumeId,
                    ApplicantRejectionReasonId = x.ApplicantRejectionReasonId,
                    OrigPostingId = x.OrigPostingId,
                    ApplicantStatusTypeId = x.ApplicantStatusTypeId,
                    AddedByAdmin = x.AddedByAdmin,					
                    CoverLetter = x.CoverLetter,
                    CoverLetterId = x.CoverLetterId
                }).FirstOrDefault();

            if (header != null)
            {
                IEnumerable<ApplicantApplicationDetailDto> detailList = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantApplicationDetailQuery()
                .ByApplicationHeaderId(header.ApplicationHeaderId)
                .ByFieldType(FieldType.File)
                .ExecuteQueryAs(x => new ApplicantApplicationDetailDto
                {
                    ApplicationHeaderId = x.ApplicationHeaderId,
                    ApplicationDetailId = x.ApplicationDetailId,
                    Response = x.Response,
                    QuestionId = x.QuestionId,
                    IsFlagged = x.IsFlagged,
                    SectionId = x.SectionId,
                    QuestionName = x.ApplicantQuestionControl.Question,
                    ResponseTitle = x.ApplicantQuestionControl.ResponseTitle,
                }).OrderBy(y => y.ApplicationDetailId);

                header.Resources = detailList;
            }

            opResult.TrySetData(() => header);

            return opResult;
        }

        /// <summary>
        /// Retrieves the Resume option for a specific Application 
        /// </summary>
        /// <param name="applicantApplicationHeaderId">specific Id of application for a applicant applying for a post</param>
        /// <returns></returns>
        IOpResult<int> IApplicantTrackingProvider.GetPostingResumeOption(int applicantApplicationHeaderId)
        {
            var opResult = new OpResult<int>();

            int applicantResumeRequiredID = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByApplicantApplicationHeaderId(applicantApplicationHeaderId)
                .ExecuteQueryAs(x => x.ApplicantResumeRequiredId)
                .FirstOrDefault();
            opResult.Data = applicantResumeRequiredID;

            return opResult;
        }

        IOpResult<AddApplicantDto> IApplicantTrackingProvider.AddNewUser(AddApplicantDto dto)
        {
            var result = new OpResult<AddApplicantDto>();
            var applicantResult = new OpResult<int>();

            result.TryCatch(() =>
            {
                #region Create Auth User
                var insertUpdateNewUserDto = new InsertUpdateUserDto(
                    _session.LoggedInUserInformation.AuthUserId > 0 ? _session.LoggedInUserInformation.AuthUserId : CommonConstants.SYS_TEMP_AUTH_USER_ID, 
                    _session.LoggedInUserInformation.UserId)
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    UserName = dto.UserName,
                    EncryptedPassword = PassInator.ToDb(dto.Password, dto.UserName),
                    Password1 = dto.Password,
                    Password2 = dto.Password,
                    Email = dto.Email ?? string.Empty,
                    DsUserType = 5,
                    ClientId = dto.ClientId,
                    ViewEmployees = 4,
                    ViewRates = 4,
                    SecuritySettings = true,
                    AllowUnSafePass = true,
                    IsStrukOut = false,
                    ViewOnly = false,
                    EmployeeSelfServiceOnly = false,
                    ReportingOnly = false,
                    BlockPayrollAccess = true,
                    Timeclock = false,
                    BlockHR = true,
                    EmployeeOnly = false,
                    ApplicantAdmin = false,
                    FromDate = null,
                    ToDate = null,
                    Timeout = null,
                    ViewTaxPackets = false,
                    MustChangePWD = false
                };
                var authuserResult = _userService.AddNewUser(insertUpdateNewUserDto);
                #endregion

                #region Create Applicant
                if (authuserResult.Success)
                {
                    dto.DsUserId = authuserResult.Data.DsUserId;
                    dto.DOB = string.Empty;
                    dto.Email = dto.Email ?? string.Empty;
                    dto.SecretAnswer = string.Empty;
                    dto.SecretQuestion = 1;
                    dto.Address2 = dto.Address2 ?? string.Empty;
                    dto.Extension = dto.Extension ?? string.Empty;
                    dto.CellPhone = dto.CellPhone ?? string.Empty;
                    dto.WorkPhone = dto.WorkPhone ?? string.Empty;
                    dto.Extension = dto.Extension ?? string.Empty;
                    dto.IsTextEnabled = dto.IsTextEnabled ?? false;


                    dto.FirstName = dto.FirstName.Substring(0, 1).ToUpper() + dto.FirstName.Substring(1);
                    dto.MiddleInitial = (dto.MiddleInitial == null) ? string.Empty : dto.MiddleInitial.ToUpper();
                    dto.LastName = dto.LastName.Substring(0, 1).ToUpper() + dto.LastName.Substring(1);

                    _objApplicantService.newApplicantSignUp(dto).MergeAll(applicantResult).MergeInto(result);
                }
                #endregion

                #region Start Application and Generate Auth Sign-In Token
                if (applicantResult.HasNoErrorAndHasData && dto.PostingId.HasValue && dto.PostingId.Value > 0 && applicantResult.Data > 0)
                {
                    dto.ApplicantId = applicantResult.Data;
                    if (HttpContext.Current != null)
                    {
                        //Start Application to get a new application and partial return Url
                        var applicationResult = _applicantTrackingProvider.StartApplication(dto.PostingId.Value, applicantResult.Data);
                        if (applicationResult.HasNoErrorAndHasData)
                        {
                            var authArgs = new AuthenticationArgs
                            {
                                ip = HttpContext.Current.GetUserIPAddress(),
                                username = dto.UserName,
                                password = dto.Password
                            };

                            //Authenticate to verify username and password, and get the AuthUserId
                            var authenticationResult = _authenticationProvider.Authenticate(authArgs);
                            if (authenticationResult.IsAuthenticated)
                            {
                                //Get just the Schema and Host (i.e. https://live.dominionsystems.com)
                                var link = string.IsNullOrWhiteSpace(dto.PostLink) ? "" : dto.PostLink.Substring(0, dto.PostLink.LastIndexOf('/'));

                                //Add the PathAndQuery from the new Application Results
                                link += $"/{applicationResult.Data}";

                                //Build the WS-Federated Message using the return Url created
                                var linkResults = _authenticationProvider.BuildStsSignInUrl(link).MergeInto(result);
                                if (linkResults.HasNoErrorAndHasData)
                                {
                                    //used PathAndQuery to remove the Schema and Host components of URI
                                    //This is needed in order for Auth to recognize /AUTH/issue as a local URI
                                    link = new Uri(linkResults.Data).PathAndQuery;

                                    var returnLink = new Uri(linkResults.Data);
                                    var siteConfigResults = _authenticationProvider.GetSiteConfiguration(ConfigValues.SiteConfigurationID);
                                    if (siteConfigResults.HasNoErrorAndHasData)
                                    {
                                        dto.PostLink = $"{siteConfigResults.Data.AuthUrl}/account/tokensignin?authToken=";
                                    }
                                }
                                else
                                {
                                    result.AddExceptionMessage(new Exception("Could not generate STS data."));
                                }

                                //Generate Sign-In Token
                                var genTokenResult = _authenticationProvider.GenerateApplicantAuthenticationToken(
                                    authArgs.ip,
                                    authenticationResult.AuthUserId,
                                    link,
                                    false
                                ).MergeInto(result);

                                if (genTokenResult.HasData && !genTokenResult.MsgObjects.Any())
                                {
                                    //return the signInToken to the client-side
                                    dto.signInToken = genTokenResult.Data.AuthToken;
                                    dto.PostLink += dto.signInToken;

                                    //We have successfully built a return Url and generated a sign-in Token!
                                    result.SetToSuccess();
                                }
                                else
                                {
                                    result.AddExceptionMessage(new Exception("could not generate sign-in token."));
                                }
                            }
                            else
                            {
                                result.AddExceptionMessage(new Exception("could not Authenticate User."));
                            }
                        }
                        else
                        {
                            result.AddExceptionMessage(new Exception("could not start Application."));
                        }
                    }
                }
                else
                {
                    result.AddExceptionMessage(new Exception("could not Add new Applicant."));
                }
                #endregion

                #region Sign Out Anonymously
                //clear session, cookies, and client-side cache and Refuse Redirect
                if(_session.LoggedInUserInformation.IsAnonymous && HttpContext.Current != null)
                StsSecurity.LogoutApplicantNonUser(allowRedirect: false);
                #endregion
            });

            if (result.HasError)
            {
                if (!result.MsgObjects.Any())
                    result.AddExceptionMessage(new Exception("could not Add New User"));
            }

            result.SetDataOnSuccess(dto);
            return result;
        }
    }
}
