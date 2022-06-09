using Dominion.Core.Dto.Labor;
using Dominion.Core.Dto.User;
using Dominion.Core.Services.Interfaces;
using Dominion.Core.Services.Security.Authorization;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Domain.Entities.Clients;
using Dominion.LaborManagement.Dto;
using Dominion.LaborManagement.Dto.ApplicantTracking;
using Dominion.LaborManagement.Service.Internal.Security;
using Dominion.Utility.OpResult;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Dominion.Utility.ExtensionMethods;
using Dominion.Utility.Query;
using Dominion.Domain.Interfaces.Query;
using Dominion.Core.Services.Api;
using Dominion.Core.Services.Emails;
using Dominion.Utility.Msg.Specific;
using System.Configuration;
using System.Diagnostics;
using Dominion.Core.Dto.Client;
using Dominion.LaborManagement.Service.Api;
using System.Text;
using System.IO;
using Dominion.Utility.Web;
using Dominion.Core.Dto;
using Dominion.Core.Dto.Core;
using Dominion.Utility.Query.LinqKit;
using Dominion.Utility.Security;
using Dominion.Domain.Entities.Employee;
using Dominion.Authentication.Dto;
using Dominion.Authentication.Interface.Api;
using Dominion.Core.Dto.Misc;
using Dominion.Domain.Entities.Misc;
using Dominion.Core.Dto.Payroll;
using Dominion.Domain.Entities.Onboarding;
using Dominion.LaborManagement.Service.Mapping;
using Dominion.Utility.Containers;
using System.Text.RegularExpressions;
using System.Web;
using Dominion.Authentication.Api.Internal.Msg;
using Dominion.Core.Services.Internal.Providers.Resources;
using Dominion.Authentication.Interface.Api.Providers;
using Dominion.Core.Services.Api.DataServicesInjectors;
using Dominion.Core.Services.Internal.Providers.Resources;
using Dominion.LaborManagement.Dto.ApplicantTracking.Application;
using Dominion.LaborManagement.Dto.Notification;
using Dominion.Utility.Sts;
using Dominion.Utility.Configs;
using Dominion.LaborManagement.Service.Api.Notification;
using Dominion.LaborManagement.Service.Internal.Providers;
using ApplicantStatusType = Dominion.LaborManagement.Dto.ApplicantTracking.ApplicantStatusType;
using Dominion.Core.Services.Security.Onboarding;
using Dominion.Core.Dto.Onboarding;
using Dominion.Core.Dto.Client;
using Dominion.Core.Services.Mapping;
using Dominion.LaborManagement.Service.Api.Notification;
using Dominion.Core.Dto.Employee;
using Dominion.Domain.Entities.Benefit;
using Dominion.Core.Services.Internal.Providers;
using Dominion.Core.Services.Pdfs;
using Dominion.Core.Dto.Core;
using Dominion.Domain.Entities.Benefit;
using Dominion.Core.Services.Api.Auth;
using Dominion.Utility.Constants;
using Dominion.Core.Services.Pdfs;
using Dominion.Core.Services.Internal.Providers;
using Dominion.Utility.Msg;
using System.Net.Http;
using System.Threading.Tasks;
using Dominion.Utility.Configs;
using Dominion.Domain.Entities.Core;
using Dominion.Taxes.Dto.Taxes;
using Dominion.Utility.Pdf.DocxToPdf;
using Dominion.LaborManagement.Dto.Enums;
using Dominion.LaborManagement.Dto.EmployeeLaborManagement;
using Dominion.Utility.Io;
using Dominion.Authentication.Intermediate.Util;
using GenericMsg = Dominion.Utility.Msg.Specific.GenericMsg;

namespace Dominion.LaborManagement.Service.Api
{
	public class ApplicantTrackingService : IApplicantTrackingService
	{
		#region Variables and Properties

		private readonly IBusinessApiSession _session;

		private readonly ISecurityManager _securityManager;
		private readonly ILoginService _loginService;
		private readonly IAuthenticationProvider _authenticationProvider;
		private readonly ISecurityService _securityService;
		private readonly IApplicantTrackingNotificationService _applicantTrackingNotificationService;
		private readonly IApplicantTrackingProvider _appTrackProvider;
		private readonly IDsDataServicesApplicantService _objApplicantService;
        private readonly IAzureResourceProvider _azureResourceProvider;
		private readonly IClientAzureService _clientAzureService;
        private readonly IContactProvider _contactProvider;
		private readonly ISupervisorProvider _supervisorProvider;
		private readonly IUserService _userService;
		private readonly IClientService _clientService;
		private readonly IRemarkProvider _remarkProvider;
        private readonly IEmployeeOnboardingFormProvider _formProvider;
        private readonly IOnboardingService _onboardingService;
		private readonly IEmployeeService _employeeService;

        #endregion

        #region Constructors and Initializers

        public ApplicantTrackingService(
			IBusinessApiSession session,
			ISecurityManager securityManager,
			ILoginService loginService,
			IAuthenticationProvider authenticationProvider,
			ISecurityService securityService,
			IApplicantTrackingNotificationService applicantTrackingNotificationService,
			IApplicantTrackingProvider appTrackProvider,
			IDsDataServicesApplicantService objApplicantService,
			IClientAzureService clientAzureService,
            IAzureResourceProvider azureResourceProvider,
            IContactProvider contactProvider,				
            ISupervisorProvider supervisorProvider,
			IUserService userService,
            IClientService clientService,
			IRemarkProvider remarkProvider,
            IEmployeeOnboardingFormProvider formProvider,
            IOnboardingService onboardingService,
			IEmployeeService employeeService

        )
		{
			Self = this;
			_session = session;
			_securityManager = securityManager;
			_loginService = loginService;
			_authenticationProvider = authenticationProvider;
			_securityService = securityService;
			_applicantTrackingNotificationService = applicantTrackingNotificationService;
			_objApplicantService = objApplicantService;
			_appTrackProvider = appTrackProvider;
			_clientAzureService = clientAzureService;
            _azureResourceProvider = azureResourceProvider;
			_contactProvider = contactProvider;	
			_supervisorProvider = supervisorProvider;
			_userService = userService;
			_clientService = clientService;
			_remarkProvider = remarkProvider;
            _formProvider = formProvider;
            _onboardingService = onboardingService;
			_employeeService = employeeService;

        }

		internal IApplicantTrackingService Self { set; get; }

		#endregion

		#region Methods and Commands 

		IOpResult<IEnumerable<ApplicantPostingDto>> IApplicantTrackingService.GetApplicantPostingListByClientId(int clientId)
		{
			var opResult = new OpResult<IEnumerable<ApplicantPostingDto>>();

			_session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			_session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository
											.ApplicantPostingsQuery()
											.ByClientId(clientId)
											.ByIsActive(true)
											.ByIsClosed(false)
				.ExecuteQueryAs(x => new ApplicantPostingDto
				{
					PostingId = x.PostingId,
					PostingNumberWithDescription = x.PostingNumber + " - " + x.Description,
					Description = x.Description,
					ApplicationId = x.ApplicationId,
					PostingTypeId = x.PostingTypeId,
					PostingNumber = x.PostingNumber,
					JobRequirements = x.JobRequirements,
					Location = (x.ClientDivision != null) ?
							   (x.ClientDivision.City ?? "") + ", " +
							   (x.ClientDivision.State != null ? x.ClientDivision.State.Abbreviation ?? "" : "") : "",
					Salary = x.Salary,
					HoursPerWeek = x.HoursPerWeek,
					JobTypeId = (int)x.JobTypeId,
					PostingCategoryId = x.PostingCategoryId,
					JobProfileId = x.JobProfileId,
					StaffHired = x.StaffHired,
					RejectionCorrespondence = x.RejectionCorrespondence,
					ApplicationCompletedCorrespondence = x.ApplicationCompletedCorrespondence,
					ApplicantResumeRequiredId = x.ApplicantResumeRequiredId,
					MinSchooling = x.MinSchooling,
					ApplicantOnBoardingProcessId = x.ApplicantOnBoardingProcessId,
					IsPublished = x.IsPublished,
					PublishedDate = x.PublishedDate,
					IsEnabled = x.IsEnabled,
					IsForceMinSchoolingMatch = x.IsForceMinSchoolingMatch,
					IsClosed = x.IsClosed,
					ClientId = x.ClientId,
					ClientDivisionId = x.ClientDivisionId,
					ClientDepartmentId = x.ClientDepartmentId,
					StartDate = x.StartDate,
					FilledDate = x.FilledDate,
					PublishStart = x.PublishStart,
					PublishEnd = x.PublishEnd,
					ModifiedBy = _session.LoggedInUserInformation.UserId,
					Modified = DateTime.Now,
                    //PostingOwnerId = x.PostingOwnerId,
                    PostingOwners = x.ApplicantPostingOwners.Select(z=> new ApplicantPostingOwnerDto
                        {
                            PostingId = z.PostingId,
                            UserId= z.UserId
                        }).ToList(),
					OwnerNotifications = x.OwnerNotifications
				}).ToList());

			return opResult;
		}

		IOpResult<ApplicantPostingDetailDto> IApplicantTrackingService.GetApplicantPosting(int postingId)
		{
			var opResult = new OpResult<ApplicantPostingDetailDto>();

			var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
			var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantTrackingEnabled || isSystemAdmin || accessRights.UserId == 0)
			{
				opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
					.ByPostingId(postingId)
					.ExecuteQueryAs(x => new ApplicantPostingDetailDto
					{
						PostingId = x.PostingId,
						Description = x.Description,
						PostingTypeId = x.PostingTypeId,
						PostingNumber = x.PostingNumber,
						ApplicationId = x.ApplicationId,
						ClientDivisionId = x.ClientDivisionId,
						ClientDepartmentId = x.ClientDepartmentId,
						PostingCategoryId = x.PostingCategoryId,
						JobTypeId = (int)x.JobTypeId,
						JobRequirements = x.JobRequirements,
						Location = (x.ClientDivision != null) ?
								   (x.ClientDivision.City ?? "") + ", " +
								   (x.ClientDivision.State != null ? x.ClientDivision.State.Abbreviation ?? "" : "") : "",
						Salary = x.Salary,
						HoursPerWeek = x.HoursPerWeek,
						StartDate = x.StartDate,
						FilledDate = x.FilledDate,
						ClientId = x.ClientId,
						IsEnabled = x.IsEnabled,
						PublishedDate = x.PublishedDate,
						JobProfileId = x.JobProfileId,
						PublishStart = x.PublishStart,
						PublishEnd = x.PublishEnd,
						StaffHired = x.StaffHired,
						RejectionCorrespondence = x.RejectionCorrespondence,
						ApplicationCompletedCorrespondence = x.ApplicationCompletedCorrespondence,
                        ApplicationReceivedTextCorrespondence=x.ReceivedTextCorrespondence,
						ApplicantResumeRequiredId = x.ApplicantResumeRequiredId,
						MinSchooling = x.MinSchooling,
						ApplicantOnBoardingProcessId = x.ApplicantOnBoardingProcessId,
						IsPublished = x.IsPublished,
						ModifiedBy = x.ModifiedBy,
						Modified = x.Modified,
						DisabledDate = x.DisabledDate,
						IsForceMinSchoolingMatch = x.IsForceMinSchoolingMatch,
						IsClosed = x.IsClosed,
						//JobType = x.ApplicantJobType.Description,
						JobType = x.EmployeeStatus.Description,
						Department = x.ClientDepartment.Name,
						Division = x.ClientDivision.Name,
						JobProfile = x.JobProfile.Description,
						ResumeType = x.ApplicantResumeRequired.Description,
						Category = x.ApplicantPostingCategory.Name,
						Application = x.ApplicantCompanyApplication.Description,
                        //PostingOwnerId = x.PostingOwnerId,
                        PostingOwners = x.ApplicantPostingOwners.Select(z => new ApplicantPostingOwnerDto
                        {
                            PostingId = z.PostingId,
                            UserId = z.UserId,
                            Name = z.User.FirstName + " " + z.User.LastName
                        }).ToList(),
						PostingTypeDescription = ((PostingType)x.PostingTypeId).ToString(), //Enum.GetName(typeof(PostingType), x.PostingTypeId),
                        //PostingOwnerName = x.PostingOwner.FirstName + " " + x.PostingOwner.LastName,
						OwnerNotifications = x.OwnerNotifications,
						IsGeneralApplication = x.IsGeneralApplication,
						CompanyName = x.Client.ClientName,
                        NumOfPositions = (x.NumOfPositions.HasValue && x.NumOfPositions.Value > 0) ? x.NumOfPositions.Value : (x.IsGeneralApplication ? (int?)null : 1),
						IsCoverLetterRequired=x.IsCoverLetterRequired
					}).FirstOrDefault());

				if (opResult.HasData)
				{
					_session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, opResult.Data.ClientId).MergeInto(opResult);
					if (opResult.HasError)
						return opResult;

					if (!opResult.Data.MinSchooling.HasValue)
					{
						opResult.Data.SchoolDescription = "No Requirement";
					}
					else
					{
						opResult.TryCatch(() =>
						{
							opResult.Data.SchoolDescription = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantSchoolTypeQuery().BySchoolTypeId(opResult.Data.MinSchooling.Value).ExecuteQueryAs(x => x.Description).FirstOrDefault();
						});
					}

					if (opResult.Data.RejectionCorrespondence == -1)
					{
						opResult.Data.RejectionCorrespondenceDescription = "No Email";
					}
					else
					{
						opResult.TryCatch(() =>
						{
							opResult.Data.RejectionCorrespondenceDescription = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantCompanyCorrespondenceQuery().ByCorrespondenceTypeId((Dto.ApplicantTracking.ApplicantCorrespondenceType)opResult.Data.RejectionCorrespondence).ByIsActive(true).ExecuteQueryAs(x => x.Description).FirstOrDefault();
						});
					}

					if (opResult.Data.ApplicationCompletedCorrespondence == -1)
					{
						opResult.Data.ApplicationCorrespondenceDescription = "No Email";
					}
					else
					{
						opResult.TryCatch(() =>
						{
							opResult.Data.ApplicationCorrespondenceDescription = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantCompanyCorrespondenceQuery().ByCorrespondenceTypeId((Dto.ApplicantTracking.ApplicantCorrespondenceType)opResult.Data.ApplicationCompletedCorrespondence).ByIsActive(true).ExecuteQueryAs(x => x.Description).FirstOrDefault();
						});
					}
				}
			}

			return opResult;
		}

		IOpResult<ApplicantPostingDetailDto> IApplicantTrackingService.UpdateApplicantPosting(ApplicantPostingDetailDto dto)
		{
			var opResult = new OpResult<ApplicantPostingDetailDto>();
			var isApplicantAdmin = _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).Success;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantAdmin || isSystemAdmin)
			{
				var applicantPosting = new ApplicantPosting();
				int postingId = dto.PostingId;
				if (postingId == 0)
				{
					dto.IsPublished = true;// CURRENTLY ALLPOSTINGS ARE PUBLISHED REMOVE THIS LINE WHEN WE CHANGE THAT

					applicantPosting = new ApplicantPosting()
					{
						Description = dto.Description,
						ApplicationId = dto.ApplicationId,
						PostingTypeId = dto.PostingTypeId,
						PostingNumber = dto.PostingNumber,
						JobRequirements = dto.JobRequirements,
						Salary = dto.Salary,
						HoursPerWeek = dto.HoursPerWeek,
						JobTypeId = (EmployeeStatusType)dto.JobTypeId,
						PostingCategoryId = dto.PostingCategoryId,
						JobProfileId = dto.JobProfileId,
						StaffHired = dto.StaffHired,
						RejectionCorrespondence = dto.RejectionCorrespondence,
						ApplicationCompletedCorrespondence = dto.ApplicationCompletedCorrespondence,
						ApplicantResumeRequiredId = dto.ApplicantResumeRequiredId,
						MinSchooling = dto.MinSchooling,
						ApplicantOnBoardingProcessId = dto.ApplicantOnBoardingProcessId,
						IsPublished = dto.IsPublished,
						IsEnabled = dto.IsEnabled,
						IsForceMinSchoolingMatch = dto.IsForceMinSchoolingMatch,
						IsClosed = dto.IsClosed,
						ClientId = dto.ClientId,
						ClientDivisionId = dto.ClientDivisionId,
						ClientDepartmentId = dto.ClientDepartmentId,
						StartDate = dto.StartDate,
						FilledDate = dto.FilledDate,
						PublishStart = dto.PublishStart,
						PublishEnd = dto.PublishEnd,
						PublishedDate = DateTime.Now,
						ModifiedBy = _session.LoggedInUserInformation.UserId,
						Modified = DateTime.Now,
						DisabledDate = dto.DisabledDate,
                        //PostingOwnerId = dto.PostingOwnerId,
						OwnerNotifications = dto.OwnerNotifications,
						NumOfPositions = dto.NumOfPositions,
                        IsGeneralApplication = dto.IsGeneralApplication,
                        IsCoverLetterRequired=dto.IsCoverLetterRequired,
                        ReceivedTextCorrespondence=dto.ApplicationReceivedTextCorrespondence
					};
					_session.UnitOfWork.RegisterNew(applicantPosting);
				}
				else
				{
					applicantPosting = _session.UnitOfWork.ApplicantTrackingRepository
												.ApplicantPostingsQuery()
												.ByPostingId(dto.PostingId)
												.FirstOrDefault();

					applicantPosting.PostingId = dto.PostingId;
					applicantPosting.Description = dto.Description;
					applicantPosting.ApplicationId = dto.ApplicationId;
					applicantPosting.PostingTypeId = dto.PostingTypeId;
					applicantPosting.PostingNumber = dto.PostingNumber;
					applicantPosting.JobRequirements = dto.JobRequirements;
					applicantPosting.Salary = dto.Salary;
					applicantPosting.HoursPerWeek = dto.HoursPerWeek;
					applicantPosting.JobTypeId = (EmployeeStatusType)dto.JobTypeId;
					applicantPosting.PostingCategoryId = dto.PostingCategoryId;
					applicantPosting.JobProfileId = dto.JobProfileId;
					applicantPosting.StaffHired = dto.StaffHired;
					applicantPosting.RejectionCorrespondence = dto.RejectionCorrespondence;
					applicantPosting.ApplicationCompletedCorrespondence = dto.ApplicationCompletedCorrespondence;
					applicantPosting.ApplicantResumeRequiredId = dto.ApplicantResumeRequiredId;
					applicantPosting.MinSchooling = dto.MinSchooling;
					applicantPosting.ApplicantOnBoardingProcessId = dto.ApplicantOnBoardingProcessId;
					applicantPosting.IsPublished = dto.IsPublished;
					applicantPosting.IsEnabled = dto.IsEnabled;
					applicantPosting.IsForceMinSchoolingMatch = dto.IsForceMinSchoolingMatch;
					applicantPosting.IsClosed = dto.IsClosed;
					applicantPosting.ClientId = dto.ClientId;
					applicantPosting.ClientDivisionId = dto.ClientDivisionId;
					applicantPosting.ClientDepartmentId = dto.ClientDepartmentId;
					applicantPosting.StartDate = dto.StartDate;
					applicantPosting.FilledDate = dto.FilledDate;
					applicantPosting.PublishStart = dto.PublishStart;
					applicantPosting.PublishEnd = dto.PublishEnd;
					applicantPosting.ModifiedBy = _session.LoggedInUserInformation.UserId;
					applicantPosting.Modified = DateTime.Now;
                    //applicantPosting.PostingOwnerId = dto.PostingOwnerId;
					applicantPosting.OwnerNotifications = dto.OwnerNotifications;
					applicantPosting.NumOfPositions = dto.NumOfPositions;
					applicantPosting.IsGeneralApplication = dto.IsGeneralApplication;
                    applicantPosting.IsCoverLetterRequired = dto.IsCoverLetterRequired;
                    applicantPosting.ReceivedTextCorrespondence = dto.ApplicationReceivedTextCorrespondence;
					_session.UnitOfWork.RegisterModified(applicantPosting);
				}
				_session.UnitOfWork.Commit().MergeInto(opResult);

				if (dto.PostingId == 0)
				{
					dto.PostingId = applicantPosting.PostingId;
					dto.PublishedDate = applicantPosting.PublishedDate;
				}
                else
                {
                    //Delete entries in ApplicantPostingOwner table if it is an existing Post
                    var applicantPostingOwners = _session.UnitOfWork.ApplicantTrackingRepository
                                                .ApplicantPostingOwnerQuery()
                                                .ByPostingId(dto.PostingId)
                                                .ExecuteQuery()
                                                .ToList();

                    foreach (var applicantPostingOwner in applicantPostingOwners)
                    {
                        _session.UnitOfWork.RegisterDeleted(applicantPostingOwner);
                    }
                }

				dto.ModifiedBy = applicantPosting.ModifiedBy;
				dto.Modified = applicantPosting.Modified;

                //Make entries to ApplicantPostingOwner table
                foreach (var postingOwner in dto.PostingOwners)
                {
                    var owner = new ApplicantPostingOwner();
                    owner.PostingId = dto.PostingId;
                    owner.UserId = postingOwner.UserId;
                    _session.UnitOfWork.RegisterNew(owner);

                    //Update the PostingId as PostingId for new Postings will be 0
                    postingOwner.PostingId = dto.PostingId;
                }
                _session.UnitOfWork.Commit().MergeInto(opResult);

				opResult.Data = dto;
			}

			return opResult;
		}

		IOpResult<ApplicantPostingDto> IApplicantTrackingService.DeleteApplicantPosting(ApplicantPostingDto dto)
		{
			var opResult = new OpResult<ApplicantPostingDto>();
			var isApplicantAdmin = _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).Success;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if ((isApplicantAdmin || isSystemAdmin) && dto.PostingId > 0)
			{
				var applicantPosting = new ApplicantPosting();
				applicantPosting = _session.UnitOfWork.ApplicantTrackingRepository
											.ApplicantPostingsQuery()
											.ByPostingId(dto.PostingId)
											.FirstOrDefault();

				applicantPosting.IsEnabled = false;
				applicantPosting.ModifiedBy = _session.LoggedInUserInformation.UserId;
				applicantPosting.Modified = DateTime.Now;
				applicantPosting.DisabledDate = DateTime.Now;

				_session.UnitOfWork.RegisterModified(applicantPosting);
				_session.UnitOfWork.Commit().MergeInto(opResult);

				dto.IsEnabled = applicantPosting.IsEnabled;
				dto.ModifiedBy = applicantPosting.ModifiedBy;
				dto.Modified = applicantPosting.Modified;
				dto.DisabledDate = applicantPosting.DisabledDate;

				opResult.Data = dto;
			}

			return opResult;

		}

		IOpResult<ApplicantPostingDto> IApplicantTrackingService.DeleteApplicantPosting(int postingId, int modifiedBy)
		{
			var opResult = new OpResult<ApplicantPostingDto>();
			var isApplicantAdmin = _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).Success;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantAdmin || isSystemAdmin)
			{
				var postingToDelete = _session.UnitOfWork.ApplicantTrackingRepository
											.ApplicantPostingsQuery()
											.ByPostingId(postingId)
											.FirstOrDefault();

				postingToDelete.IsEnabled = false;
				postingToDelete.ModifiedBy = modifiedBy;
				postingToDelete.Modified = DateTime.Now;
				postingToDelete.DisabledDate = DateTime.Now;
				_session.UnitOfWork.RegisterModified(postingToDelete);

			}
			return opResult;
		}

		IOpResult<ApplicantPostingDto> IApplicantTrackingService.UnDeleteApplicantPosting(ApplicantPostingDto dto)
		{
			var opResult = new OpResult<ApplicantPostingDto>();
			var isApplicantAdmin = _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).Success;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if ((isApplicantAdmin || isSystemAdmin) && dto.PostingId > 0)
			{
				var applicantPosting = new ApplicantPosting();
				int postingId = dto.PostingId;
				applicantPosting = _session.UnitOfWork.ApplicantTrackingRepository
											.ApplicantPostingsQuery()
											.ByPostingId(dto.PostingId)
											.FirstOrDefault();

				applicantPosting.IsEnabled = true;
				applicantPosting.ModifiedBy = _session.LoggedInUserInformation.UserId;
				applicantPosting.Modified = DateTime.Now;
				applicantPosting.DisabledDate = null;

				_session.UnitOfWork.RegisterModified(applicantPosting);
				_session.UnitOfWork.Commit().MergeInto(opResult);

				dto.IsEnabled = applicantPosting.IsEnabled;
				dto.ModifiedBy = applicantPosting.ModifiedBy;
				dto.Modified = applicantPosting.Modified;
				dto.DisabledDate = applicantPosting.DisabledDate;

				opResult.Data = dto;
			}

			return opResult;
		}

		#region application-controls

		IOpResult<IEnumerable<ApplicantCompanyApplicationDto>> IApplicantTrackingService.GetApplicantCompanyApplications(int? clientId)
		{
			var opResult = new OpResult<IEnumerable<ApplicantCompanyApplicationDto>>();
			if (!clientId.HasValue)
				clientId = _session.LoggedInUserInformation.ClientId;

			_session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			_session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId.Value).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantCompanyApplicationQuery()
				.ByClientId(clientId.Value)
				.ByIsActive(true)
				.ExecuteQueryAs(ApplicantTrackingMaps.FromApplicationControl.ToApplicationControlDto.Instance).ToList());

			return opResult;
		}

		IOpResult<IEnumerable<ApplicationQuestionSectionDto>> IApplicantTrackingService.GetApplicationSectionsWithQuestions(int? clientId)
		{
			var opResult = new OpResult<IEnumerable<ApplicationQuestionSectionDto>>();
			if (!clientId.HasValue)
				clientId = _session.LoggedInUserInformation.ClientId;

			_session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			_session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId.Value).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository
					 .ApplicationQuestionSectionQuery()
					 .ByClientIdWithDefaultClient(clientId.Value)
					 .ByIsActive(true)
					 .OrderByDisplayOrder()
					 .ExecuteQueryAs(x => new ApplicationQuestionSectionDto
					 {
						 SectionId = x.SectionId,
						 Description = x.Description,
						 DisplayOrder = x.DisplayOrder,
                         Instruction = x.ApplicationSectionInstruction.Where(y=> y.ClientId == clientId.Value).Select(z => z.Instruction).FirstOrDefault(),
                         Questions = x.ApplicantQuestionControl
									  .Where(y => y.ClientId == clientId.Value && y.IsEnabled == true)
									  .Select(y => new ApplicantQuestionControlWithDetailDto
									  {
										  QuestionId = y.QuestionId,
										  Question = y.Question,
                                          ResponseTitle = y.ResponseTitle,
                                          IsFlagged = y.IsFlagged,
									  })
									  .ToList()

					 }).ToList());

			return opResult;
		}

		IOpResult<ApplicantCompanyApplicationDto> IApplicantTrackingService.GetApplicantCompanyApplication(int applicationId)
		{
			var opResult = new OpResult<ApplicantCompanyApplicationDto>();

			if (_session.CanPerformAction(ApplicantTrackingActionType.WriteApplicantInfo).Success ||
                _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).Success)
            {

                opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantCompanyApplicationQuery()
                    .ByApplicationId(applicationId)
                    .ByIsActive(true)
                    .ExecuteQueryAs(ApplicantTrackingMaps.FromApplicationControl.ToApplicationControlDto.Instance).FirstOrDefault());
                return opResult;
            }
            else
            {
                return opResult.SetToFail(() => new GenericMsg("No permissions to get the application."));
            }
		}

		IOpResult<ApplicantCompanyApplicationDto> IApplicantTrackingService.GetApplicationSettings(int applicationId)
		{
			var opResult = new OpResult<ApplicantCompanyApplicationDto>();
			_session.CanPerformAction(ApplicantTrackingActionType.WriteApplicantInfo).MergeInto(opResult);

			if (opResult.HasError)
				return opResult;

			var application = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantCompanyApplicationQuery()
				.ByApplicationId(applicationId).ExecuteQueryAs(ApplicantTrackingMaps.FromApplicationControl.ToApplicationControlDto.Instance).FirstOrDefault();
			opResult.Data = new ApplicantCompanyApplicationDto()
			{
				IsExcludeHistory = application.IsExcludeHistory,
				IsExcludeReferences = application.IsExcludeReferences,
				ReferenceNo = application.ReferenceNo
			};

			return opResult;
		}

		IOpResult<ApplicantCompanyApplicationDto> IApplicantTrackingService.SaveApplicantCompanyApplication(ApplicantCompanyApplicationDto dto)
		{
			var opResult = new OpResult<ApplicantCompanyApplicationDto>();
			_session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);

			if (opResult.HasError)
				return opResult;

			_session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, dto.ClientId).MergeInto(opResult);

			if (opResult.HasError)
				return opResult;

			if (dto.IsNewEntity(x => x.CompanyApplicationId))
			{
				var newACA = new ApplicantCompanyApplication
				{
					Description = dto.Description,
					ClientId = dto.ClientId,
					ReferenceNo = dto.ReferenceNo,
					Education = dto.Education,
					IsCurrentEmpApp = dto.IsCurrentEmpApp,
					IsExcludeHistory = dto.IsExcludeHistory,
					IsExcludeReferences = dto.IsExcludeReferences,
					IsFlagNoEmail = dto.IsFlagNoEmail,
					IsFlagReferenceCheck = dto.IsFlagReferenceCheck,
					YearsOfEmployment = dto.YearsOfEmployment,
					IsEnabled = true,
					IsFlagVolResign = dto.IsFlagVolResign,
                    IsExperience=dto.IsExperience
				};
				_session.SetModifiedProperties(newACA);
				_session.UnitOfWork.RegisterNew(newACA);
				_session.UnitOfWork.RegisterPostCommitAction(() => dto.CompanyApplicationId = newACA.CompanyApplicationId);
			}
			else
			{
				var editACA = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantCompanyApplicationQuery()
					.ByApplicationId(dto.CompanyApplicationId).FirstOrDefault();

				editACA.Description = dto.Description;
				editACA.ClientId = dto.ClientId;
				editACA.ReferenceNo = dto.ReferenceNo;
				editACA.Education = dto.Education;
				editACA.IsCurrentEmpApp = dto.IsCurrentEmpApp;
				editACA.IsExcludeHistory = dto.IsExcludeHistory;
				editACA.IsExcludeReferences = dto.IsExcludeReferences;
				editACA.IsFlagVolResign = dto.IsFlagVolResign;
				editACA.IsFlagNoEmail = dto.IsFlagNoEmail;
				editACA.IsFlagReferenceCheck = dto.IsFlagReferenceCheck;
				editACA.YearsOfEmployment = dto.YearsOfEmployment;
                editACA.IsExperience = dto.IsExperience;
                _session.SetModifiedProperties(editACA);
				_session.UnitOfWork.RegisterModified(editACA);

                var deletedQuestions = _session.UnitOfWork
                                               .ApplicantTrackingRepository
                                               .ApplicantQuestionSetQuery()
                                               .ByApplicationId(dto.CompanyApplicationId)
                                               .ByExcludeQuestionIds(dto.ApplicantQuestionSets.Select(x => x.QuestionId).ToList())
                                               .ExecuteQuery()
                                               .ToList();

				foreach (var question in deletedQuestions)
				{
					_session.UnitOfWork.RegisterDeleted(question);
				}
			}

            foreach (var questionSection in dto.QuestionSections)
            {
                var sectionInstruction = _session.UnitOfWork
                    .ApplicantTrackingRepository
                    .ApplicationSectionInstructionQuery()
                    .ByClientIdAndSectionId(dto.ClientId, questionSection.SectionId)
                    .FirstOrDefault();

                if (sectionInstruction != null)
                {
                    sectionInstruction.Instruction = questionSection.Instruction;
                    _session.UnitOfWork.RegisterModified(sectionInstruction);
                }
                else
                {
                    if (questionSection.Instruction != null && questionSection.Instruction.Trim() != "")
                    {
                        var newQuestionSection = new ApplicationSectionInstruction()
                        {
                            ClientId = dto.ClientId,
                            SectionId = questionSection.SectionId,
                            Instruction = questionSection.Instruction
                        };
                        _session.UnitOfWork.RegisterNew(newQuestionSection);
                    }
                }
            }

            foreach (var question in dto.ApplicantQuestionSets)
			{ 
				var questionSet = _session.UnitOfWork
					.ApplicantTrackingRepository
					.ApplicantQuestionSetQuery()
					.ByApplicationId(dto.CompanyApplicationId)
					.ByQuestionId(question.QuestionId)
					.FirstOrDefault();
				if (questionSet != null)
				{
					questionSet.OrderId = question.OrderId;
					_session.UnitOfWork.RegisterModified(questionSet);
				}
				else
				{
					var newQS = new ApplicantQuestionSet()
					{
						ApplicationId = dto.CompanyApplicationId,
						QuestionId = question.QuestionId,
						OrderId = question.OrderId
					};
					_session.UnitOfWork.RegisterNew(newQS);
				}
			}

			_session.UnitOfWork.Commit().MergeInto(opResult);

			opResult.SetDataOnSuccess(dto);
			return opResult;
		}

		IOpResult IApplicantTrackingService.DeleteApplicantCompanyApplication(int companyApplicationId)
		{
			var opResult = new OpResult();

			//use action types to determine if user can administrate application info
			_session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			//only bring back the info we need from the DB to "delete" the application 
			var deleteACA = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantCompanyApplicationQuery()
				.ByApplicationId(companyApplicationId)
				.ExecuteQueryAs(x => new
				{
					x.CompanyApplicationId,
					x.ClientId,
					x.IsEnabled
				})
				.FirstOrDefault();

			opResult.CheckForNotFound(deleteACA);
			if (opResult.HasError)
				return opResult;

			//make sure the user has access to this client's application info
			_session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, deleteACA.ClientId).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			//check if the application isn't already disabled
			if (deleteACA.IsEnabled)
			{
				//spin up an actual entity and disable it
				var entity = new ApplicantCompanyApplication
				{
					CompanyApplicationId = deleteACA.CompanyApplicationId,
					IsEnabled = false
				};
				//register only the IsEnable property as being changed
				_session.SetModifiedProperties(entity);
				_session.UnitOfWork.RegisterModified(entity, new PropertyList<ApplicantCompanyApplication>().Include(x => x.IsEnabled));
				_session.UnitOfWork.Commit().MergeInto(opResult);
			}

			return opResult;
		}

		#endregion

		IOpResult<ApplicantCompanyApplicationDto> IApplicantTrackingService.GetApplicantCompanyApplication(int clientId, bool isCurrentEmpApp)
		{
			var opResult = new OpResult<ApplicantCompanyApplicationDto>();
			var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
			var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantTrackingEnabled || isSystemAdmin)
			{
				opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantCompanyApplicationQuery()
					.ByClientId(clientId)
					.ByIsCurrentEmpApp(isCurrentEmpApp)
					.ExecuteQueryAs(x => new ApplicantCompanyApplicationDto
					{
						CompanyApplicationId = x.CompanyApplicationId,
						Description = x.Description,
						ClientId = x.ClientId,
						IsEnabled = x.IsEnabled
					}).FirstOrDefault());
			}

			return opResult;
		}

		IOpResult<IEnumerable<ApplicantJobTypeDto>> IApplicantTrackingService.GetApplicantJobTypes()
		{
			var opResult = new OpResult<IEnumerable<ApplicantJobTypeDto>>();

			_session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantJobTypeQuery()
					.OrderByDescription()
					.ExecuteQueryAs(x => new ApplicantJobTypeDto
					{
						ApplicantJobTypeId = x.ApplicantJobTypeId,
						Description = x.Description
					}).ToList());


			return opResult;
		}

		IOpResult<IEnumerable<ApplicantResumeRequiredDto>> IApplicantTrackingService.GetApplicantResumeRequiredItems()
		{
			var opResult = new OpResult<IEnumerable<ApplicantResumeRequiredDto>>();
			var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
			var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantTrackingEnabled || isSystemAdmin)
			{
				opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantResumeRequiredQuery()
					.OrderByDescription()
					.ExecuteQueryAs(x => new ApplicantResumeRequiredDto
					{
						ApplicantResumeRequiredId = x.ApplicantResumeRequiredId,
						Description = x.Description
					}).ToList());
			}

			return opResult;
		}

		IOpResult<IEnumerable<ApplicantSchoolTypeDto>> IApplicantTrackingService.GetApplicantSchoolTypes()
		{
			var opResult = new OpResult<IEnumerable<ApplicantSchoolTypeDto>>();
			var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
			var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantTrackingEnabled || isSystemAdmin)
			{
				opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantSchoolTypeQuery()
					.OrderByDescription()
					.ExecuteQueryAs(x => new ApplicantSchoolTypeDto
					{
						ApplicantSchoolTypeId = x.ApplicantSchoolTypeId,
						Description = x.Description,
						ApplicationOrder = x.ApplicationOrder
					}).ToList());
			}

			return opResult;
		}

        //This is just the list of eligible Users for being assigned as Posting Owners. Should rename the method. Confusing at times.
		IOpResult<IEnumerable<PostingOwnerDto>> IApplicantTrackingService.GetPostingOwners(int clientId)
		{
			var opResult = new OpResult<IEnumerable<PostingOwnerDto>>();
			var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
			var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantTrackingEnabled || isSystemAdmin)
			{
                opResult.TrySetData(() =>
                {
                    var postingOwners = _session.UnitOfWork.UserRepository.QueryUsers()
                        .ByUserClientOrEmployeeClientId(clientId)
                        .ByApplicantAdmin()
                        .ExecuteQueryAs(x => new PostingOwnerDto
                        {
                            PostingOwnerId = x.UserId,
                            UserId = x.UserId,
                            EmployeeId = x.EmployeeId,
                            FirstName = x.FirstName,
                            LastName = x.LastName,
                            FullName = x.FirstName + " " + x.LastName,
                            IsEnabled = !x.IsUserDisabled
                        }).ToList();

                    _contactProvider.LoadProfileImageForContacts(clientId, postingOwners).MergeInto(opResult);
                    return postingOwners;
                });
			}

			return opResult;
		}

		IOpResult<IEnumerable<ApplicantCorrespondenceTypeDto>> IApplicantTrackingService.GetApplicantCorrespondenceTypes()
		{
			var opResult = new OpResult<IEnumerable<ApplicantCorrespondenceTypeDto>>();

			_session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			return opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantCorrespondenceTypeQuery()
				.OrderByDescription()
				.ExecuteQueryAs(x => new ApplicantCorrespondenceTypeDto
				{
					ApplicantCorrespondenceTypeId = x.ApplicantCorrespondenceTypeId,
					Description = x.Description
				}).ToList());
		}

        IOpResult<IEnumerable<ApplicantCompanyCorrespondenceDto>> IApplicantTrackingService.GetApplicantCompanyCorrespondences(int clientId, Dto.ApplicantTracking.ApplicantCorrespondenceType? correspondenceTypeId, bool? isText, bool? isApplicantAdmin)
        {
            var opResult = new OpResult<IEnumerable<ApplicantCompanyCorrespondenceDto>>();
            IApplicantTrackingService _self = this.Self;

            //if (isApplicantAdmin==true)
            //{
            //    _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
            //    if (opResult.HasError)
            //        return opResult;
            //}
            //else
            //{
            //    _session.CanPerformAction(OnboardingActionType.OnboardingAdministrator).MergeInto(opResult);
            //    if (opResult.HasError)
            //        return opResult;
            //}

            var appResult = _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin);
            var onbResult = _session.CanPerformAction(OnboardingActionType.OnboardingAdministrator);
            if (appResult.HasError && onbResult.HasError)
            {
                if (appResult.HasError)
                {
                    _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
                }
                else
                {
                    _session.CanPerformAction(OnboardingActionType.OnboardingAdministrator).MergeInto(opResult);
                }
            }

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

            IApplicantCompanyCorrespondenceQuery qry = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantCompanyCorrespondenceQuery()
                .ByClientId(clientId)
                .ByIsActive(true);

            if (isText.HasValue) qry = qry.ByIsText(isText.Value);

            qry.ByCorrespondenceTypeId(correspondenceTypeId);

            return opResult.TrySetData(() => {
                List<ApplicantCompanyCorrespondenceDto> dtos = qry.ExecuteQueryAs(x => new ApplicantCompanyCorrespondenceDto
                    {
                        ApplicantCompanyCorrespondenceId = x.ApplicantCompanyCorrespondenceId,
                        ApplicantCorrespondenceTypeId = x.ApplicantCorrespondenceTypeId,
                        ApplicantCorrespondenceType = x.ApplicantCorrespondenceType.Description,
                        Body = x.Body,
                        Subject = x.Subject,
                        Description = x.Description,
                        ClientId = x.ClientId,
                        IsActive = x.IsActive,
                        IsText = x.IsText.HasValue ? x.IsText.Value : false,
                    }).ToList();

                if (correspondenceTypeId.HasValue)
                {
                    return dtos;
                }
                else
                {
                    if (isText != true)
                    {
                        // check if there is any onboarding template already present. If not create one
                        if (dtos.Where(x => x.ApplicantCorrespondenceTypeId == ApplicantCorrespondenceType.OnboardingInvitation).Count() == 0)
                        {
                            string msg = "{*Applicant} :";
                            msg += "<br/><br/>Welcome to the team! Please access the Onboarding link to assist us in obtaining required personnel information. Your username and temporary password are provided below.  Upon logging in to Onboarding, you will be prompted to reset your password.";
                            msg += "<br/><br/>Thank you in advance for completing this process.";
                            msg += "<br/><br/>{*OnboardingUrl}";
                            msg += "<br/><br/>Username: {*UserName}";
                            msg += "<br/>Password: {*Password}";
                            msg += "<br/><br/>Sincerely,";
                            msg += "<br/>{*CompanyName}";

                            ApplicantCompanyCorrespondenceDto dtoNew = new ApplicantCompanyCorrespondenceDto();
                            dtoNew.ApplicantCompanyCorrespondenceId = CommonConstants.NEW_ENTITY_ID;
                            dtoNew.ClientId = clientId;
                            dtoNew.ApplicantCorrespondenceTypeId = ApplicantCorrespondenceType.OnboardingInvitation;
                            dtoNew.ApplicantCorrespondenceType = "Onboarding Invitation";
                            dtoNew.IsActive = true;
                            dtoNew.Description = "Dominion Invitation";
                            dtoNew.Subject = "Onboarding Invitation";
                            dtoNew.IsActive = true;
                            dtoNew.Body = msg;
                            dtoNew.IsText = false;

                            dtos.Add(_self.SaveApplicantCompanyCorrespondence(dtoNew).Data);
                        }
                    }

                    // Remove disclaimers when correspondence type is not passed
                    dtos = dtos.Where(x => x.ApplicantCorrespondenceTypeId != ApplicantCorrespondenceType.ApplicationDisclaimer).ToList();
                    return dtos;
                }
            });
		}

		IOpResult<ApplicantCompanyCorrespondenceDto> IApplicantTrackingService.SaveApplicantCompanyCorrespondence(ApplicantCompanyCorrespondenceDto dto)
		{
			var opResult = new OpResult<ApplicantCompanyCorrespondenceDto>();

            var appResult = _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin);
            var onbResult = _session.CanPerformAction(OnboardingActionType.OnboardingAdministrator);
            if (appResult.HasError && onbResult.HasError)
            {
                if (appResult.HasError)
                {
                    _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
                }
                else
                {
                    _session.CanPerformAction(OnboardingActionType.OnboardingAdministrator).MergeInto(opResult);
                }
            }
            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, dto.ClientId).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			string mailBody = dto.Body;
			
			var entity = new ApplicantCompanyCorrespondence
			{
				ClientId = dto.ClientId,
				Description = dto.Description,
                Subject = dto.Subject,
				ApplicantCorrespondenceTypeId = dto.ApplicantCorrespondenceTypeId.Value,
				Body = mailBody,
				IsActive = dto.IsActive,
                IsText = dto.IsText,
			};

			if (dto.IsNewEntity(x => x.ApplicantCompanyCorrespondenceId))
			{
				entity.IsActive = true;
				_session.SetModifiedProperties(entity);
				_session.UnitOfWork.RegisterNew(entity);
				_session.UnitOfWork.RegisterPostCommitAction(() => dto.ApplicantCompanyCorrespondenceId = entity.ApplicantCompanyCorrespondenceId);
			}
			else
			{
				//make sure ID is set on the entity before registering
				entity.ApplicantCompanyCorrespondenceId = dto.ApplicantCompanyCorrespondenceId;

				var existing = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantCompanyCorrespondenceQuery()
					.ByCorrespondenceId(dto.ApplicantCompanyCorrespondenceId)
					.ExecuteQueryAs(x => new ApplicantCompanyCorrespondenceDto
					{
						ApplicantCompanyCorrespondenceId = x.ApplicantCompanyCorrespondenceId,
						Description = x.Description,
                        Subject = x.Subject,
						ClientId = x.ClientId,
						ApplicantCorrespondenceTypeId = x.ApplicantCorrespondenceTypeId,
						ApplicantCorrespondenceType = x.ApplicantCorrespondenceTypeId.ToString(),
						Body = x.Body,
						IsActive = x.IsActive,
                        IsText = x.IsText.HasValue ? x.IsText.Value : false,
                        Modified = x.Modified,
					}).FirstOrDefault();

				//make sure we found an existing one
				opResult.CheckForNotFound(existing);
				if (opResult.HasError)
					return opResult;

				//one last security check just in case the client id was changed via the API
				if (existing.ClientId != dto.ClientId)
				{
					//make sure user has access to the "real" client
					_session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, existing.ClientId).MergeInto(opResult);
					if (opResult.HasError)
						return opResult;

					//update back to original clientid
					dto.ClientId = existing.ClientId;
					entity.ClientId = existing.ClientId;
				}

                if (dto.ApplicantCorrespondenceTypeId == ApplicantCorrespondenceType.ApplicationDisclaimer && (!existing.Modified.HasValue ||
                    (existing.Modified.HasValue && existing.Modified.Value < DateTime.Now.Date)) && existing.Body.Trim() != dto.Body.Trim() &&
					dto.IsActive )
                {
                    // Since the disclaimer is being modified, archive old one
                    Self.DeleteApplicantCompanyCorrespondence(dto.ApplicantCompanyCorrespondenceId, true).MergeInto(opResult);
                    if (opResult.Success)
                    {
                        // Add the new disclaimer
                        dto.ApplicantCompanyCorrespondenceId = CommonConstants.NEW_ENTITY_ID;
                        return Self.SaveApplicantCompanyCorrespondence(dto);
                    }
                }

				//now check for changes and add any to the properties to update
				var props = new PropertyList<ApplicantCompanyCorrespondence>();
                if (dto.IsActive)
                {
                    if (entity.Description != existing.Description)
					    props.Include(x => x.Description);
                    if (entity.Subject != existing.Subject)
                        props.Include(x => x.Subject);
                    if (entity.IsActive != existing.IsActive)
					    props.Include(x => x.IsActive);
                    if (entity.IsText != existing.IsText)
                        props.Include(x => x.IsText);
                    if (entity.Body != existing.Body)
					    props.Include(x => x.Body);
				    if ((Dto.ApplicantTracking.ApplicantCorrespondenceType)entity.ApplicantCorrespondenceTypeId != existing.ApplicantCorrespondenceTypeId)
					    props.Include(x => x.ApplicantCorrespondenceTypeId);
                }
                else
                {
                    props.Include(x => x.IsActive);
                }

				//if there are any properties that have changed then we can 
				//register the entity as modified
				if (props.Any())
				{
					//make sure "Modified" properties are registered
					props
						.Include(x => x.Modified)
						.Include(x => x.ModifiedBy);

					_session.SetModifiedProperties(entity);
					_session.UnitOfWork.RegisterModified(entity, props);
				}
			}

			//now we can commit
			_session.UnitOfWork.Commit().MergeInto(opResult);

			//if all good return the dto on the result
			opResult.SetDataOnSuccess(dto);

			return opResult;
		}

		IOpResult IApplicantTrackingService.DeleteApplicantCompanyCorrespondence(int correspondenceId, bool isApplicantAdmin)
		{
			var opResult = new OpResult();
            if (isApplicantAdmin)
            {
                _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
            }
            else
            {
                _session.CanPerformAction(OnboardingActionType.OnboardingAdministrator).MergeInto(opResult);
            }
			if (opResult.HasError)
				return opResult;

			var deleteCorrespondence = _session.UnitOfWork.ApplicantTrackingRepository
				.ApplicantCompanyCorrespondenceQuery()
				.ByCorrespondenceId(correspondenceId)
				.ExecuteQueryAs(x => new
				{
					x.ApplicantCompanyCorrespondenceId,
					x.IsActive,
					x.ClientId
				})
				.FirstOrDefault();

			opResult.CheckForNotFound(deleteCorrespondence);
			if (opResult.HasError)
				return opResult;

			_session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, deleteCorrespondence.ClientId).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			//only disable/"delete" the reference if it is not already 
			if (deleteCorrespondence.IsActive)
			{
				var entity = new ApplicantCompanyCorrespondence
				{
					ApplicantCompanyCorrespondenceId = deleteCorrespondence.ApplicantCompanyCorrespondenceId,
					ClientId = deleteCorrespondence.ClientId,
					IsActive = false
				};

				var props = new PropertyList<ApplicantCompanyCorrespondence>();
				props
					.Include(x => x.ApplicantCompanyCorrespondenceId)
					.Include(x => x.IsActive)
					.Include(x => x.Modified)
					.Include(x => x.ModifiedBy);

				_session.SetModifiedProperties(entity);
				_session.UnitOfWork.RegisterModified(entity, props);

				_session.UnitOfWork.Commit().MergeInto(opResult);
			}

			return opResult;
		}

		IOpResult<IEnumerable<ApplicantOnBoardingProcessDto>> IApplicantTrackingService.GetApplicantOnboardingProcesses(int clientId, int customToPostingId, bool isEnabled)
		{
			var opResult = new OpResult<IEnumerable<ApplicantOnBoardingProcessDto>>();
			var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
			var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantTrackingEnabled || isSystemAdmin)
			{
				opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantOnBoardingProcessQuery()
					.ByClientId(clientId)
					.ByCustomToPostingId(customToPostingId)
					.ByIsEnabled(isEnabled)
					.ExecuteQueryAs(x => new ApplicantOnBoardingProcessDto
					{
						ApplicantOnboardingProcessId = x.ApplicantOnboardingProcessId,
						Description = x.Description,
						ClientId = x.ClientId,
						IsEnabled = x.IsEnabled
					}).ToList());
			}

			return opResult;
		}

		IOpResult<int> IApplicantTrackingService.GetNextPostingNumber(int clientId)
		{
			var opResult = new OpResult<int>();
			var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
			var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantTrackingEnabled || isSystemAdmin)
			{
				//int? lastPostingNumber = _session.UnitOfWork.ApplicantTrackingRepository.GetNextPostingNumber(clientId);
				int? lastPostingNumber = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery().ByClientId(clientId).ExecuteQueryAs(x => x.PostingNumber).DefaultIfEmpty(0).Max();

				opResult.Data = (lastPostingNumber ?? 0) + 1;
			}

			return opResult;
		}

		IOpResult<ApplicantApplicationHeaderDto> IApplicantTrackingService.GetApplicantApplicationHeader(int applicationHeaderId)
		{
			var opResult = new OpResult<ApplicantApplicationHeaderDto>();
			var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
			var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantTrackingEnabled || isSystemAdmin)
			{
                ApplicantApplicationHeaderDto header = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantApplicationHeaderQuery()
				.ByApplicationHeaderId(applicationHeaderId)
				.ExecuteQueryAs(x => new ApplicantApplicationHeaderDto
				{
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
                    AddedByUserName = (x.AddedByUser != null) ? (x.AddedByUser.FirstName + " " + x.AddedByUser.LastName) : "",
					Score = x.Score.HasValue ? x.Score.Value : 0,
                    ApplicantDocuments = x.ApplicantDocument.Select(z => new ApplicantDocumentDto
                    {
                        ApplicantDocumentId = z.ApplicantDocumentId,
                        DocumentName = z.DocumentName,
                        ApplicationHeaderId = z.ApplicationHeaderId,
                        DateAdded = z.DateAdded,
                        LinkLocation = z.LinkLocation
                    }).ToList(),
                    CoverLetter = x.CoverLetter,
                    CoverLetterId = x.CoverLetterId,
                    JobSiteName = (x.ExternalApplicationIdentity != null) ? x.ExternalApplicationIdentity.ApplicantJobSite.JobSiteDescription : "",
                    DisclaimerId = x.DisclaimerId,
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
			}

			return opResult;

		}

		IOpResult<bool> IApplicantTrackingService.IsApplicationComplete(int applicationHeaderId)
		{
			IOpResult<bool> opResult = new OpResult<bool>();
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			var isApplicantAdmin = _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).Success;
			var isExternalApplicant = _session.CanPerformAction(ApplicantTrackingActionType.ExternalApplicant).Success;
			var isInternalApplicant = _session.CanPerformAction(ApplicantTrackingActionType.InternalApplicant).Success;

			if (isSystemAdmin || isApplicantAdmin || isExternalApplicant || isInternalApplicant)
			{
				opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantApplicationHeaderQuery()
				.ByApplicationHeaderId(applicationHeaderId)
				.ExecuteQueryAs(x => x.IsApplicationCompleted).FirstOrDefault());
			}

			return opResult;
		}

		IOpResult<ApplicantApplicationHeaderDto> IApplicantTrackingService.GetApplicantApplicationHeader(int postingId, int applicantId)
		{
			var opResult = new OpResult<ApplicantApplicationHeaderDto>();
			var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
			var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantTrackingEnabled || isSystemAdmin)
			{
				_appTrackProvider.GetApplicantApplicationHeader(postingId, applicantId).MergeAll(opResult);
			}

			return opResult;

		}
        
		IOpResult<IEnumerable<ApplicantApplicationHeaderDto>> IApplicantTrackingService.GetApplicantApplicationHeaders(int applicantId)
		{
			var opResult = new OpResult<IEnumerable<ApplicantApplicationHeaderDto>>();
			var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
			var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantTrackingEnabled || isSystemAdmin)
			{
                IEnumerable<ApplicantApplicationHeaderDto> headerList = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantApplicationHeaderQuery()
				.ByApplicantId(applicantId)
				.ExecuteQueryAs(x => new ApplicantApplicationHeaderDto
				{
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
					IsExternalApplicant = x.ExternalApplicationIdentity != null ? true : false,
                    CoverLetter=x.CoverLetter,
					CoverLetterId=x.CoverLetterId,
                    DisclaimerId = x.DisclaimerId,
					JobSiteName = (x.ExternalApplicationIdentity != null) ? x.ExternalApplicationIdentity.ApplicantJobSite.JobSiteDescription : "",
					AddedByAdmin = x.AddedByAdmin,
					AddedByUserName = (x.AddedByUser != null) ? (x.AddedByUser.FirstName + " " + x.AddedByUser.LastName) : "",
					Score = x.Score.HasValue ? x.Score.Value : 0,
                    ApplicantDocuments = x.ApplicantDocument.Select(z => new ApplicantDocumentDto
                    {
                        ApplicantDocumentId = z.ApplicantDocumentId,
                        DocumentName = z.DocumentName,
                        ApplicationHeaderId = z.ApplicationHeaderId,
                        DateAdded = z.DateAdded,
                        LinkLocation = z.LinkLocation
                    }).ToList()
                });

                IEnumerable<ApplicantApplicationDetailDto> detailList = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantApplicationDetailQuery()
                .ByApplicantId(applicantId)
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
                }).OrderBy(y=>y.ApplicationDetailId);

                if (detailList.Count() > 0)
                {
                    headerList.ForEach(x =>
                    {
                        x.Resources = detailList.Where(y => y.ApplicationHeaderId == x.ApplicationHeaderId);
                    });
                }

                opResult.TrySetData(() => headerList);
			}

			return opResult;

		}

		IOpResult<int> IApplicantTrackingService.AddApplicantApplicationHeader(int postingId, int applicantId)
		{
			var opResult = new OpResult<int>();
			var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
			var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantTrackingEnabled || isSystemAdmin)
			{
				_appTrackProvider.AddApplicantApplicationHeader(postingId, applicantId).MergeAll(opResult);
			}

			return opResult;

		}

		IOpResult<ApplicantApplicationHeaderDto> IApplicantTrackingService.UpdateApplicantApplicationHeader(ApplicantApplicationHeaderDto dto)
		{
			var opResult = new OpResult<ApplicantApplicationHeaderDto>();
			var isApplicantAdmin = _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).Success;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if ((isApplicantAdmin || isSystemAdmin) && dto.PostingId > 0)
			{
				var applicationHeader = new ApplicantApplicationHeader();
				int applicationHeaderId = dto.ApplicationHeaderId;
				applicationHeader = _session.UnitOfWork.ApplicantTrackingRepository
											.ApplicantApplicationHeaderQuery()
											.ByApplicationHeaderId(applicationHeaderId)
											.FirstOrDefault();

				applicationHeader.ApplicantId = dto.ApplicantId;
				applicationHeader.PostingId = dto.PostingId;
				applicationHeader.IsApplicationCompleted = dto.IsApplicationCompleted;
				applicationHeader.IsRecommendInterview = dto.IsRecommendInterview;
                
				applicationHeader.DateSubmitted = dto.DateSubmitted;
				applicationHeader.ApplicantResumeId = dto.ApplicantResumeId;
				applicationHeader.ApplicantRejectionReasonId = dto.ApplicantRejectionReasonId;
				applicationHeader.OrigPostingId = dto.OrigPostingId;
				applicationHeader.ApplicantStatusTypeId = dto.ApplicantStatusTypeId;
				applicationHeader.Score = dto.Score;

				_session.UnitOfWork.RegisterModified(applicationHeader);
				_session.UnitOfWork.Commit().MergeInto(opResult);

				opResult.Data = dto;
			}

			return opResult;
		}

		IOpResult<ApplicantEmploymentHistoryDto> IApplicantTrackingService.GetApplicantEmploymentHistory(int applicantEmploymentHistoryId)
		{
			var opResult = new OpResult<ApplicantEmploymentHistoryDto>();
			var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
			var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantTrackingEnabled || isSystemAdmin)
			{
				opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantEmploymentHistoryQuery()
				.ByApplicantEmploymentHistoryId(applicantEmploymentHistoryId)
				.ExecuteQueryAs(x => new ApplicantEmploymentHistoryDto
				{
					ApplicantEmploymentId = x.ApplicantEmploymentId,
					ApplicantId = x.ApplicantId,
					Company = x.Company,
					City = x.City,
					StateId = x.StateId,
					Zip = x.Zip,
					Title = x.Title,
					StartDate = x.StartDate,
					EndDate = x.EndDate,
					IsContactEmployer = x.IsContactEmployer,
					IsVoluntaryResign = x.IsVoluntaryResign,
					IsEnabled = x.IsEnabled,
					Responsibilities = x.Responsibilities,
					CountryId = x.CountryId
				}).FirstOrDefault());
			}

			return opResult;

		}

		IOpResult<IEnumerable<ApplicantEmploymentHistoryDto>> IApplicantTrackingService.
			GetApplicantEmploymentHistoryForCurrentUser()
		{
			IApplicantTrackingService service = this;
			var applicant = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantsQuery()
				.ByUserId(_session.LoggedInUserInformation.UserId).ExecuteQuery().First();
			return service.GetApplicantEmploymentHistoryByApplicantId(applicant.ApplicantId);
		}

		IOpResult<IEnumerable<ApplicantEmploymentHistoryDto>> IApplicantTrackingService.GetApplicantEmploymentHistoryByApplicantId(int applicantId)
		{
			var opResult = new OpResult<IEnumerable<ApplicantEmploymentHistoryDto>>();
			var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
			var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantTrackingEnabled || isSystemAdmin)
			{
				opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantEmploymentHistoryQuery()
					.ByApplicantId(applicantId)
					.ByIsActive(true)
					.ExecuteQueryAs(x => new ApplicantEmploymentHistoryDto
					{
						ApplicantEmploymentId = x.ApplicantEmploymentId,
						ApplicantId = x.ApplicantId,
						Company = x.Company,
						City = x.City,
						StateId = x.StateId,
						Zip = x.Zip,
						Title = x.Title,
						StartDate = x.StartDate,
						EndDate = x.EndDate,
						IsContactEmployer = x.IsContactEmployer,
						IsVoluntaryResign = x.IsVoluntaryResign,
						VoluntaryResignText = x.IsVoluntaryResign.HasValue ? (x.IsVoluntaryResign == false ? "No" : "Yes") : "Still Employed",
						IsEnabled = x.IsEnabled,
						Responsibilities = x.Responsibilities,
						CountryId = x.CountryId,
						CompanyPlusJobTitle = x.Company + " / " + x.Title

					}).ToList());
			}

			return opResult;
		}

		IOpResult<int> IApplicantTrackingService.GetPostingResumeOption(int applicantApplicationHeaderId)
		{
			var opResult = new OpResult<int>();
			var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
			var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantTrackingEnabled || isSystemAdmin)
			{
				_appTrackProvider.GetPostingResumeOption(applicantApplicationHeaderId).MergeAll(opResult);
			}

			return opResult;
		}

		IOpResult<ApplicantDto> IApplicantTrackingService.GetApplicantByUserId(int userId)
		{
			var opResult = new OpResult<ApplicantDto>();
			_session.CanPerformAction(ApplicantTrackingActionType.ReadApplicantInfo).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			var data = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantsQuery()
					.ByUserId(userId)
					.ExecuteQueryAs(x => new ApplicantDto
					{
						ApplicantId = x.ApplicantId,
						FirstName = x.FirstName,
						MiddleInitial = x.MiddleInitial,
						LastName = x.LastName,
                        EmailAddress = x.EmailAddress,
						UserId = x.UserId,
						Address = x.Address,
						AddressLine2 = x.AddressLine2,
						City = x.City,
						State = x.State ?? 0,
						Zip = x.Zip,
						PhoneNumber = x.PhoneNumber,
						CellPhoneNumber = x.CellPhoneNumber,
                        WorkPhoneNumber = x.WorkPhoneNumber,
                        WorkExtension = x.WorkExtension,
						EmployeeId = x.EmployeeId,
						ClientId = x.ClientId,
						CountryId = x.CountryId,
                        CountryAbbreviation = x.Country!=null? x.Country.Abbreviation : null,
                        IsTextEnabled = x.IsTextEnabled.HasValue ? x.IsTextEnabled.Value: false,
						IsDenied = x.IsDenied,
                        UserName = x.User.UserName,
					}).FirstOrDefault();

			opResult.CheckForNotFound(data);
			if (opResult.HasError)
				return opResult;

			_session.ResourceAccessChecks.HasAccessToApplicant(data.ApplicantId).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			opResult.SetDataOnSuccess(data);

			return opResult;
		}

		IOpResult<ApplicantDto> IApplicantTrackingService.GetApplicantEmployeeByUserId(int userId)
		{
			var opResult = new OpResult<ApplicantDto>();
			_session.CanPerformAction(ApplicantTrackingActionType.ReadApplicantInfo).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			opResult.TrySetData(() => _session.UnitOfWork.UserRepository.QueryUsers()
					.ByUserId(userId)
					.ExecuteQueryAs(x => new ApplicantDto
					{
						ApplicantId = 0,
						FirstName = x.Employee != null ? x.Employee.FirstName : "",
						MiddleInitial = x.Employee != null ? x.Employee.MiddleInitial : "",
						LastName = x.Employee != null ? x.Employee.LastName : "",
						UserId = x.UserId,
						Address = x.Employee != null ? x.Employee.AddressLine1 : "",
						AddressLine2 = x.Employee != null ? x.Employee.AddressLine2 : "",
						City = x.Employee != null ? x.Employee.City : "",
						State = x.Employee != null ? x.Employee.StateId.Value : 0,
						Zip = x.Employee != null ? x.Employee.PostalCode : "",
						PhoneNumber = x.Employee != null ? x.Employee.HomePhoneNumber : "",
						CellPhoneNumber = x.Employee != null ? x.Employee.CellPhoneNumber : "",
						EmployeeId = x.EmployeeId,
						CountryId = x.Employee != null ? x.Employee.CountryId : 0,
                        CountryAbbreviation = x.Employee!= null && x.Employee.Country!=null ? x.Employee.Country.Abbreviation : "",
						IsDenied = false,
						ClientId = x.Employee.ClientId
					}).FirstOrDefault());

			return opResult;
		}

		IOpResult<IEnumerable<ApplicantPostingListDto>> IApplicantTrackingService.GetAdminPostingList(GetJobBoardParametersDto dto)
		{
			IOpResult<IEnumerable<ApplicantPostingListDto>> result = this.Self.GetAdminPostingList(dto.ClientId, dto.UserId);

			if (dto.ShowAllPostings) return result;
			else
			{
                IEnumerable<ApplicantPostingListDto> filteredRes =  result.Data.Where(x => x.IsClosed == (!dto.ShowOpenPostings)).ToList();
				result.Data = filteredRes;
				return result;
            }            
		}

		IOpResult<IEnumerable<ApplicantPostingListDto>> IApplicantTrackingService.GetAdminPostingList(int clientId, int userId)
		{
			var opResult = new OpResult<IEnumerable<ApplicantPostingListDto>>();

			//var user = _session.UnitOfWork.UserRepository.GetUser(userId);
			var isAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success ||
						  _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).Success;

			if (isAdmin)
			{
				var qry = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery();
				qry.ByClientId(clientId);
				qry.ByIsActive(true);

				var data = qry.ExecuteQueryAs(x => new ApplicantPostingListDto
				{
                    ApplicationId = x.ApplicationId,
					PostingCategoryId = x.PostingCategoryId,
					CategoryName = x.ApplicantPostingCategory.Name,
					PostingId = x.PostingId,
					PostingNumber = x.PostingNumber,
					PostingTypeId = x.PostingTypeId,
					Description = x.Description,
					ClientDepartmentId = x.ClientDepartmentId,
					DepartmentName = x.ClientDepartment.Name,
					ClientDivisionId = x.ClientDivisionId,
					ClientId = x.ClientId,
					IsClosed = x.IsClosed,
                    PostingOwners = x.ApplicantPostingOwners.Select(z => new ApplicantPostingOwnerDto
                    {
                        PostingId = z.PostingId,
                        UserId = z.UserId,
						Name = z.User.FirstName + " " + z.User.LastName
					}).ToList(),
                    //PostingOwnerId = x.PostingOwnerId,
					Location = (x.ClientDivision != null) ?
									(x.ClientDivision.City ?? "") + ", " +
									(x.ClientDivision.State != null ? x.ClientDivision.State.Abbreviation ?? "" : "") : "",
					PublishEnd = x.PublishEnd,
					PublishStart = x.PublishStart,
					IsPublished = x.IsPublished,
                    //PostingOwnerName = x.PostingOwnerId.HasValue ? (x.PostingOwner.FirstName + " " + x.PostingOwner.LastName) : String.Empty,
					NumOfPositions = (x.NumOfPositions.HasValue && x.NumOfPositions.Value > 0) ? x.NumOfPositions.Value : 1,
					Applications = x.ApplicantApplicationHeaders.Select(apps => new ApplicantApplicationHeaderDto()
					{
						ApplicantId = apps.ApplicantId,
						ApplicantStatusTypeId = apps.ApplicantStatusTypeId,
						PostingId = apps.PostingId,
                        IsApplicationCompleted = apps.IsApplicationCompleted,
                        AddedByAdmin = apps.AddedByAdmin,
                    }).Where(a => a.IsApplicationCompleted).ToList()
					//todo: set up username PostingOwnerName = x.PostingOwner.


				}).ToList();
				opResult.Data = data;
			}
			else
			{
				opResult.SetToFail();
			}

			return opResult;
		}

		IOpResult<IEnumerable<ApplicantJobBoardCategoryDto>> IApplicantTrackingService.GetApplicantJobBoard(int clientId, int applicantId, int userId)
		{
			var opResult = new OpResult<IEnumerable<ApplicantJobBoardCategoryDto>>();

			//var user = _session.UnitOfWork.UserRepository.GetUser(userId);
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			var isApplicantAdmin = _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).Success;
			var isExternalApplicant = _session.CanPerformAction(ApplicantTrackingActionType.ExternalApplicant).Success || userId == -2147483648;
			var isInternalApplicant = _session.CanPerformAction(ApplicantTrackingActionType.InternalApplicant).Success;

			var qry = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingCategoriesQuery();
			qry.ByClientId(clientId);
			qry.ByIsActive(true);

			var data = qry.ExecuteQueryAs(x => new ApplicantJobBoardCategoryDto
			{
				PostingCategoryId = x.PostingCategoryId,
				Name = x.Name,
				Description = x.Description,
				ClientId = x.ClientId,
				IsEnabled = x.IsEnabled,
				Postings = x.ApplicantPostings.Select(ent => new ApplicantJobBoardPostingDto
				{
					PostingId = ent.PostingId,
					Description = ent.Description,
					PostingTypeId = ent.PostingTypeId,
					PostingNumber = ent.PostingNumber,
					ApplicationId = ent.ApplicationId,
					ClientDivisionId = ent.ClientDivisionId,
					ClientDepartmentId = ent.ClientDepartmentId,
					PostingCategoryId = ent.PostingCategoryId,
					JobTypeId = (int)ent.JobTypeId,
					JobRequirements = ent.JobRequirements,
					Location = (ent.ClientDivision != null) ?
									(ent.ClientDivision.City ?? "") + ", " +
									(ent.ClientDivision.State != null ? ent.ClientDivision.State.Abbreviation ?? "" : "") : "",
					Salary = ent.Salary,
					HoursPerWeek = ent.HoursPerWeek,
					StartDate = ent.StartDate,
					IsEnabled = ent.IsEnabled,
					ClientId = ent.ClientId,
					PublishedDate = ent.PublishedDate,
					JobProfileId = ent.JobProfileId,
					PublishStart = ent.PublishStart,
					PublishEnd = ent.PublishEnd,
					ApplicationCompletedCorrespondence = ent.ApplicationCompletedCorrespondence,
					ApplicantResumeRequiredId = ent.ApplicantResumeRequiredId,
					IsPublished = ent.IsPublished,
					IsClosed = ent.IsClosed,
					//JobType = ent.ApplicantJobType.Description,
					JobType = ent.EmployeeStatus.Description,
					Category = ent.ApplicantPostingCategory.Name,
					Department = ent.ClientDepartment.Name,
					Division = ent.ClientDivision.Name,
					//JobProfile = ent.JobProfile.Description,
					JobProfile = ent.JobProfile != null ? new JobProfileDto()
					{
						JobProfileId = ent.JobProfile.JobProfileId,
						Description = ent.JobProfile.Description,
						Requirements = ent.JobProfile.Requirements,
						WorkingConditions = ent.JobProfile.WorkingConditions,
						Benefits = ent.JobProfile.Benefits,
						JobResponsibilities= ent.JobProfile.JobProfileResponsibilities.Select( res=> new JobResponsibilitiesDto
						{
							JobResponsibilityId=res.JobResponibilityId,
							Description=res.JobResponsibilities.Description
						}).ToList(),
						JobSkills= ent.JobProfile.JobProfileSkills.Select(skl => new JobSkillsDto
						{
							JobSkillId = skl.JobSkillId,
							Description = skl.JobSkills.Description
						}).ToList()

					} : null,
					ApplicantHasApplied = ent.ApplicantApplicationHeaders.Any(h => h.ApplicantId == applicantId),
					ApplicantsHeaderId = ent.ApplicantApplicationHeaders.Any(h => h.ApplicantId == applicantId) ? ent.ApplicantApplicationHeaders.FirstOrDefault(h => h.ApplicantId == applicantId).ApplicationHeaderId : 0,
					ApplicantCompletedApplication = ent.ApplicantApplicationHeaders.Any(h => h.ApplicantId == applicantId && h.IsApplicationCompleted),
					ApplicantsHired = ent.ApplicantApplicationHeaders.Count(h=>h.ApplicantStatusTypeId == ApplicantStatusType.Hired),
					NumOfPositions = (ent.NumOfPositions.HasValue && ent.NumOfPositions.Value > 0) ? ent.NumOfPositions.Value : 1
				}

				).Where(p =>
							((p.PostingTypeId == PostingType.Both) ||
							(p.PostingTypeId == PostingType.External && isExternalApplicant) ||
							(p.PostingTypeId == PostingType.Internal && isInternalApplicant)) &&
							p.IsEnabled && !p.IsClosed && p.IsPublished &&
							(!p.PublishStart.HasValue || p.PublishStart <= DateTime.Now) &&
							(!p.PublishEnd.HasValue || p.PublishEnd >= DateTime.Now)

						).ToList()

			}).ToList();

			foreach (var cat in data)
			{
				cat.PostingCount = cat.Postings.Count();
				//foreach (var posting in cat.Postings)
				//{
				//    if (posting.JobProfile != null)
				//    {
				//        posting.JobProfileResponsibilities = getJobProfileResponsibilitiesByJobProfileId(posting.JobProfile.JobProfileId);
				//        posting.JobProfileSkills = getJobProfileSkillsByJobProfileId(posting.JobProfile.JobProfileId);
				//    }

				//}
			}
			data.RemoveAll(x => x.PostingCount == 0);


			opResult.Data = data;
			return opResult;
		}

		//private IEnumerable<JobProfileResponsibilitiesDto> getJobProfileResponsibilitiesByJobProfileId(int jobProfileId)
		//{
		//    var jpr = _session.UnitOfWork.ClientRepository.JobProfileResponsibilitiesQuery()
		//               .ByJobProfileId(jobProfileId)
		//               .ExecuteQueryAs(x => new JobProfileResponsibilitiesDto
		//               {
		//                   JobProfileId = x.JobProfileId,
		//                   JobResponibilityId = x.JobResponibilityId,
		//                   Description = x.JobResponsibilities.Description
		//               }).ToList();
		//    return jpr;
		//}
		//private IEnumerable<JobProfileSkillsDto> getJobProfileSkillsByJobProfileId(int jobProfileId)
		//{
		//    var jps = _session.UnitOfWork.ClientRepository.JobProfileSkillsQuery()
		//               .ByJobProfileId(jobProfileId)
		//               .ExecuteQueryAs(x => new JobProfileSkillsDto
		//               {
		//                   JobProfileId = x.JobProfileId,
		//                   JobSkillId = x.JobSkillId,
		//                   Description = x.JobSkills.Description
		//               }).ToList();
		//    return jps;
		//}

		IOpResult<ApplicantCompanyJobBoardInfoDto> IApplicantTrackingService.GetCompanyJobBoardInfo(int clientId)
		{
			var opResult = new OpResult<ApplicantCompanyJobBoardInfoDto>();

			//Use this function to pull company images, and About the company sections of the job board once they are available


			var qry = _session.UnitOfWork.ClientRepository.QueryClients();
			qry.ByClientId(clientId);


			var data = qry.ExecuteQueryAs(x => new ApplicantCompanyJobBoardInfoDto()
			{

                CompanyName = x.ApplicantClient.JobBoardTitle==null? x.ClientName : x.ApplicantClient.JobBoardTitle,
				ClientId = x.ClientId,
				ClientCode = x.ClientCode
			}).FirstOrDefault();

			opResult.Data = data;
			return opResult;
		}


		IOpResult<IEnumerable<ApplicantPostingCategoryDto>> IApplicantTrackingService.GetApplicantPostingCategories(int clientId)
		{
			var opResult = new OpResult<IEnumerable<ApplicantPostingCategoryDto>>();

			_session.CanPerformAction(ApplicantTrackingActionType.ReadApplicantInfo).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			_session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingCategoriesQuery()
			.ByClientId(clientId)
			.ExecuteQueryAs(x => new ApplicantPostingCategoryDto
			{
				PostingCategoryId = x.PostingCategoryId,
				Name = x.Name,
				Description = x.Description,
				ClientId = x.ClientId,
				IsEnabled = x.IsEnabled
			}).ToList());

			return opResult;
		}

		IOpResult<ApplicantPostingCategoryDto> IApplicantTrackingService.GetApplicantPostingCategoryByCategoryId(int categoryId)
		{
			var opResult = new OpResult<ApplicantPostingCategoryDto>();
			var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
			var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantTrackingEnabled || isSystemAdmin)
			{
				opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingCategoriesQuery()
					.ByPostingCategoryId(categoryId)
					.ExecuteQueryAs(x => new ApplicantPostingCategoryDto
					{
						PostingCategoryId = x.PostingCategoryId,
						Name = x.Name,
						Description = x.Description,
						ClientId = x.ClientId,
						IsEnabled = x.IsEnabled
					}).FirstOrDefault());
			}

			return opResult;
		}

		IOpResult<IEnumerable<ApplicantPostingCategoryDto>> IApplicantTrackingService.UpdateApplicantPostingCategory(ApplicantPostingCategoryDto dto)
		{

			var opResult = new OpResult<IEnumerable<ApplicantPostingCategoryDto>>();
			var isApplicantAdmin = _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).Success;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantAdmin || isSystemAdmin)
			{
				if (dto.PostingCategoryId == 0)
				{

					ApplicantPostingCategory existing = _session.UnitOfWork.ApplicantTrackingRepository
												.ApplicantPostingCategoriesQuery()
												.ByClientId(dto.ClientId)
												.ByPostingCategoryName(dto.Name).FirstOrDefault();

					if (existing == null)
					{
						var newPostingCategory = new ApplicantPostingCategory()
						{
							PostingCategoryId = dto.PostingCategoryId,
							Name = dto.Name,
							Description = dto.Description,
							ClientId = dto.ClientId,
							IsEnabled = dto.IsEnabled
						};
						_session.SetModifiedProperties(newPostingCategory);
						_session.UnitOfWork.RegisterNew(newPostingCategory);
					}
					else
					{
						opResult
							.AddMessage(new GenericMsg("Already a category present with the same name."))
							.SetToFail();
					}
				}
				else
				{
					ApplicantPostingCategory existing = _session.UnitOfWork.ApplicantTrackingRepository
												.ApplicantPostingCategoriesQuery()
												.ByClientId(dto.ClientId)
												.ByPostingCategoryName(dto.Name).FirstOrDefault();

					if (existing == null || (existing != null && existing.PostingCategoryId == dto.PostingCategoryId))
					{
						var editPostingCategory = _session.UnitOfWork.ApplicantTrackingRepository
												.ApplicantPostingCategoriesQuery()
												.ByPostingCategoryId(dto.PostingCategoryId)
												.FirstOrDefault();
						editPostingCategory.Description = dto.Description;
						editPostingCategory.Name = dto.Name;
						editPostingCategory.ClientId = dto.ClientId;
						editPostingCategory.IsEnabled = dto.IsEnabled;

						_session.SetModifiedProperties(editPostingCategory);
						_session.UnitOfWork.RegisterModified(editPostingCategory);
					}
					else
					{
						opResult
							.AddMessage(new GenericMsg("Already a category present with the same name."))
							.SetToFail();
					}
				}
				_session.UnitOfWork.Commit().MergeInto(opResult);

				var postingCategoriesList = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingCategoriesQuery()
					.ByClientId(dto.ClientId)
					.ExecuteQueryAs(x => new ApplicantPostingCategoryDto
					{
						PostingCategoryId = x.PostingCategoryId,
						Name = x.Name,
						Description = x.Description,
						ClientId = x.ClientId,
						IsEnabled = x.IsEnabled
					}).ToList();

				opResult.Data = postingCategoriesList;
			}

			return opResult;
		}

		IOpResult<IEnumerable<ApplicantPostingCategoryDto>> IApplicantTrackingService.DeleteApplicantPostingCategory(ApplicantPostingCategoryDto dto)
		{

			var opResult = new OpResult<IEnumerable<ApplicantPostingCategoryDto>>();
			var isApplicantAdmin = _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).Success;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantAdmin || isSystemAdmin)
			{

				var deletePostingCategory = _session.UnitOfWork.ApplicantTrackingRepository
											.ApplicantPostingCategoriesQuery()
											.ByPostingCategoryId(dto.PostingCategoryId)
											.FirstOrDefault();

				var postingCategoryClientId = deletePostingCategory.ClientId;
				_session.SetModifiedProperties(deletePostingCategory);
				_session.UnitOfWork.RegisterDeleted(deletePostingCategory);

				_session.UnitOfWork.Commit().MergeInto(opResult);

				var postingCategoriesList = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingCategoriesQuery()
					.ByClientId(postingCategoryClientId)
					.ExecuteQueryAs(x => new ApplicantPostingCategoryDto
					{
						PostingCategoryId = x.PostingCategoryId,
						Name = x.Name,
						Description = x.Description,
						ClientId = x.ClientId,
						IsEnabled = x.IsEnabled
					}).ToList();

				opResult.Data = postingCategoriesList;
			}

			return opResult;
		}

		IOpResult<ApplicantDto> IApplicantTrackingService.GetApplicantByApplicantId(int applicantId)
		{
			var opResult = new OpResult<ApplicantDto>();
			var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
			var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantTrackingEnabled || isSystemAdmin)
			{
				opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantsQuery()
					.ByApplicantId(applicantId)
					.ExecuteQueryAs(x => new ApplicantDto
					{
						ApplicantId = x.ApplicantId,
						FirstName = x.FirstName,
						MiddleInitial = x.MiddleInitial,
						LastName = x.LastName,
						Address = x.Address,
						AddressLine2 = x.AddressLine2,
						City = x.City,
						State = x.State ?? 0,
						Zip = x.Zip,
						PhoneNumber = x.PhoneNumber,
						CellPhoneNumber = x.CellPhoneNumber,
						EmailAddress = x.EmailAddress,
						Dob = x.Dob,
						EmployeeId = x.EmployeeId,
						UserId = x.UserId,
						ClientId = x.ClientId,
						IsDenied = x.IsDenied,
						WorkPhoneNumber = x.WorkPhoneNumber,
						WorkExtension = x.WorkExtension,
						CountryId = x.CountryId,
                        IsTextEnabled = x.IsTextEnabled.HasValue ? x.IsTextEnabled.Value : false,
						StateAbbreviation = x.StateDetails != null ? x.StateDetails.Abbreviation : null
					}).FirstOrDefault());
			}

			return opResult;
		}

		IOpResult<ApplicantDto> IApplicantTrackingService.UpdateApplicant(ApplicantDto dto)
		{
			var opResult = new OpResult<ApplicantDto>();
			_session.CanPerformAction(ApplicantTrackingActionType.WriteApplicantInfo).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			if (dto.IsNewEntity(x => x.ApplicantId))
			{
				var applicant = new Applicant()
				{
					FirstName = dto.FirstName,
					MiddleInitial = dto.MiddleInitial,
					LastName = dto.LastName,
					Address = dto.Address,
					AddressLine2 = dto.AddressLine2,
					City = dto.City,
					State = dto.State,
					Zip = dto.Zip,
					PhoneNumber = dto.PhoneNumber,
					CellPhoneNumber = dto.CellPhoneNumber,
					EmailAddress = dto.EmailAddress,
					Dob = dto.Dob,
					EmployeeId = dto.EmployeeId,
					UserId = dto.UserId,
					ClientId = dto.ClientId,
					IsDenied = dto.IsDenied,
					WorkPhoneNumber = dto.WorkPhoneNumber,
					WorkExtension = dto.WorkExtension,
					CountryId = dto.CountryId,
                    IsTextEnabled = dto.IsTextEnabled,
                };
				_session.UnitOfWork.RegisterNew(applicant);
				_session.UnitOfWork.RegisterPostCommitAction(() => dto.ApplicantId = applicant.ApplicantId);
			}
			else
			{
				_session.ResourceAccessChecks.HasAccessToApplicant(dto.ApplicantId).MergeInto(opResult);
				if (opResult.HasError)
					return opResult;

				var editApplicant = _session.UnitOfWork.ApplicantTrackingRepository
											.ApplicantsQuery()
											.ByApplicantId(dto.ApplicantId)
											.FirstOrDefault();

				editApplicant.ApplicantId = dto.ApplicantId;
				editApplicant.FirstName = dto.FirstName;
				editApplicant.MiddleInitial = dto.MiddleInitial;
				editApplicant.LastName = dto.LastName;
				editApplicant.Address = dto.Address;
				editApplicant.AddressLine2 = dto.AddressLine2;
				editApplicant.City = dto.City;
				editApplicant.State = dto.State;
				editApplicant.Zip = dto.Zip;
				editApplicant.PhoneNumber = dto.PhoneNumber;
				editApplicant.CellPhoneNumber = dto.CellPhoneNumber;
				editApplicant.EmailAddress = dto.EmailAddress;
				editApplicant.Dob = dto.Dob;
				editApplicant.EmployeeId = dto.EmployeeId;
				editApplicant.UserId = dto.UserId;
				editApplicant.ClientId = dto.ClientId;
				editApplicant.IsDenied = dto.IsDenied;
				editApplicant.WorkPhoneNumber = dto.WorkPhoneNumber;
				editApplicant.WorkExtension = dto.WorkExtension;
				editApplicant.CountryId = dto.CountryId;
                editApplicant.IsTextEnabled = dto.IsTextEnabled;

				_session.UnitOfWork.RegisterModified(editApplicant);
			}
			_session.UnitOfWork.Commit().MergeInto(opResult);

			opResult.SetDataOnSuccess(dto);

			return opResult;
		}

		IOpResult<IEnumerable<ApplicantStatusTypeDto>> IApplicantTrackingService.GetApplicantStatusTypes(bool isActive)
		{
			var opResult = new OpResult<IEnumerable<ApplicantStatusTypeDto>>();
			var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
			var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantTrackingEnabled || isSystemAdmin)
			{
				opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantStatusTypeQuery()
					.ByIsActive(isActive)
					.ExecuteQueryAs(x => new ApplicantStatusTypeDto
					{
						ApplicantStatusId = x.ApplicantStatusId,
						Description = x.Description,
						IsActive = x.IsActive,
						SortOrder = x.SortOrder,
					}).ToList());
			}

			return opResult;
		}

		IOpResult<IEnumerable<ApplicantRejectionReasonDto>> IApplicantTrackingService.GetApplicantRejectionReasons(int clientId, bool isActive)
		{
			var opResult = new OpResult<IEnumerable<ApplicantRejectionReasonDto>>();
			var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
			var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantTrackingEnabled || isSystemAdmin)
			{
				opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantRejectionReasonQuery()
					.ByClientId(clientId)
					.ByIsActive(isActive)
					.ExecuteQueryAs(x => new ApplicantRejectionReasonDto
					{
						ApplicantRejectionReasonId = x.ApplicantRejectionReasonId,
						ClientId = x.ClientId,
						Description = x.Description,
						IsEnabled = x.IsEnabled,
						Modified = x.Modified,
						ModifiedBy = x.ModifiedBy,
					}).ToList());
			}

			return opResult;
		}

		IOpResult<IEnumerable<ApplicantRejectionReasonDto>> IApplicantTrackingService.UpdateApplicantRejectionReasons(ApplicantRejectionReasonDto dto)
		{

			var opResult = new OpResult<IEnumerable<ApplicantRejectionReasonDto>>();
			var isApplicantAdmin = _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).Success;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantAdmin || isSystemAdmin)
			{
				if (dto.ApplicantRejectionReasonId == 0)
				{
					var newRejectionReason = new ApplicantRejectionReason()
					{
						ApplicantRejectionReasonId = dto.ApplicantRejectionReasonId,
						ClientId = dto.ClientId,
						Description = dto.Description,
						IsEnabled = true,
						ModifiedBy = _session.LoggedInUserInformation.UserId,
						Modified = DateTime.Now

					};
					_session.UnitOfWork.RegisterNew(newRejectionReason);
				}
				else
				{
					var editRejectionReason = _session.UnitOfWork.ApplicantTrackingRepository
												.ApplicantRejectionReasonQuery()
												.ByApplicantRejectionReasonId(dto.ApplicantRejectionReasonId)
												.FirstOrDefault();
					editRejectionReason.Description = dto.Description;
					editRejectionReason.ModifiedBy = _session.LoggedInUserInformation.UserId;
					editRejectionReason.Modified = DateTime.Now;
					_session.UnitOfWork.RegisterModified(editRejectionReason);
				}
				_session.UnitOfWork.Commit().MergeInto(opResult);

				if (opResult.Success)
				{
					opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantRejectionReasonQuery()
					.ByClientId(dto.ClientId)
					.ByIsActive(true)
					.ExecuteQueryAs(x => new ApplicantRejectionReasonDto
					{
						ApplicantRejectionReasonId = x.ApplicantRejectionReasonId,
						ClientId = x.ClientId,
						Description = x.Description,
						IsEnabled = x.IsEnabled,
						Modified = x.Modified,
						ModifiedBy = x.ModifiedBy,
					}).ToList());
				}
			}

			return opResult;
		}

		IOpResult<IEnumerable<ApplicantRejectionReasonDto>> IApplicantTrackingService.DeleteApplicantRejectionReasons(ApplicantRejectionReasonDto dto)
		{

			var opResult = new OpResult<IEnumerable<ApplicantRejectionReasonDto>>();
			var isApplicantAdmin = _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).Success;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantAdmin || isSystemAdmin)
			{
				var deleteRejectionReason = _session.UnitOfWork.ApplicantTrackingRepository
												.ApplicantRejectionReasonQuery()
												.ByApplicantRejectionReasonId(dto.ApplicantRejectionReasonId)
												.FirstOrDefault();
				deleteRejectionReason.IsEnabled = false;
				deleteRejectionReason.ModifiedBy = _session.LoggedInUserInformation.UserId;
				deleteRejectionReason.Modified = DateTime.Now;
				_session.UnitOfWork.RegisterModified(deleteRejectionReason);

				_session.UnitOfWork.Commit().MergeInto(opResult);

				if (opResult.Success)
				{
					opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantRejectionReasonQuery()
					.ByClientId(dto.ClientId)
					.ByIsActive(true)
					.ExecuteQueryAs(x => new ApplicantRejectionReasonDto
					{
						ApplicantRejectionReasonId = x.ApplicantRejectionReasonId,
						ClientId = x.ClientId,
						Description = x.Description,
						IsEnabled = x.IsEnabled,
						Modified = x.Modified,
						ModifiedBy = x.ModifiedBy,
					}).ToList());
				}
			}

			return opResult;
		}

		IOpResult<ClosePostingDto> IApplicantTrackingService.ClosePosting(ClosePostingDto dto)
		{
			var opResult = new OpResult<ClosePostingDto>();
			var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
			var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantTrackingEnabled || isSystemAdmin)
			{
                AzureViewDto imgSource = null;
                imgSource = _clientAzureService.GetAzureClientResource(ResourceSourceType.AzureClientImage, dto.ClientId, "logo").Data;
                string imgUrl = imgSource?.Source;
				foreach (var applicantFinalCorrespondence in dto.ApplicantFinalCorrespondences)
				{
					int applicantCorrespondenceId = applicantFinalCorrespondence.ApplicantCorrespondenceId ?? 0;
					if (applicantCorrespondenceId > 0)
					{
						var applicantsAutoDenials = this.Self.GetApplicantsAutoDenyList(dto.PostingId, applicantFinalCorrespondence.ApplicantStatusId).Data;
						foreach (var applicantsAutoDenial in applicantsAutoDenials)
						{
							var applicantEmail = applicantsAutoDenial.Email;
							var applicationHeader = this.Self.GetApplicantApplicationHeader(dto.PostingId, applicantsAutoDenial.ApplicantId).Data;
                            int correspondenceId = -1;

                            if (!string.IsNullOrEmpty( applicantEmail ))
							{
								correspondenceId = applicantCorrespondenceId;

								//Get Applicants last email correspondence for this post
								int? recentApplicantCorrespondenceId = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantApplicationEmailHistoryQuery()
								    .ByApplicationHeaderId(applicationHeader.ApplicationHeaderId)
								    .ExecuteQueryAs(h => new { h.ApplicantCompanyCorrespondenceId, h.SentDate })
								    .OrderByDescending(i => i.SentDate)
								    .Select(j => j.ApplicantCompanyCorrespondenceId)
								    .FirstOrDefault();

								//Skip
								if (recentApplicantCorrespondenceId.HasValue && recentApplicantCorrespondenceId.Value == correspondenceId) continue;
							}

							var updatedApplicationHeader = this.Self.UpdateApplicantApplicationHeader(applicationHeader);

							//GetEmail Body
							var qry = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantCompanyCorrespondenceQuery();
							qry.ByIsActive(true);

							var correspondenceData = qry.ExecuteQueryAs(x => new ApplicantCompanyCorrespondenceDto
							{
								ApplicantCompanyCorrespondenceId = x.ApplicantCompanyCorrespondenceId,
								Body = x.Body,
                                Subject = x.Subject,
								Description = x.Description,
								ClientId = x.ClientId,
								ApplicantCorrespondenceTypeId = (Dto.ApplicantTracking.ApplicantCorrespondenceType)x.ApplicantCorrespondenceTypeId,
								IsActive = x.IsActive,
                                IsText = x.IsText.HasValue ? x.IsText.Value : false,
                            }).Where(p => p.ApplicantCompanyCorrespondenceId == correspondenceId).FirstOrDefault();

							if (correspondenceData != null)
							{
                                //Send Email
                                var applicantDetailDto = new ApplicantDetailDto {
                                    ApplicantId = applicationHeader.ApplicantId, 
                                    ApplicantName = applicationHeader.Applicant.FirstName + " " + applicationHeader.Applicant.LastName,
                                    ApplicationHeaderId = applicationHeader.ApplicationHeaderId,
                                    PostingId = applicationHeader.PostingId,
                                    Posting = applicationHeader.PostingDescription
                                };

                                var qry2 = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantApplicationHeaderQuery()
                                            .ByApplicationHeaderId(applicationHeader.ApplicationHeaderId);

                                var replacementData = qry2.ExecuteQueryAs(x => new ApplicantTrackingCorrespondenceReplacementInfoDto
								{
                                    ApplicantClientId = x.Applicant.ClientId,
                                    ApplicantFirstName = x.Applicant.FirstName,
                                    ApplicantLastName = x.Applicant.LastName,
                                    ApplicantEmail = x.Applicant.EmailAddress,
                                    UserName = x.Applicant.User != null ? x.Applicant.User.UserName : "",
                                    Posting = x.ApplicantPosting.Description,
                                    Date = DateTime.Now,
                                    Address = x.Applicant.AddressLine2 != ""
                                        ? x.Applicant.Address + "<br/>" + x.Applicant.AddressLine2 + "<br/>" + x.Applicant.City + ", " +
                                          x.Applicant.StateDetails.Name + " " + x.Applicant.Zip
                                        : x.Applicant.Address + "<br/>" + x.Applicant.City + ", " + x.Applicant.StateDetails.Name +
                                          " " + x.Applicant.Zip,
                                                    Phone = x.Applicant.PhoneNumber,
                                                    CompanyAddress = x.Applicant.Client.AddressLine2 != ""
                                        ? x.Applicant.Client.AddressLine1 + "<br/>" + x.Applicant.Client.AddressLine2 + "<br/>" +
                                          x.Applicant.Client.City + ", " + x.Applicant.Client.State.Name + " " +
                                          x.Applicant.Client.PostalCode
                                        : x.Applicant.Client.AddressLine1 + "<br/>" + x.Applicant.Client.City + ", " +
                                          x.Applicant.Client.State.Name + " " + x.Applicant.Client.PostalCode,
                                                    CompanyName = x.Applicant.Client.ApplicantClient.JobBoardTitle == null ? x.Applicant.Client.ClientName : x.Applicant.Client.ApplicantClient.JobBoardTitle,
                                                    CompanyLogo = imgUrl
                                }).FirstOrDefault();

                                // Handle notifications
                                _applicantTrackingNotificationService.ProcessCorrespondenceNotification(applicantDetailDto, correspondenceData.ApplicantCompanyCorrespondenceId, correspondenceData.Subject, correspondenceData.Body, replacementData);

								// Saving emailhistory
								var applicationEmail = new ApplicantApplicationEmailHistory();
								applicationEmail.ApplicationHeaderId = applicationHeader.ApplicationHeaderId;
								applicationEmail.ApplicantCompanyCorrespondenceId = correspondenceData.ApplicantCompanyCorrespondenceId;
								applicationEmail.ApplicantStatusTypeId = applicationHeader.ApplicantStatusTypeId;
								applicationEmail.SenderId = _session.LoggedInUserInformation.UserId;
								applicationEmail.SentDate = DateTime.Now;
								applicationEmail.Subject = correspondenceData.Subject;
								applicationEmail.Body = correspondenceData.Body;
								if (applicationEmail.SenderId.HasValue)
								{
									applicationEmail.SenderEmail = _session.UnitOfWork.UserRepository.QueryUsers()
									.ByUserId(applicationEmail.SenderId.Value).ExecuteQueryAs(x => x.EmailAddress).FirstOrDefault();
								}

								_session.UnitOfWork.RegisterNew(applicationEmail);
								_session.UnitOfWork.Commit().MergeInto(opResult);
							}
						}
					}
				}

				//Reject remaining Applicants
				if (dto.IsRejectRemainingApplicants)
				{
					var applicantApplicationHeaders = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantApplicationHeaderQuery()
					.ByPostingId(dto.PostingId)
					.RejectableApplications()
					.ExecuteQuery().ToList();

					if (applicantApplicationHeaders.Count > 0)
					{
						foreach (var applicantApplicationHeader in applicantApplicationHeaders)
						{
							applicantApplicationHeader.ApplicantStatusTypeId = Dto.ApplicantTracking.ApplicantStatusType.Rejected;
							applicantApplicationHeader.ApplicantRejectionReasonId = dto.RejectionReason;
							_session.UnitOfWork.RegisterModified<ApplicantApplicationHeader>(applicantApplicationHeader);
						}
						_session.UnitOfWork.Commit().MergeInto(opResult);
					}
				}

				//Close Posting
				if (dto.IsClosePosting)
				{
					var applicantPostings = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
					.ByPostingId(dto.PostingId)
					.ExecuteQuery().ToList();

					if (applicantPostings.Count > 0)
					{
						foreach (var applicantPosting in applicantPostings)
						{
							applicantPosting.IsClosed = true;
							_session.UnitOfWork.RegisterModified<ApplicantPosting>(applicantPosting);
						}
						_session.UnitOfWork.Commit().MergeInto(opResult);
					}
				}
			}

			return opResult;

		}

		IOpResult<IEnumerable<ApplicantsAutoDenialDto>> IApplicantTrackingService.GetApplicantsAutoDenyList(int postingId, ApplicantStatusType applicantStatusId)
		{
			var opResult = new OpResult<IEnumerable<ApplicantsAutoDenialDto>>();
			var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
			var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantTrackingEnabled || isSystemAdmin)
			{
				opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantApplicationHeaderQuery()
					.ByPostingId(postingId)
					.ByApplicantStatusTypeId(applicantStatusId)
					.ExecuteQueryAs(x => new ApplicantsAutoDenialDto
					{
						ApplicantId = x.ApplicantId,
						Email = x.Applicant.EmailAddress,
						ApplicationHeaderId = x.ApplicationHeaderId,
						ClientName = x.ApplicantPosting.Client.ClientName
					}).ToList());
			}

			return opResult;
		}

		#region Applicant References
		IOpResult<IEnumerable<ApplicantReferenceDto>> IApplicantTrackingService.GetApplicantReferencesByApplicantId(int applicantId)
		{
			var opResult = new OpResult<IEnumerable<ApplicantReferenceDto>>();

			//check that the user has the ability to read the requested applicant's info
			var readApplicant = _session.CanPerformAction(ApplicantTrackingActionType.ReadApplicantInfo);
			var applicantAdmin = _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin);
			if (readApplicant.HasError && applicantAdmin.HasError)
			{
				readApplicant.MergeInto(opResult);
				applicantAdmin.MergeInto(opResult);
				return opResult;
			}

			if (applicantId != 0 && !applicantAdmin.Success)
			{
				_session.ResourceAccessChecks.HasAccessToApplicant(applicantId).MergeInto(opResult);

				if (opResult.HasError)
					return opResult;
			}
			//if (!_session.UnitOfWork.ApplicantTrackingRepository.ApplicantsQuery()
			//        .ByUserId(_session.LoggedInUserInformation.UserId).ExecuteQuery().Any())
			//{
			//    var applicantData = _session.UnitOfWork.UserRepository.QueryUsers()
			//        .ByUserId(_session.LoggedInUserInformation.UserId)
			//        .ExecuteQueryAs(ApplicantTrackingMaps.FromUserReference.ToApplicationDto.Instance).FirstOrDefault();
			//    if (applicantData.EmployeeId != null)
			//    {
			//        Self.UpdateApplicant(applicantData).MergeInto(opResult);
			//    }
			//    else
			//    {
			//        return opResult;
			//    }
			//}

			return opResult.TryCatchIfSuccessful(() =>
			{
				opResult.Data = _session.UnitOfWork.ApplicantTrackingRepository
				  .ApplicantReferenceQuery()
				  .ByApplicantId(applicantId)
				  .IsEnabled(true)
				  .ExecuteQueryAs(ApplicantTrackingMaps.FromApplicantReference.ToApplicantReferenceDto.Instance).ToList();
			});
		}

		IOpResult<ApplicantReferenceDto> IApplicantTrackingService.GetApplicantReference(int applicantReferenceId)
		{
			var opResult = new OpResult<ApplicantReferenceDto>();

			_session.CanPerformAction(ApplicantTrackingActionType.ReadApplicantInfo).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			var appRef = opResult.TryGetData(() => _session.UnitOfWork.ApplicantTrackingRepository
				.ApplicantReferenceQuery()
				.ByApplicantReferenceId(applicantReferenceId)
				.ExecuteQueryAs(ApplicantTrackingMaps.FromApplicantReference.ToApplicantReferenceDto.Instance)
				.FirstOrDefault());

			//check if an applicant reference object was found
			opResult.CheckForNotFound(appRef);
			if (opResult.HasError)
				return opResult;

			//...if so, now that we have one, make sure the user can access the applicant it's associated with
			_session.ResourceAccessChecks.HasAccessToApplicant(appRef.ApplicantId).MergeInto(opResult);

			//if all's good up until now we can finally return the requested reference
			opResult.SetDataOnSuccess(appRef);

			return opResult;
		}

		IOpResult<ApplicantReferenceDto> IApplicantTrackingService.SaveApplicantReferenceForUser(
			ApplicantReferenceDto dto)
		{
			IOpResult<ApplicantReferenceDto> opResult = new OpResult<ApplicantReferenceDto>();
			IApplicantTrackingService service = this;

			var applicant = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantsQuery()
				.ByUserId(_session.LoggedInUserInformation.UserId).ExecuteQuery().FirstOrDefault();

			if (applicant != null)
			{
				dto.ApplicantId = applicant.ApplicantId;
				opResult = service.SaveApplicantReference(dto);
			}

			return opResult;
		}

		IOpResult<ApplicantReferenceDto> IApplicantTrackingService.SaveApplicantReference(ApplicantReferenceDto dto)
		{
			var opResult = new OpResult<ApplicantReferenceDto>();

			//check that the user has the ability to update the requested applicant's info
			_session.CanPerformAction(ApplicantTrackingActionType.WriteApplicantInfo).MergeInto(opResult);
			_session.ResourceAccessChecks.HasAccessToApplicant(dto.ApplicantId).MergeInto(opResult);

			if (opResult.HasError)
				return opResult;

			if (dto.IsNewEntity(x => x.ApplicantReferenceId))
			{
				var newReference = new ApplicantReference()
				{
					ApplicantId = dto.ApplicantId,
					FirstName = dto.FirstName,
					LastName = dto.LastName,
					Relationship = dto.Relationship,
					EmailAddress = dto.EmailAddress,
					PhoneNumber = dto.PhoneNumber,
					YearsKnown = dto.YearsKnown,
					IsEnabled = true
				};
				_session.UnitOfWork.RegisterNew(newReference);
				_session.UnitOfWork.RegisterPostCommitAction(() => dto.ApplicantReferenceId = newReference.ApplicantReferenceId);
			}
			else
			{
				var editReference = _session.UnitOfWork.ApplicantTrackingRepository
					.ApplicantReferenceQuery()
					.ByApplicantReferenceId(dto.ApplicantReferenceId)
					.FirstOrDefault();

				//handle rare scenario where the DTO's applicant ID is different than the saved one
				//if it is different, make sure user can update it ... also reset the DTO's ID to the saved one
				//since it shouldn't be possible to change the ApplicantID
				if (dto.ApplicantId != editReference.ApplicantId)
				{
					_session.ResourceAccessChecks.HasAccessToApplicant(editReference.ApplicantId).MergeInto(opResult);
					if (opResult.HasError)
						return opResult;

					dto.ApplicantId = editReference.ApplicantId;
				}

				editReference.FirstName = dto.FirstName;
				editReference.LastName = dto.LastName;
				editReference.Relationship = dto.Relationship;
				editReference.EmailAddress = dto.EmailAddress;
				editReference.PhoneNumber = dto.PhoneNumber;
				editReference.YearsKnown = dto.YearsKnown;
				editReference.IsEnabled = true;

				_session.UnitOfWork.RegisterModified(editReference);
			}

			_session.UnitOfWork.Commit().MergeInto(opResult);

			opResult.SetDataOnSuccess(dto);

			return opResult;
		}

		IOpResult IApplicantTrackingService.DeleteApplicantReference(int applicantReferenceId)
		{
			var opResult = new OpResult();

			//check that the user has the ability to delete applicant info
			_session.CanPerformAction(ApplicantTrackingActionType.WriteApplicantInfo).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			var deleteReference = _session.UnitOfWork.ApplicantTrackingRepository
				.ApplicantReferenceQuery()
				.ByApplicantReferenceId(applicantReferenceId)
				.ExecuteQueryAs(x => new
				{
					x.ApplicantId,
					x.ApplicantReferenceId,
					x.IsEnabled
				})
				.FirstOrDefault();

			opResult.CheckForNotFound(deleteReference);
			if (opResult.HasError)
				return opResult;

			//make sure user has access to this applicant's info
			_session.ResourceAccessChecks.HasAccessToApplicant(deleteReference.ApplicantId).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			//only disable/"delete" the reference if it is not already 
			if (deleteReference.IsEnabled)
			{
				var entity = new ApplicantReference
				{
					ApplicantReferenceId = applicantReferenceId,
					IsEnabled = false
				};

				//only update the IsEnabled field in the DB
				_session.UnitOfWork.RegisterModified(entity, new PropertyList<ApplicantReference>().Include(x => x.IsEnabled));
				_session.UnitOfWork.Commit().MergeInto(opResult);
			}

			return opResult;
		}

		#endregion

		#region ApplicantOnBoardingHeader

		IOpResult<ApplicantOnBoardingHeaderDto> IApplicantTrackingService.GetApplicantOnBoardingHeaderByHeaderId(int applicantOnBoardingHeaderId)
		{
			var opResult = new OpResult<ApplicantOnBoardingHeaderDto>();
			var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
			var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantTrackingEnabled || isSystemAdmin || accessRights.UserId == 0)
			{
				opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantOnBoardingHeaderQuery()
					.ByApplicantOnBoardingHeaderId(applicantOnBoardingHeaderId)
					.ExecuteQueryAs(x => new ApplicantOnBoardingHeaderDto
					{
						ApplicantOnBoardingHeaderId = x.ApplicantOnBoardingHeaderId,
						ApplicantApplicationHeaderId = x.ApplicantApplicationHeaderId,
						ApplicantOnBoardingProcessId = x.ApplicantOnBoardingProcessId,
						ApplicantId = x.ApplicantId,
						ClientId = x.ClientId,
						OnBoardingStarted = x.OnBoardingStarted,
						OnBoardingEnded = x.OnBoardingEnded,
						IsHired = x.IsHired,
						IsRejected = x.IsRejected,
						ModifiedBy = x.ModifiedBy,
						Modified = x.Modified
					}).FirstOrDefault());
			}

			return opResult;
		}



		IOpResult<ApplicantOnBoardingHeaderDto> IApplicantTrackingService.UpdateApplicantOnBoardingHeader(ApplicantOnBoardingHeaderDto dto)
		{
			var opResult = new OpResult<ApplicantOnBoardingHeaderDto>();
			var isApplicantAdmin = _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).Success;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantAdmin || isSystemAdmin)
			{
				var applicantOnBoardingHeader = new ApplicantOnBoardingHeader();
				int applicantOnBoardingHeaderId = dto.ApplicantOnBoardingHeaderId;
				if (applicantOnBoardingHeaderId == 0)
				{
					applicantOnBoardingHeader = new ApplicantOnBoardingHeader()
					{
						ApplicantOnBoardingHeaderId = dto.ApplicantOnBoardingHeaderId,
						ApplicantApplicationHeaderId = dto.ApplicantApplicationHeaderId,
						ApplicantOnBoardingProcessId = dto.ApplicantOnBoardingProcessId,
						ApplicantId = dto.ApplicantId,
						ClientId = dto.ClientId,
						OnBoardingStarted = dto.OnBoardingStarted,
						OnBoardingEnded = dto.OnBoardingEnded,
						IsHired = dto.IsHired,
						IsRejected = dto.IsRejected,
						ModifiedBy = dto.ModifiedBy,
						Modified = dto.Modified
					};
					_session.UnitOfWork.RegisterNew(applicantOnBoardingHeader);
				}
				else
				{
					applicantOnBoardingHeader = _session.UnitOfWork.ApplicantTrackingRepository
												.ApplicantOnBoardingHeaderQuery()
												.ByApplicantOnBoardingHeaderId(dto.ApplicantOnBoardingHeaderId)
												.FirstOrDefault();

					applicantOnBoardingHeader.ApplicantOnBoardingHeaderId = dto.ApplicantOnBoardingHeaderId;
					applicantOnBoardingHeader.ApplicantApplicationHeaderId = dto.ApplicantApplicationHeaderId;
					applicantOnBoardingHeader.ApplicantOnBoardingProcessId = dto.ApplicantOnBoardingProcessId;
					applicantOnBoardingHeader.ApplicantId = dto.ApplicantId;
					applicantOnBoardingHeader.ClientId = dto.ClientId;
					applicantOnBoardingHeader.OnBoardingStarted = dto.OnBoardingStarted;
					applicantOnBoardingHeader.OnBoardingEnded = dto.OnBoardingEnded;
					applicantOnBoardingHeader.IsHired = dto.IsHired;
					applicantOnBoardingHeader.IsRejected = dto.IsRejected;
					applicantOnBoardingHeader.ModifiedBy = dto.ModifiedBy;
					applicantOnBoardingHeader.Modified = dto.Modified;

					_session.UnitOfWork.RegisterModified(applicantOnBoardingHeader);
				}
				_session.UnitOfWork.Commit().MergeInto(opResult);

				if (dto.ApplicantOnBoardingHeaderId == 0)
				{
					dto.ApplicantOnBoardingHeaderId = applicantOnBoardingHeader.ApplicantOnBoardingHeaderId;
				}

				opResult.Data = dto;
			}

			return opResult;
		}

		#endregion

		#region ApplicantApplicationDetail

		IOpResult<ApplicantApplicationDetailDto> IApplicantTrackingService.GetApplicantApplicationDetail(int detailId)
		{
			var opResult = new OpResult<ApplicantApplicationDetailDto>();
			var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
			var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantTrackingEnabled || isSystemAdmin || accessRights.UserId == 0)
			{

				opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantApplicationDetailQuery()
					.ByApplicationDetailId(detailId)
					.ExecuteQueryAs(x => new ApplicantApplicationDetailDto
					{
						ApplicationHeaderId = x.ApplicationHeaderId,
						QuestionId = x.QuestionId,
						Response = x.Response,
						IsFlagged = x.IsFlagged,
						SectionId = x.SectionId,
						ApplicationDetailId = x.ApplicationDetailId
					}).FirstOrDefault());
			}

			return opResult;
		}

		IOpResult<IEnumerable<ApplicantApplicationDetailDto>> IApplicantTrackingService.GetApplicantApplicationDetailsByHeaderId(int headerId)
		{
			var opResult = new OpResult<IEnumerable<ApplicantApplicationDetailDto>>();
			var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
			var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantTrackingEnabled || isSystemAdmin || accessRights.UserId == 0)
			{

				opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantApplicationDetailQuery()
					.ByApplicationHeaderId(headerId)
					.ExecuteQueryAs(x => new ApplicantApplicationDetailDto
					{
						ApplicationHeaderId = x.ApplicationHeaderId,
						QuestionId = x.QuestionId,
						Response = x.Response,
						IsFlagged = x.IsFlagged,
						SectionId = x.SectionId,
						ApplicationDetailId = x.ApplicationDetailId
					}).ToList());
			}

			return opResult;
		}

		IOpResult<ApplicantApplicationDetailDto> IApplicantTrackingService.UpdateApplicantApplicationDetail(ApplicantApplicationDetailDto dto)
		{
			var opResult = new OpResult<ApplicantApplicationDetailDto>();
			var isApplicantAdmin = _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).Success;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantAdmin || isSystemAdmin)
			{
				var applicantApplicationDetail = new ApplicantApplicationDetail();
				int detailId = dto.ApplicationDetailId;
				if (detailId == 0)
				{
					applicantApplicationDetail = new ApplicantApplicationDetail()
					{
						ApplicationHeaderId = dto.ApplicationHeaderId,
						QuestionId = dto.QuestionId,
						Response = dto.Response,
						IsFlagged = dto.IsFlagged,
						SectionId = dto.SectionId
					};
					_session.UnitOfWork.RegisterNew(applicantApplicationDetail);
				}
				else
				{
					applicantApplicationDetail = _session.UnitOfWork.ApplicantTrackingRepository
												.ApplicantApplicationDetailQuery()
												.ByApplicationDetailId(dto.ApplicationDetailId)
												.FirstOrDefault();

					applicantApplicationDetail.ApplicationDetailId = dto.ApplicationDetailId;
					applicantApplicationDetail.ApplicationHeaderId = dto.ApplicationHeaderId;
					applicantApplicationDetail.QuestionId = dto.QuestionId;
					applicantApplicationDetail.Response = dto.Response;
					applicantApplicationDetail.IsFlagged = dto.IsFlagged;
					applicantApplicationDetail.SectionId = dto.SectionId;

					_session.UnitOfWork.RegisterModified(applicantApplicationDetail);
				}
				_session.UnitOfWork.Commit().MergeInto(opResult);

				if (dto.ApplicationDetailId == 0)
				{
					dto.ApplicationDetailId = applicantApplicationDetail.ApplicationDetailId;
				}

				opResult.Data = dto;
			}

			return opResult;
		}

		IOpResult<IEnumerable<ApplicantApplicationDetailDto>> IApplicantTrackingService.DeleteApplicantApplicationDetail(int detailId, int headerId)
		{
			var opResult = new OpResult<IEnumerable<ApplicantApplicationDetailDto>>();
			var isApplicantAdmin = _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).Success;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantAdmin || isSystemAdmin)
			{
				var detailToDelete = _session.UnitOfWork.ApplicantTrackingRepository
											.ApplicantApplicationDetailQuery()
											.ByApplicationDetailId(detailId)
											.FirstOrDefault();

				_session.UnitOfWork.RegisterDeleted(detailToDelete);
				_session.UnitOfWork.Commit().MergeInto(opResult);

				var applicantApplicationDetails = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantApplicationDetailQuery()
					.ByApplicationHeaderId(headerId)
					.ExecuteQueryAs(x => new ApplicantApplicationDetailDto
					{
						ApplicationHeaderId = x.ApplicationHeaderId,
						QuestionId = x.QuestionId,
						Response = x.Response,
						IsFlagged = x.IsFlagged,
						SectionId = x.SectionId,
						ApplicationDetailId = x.ApplicationDetailId
					}).ToList();
				opResult.Data = applicantApplicationDetails;
			}
			return opResult;
		}

		#endregion

		#region ApplicantEducationHistory

		IOpResult<IEnumerable<ApplicantEducationHistoryDto>> IApplicantTrackingService.GetApplicantEducationHistoryByApplicantId(int applicantId)
		{
			var opResult = new OpResult<IEnumerable<ApplicantEducationHistoryDto>>();
			var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
			var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantTrackingEnabled || isSystemAdmin || accessRights.UserId == 0)
			{
				opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantEducationHistoryQuery()
					.ByApplicantId(applicantId)
					.ExecuteQueryAs(x => new ApplicantEducationHistoryDto
					{
						ApplicantEducationId = x.ApplicantEducationId,
						ApplicantId = x.ApplicantId,
						Description = x.Description,
						DateStarted = x.DateStarted,
						DateEnded = x.DateEnded,
						HasDegree = x.HasDegree,
						DegreeId = x.DegreeId,
						Degree = x.ApplicantDegreeList.Description,
						Studied = x.Studied,
						IsEnabled = x.IsEnabled,
						YearsCompleted = x.YearsCompleted,
						ApplicantSchoolTypeId = x.ApplicantSchoolTypeId,
						ApplicantSchoolType = x.ApplicantSchoolType.Description

					}).ToList());
			}

			return opResult;
		}



		#endregion

        #region "ApplicantSkills"
        IOpResult<IEnumerable<ApplicantSkillDto>> IApplicantTrackingService.GetApplicantSkillsByApplicantId(int applicantId)
        {
            var opResult = new OpResult<IEnumerable<ApplicantSkillDto>>();

            //check that the user has the ability to read the requested applicant's info
            var readApplicant = _session.CanPerformAction(ApplicantTrackingActionType.ReadApplicantInfo);
            var applicantAdmin = _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin);
            if (readApplicant.HasError && applicantAdmin.HasError)
            {
                readApplicant.MergeInto(opResult);
                applicantAdmin.MergeInto(opResult);
                return opResult;
            }

            if (applicantId != 0 && !applicantAdmin.Success)
            {
                _session.ResourceAccessChecks.HasAccessToApplicant(applicantId).MergeInto(opResult);

                if (opResult.HasError)
                    return opResult;
            }

            return opResult.TrySetData( () => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantSkillQuery()
                    .ByApplicantId(applicantId).ByEnabled(true).ExecuteQueryAs(new ApplicantSkillMapper.ToApplicantSkillDto()) );
        }
        #endregion

		#region ApplicantNote

		IOpResult<IEnumerable<ApplicantNoteDto>> IApplicantTrackingService.GetApplicantNotesByApplicantIdAndApplicantPostingId(int applicantId, int applicantPostingId)
		{
			var opResult = new OpResult<IEnumerable<ApplicantNoteDto>>();

			var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
			var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantTrackingEnabled || isSystemAdmin || accessRights.UserId == 0)
			{
                opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantNoteQuery()
                    .ByApplicantId(applicantId)
                    .ByClientId(_session.LoggedInUserInformation?.ClientId ?? 0)
                    .ByApplicantPostingId(applicantPostingId)
                    .ExecuteQueryAs(x => new ApplicantNoteDto
                    {
                        ApplicantNoteId = x.Remark.RemarkId,
                        ApplicantId = x.ApplicantId,
                        UserId = x.Remark.AddedBy,
                        PostedBy = x.Remark.User.FirstName + " " + x.Remark.User.LastName,
                        Note = x.Remark.Description,
                        DateSubmitted = x.Remark.AddedDate
                    })
                );
            }

			return opResult;
		}

		IOpResult<IEnumerable<ApplicantNoteDto>> IApplicantTrackingService.GetApplicantNotesByApplicantId(int applicantId)
		{
			var opResult = new OpResult<IEnumerable<ApplicantNoteDto>>();

			var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
			var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantTrackingEnabled || isSystemAdmin || accessRights.UserId == 0)
			{
                opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantNoteQuery()
                    .ByApplicantId(applicantId)
                    .ByClientId(_session.LoggedInUserInformation?.ClientId ?? 0)
                    .ExecuteQueryAs(x => new ApplicantNoteDto
                    {
                        ApplicantNoteId = x.Remark.RemarkId,
                        ApplicantId = x.ApplicantId,
                        UserId = x.Remark.AddedBy,
                        PostedBy = x.Remark.User.FirstName + " " + x.Remark.User.LastName,
                        Note = x.Remark.Description,
                        DateSubmitted = x.Remark.AddedDate
                    })
                );
            }

			return opResult;
		}

		IOpResult<ApplicantNoteDto> IApplicantTrackingService.GetApplicantNote(int noteId)
		{
			var opResult = new OpResult<ApplicantNoteDto>();

			var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
			var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantTrackingEnabled || isSystemAdmin || accessRights.UserId == 0)
			{

				opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantNoteQuery()
					.ByApplicantNoteId(noteId)
					.ExecuteQueryAs(x => new ApplicantNoteDto
					{
						ApplicantNoteId = x.Remark.RemarkId,
						ApplicantId = x.ApplicantId,
						UserId = x.Remark.AddedBy,
						Note = x.Remark.Description,
						DateSubmitted = x.Remark.AddedDate
					}).FirstOrDefault());
			}

			return opResult;
		}

		IOpResult<ApplicantNoteDto> IApplicantTrackingService.SaveApplicantNote(ApplicantNoteDto dto)
		{
			var opResult = new OpResult<ApplicantNoteDto>();

			_session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			var applicant = _session.UnitOfWork.ApplicantTrackingRepository
								.ApplicantsQuery()
								.ByApplicantId(dto.ApplicantId)
								.ExecuteQueryAs(x => new
								{
									x.ApplicantId,
									x.FirstName,
									x.MiddleInitial,
									x.LastName,
									x.ClientId,
								})
								.FirstOrDefault();

			_session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, applicant.ClientId).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			var user = _session.UnitOfWork.UserRepository
						.QueryUsers()
						.ByUserId(_session.LoggedInUserInformation.UserId)
						.FirstOrDefault();

            // Server date time override for new notes
            if (dto.IsNewEntity(x => x.ApplicantNoteId)) dto.DateSubmitted = DateTime.Now;

            var remark = _remarkProvider.RegisterRemarkChanges(new RemarkDto {
                RemarkId = dto.ApplicantNoteId,
                Description = dto.Note,
                AddedBy = dto.UserId,
                AddedDate = dto.DateSubmitted,
                IsArchived = false,
                IsSystemGenerated = false,
                Modified= DateTime.Now,
                ModifiedBy = dto.UserId
            })
            .MergeInto(opResult)
            .Data;

            var applicantNote = new ApplicantNote();
            if (dto.IsNewEntity(x => x.ApplicantNoteId))
            {
                applicantNote = new ApplicantNote()
                {
                    ApplicantId = dto.ApplicantId,
                    Remark = remark
                };
                _session.UnitOfWork.RegisterNew(applicantNote);
                _session.UnitOfWork.RegisterPostCommitAction(() => dto.ApplicantNoteId = remark.RemarkId);
                dto.PostedBy = user.FirstName + " " + user.LastName;
            }
			

			_session.UnitOfWork.Commit().MergeInto(opResult);

            if (opResult.Success && dto.PostingId.HasValue)
            {
                var detail = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantApplicationHeaderQuery()
                    .ByPostingId(dto.PostingId.Value)
                    .ByApplicantId(dto.ApplicantId)
                    .ExecuteQueryAs(x => new ApplicantDetailDto()
                    {
                        Posting = x.ApplicantPosting.Description,
                        PostingId = dto.PostingId.Value,
                        ApplicantName = x.Applicant.FirstName + " " + x.Applicant.LastName,
                        ApplicantId = x.ApplicantId,
                    })
                    .FirstOrDefault();
                var applicantPostingOwners = _session.UnitOfWork.ApplicantTrackingRepository
                                                .ApplicantPostingOwnerQuery()
                                                .ByPostingId(dto.PostingId.Value)
                                                .ExecuteQueryAs(x=> new ApplicantPostingOwnerDto()
                                                {
                                                    UserId = x.UserId,
                                                    PostingId = dto.PostingId.Value,
                                                    Name = x.User.FirstName + " " + x.User.LastName,
                                                })
                                                .Where(y => y.UserId != dto.UserId)
                                                .ToList();

                // Send a notification saying a note has been added for an applicant for a posting
                if (applicantPostingOwners.Count > 0)
                    _applicantTrackingNotificationService.ProcessPostingOwnerApplicantNoteNotification(applicantPostingOwners, detail);
            }
			opResult.SetDataOnSuccess(dto);
			return opResult;
		}

		IOpResult IApplicantTrackingService.DeleteApplicantNote(int applicantNoteId)
		{
			var opResult = new OpResult();

			_session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			var applicantNoteToDelete = _session.UnitOfWork.ApplicantTrackingRepository
										.ApplicantNoteQuery()
										.ByApplicantNoteId(applicantNoteId)
										.FirstOrDefault();

			var applicant = _session.UnitOfWork.ApplicantTrackingRepository
								.ApplicantsQuery()
								.ByApplicantId(applicantNoteToDelete.ApplicantId)
								.ExecuteQueryAs(x => new
								{
									x.ApplicantId,
									x.FirstName,
									x.MiddleInitial,
									x.LastName,
									x.ClientId,
								})
								.FirstOrDefault();

			_session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, applicant.ClientId).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;
            _session.UnitOfWork.RegisterDeleted(applicantNoteToDelete.Remark);
            _session.UnitOfWork.RegisterDeleted(applicantNoteToDelete);

            _session.UnitOfWork.Commit().MergeInto(opResult);

			return opResult;
		}

		#endregion

		#region ApplicantQuestionDdlItem

		IOpResult<ApplicantQuestionDdlItemDto> IApplicantTrackingService.GetApplicantQuestionDdlItem(int applicantQuestionDdlItemId)
		{
			var opResult = new OpResult<ApplicantQuestionDdlItemDto>();
			var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
			var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantTrackingEnabled || isSystemAdmin || accessRights.UserId == 0)
			{

				opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantQuestionDdlItemQuery()
					.ByApplicantQuestionDdlItemId(applicantQuestionDdlItemId)
					.ExecuteQueryAs(x => new ApplicantQuestionDdlItemDto
					{
						ApplicantQuestionDdlItemId = x.ApplicantQuestionDdlItemId,
						QuestionId = x.QuestionId,
						Description = x.Description,
						IsDefault = x.IsDefault,
						Value = x.Value,
						FlaggedResponse = x.FlaggedResponse,
						IsEnabled = x.IsEnabled
					}).FirstOrDefault());
			}

			return opResult;
		}

		IOpResult<IEnumerable<ApplicantQuestionDdlItemDto>> IApplicantTrackingService.GetApplicantQuestionDdlItems(int questionId)
		{
			var opResult = new OpResult<IEnumerable<ApplicantQuestionDdlItemDto>>();
			var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
			var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantTrackingEnabled || isSystemAdmin || accessRights.UserId == 0)
			{

				opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantQuestionDdlItemQuery()
					.ByQuestionId(questionId)
					.ByIsActive(true)
					.OrderByValue()
					.ExecuteQueryAs(x => new ApplicantQuestionDdlItemDto
					{
						ApplicantQuestionDdlItemId = x.ApplicantQuestionDdlItemId,
						Description = x.Description
					}).ToList());
			}

			return opResult;
		}

		IOpResult<ApplicantQuestionDdlItemDto> IApplicantTrackingService.SaveApplicantQuestionDdlItem(ApplicantQuestionDdlItemDto dto)
		{
			var opResult = new OpResult<ApplicantQuestionDdlItemDto>();

			_session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			var questionControl = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantQuestionControlQuery()
					.ByQuestionId(dto.QuestionId)
					.ExecuteQueryAs(x => new ApplicantQuestionControlDto
					{
						QuestionId = x.QuestionId,
						ClientId = x.ClientId
					}).FirstOrDefault();

			opResult.CheckForNotFound(questionControl);

			if (opResult.HasError)
				return opResult;

			_session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, questionControl.ClientId).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;


			var applicantQuestionDdlItem = new ApplicantQuestionDdlItem();

			if (dto.IsNewEntity(x => x.ApplicantQuestionDdlItemId))
			{
				List<string> valueList = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantQuestionDdlItemQuery().ExecuteQueryAs(x => x.Value).ToList();
				int maxValue = valueList.Select(x => Convert.ToInt32(x)).DefaultIfEmpty(0).Max() + 1;
				var newApplicantQuestionDdlItem = new ApplicantQuestionDdlItem()
				{
					QuestionId = dto.QuestionId,
					Description = dto.Description,
					IsDefault = false,
					Value = maxValue.ToString(),
					FlaggedResponse = dto.FlaggedResponse,
					IsEnabled = true
				};
				_session.UnitOfWork.RegisterNew(newApplicantQuestionDdlItem);
				_session.UnitOfWork.RegisterPostCommitAction(() => dto.ApplicantQuestionDdlItemId = newApplicantQuestionDdlItem.ApplicantQuestionDdlItemId);
			}
			else
			{
				var editApplicantQuestionDdlItem = _session.UnitOfWork.ApplicantTrackingRepository
											.ApplicantQuestionDdlItemQuery()
											.ByApplicantQuestionDdlItemId(dto.ApplicantQuestionDdlItemId)
											.FirstOrDefault();
				editApplicantQuestionDdlItem.Description = dto.Description;

				_session.UnitOfWork.RegisterModified(editApplicantQuestionDdlItem);
			}
			_session.UnitOfWork.Commit().MergeInto(opResult);
			opResult.SetDataOnSuccess(dto);

			return opResult;
		}

		IOpResult IApplicantTrackingService.DeleteApplicantQuestionDdlItem(int applicantQuestionDdlItemId)
		{
			var opResult = new OpResult();
			_session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;


			var applicantQuestionDdlItemToDelete = _session.UnitOfWork.ApplicantTrackingRepository
											.ApplicantQuestionDdlItemQuery()
											.ByApplicantQuestionDdlItemId(applicantQuestionDdlItemId)
											.FirstOrDefault();

			opResult.CheckForNotFound(applicantQuestionDdlItemToDelete);

			if (opResult.HasError)
				return opResult;

			_session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, applicantQuestionDdlItemToDelete.ApplicantQuestionControl.ClientId).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			applicantQuestionDdlItemToDelete.IsEnabled = false;
			_session.UnitOfWork.RegisterModified(applicantQuestionDdlItemToDelete);
			_session.UnitOfWork.Commit().MergeInto(opResult);
			return opResult;
		}

		#endregion

		#region ApplicantResume

		IOpResult<ApplicantResumeDto> IApplicantTrackingService.GetApplicantResume(int applicantResumeId)
		{
			var opResult = new OpResult<ApplicantResumeDto>();
			var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
			var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantTrackingEnabled || isSystemAdmin || accessRights.UserId == 0)
			{

				opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantResumeQuery()
					.ByApplicantResumeId(applicantResumeId)
					.ExecuteQueryAs(x => new ApplicantResumeDto
					{
						ApplicantResumeId = x.ApplicantResumeId,
						ApplicantId = x.ApplicantId,
						LinkLocation = x.LinkLocation,
						DateAdded = x.DateAdded
					}).FirstOrDefault());
			}

			return opResult;
		}

		IOpResult<ApplicantResumeDto> IApplicantTrackingService.UpdateApplicantResume(ApplicantResumeDto dto)
		{
			var opResult = new OpResult<ApplicantResumeDto>();
			var isApplicantAdmin = _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).Success;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantAdmin || isSystemAdmin)
			{
				var applicantResume = new ApplicantResume();
				int applicantResumeId = dto.ApplicantResumeId;
				if (applicantResumeId == 0)
				{
					applicantResume = new ApplicantResume()
					{
						ApplicantId = dto.ApplicantId,
						LinkLocation = dto.LinkLocation,
						DateAdded = dto.DateAdded
					};
					_session.UnitOfWork.RegisterNew(applicantResume);
				}
				else
				{
					applicantResume = _session.UnitOfWork.ApplicantTrackingRepository
												.ApplicantResumeQuery()
												.ByApplicantResumeId(dto.ApplicantResumeId)
												.FirstOrDefault();

					applicantResume.ApplicantResumeId = dto.ApplicantResumeId;
					applicantResume.ApplicantId = dto.ApplicantId;
					applicantResume.LinkLocation = dto.LinkLocation;
					applicantResume.DateAdded = dto.DateAdded;

					_session.UnitOfWork.RegisterModified(applicantResume);
				}
				_session.UnitOfWork.Commit().MergeInto(opResult);

				if (dto.ApplicantResumeId == 0)
				{
					dto.ApplicantResumeId = applicantResume.ApplicantResumeId;
				}

				opResult.Data = dto;
			}

			return opResult;
		}

		IOpResult<IEnumerable<ApplicantResumeDto>> IApplicantTrackingService.DeleteApplicantResume(int applicantResumeId, int applicantId)
		{
			var opResult = new OpResult<IEnumerable<ApplicantResumeDto>>();
			var isApplicantAdmin = _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).Success;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantAdmin || isSystemAdmin)
			{
				var resumeToDelete = _session.UnitOfWork.ApplicantTrackingRepository
											.ApplicantResumeQuery()
											.ByApplicantResumeId(applicantResumeId)
											.FirstOrDefault();

				_session.UnitOfWork.RegisterDeleted(resumeToDelete);
				_session.UnitOfWork.Commit().MergeInto(opResult);

				var applicantResumes = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantResumeQuery()
					.ByApplicantId(applicantId)
					.ExecuteQueryAs(x => new ApplicantResumeDto
					{
						ApplicantResumeId = x.ApplicantResumeId,
						ApplicantId = x.ApplicantId,
						LinkLocation = x.LinkLocation,
						DateAdded = x.DateAdded
					}).ToList();
				opResult.Data = applicantResumes;
			}
			return opResult;
		}

        OpResult<ResourceDto> IApplicantTrackingService.SaveApplicantResource( int applicantId, string fileName, byte[] fileData)
        {
            var result = new OpResult<ResourceDto>();
            var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
            var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
            var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
            if (!(isApplicantTrackingEnabled || isSystemAdmin))
                return result.SetToFail(() => new GenericMsg("No permissions to perform save."));

            string resourceLocation = string.Empty;
            ResourceSourceType resourceType = ResourceSourceType.AzureClientFile;
            int clientId = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantsQuery().ByApplicantId(applicantId)
                .FirstOrDefault().ClientId;

            if (!string.IsNullOrEmpty(ConfigValues.AzureFile))
            {
                resourceLocation = _azureResourceProvider.CreateOrUpdateApplicantAzureFile(clientId, applicantId, fileName, fileData,
                    ConfigValues.AzureFile, true).Data;
                if (string.IsNullOrEmpty(resourceLocation))
                {
                    result.AddMessage(new GenericMsg("Azure file failed to create."));
                    result.SetToFail();
                    return result;
                }
            }
            else
            {
                string filePath = string.Format(@"C:\Upload\{0}\ApplicantResources\{1}\{2}", clientId, applicantId, fileName);
                var fileInfo = new FileInfo(filePath);
                fileInfo.Directory.Create();
                File.WriteAllBytes(filePath, fileData);
                resourceLocation = filePath;
                resourceType = ResourceSourceType.LocalServerFile;
            }

            int azureAccountEnum = _azureResourceProvider.GetAzureAccountNum(AzureDirectoryType.ApplicantFile);

            //create resource and register it
            var resource = new Resource
            {
                ClientId = clientId,
                EmployeeId = null,
                Name = fileName.Trim(),
                SourceType = resourceType,
                Source = resourceLocation,
                AddedById = _session.LoggedInUserInformation.UserId,
                AddedDate = DateTime.Now,
                AzureAccount = azureAccountEnum,
                Modified = DateTime.Now,
                ModifiedBy = _session.LoggedInUserInformation.UserId,
            };
            var resourceDto = new ResourceDto()
            {
                ClientId = resource.ClientId,
                EmployeeId = resource.EmployeeId,
                Name = resource.Name,
                Source = resource.Source,
                SourceTypeId = resource.SourceType,
                AddedDate = resource.AddedDate,
                AddedBy = resource.AddedById,
                Modified = resource.Modified,
                ModifiedBy = resource.ModifiedBy,
            };
            _session.UnitOfWork.RegisterNew(resource);
            _session.UnitOfWork.RegisterPostCommitAction(() => resourceDto.ResourceId = resource.ResourceId);
            _session.UnitOfWork.Commit().MergeInto(result);

            result.SetDataOnSuccess(resourceDto);
            return result;
        }

        IOpResult<FileStreamDto> IApplicantTrackingService.GetApplicantResource(int resourceId)
        {
            var result = new OpResult<FileStreamDto>();
            var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
            var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
            var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
            if(!(isApplicantTrackingEnabled || isSystemAdmin ))
                return result.SetToFail(() => new GenericMsg("No permissions to access the resource."));
                
            ResourceDto dto = (_session.UnitOfWork.CoreRepository
                .QueryResources()
                .ByResource(resourceId)
                .ExecuteQueryAs(r => new ResourceDto
                {
                    ResourceId = r.ResourceId,
                    Name = r.Name,
                    Source = r.Source,
                    SourceTypeId = r.SourceType,
                    EmployeeId = null,
                    AddedDate = r.AddedDate,
                    AddedBy = r.AddedById,
                })
                .FirstOrDefault());
            if (dto == null)
            {
                return result.SetToFail(() => new GenericMsg("Unable to find specified resource."));
            }

            if (dto.Source.Contains(@"C:\"))
            {
                var fileName = Path.GetFileName(dto.Source);
                string ext = Path.GetExtension(dto.Source);
                string mimeType = MimeTypeMap.GetMimeType(ext);
                if (File.Exists(dto.Source))
                {
                    byte[] fb = File.ReadAllBytes(dto.Source);

                    var fileStreamDto = new FileStreamDto
                    {
                        FileExtension = ext,
                        FileName = fileName,
                        FileStream = new MemoryStream(fb),
                        MimeType = mimeType
                    };
                    result.Data = fileStreamDto;
                }
                else
                {
                    return result.SetToFail(() => new GenericMsg("Unable to find specified resource."));
                }
            }
            else
            {
                _azureResourceProvider.GetBlob(dto.Source, ConfigValues.AzureFile).MergeAll(result);
            }

            return result;
        }

        IOpResult<ResourceDto> IApplicantTrackingService.GetApplicantResourceInfo(int resourceId)
        {
            var result = new OpResult<ResourceDto>();
            result.Data = (_session.UnitOfWork.CoreRepository
                .QueryResources()
                .ByResource(resourceId)
                .ExecuteQueryAs(r => new ResourceDto
                {
                    ResourceId = r.ResourceId,
                    Name = r.Name,
                    Source = r.Source,
                    SourceTypeId = r.SourceType,
                    EmployeeId = (r.Employee != null) ? r.Employee.EmployeeId : 0
                })
                .FirstOrDefault());

            if (result.Data == null) result.SetToFail("Unable to find the resource");
            return result;
        }
		#endregion


        #region ApplicantDocument

        IOpResult<ApplicantDocumentDto> IApplicantTrackingService.GetApplicantDocument(int applicantDocumentId)
        {
            var opResult = new OpResult<ApplicantDocumentDto>();

            if (applicantDocumentId > 0)
            {
                _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
                if (opResult.HasError)
                    return opResult;

                var clientId = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantDocumentQuery()
                        .ByApplicantDocumentId(applicantDocumentId)
                        .ExecuteQueryAs(x => new
                        {
                            ClientId = x.ApplicantApplicationHeader.Applicant.ClientId
                        }).FirstOrDefault().ClientId;

                _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(opResult);
                if (opResult.HasError)
                    return opResult;

                opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantDocumentQuery()
                    .ByApplicantDocumentId(applicantDocumentId)
                    .ExecuteQueryAs(x => new ApplicantDocumentDto
                    {
                        ApplicantDocumentId = x.ApplicantDocumentId,
                        DocumentName = x.DocumentName,
                        ApplicationHeaderId = x.ApplicationHeaderId,
                        LinkLocation = x.LinkLocation,
                        DateAdded = x.DateAdded
                    }).FirstOrDefault());

            }
            return opResult;
        }

        IOpResult<IEnumerable<ApplicantDocumentDto>> IApplicantTrackingService.GetApplicantDocumentsByApplicationHeader(int applicationHeaderId)
        {
            var opResult = new OpResult<IEnumerable<ApplicantDocumentDto>>();

            _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
            if (opResult.HasError)
                return opResult;

            var clientId = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantApplicationHeaderQuery()
                    .ByApplicationHeaderId(applicationHeaderId)
                    .ExecuteQueryAs(x => new 
                    {
                        ClientId = x.Applicant.ClientId
                    }).FirstOrDefault().ClientId;

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(opResult);
            if (opResult.HasError)
                return opResult;

            opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantDocumentQuery()
                .ByApplicationHeaderId(applicationHeaderId)
                .ExecuteQueryAs(x => new ApplicantDocumentDto
                {
                    ApplicantDocumentId = x.ApplicantDocumentId,
                    DocumentName = x.DocumentName,
                    ApplicationHeaderId = x.ApplicationHeaderId,
                    LinkLocation = x.LinkLocation,
                    DateAdded = x.DateAdded
                }).ToList());


            return opResult;
        }

        IOpResult<ApplicantDocumentDto> IApplicantTrackingService.SaveApplicantDocumentData(ApplicantDocumentDto dto)
        {
            var result = new OpResult<ApplicantDocumentDto>();

            // check action types

            var entity = new ApplicantDocument
            {
                ApplicantDocumentId = dto.ApplicantDocumentId,
                DocumentName = dto.DocumentName,
                ApplicationHeaderId = dto.ApplicationHeaderId,
                LinkLocation = dto.LinkLocation,
                DateAdded = dto.DateAdded
            };

            _session.UnitOfWork.RegisterNewOrModified(entity, e => e.ApplicantDocumentId == 0);
            _session.UnitOfWork.Commit().MergeInto(result);

            if (result.HasError) return result;

            result.TrySetData(() => new ApplicantDocumentDto
            {
                ApplicantDocumentId = entity.ApplicantDocumentId,
                DocumentName = entity.DocumentName,
                ApplicationHeaderId = entity.ApplicationHeaderId,
                LinkLocation = entity.LinkLocation,
                DateAdded = entity.DateAdded
            });

            return result;
        }

        public async Task<OpResult<ApplicantDocumentDto>> SaveApplicantDocument(int applicationHeaderId, string documentName, int? applicantDocumentId, HttpRequestMessage request)
        {
            var result = new OpResult<ApplicantDocumentDto>();
            var existingDocument = Self.GetApplicantDocument(applicantDocumentId ?? 0).MergeInto(result).Data;

            var applicationHeader = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantApplicationHeaderQuery()
                    .ByApplicationHeaderId(applicationHeaderId)
                    .ExecuteQueryAs(x => new
                    {
                        ClientId = x.Applicant.ClientId,
                        ApplicantId = x.ApplicantId,
                        EmployeeId = x.Applicant.EmployeeId
                    }).FirstOrDefault();

            if (!request.Content.IsMimeMultipartContent("form-data"))
            {
                result.AddMessage(new GenericMsg("Unsupported request."));
                result.SetToFail();
            }
            else
            {
                try
                {
                    var provider = new MultipartMemoryStreamProvider();
                    await request.Content.ReadAsMultipartAsync(provider);

                    if (provider.Contents.Count > 0)
                    {
                        byte[] fileData = await provider.Contents.GetByteArrayContentByNameAsync("file");
                        string filename = await provider.Contents.GetStringObjectByName("fileName");

                        if (existingDocument != null && existingDocument.ApplicantDocumentId != CommonConstants.NEW_ENTITY_ID && existingDocument.LinkLocation.Contains(@"C:\"))
                        {
                            DsIo.DeleteLocalFile(existingDocument.LinkLocation);
                        }

                        if (!string.IsNullOrEmpty(ConfigValues.AzureFile))
                            SaveApplicantDocumentToAzure(applicationHeader.ClientId, applicationHeader.ApplicantId, applicationHeader.EmployeeId, applicationHeaderId, applicantDocumentId, documentName, filename, fileData).MergeAll(result);
                        else
                            WriteApplicantDocumentToApplicantDocuments(applicationHeader.ApplicantId, applicationHeaderId, filename, result, string.Format(@"C:\Upload\{0}\ApplicantDocuments", applicationHeader.ClientId),
                                null, Convert.ToBase64String(fileData)).MergeAll(result);
                    }
                    else
                    {
                        result.AddMessage(new GenericMsg("Invalid Data"));
                        result.SetToFail();
                    }

                }
                catch (Exception e)
                {
                    result.AddExceptionMessage(e);
                }
            }

            return result;
        }

        public OpResult<ApplicantDocumentDto> SaveApplicantDocumentToAzure(int clientId, int applicantId, int? employeeId, int applicationHeaderId, int? applicantDocumentId, string documentName, string fileName, byte[] fileData)
        {
            var result = new OpResult<ApplicantDocumentDto>();

            var applicantDocumentResource = _azureResourceProvider.CreateOrUpdateApplicantAzureFile(clientId, applicantId, fileName, fileData,
                ConfigValues.AzureFile, true).Data;
            if (string.IsNullOrEmpty(applicantDocumentResource))
            {
                result.AddMessage(new GenericMsg("Azure file failed to create."));
                result.SetToFail();
                return result;
            }

            ApplicantDocument applicantDocument = new ApplicantDocument()
            {
                ApplicantDocumentId = applicantDocumentId ?? 0,
                DocumentName = documentName,
                ApplicationHeaderId = applicationHeaderId,
                DateAdded = DateTime.Now,
                LinkLocation = applicantDocumentResource,
            };
            _session.UnitOfWork.RegisterNewOrModified(applicantDocument, e => e.ApplicantDocumentId == 0);
            _session.UnitOfWork.Commit().MergeInto(result);

            if (result.Success && employeeId != null && employeeId > 0)
            {
                var folder = result.TryGetData(() => _session.UnitOfWork.CoreRepository
                    .QueryEmployeeAttachmentFolders()
                    .ByClientId(includeGenericFolders: false, clientId: clientId)
                    .ByIsDefaultATFolder()
                    .FirstOrDefault());

                if (folder == null)
                {
                    folder = new EmployeeAttachmentFolder
                    {
                        Description = "Applicant Tracking",
                        ClientId = clientId,
                        DefaultATFolder = true,
                        IsEmployeeView = true,
                        IsAdminViewOnly = false,
                    };

                    _session.SetModifiedProperties(folder);
                    _session.UnitOfWork.RegisterNew(folder);

                    _session.UnitOfWork.Commit().MergeInto(result);
                }

                //
                var resource = new Resource()
                {
                    ClientId = _session.LoggedInUserInformation.ClientId,
                    EmployeeId = employeeId,
                    Name = applicantDocument.DocumentName,
                    SourceType = ResourceSourceType.LocalServerFile,
                    Source = applicantDocument.LinkLocation,
                    AddedDate = applicantDocument.DateAdded,
                    IsDeleted = false,
                    ModifiedBy = _session.LoggedInUserInformation.UserId,
                    Modified = DateTime.Now,
                    AddedById = _session.LoggedInUserInformation.UserId,
                    AzureAccount = 1
                };
                _session.UnitOfWork.RegisterNew(resource);
                _session.UnitOfWork.Commit();

                var newAttachment = new EmployeeAttachment()
                {
                    EmployeeFolderId = folder.EmployeeFolderId,
                    IsEmployeeView = true,
                    ResourceId = resource.ResourceId
                };
                _session.UnitOfWork.RegisterNew(newAttachment);
                _session.UnitOfWork.Commit();
            }

            result.SetDataOnSuccess(new ApplicantDocumentDto()
            {
                ApplicantDocumentId = applicantDocument.ApplicantDocumentId,
                DocumentName = applicantDocument.DocumentName,
                ApplicationHeaderId = applicationHeaderId,
                DateAdded = applicantDocument.DateAdded,
                LinkLocation = applicantDocumentResource
            });
            return result;
        }

        private IOpResult<ApplicantDocumentDto> WriteApplicantDocumentToApplicantDocuments(int applicantId, int applicationHeaderId, string providedFileName, OpResult<ApplicantDocumentDto> result, string baseFolder, ApplicantDocumentDto existingApplicantDocument, string fileContent = null)
        {


            var file = Path.Combine(baseFolder, applicantId.ToString(),
                Path.GetFileName(providedFileName) ?? "");
            var fileName = file;

            if (File.Exists(file))
            {
                var extension = Path.GetExtension(fileName);

                var i = 0;
                while (File.Exists(fileName))
                {
                    fileName = i == 0 ? fileName.Replace(extension, "_" + ++i + extension) : fileName.Replace("_" + i + extension, "_" + ++i + extension);
                }

                file = fileName;
            }

            var fileInfo = new FileInfo(file);
            fileInfo.Directory.Create();
            if (fileContent.IsNotNullOrEmpty())
            {
                File.WriteAllBytes(file, Convert.FromBase64String(fileContent));
            }
            else
            {
                File.Move(providedFileName, file);
            }


            // update existing Applicant Document
            var dto = new ApplicantDocumentDto()
            {
                ApplicantDocumentId = existingApplicantDocument?.ApplicantDocumentId ?? 0,
                ApplicationHeaderId = existingApplicantDocument?.ApplicationHeaderId ?? applicationHeaderId,
                DateAdded = DateTime.Now,
                LinkLocation = file
            };

            return Self.SaveApplicantDocumentData(dto).MergeAll(result);
        }

        IOpResult<FileStreamDto> IApplicantTrackingService.GetApplicantDocumentPreview(int applicantDocumentId)
        {
            var result = new OpResult<FileStreamDto>();
            ApplicantDocumentDto dto = (_session.UnitOfWork.ApplicantTrackingRepository
                .ApplicantDocumentQuery()
                .ByApplicantDocumentId(applicantDocumentId)
                .ExecuteQueryAs(r => new ApplicantDocumentDto
                {
                    ApplicantDocumentId = r.ApplicantDocumentId,
                    DocumentName = r.DocumentName,
                    ApplicationHeaderId = r.ApplicationHeaderId,
                    DateAdded = r.DateAdded,
                    LinkLocation = r.LinkLocation
                })
                .FirstOrDefault());
            if (dto == null)
            {
                return result.SetToFail(() => new GenericMsg("Unable to find specified document."));
            }

            if (dto.LinkLocation.Contains(@"C:\"))
            {
                var fileName = Path.GetFileName(dto.LinkLocation);
                string ext = Path.GetExtension(dto.LinkLocation);
                string mimeType = MimeTypeMap.GetMimeType(ext);
                if (File.Exists(dto.LinkLocation))
                {
                    var fs = File.Open(dto.LinkLocation, FileMode.Open, FileAccess.Read);
                    Stream fileStream = fs;

                    var fileStreamDto = new FileStreamDto
                    {
                        FileExtension = ext,
                        FileName = fileName,
                        FileStream = fileStream,
                        MimeType = mimeType
                    };
                    result.Data = fileStreamDto;
                }
                else
                {
                    return result.SetToFail(() => new GenericMsg("Unable to find specified document."));
                }
            }
            else
            {
                _azureResourceProvider.GetBlob(dto.LinkLocation, ConfigValues.AzureFile).MergeAll(result);
            }

            return result;
        }

        #endregion

        #region ApplicationQuestionSection

		IOpResult<IEnumerable<ApplicationQuestionSectionDto>> IApplicantTrackingService.GetApplicantQuestionSections(int clientId)
		{
			var opResult = new OpResult<IEnumerable<ApplicationQuestionSectionDto>>();

            _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
            if (opResult.HasError)
                return opResult;

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(opResult);
            if (opResult.HasError)
                return opResult;

			opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicationQuestionSectionQuery()
				.ByClientIdWithDefaultClient(clientId)
				.ByIsActive(true)
				.OrderByDisplayOrder()
				.ExecuteQueryAs(x => new ApplicationQuestionSectionDto
				{
					SectionId = x.SectionId,
					Description = x.Description,
                    Instruction = x.ApplicationSectionInstruction.Where(y => y.ClientId == clientId).Select(z => z.Instruction).FirstOrDefault(),
                    DisplayOrder = x.DisplayOrder,
					ClientId = x.ClientId,
					IsEnabled = x.IsEnabled
				}).ToList());

			return opResult;
		}

		IOpResult<ApplicationQuestionSectionDto> IApplicantTrackingService.GetApplicationQuestionSection(int applicationQuestionSectionId, int clientId)
		{
			var opResult = new OpResult<ApplicationQuestionSectionDto>();

            _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
            if (opResult.HasError)
                return opResult;

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(opResult);
            if (opResult.HasError)
                return opResult;

			opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicationQuestionSectionQuery()
				.BySectionId(applicationQuestionSectionId)
				.ExecuteQueryAs(x => new ApplicationQuestionSectionDto
				{
					SectionId = x.SectionId,
					Description = x.Description,
                    Instruction = x.ApplicationSectionInstruction.Where(y => y.ClientId == clientId).Select(z => z.Instruction).FirstOrDefault(),
                    DisplayOrder = x.DisplayOrder,
					ClientId = x.ClientId,
					IsEnabled = x.IsEnabled
				}).FirstOrDefault());

			return opResult;
		}

		IOpResult<ApplicationQuestionSectionDto> IApplicantTrackingService.UpdateApplicationQuestionSection(ApplicationQuestionSectionDto dto)
		{
			var opResult = new OpResult<ApplicationQuestionSectionDto>();

			_session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			_session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, dto.ClientId).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			if (dto.IsNewEntity(x => x.SectionId))
			{
				int maxValue = _session.UnitOfWork.ApplicantTrackingRepository
									.ApplicationQuestionSectionQuery()
									.ExecuteQuery()
									.Select(x => x.DisplayOrder).DefaultIfEmpty(0).Max() + 1;
				var applicationSection = new ApplicationQuestionSection()
				{
					Description = dto.Description,
					DisplayOrder = maxValue,
					ClientId = dto.ClientId,
					IsEnabled = dto.IsEnabled
				};
				dto.DisplayOrder = maxValue;
				_session.UnitOfWork.RegisterNew(applicationSection);
				_session.UnitOfWork.RegisterPostCommitAction(() => dto.SectionId = applicationSection.SectionId);
			}
			else
			{
				var editSection = _session.UnitOfWork.ApplicantTrackingRepository
											.ApplicationQuestionSectionQuery()
											.BySectionId(dto.SectionId)
											.FirstOrDefault();

				editSection.Description = dto.Description;

				_session.UnitOfWork.RegisterModified(editSection);

            }
            _session.UnitOfWork.Commit().MergeInto(opResult);

            //Section Instruction
            if (opResult.Success)
            {
                var sectionInstruction = _session.UnitOfWork
                    .ApplicantTrackingRepository
                    .ApplicationSectionInstructionQuery()
                    .ByClientIdAndSectionId(dto.ClientId, dto.SectionId)
                    .FirstOrDefault();

                if (sectionInstruction != null)
                {
                    sectionInstruction.Instruction = dto.Instruction;
                    _session.UnitOfWork.RegisterModified(sectionInstruction);
                }
                else
                {
                    if (dto.Instruction != null && dto.Instruction.Trim() != "")
                    {
                        var newSectionInstruction = new ApplicationSectionInstruction()
                        {
                            ClientId = dto.ClientId,
                            SectionId = dto.SectionId,
                            Instruction = dto.Instruction
                        };
                        _session.UnitOfWork.RegisterNew(newSectionInstruction);
                    }
                }
                _session.UnitOfWork.Commit().MergeInto(opResult);
            }

            opResult.SetDataOnSuccess(dto);

			return opResult;
		}

		IOpResult<ApplicationQuestionSectionDto> IApplicantTrackingService.DeleteApplicationQuestionSection(int sectionId)
		{
			var opResult = new OpResult<ApplicationQuestionSectionDto>();

			_session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			var sectionExists = _session.UnitOfWork.ApplicantTrackingRepository
										.ApplicantQuestionControlQuery()
										.BySectionId(sectionId)
										.ByIsActive(true)
										.ExecuteQuery()
										.Any();
			if (!sectionExists)
			{
				var sectionToDelete = _session.UnitOfWork.ApplicantTrackingRepository
											.ApplicationQuestionSectionQuery()
											.BySectionId(sectionId)
											.FirstOrDefault();

				var clientId = sectionToDelete.ClientId;
				_session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(opResult);
				if (opResult.HasError)
					return opResult;

				sectionToDelete.IsEnabled = false;
				_session.UnitOfWork.RegisterModified(sectionToDelete);

                var sectionInstructionToDelete = _session.UnitOfWork.ApplicantTrackingRepository
                                            .ApplicationSectionInstructionQuery()
                                            .ByClientIdAndSectionId(sectionToDelete.ClientId, sectionToDelete.SectionId)
                                            .FirstOrDefault();
                if (sectionInstructionToDelete != null)
                    _session.UnitOfWork.RegisterDeleted(sectionInstructionToDelete);

                _session.UnitOfWork.Commit().MergeInto(opResult);
				opResult.SetDataOnSuccess(new ApplicationQuestionSectionDto() { SectionId = sectionId });
				if (opResult.Success)
				{
					opResult.Data.IsAlreadyUsed = false;
				}
			}
			else
			{
				opResult.SetDataOnSuccess(new ApplicationQuestionSectionDto() { SectionId = sectionId });
				opResult.Data.IsAlreadyUsed = true;
				//opResult.AddExceptionMessage(new Exception("Section cannot be deleted. It is currently assigned to one or more questions."));
				//opResult.Success = false;
			}
			return opResult;
		}

		IOpResult IApplicantTrackingService.UpdateSectionSequence(List<ApplicationQuestionSectionDto> dto)
		{
			var opResult = new OpResult();
			_session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			foreach (ApplicationQuestionSectionDto aqd in dto)
			{
				var existingRec = _session.UnitOfWork.ApplicantTrackingRepository
											.ApplicationQuestionSectionQuery()
											.BySectionId(aqd.SectionId)
											.FirstOrDefault();

				var clientId = existingRec.ClientId;
				_session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(opResult);
				if (opResult.HasError)
					return opResult;

				existingRec.DisplayOrder = aqd.DisplayOrder;
				_session.UnitOfWork.RegisterModified(existingRec);
			}
			_session.UnitOfWork.Commit().MergeInto(opResult);
			return opResult;
		}

        #endregion

        #region ApplicationSectionInstruction

        IOpResult<ApplicationSectionInstructionDto> IApplicantTrackingService.GetApplicationSectionInstruction(int sectionInstructionId)
        {
            var opResult = new OpResult<ApplicationSectionInstructionDto>();

            _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
            if (opResult.HasError)
                return opResult;

            opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicationSectionInstructionQuery()
                .BySectionInstructionId(sectionInstructionId)
                .ExecuteQueryAs(x => new ApplicationSectionInstructionDto
                {
                    SectionInstructionId = x.SectionInstructionId,
                    SectionId = x.SectionId,
                    ClientId = x.ClientId,
                    Instruction = x.Instruction
                }).FirstOrDefault());

            return opResult;
        }

        IOpResult<ApplicationSectionInstructionDto> IApplicantTrackingService.GetApplicationSectionInstructionByClientIdAndSectionId(int clientId, int sectionId)
        {
            var opResult = new OpResult<ApplicationSectionInstructionDto>();

            _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
            if (opResult.HasError)
                return opResult;

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(opResult);
            if (opResult.HasError)
                return opResult;

            opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicationSectionInstructionQuery()
                .ByClientIdAndSectionId(clientId, sectionId)
                .ExecuteQueryAs(x => new ApplicationSectionInstructionDto
                {
                    SectionInstructionId = x.SectionInstructionId,
                    SectionId = x.SectionId,
                    ClientId = x.ClientId,
                    Instruction = x.Instruction
                }).FirstOrDefault());

            return opResult;
        }

        IOpResult<ApplicationSectionInstructionDto> IApplicantTrackingService.UpdateApplicationSectionInstruction(ApplicationSectionInstructionDto dto)
        {
            var opResult = new OpResult<ApplicationSectionInstructionDto>();

            _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
            if (opResult.HasError)
                return opResult;

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, dto.ClientId).MergeInto(opResult);
            if (opResult.HasError)
                return opResult;

            if (dto.IsNewEntity(x => x.SectionInstructionId))
            {
                var applicationSectionInstruction = new ApplicationSectionInstruction()
                {
                    Instruction = dto.Instruction,
                    ClientId = dto.ClientId,
                    SectionId = dto.SectionId
                };
                _session.UnitOfWork.RegisterNew(applicationSectionInstruction);
                _session.UnitOfWork.RegisterPostCommitAction(() => dto.SectionInstructionId = applicationSectionInstruction.SectionInstructionId);
            }
            else
            {
                var editSectionInstruction = _session.UnitOfWork.ApplicantTrackingRepository
                                            .ApplicationSectionInstructionQuery()
                                            .BySectionInstructionId(dto.SectionInstructionId)
                                            .FirstOrDefault();

                editSectionInstruction.Instruction = dto.Instruction;

                _session.UnitOfWork.RegisterModified(editSectionInstruction);
            }
            _session.UnitOfWork.Commit().MergeInto(opResult);

            opResult.SetDataOnSuccess(dto);

            return opResult;
        }

        IOpResult<ApplicationSectionInstructionDto> IApplicantTrackingService.DeleteApplicationSectionInstruction(int sectionInstructionId)
        {
            var opResult = new OpResult<ApplicationSectionInstructionDto>();

            _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
            if (opResult.HasError)
                return opResult;

            var sectionInstructionToDelete = _session.UnitOfWork.ApplicantTrackingRepository
                                        .ApplicationSectionInstructionQuery()
                                        .BySectionInstructionId(sectionInstructionId)
                                        .FirstOrDefault();

            var clientId = sectionInstructionToDelete.ClientId;
            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(opResult);
            if (opResult.HasError)
                return opResult;

            _session.UnitOfWork.RegisterDeleted(sectionInstructionToDelete);
            _session.UnitOfWork.Commit().MergeInto(opResult);
            opResult.SetDataOnSuccess(new ApplicationSectionInstructionDto() { SectionInstructionId = sectionInstructionId });
            return opResult;
        }

        #endregion

        #region ApplicantQuestionControl
        IOpResult<IEnumerable<ApplicantQuestionControlDto>> IApplicantTrackingService.GetApplicantQuestionControls(int? clientId)
		{
			var opResult = new OpResult<IEnumerable<ApplicantQuestionControlDto>>();
			if (!clientId.HasValue)
				clientId = _session.LoggedInUserInformation.ClientId;

			_session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			_session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId.Value).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			IApplicantQuestionControlQuery qcResult = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantQuestionControlQuery()
					.ByClientId(clientId.Value)
					.ByIsActive(true);

			// Retrieve all the questions referred by applicants
			IQueryResult<int> referredQuestions = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantApplicationDetailQuery()
					.JoinApplicantQuestionControlWithDetail(qcResult);

            opResult.TrySetData(() =>
            {
				IEnumerable<int> referredQs = referredQuestions.Execute();

				return qcResult
				.ExecuteQueryAs(x => new ApplicantQuestionControlDto
				{
					QuestionId = x.QuestionId,
					Question = x.Question,
					SectionId = x.SectionId,
                    IsReferred = referredQs.Contains(x.QuestionId),
                    ResponseTitle = x.ResponseTitle,
				}).ToList();
			});

			return opResult;
		}
        
		IOpResult<IEnumerable<ApplicationQuestionDto>> IApplicantTrackingService.GetEEOCApplicantQuestionControls(PostingFilterDto postingFilter)
		{
			var opResult = new OpResult<IEnumerable<ApplicationQuestionDto>>();
			
			_session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			_session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, postingFilter.ClientId.Value).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			opResult.TrySetData(() => {
				IEnumerable<int> applicationIds;

				var applicationQuery = _session.UnitOfWork.ApplicantTrackingRepository
				  .ApplicantPostingsQuery()
				  .ByPublishDateRange(postingFilter.StartDate.Value, postingFilter.EndDate.Value); //<-- common to all queries

				if (postingFilter.PostingId.HasValue && postingFilter.PostingId.Value > 0)
				{
					//add posting ID filter
					applicationQuery.ByPostingId(postingFilter.PostingId.Value);
				}
				else
				{
					//common filter to non-posting-id specific queries
					applicationQuery
					  .ByClientId(postingFilter.ClientId.Value)
					  .ByIsActive(true);

					if (postingFilter.PostingId.Value == -1)
						applicationQuery.ByIsClosed(false);
				}

				//now we can execute the query knowing the appropriate filters have been applied
				applicationIds = applicationQuery.ExecuteQueryAs(x => x.ApplicantCompanyApplication.CompanyApplicationId).Distinct<int>();

				return _session.UnitOfWork.ApplicantTrackingRepository.ApplicantQuestionControlQuery()
					.ByClientId(postingFilter.ClientId.Value)
					.BySectionId((int)ApplicationSectionSystemType.EqualOpportunityEmployer)
					.ByIsActive(true)
					.ByApplication(applicationIds)
					.ExecuteQueryAs(x => new ApplicationQuestionDto
					{
						QuestionId = x.QuestionId,
                        QuestionText = x.Question,
                        ResponseTitle = x.ResponseTitle,
					}).ToList();
			});

			return opResult;
		}
		IOpResult<ApplicantQuestionControlDto> IApplicantTrackingService.GetApplicantQuestionControl(int applicantQuestionControlId)
		{
			var opResult = new OpResult<ApplicantQuestionControlDto>();
			_session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;


			var data = opResult.TryGetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantQuestionControlQuery()
					.ByQuestionId(applicantQuestionControlId)
					.ExecuteQueryAs(x => new ApplicantQuestionControlDto
					{
						QuestionId = x.QuestionId,
						Question = x.Question,
                        ResponseTitle = x.ResponseTitle,
						FieldTypeId = x.FieldTypeId,
						SectionId = x.SectionId,
						ClientId = x.ClientId,
						IsFlagged = x.IsFlagged,
						IsRequired = x.IsRequired,
						FlaggedResponse = x.FlaggedResponse,
						IsEnabled = x.IsEnabled,
                        SelectionCount=x.SelectionCount,
                        ApplicantQuestionDdlItem = x.ApplicantQuestionDdlItem.Select(y => new ApplicantQuestionDdlItemDto { ApplicantQuestionDdlItemId = y.ApplicantQuestionDdlItemId, Description = y.Description, OldDescription=y.Description, QuestionId=y.QuestionId }).ToList()
					}).FirstOrDefault());

			opResult.CheckForNotFound(data);
			if (opResult.HasError)
				return opResult;

			// Check if the question is already being referenced by any applicant
			IEnumerable<int> referredQuestions = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantApplicationDetailQuery()
					.JoinApplicantQuestionControlWithDetail( _session.UnitOfWork.ApplicantTrackingRepository.ApplicantQuestionControlQuery()
					.ByQuestionId(applicantQuestionControlId) ).Execute();

			if (referredQuestions.Count()>0)
				data.IsReferred = true;

			_session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, data.ClientId).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			opResult.SetDataOnSuccess(data);

			return opResult;
		}

		IOpResult<ApplicantQuestionControlDto> IApplicantTrackingService.SaveApplicantQuestionControl(ApplicantQuestionControlDto dto)
		{
			var opResult = new OpResult<ApplicantQuestionControlDto>();
			_session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			_session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, dto.ClientId).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;


			if (dto.IsNewEntity(x => x.QuestionId))
			{
				var newApplicantQuestion = new ApplicantQuestionControl()
				{
					Question = dto.Question,
                    ResponseTitle = dto.ResponseTitle,
					FieldTypeId = dto.FieldTypeId,
					SectionId = dto.SectionId,
					ClientId = dto.ClientId,
					IsFlagged = dto.IsFlagged,
					IsRequired = dto.IsRequired,
					FlaggedResponse = dto.FlaggedResponse,
					IsEnabled = dto.IsEnabled,
                    SelectionCount=dto.SelectionCount
				};
				_session.SetModifiedProperties(newApplicantQuestion);
				_session.UnitOfWork.RegisterNew(newApplicantQuestion);
				_session.UnitOfWork.RegisterPostCommitAction(() => dto.QuestionId = newApplicantQuestion.QuestionId);
			}
			else
			{
				var editapplicantQuestion = _session.UnitOfWork.ApplicantTrackingRepository
											.ApplicantQuestionControlQuery()
											.ByQuestionId(dto.QuestionId)
											.FirstOrDefault();

				editapplicantQuestion.Question = dto.Question;
                editapplicantQuestion.ResponseTitle = dto.ResponseTitle;
				editapplicantQuestion.FieldTypeId = dto.FieldTypeId;
				editapplicantQuestion.ClientId = dto.ClientId;
				editapplicantQuestion.IsEnabled = dto.IsEnabled;
				editapplicantQuestion.SectionId = dto.SectionId;
				editapplicantQuestion.IsFlagged = dto.IsFlagged;
				editapplicantQuestion.FlaggedResponse = dto.FlaggedResponse;
				editapplicantQuestion.IsRequired = dto.IsRequired;
                editapplicantQuestion.SelectionCount = dto.SelectionCount;
                _session.SetModifiedProperties(editapplicantQuestion);
				_session.UnitOfWork.RegisterModified(editapplicantQuestion);
			}
			_session.UnitOfWork.Commit().MergeInto(opResult);
			opResult.SetDataOnSuccess(dto);

			return opResult;
		}

		IOpResult<ApplicantQuestionControlDto> IApplicantTrackingService.SaveApplicantQuestionControlWithListItem(ApplicantQuestionControlDto dto)
		{
			var opResult = new OpResult<ApplicantQuestionControlDto>();

			_session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			_session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, dto.ClientId).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			var applicantQuestion = new ApplicantQuestionControl();
			//int questionId = dto.QuestionId;
			if (dto.QuestionId == 0)
			{
				applicantQuestion = new ApplicantQuestionControl()
				{
					Question = dto.Question,
                    ResponseTitle = dto.ResponseTitle,
					FieldTypeId = dto.FieldTypeId,
					SectionId = dto.SectionId,
					ClientId = dto.ClientId,
					IsFlagged = dto.IsFlagged,
					IsRequired = dto.IsRequired,
					FlaggedResponse = dto.FlaggedResponse,
					IsEnabled = dto.IsEnabled,
                    SelectionCount=dto.SelectionCount
				};
				_session.SetModifiedProperties(applicantQuestion);
				_session.UnitOfWork.RegisterNew(applicantQuestion);
				_session.UnitOfWork.RegisterPostCommitAction(() => dto.QuestionId = applicantQuestion.QuestionId);
			}

			if (opResult.Success)
			{
                if (dto.ApplicantQuestionDdlItem[0].ApplicantQuestionDdlItemId == 0)
				{
					List<string> valueList = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantQuestionDdlItemQuery().ExecuteQueryAs(x => x.Value).ToList();
					int maxValue = valueList.Select(x => Convert.ToInt32(x)).DefaultIfEmpty(0).Max() + 1;
					var applicantQuestionDdlItem = new ApplicantQuestionDdlItem()
					{
						QuestionId = dto.QuestionId,
                        Description = dto.ApplicantQuestionDdlItem[0].Description,
						IsDefault = false,
						Value = maxValue.ToString(),
						FlaggedResponse = null,
						IsEnabled = true
					};
					_session.UnitOfWork.RegisterNew(applicantQuestionDdlItem);
                    _session.UnitOfWork.RegisterPostCommitAction(() => dto.ApplicantQuestionDdlItem[0].ApplicantQuestionDdlItemId = applicantQuestionDdlItem.ApplicantQuestionDdlItemId);
					_session.UnitOfWork.Commit().MergeInto(opResult);
                    dto.ApplicantQuestionDdlItem[0].QuestionId = dto.QuestionId;
                    dto.ApplicantQuestionDdlItem[0].OldDescription = dto.ApplicantQuestionDdlItem[0].Description;
					opResult.SetDataOnSuccess(dto);
				}
			}

			return opResult;
		}

		IOpResult IApplicantTrackingService.DeleteApplicantQuestionControl(int questionId)
		{
			var opResult = new OpResult();
			_session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			var questionToDelete = _session.UnitOfWork.ApplicantTrackingRepository
										.ApplicantQuestionControlQuery()
										.ByQuestionId(questionId)
										.ExecuteQueryAs(x => new
										{
											x.QuestionId,
											x.ClientId,
											x.IsEnabled
										})
										.FirstOrDefault();

			opResult.CheckForNotFound(questionToDelete);
			if (opResult.HasError)
				return opResult;

			_session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, questionToDelete.ClientId).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			//make sure question isn't already disabled
			if (questionToDelete.IsEnabled)
			{
				//spin up an actual entity and disable it
				var entity = new ApplicantQuestionControl
				{
					QuestionId = questionToDelete.QuestionId,
					IsEnabled = false
				};
				//only register the IsEnabled property on the entity as changing
				_session.SetModifiedProperties(entity);
				_session.UnitOfWork.RegisterModified(entity, new PropertyList<ApplicantQuestionControl>().Include(x => x.IsEnabled));
				_session.UnitOfWork.Commit().MergeInto(opResult);
			}

			return opResult;
		}

        #endregion

        #region ApplicationViewed

        IOpResult<ApplicationViewedDto> IApplicantTrackingService.GetApplicationViewed(int applicationViewedId)
		{
			var opResult = new OpResult<ApplicationViewedDto>();
			var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
			var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantTrackingEnabled || isSystemAdmin || accessRights.UserId == 0)
			{
				opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicationViewedQuery()
					.ByApplicationViewedId(applicationViewedId)
					.ExecuteQueryAs(x => new ApplicationViewedDto
					{
						ApplicationViewedId = x.ApplicationViewedId,
						ClientId = x.ClientId,
						UserId = x.UserId,
						ApplicationHeaderId = x.ApplicationHeaderId,
						ViewedOn = x.ViewedOn
					}).FirstOrDefault());
			}
			return opResult;
		}

		IOpResult<ApplicationViewedDto> IApplicantTrackingService.GetApplicationViewedByApplicationHeaderId(
			int applicationHeaderId)
		{
			var result = new OpResult<ApplicationViewedDto>();
			var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
			var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantTrackingEnabled || isSystemAdmin || accessRights.UserId == 0)
			{
				result.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository
					.ApplicationViewedQuery()
					.ByApplicationHeaderId(applicationHeaderId)
					.ExecuteQueryAs(x => new ApplicationViewedDto
					{
						ApplicationViewedId = x.ApplicationViewedId,
						ClientId = x.ClientId,
						UserId = x.UserId,
						ApplicationHeaderId = x.ApplicationHeaderId,
						ViewedOn = x.ViewedOn
					})
					.FirstOrDefault());
			}
			return result;
		}

		IOpResult<ApplicationViewedDto> IApplicantTrackingService.UpdateApplicationViewed(ApplicationViewedDto dto)
		{
			var opResult = new OpResult<ApplicationViewedDto>();
			var isApplicantAdmin = _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).Success;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantAdmin || isSystemAdmin)
			{
				var applicationViewed = new ApplicationViewed();
				int applicationViewedId = dto.ApplicationViewedId;
				if (applicationViewedId == 0)
				{
					applicationViewed = new ApplicationViewed()
					{
						ClientId = dto.ClientId,
						UserId = dto.UserId,
						ApplicationHeaderId = dto.ApplicationHeaderId,
						ViewedOn = dto.ViewedOn
					};
					_session.UnitOfWork.RegisterNew(applicationViewed);
				}
				else
				{
					applicationViewed = _session.UnitOfWork.ApplicantTrackingRepository
												.ApplicationViewedQuery()
												.ByApplicationViewedId(dto.ApplicationViewedId)
												.FirstOrDefault();

					applicationViewed.ApplicationViewedId = dto.ApplicationViewedId;
					applicationViewed.ClientId = dto.ClientId;
					applicationViewed.UserId = dto.UserId;
					applicationViewed.ApplicationHeaderId = dto.ApplicationHeaderId;
					applicationViewed.ViewedOn = dto.ViewedOn;

					_session.UnitOfWork.RegisterModified(applicationViewed);
				}
				_session.UnitOfWork.Commit().MergeInto(opResult);

				if (dto.ApplicationViewedId == 0)
				{
					dto.ApplicationViewedId = applicationViewed.ApplicationViewedId;
				}

				opResult.Data = dto;
			}

			return opResult;
		}

		IOpResult<IEnumerable<ApplicationViewedDto>> IApplicantTrackingService.DeleteApplicationViewed(int applicationViewedId, int clientId)
		{
			var opResult = new OpResult<IEnumerable<ApplicationViewedDto>>();
			var isApplicantAdmin = _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).Success;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantAdmin || isSystemAdmin)
			{
				var applicationViewedToDelete = _session.UnitOfWork.ApplicantTrackingRepository
											.ApplicationViewedQuery()
											.ByApplicationViewedId(applicationViewedId)
											.FirstOrDefault();

				_session.UnitOfWork.RegisterDeleted(applicationViewedToDelete);
				_session.UnitOfWork.Commit().MergeInto(opResult);

				var applicationsViewed = _session.UnitOfWork.ApplicantTrackingRepository.ApplicationViewedQuery()
					.ByApplicationViewedId(applicationViewedId)
					.ExecuteQueryAs(x => new ApplicationViewedDto
					{
						ApplicationViewedId = x.ApplicationViewedId,
						ClientId = x.ClientId,
						UserId = x.UserId,
						ApplicationHeaderId = x.ApplicationHeaderId,
						ViewedOn = x.ViewedOn
					}).ToList();
				opResult.Data = applicationsViewed;
			}
			return opResult;
		}

		#endregion

		#region ApplicantDegreeList

		IOpResult<ApplicantDegreeListDto> IApplicantTrackingService.GetApplicantDegreeList(int applicantDegreeListId)
		{
			var opResult = new OpResult<ApplicantDegreeListDto>();
			var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
			var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantTrackingEnabled || isSystemAdmin || accessRights.UserId == 0)
			{
				opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantDegreeListQuery()
					.ByDegreeId(applicantDegreeListId)
					.ExecuteQueryAs(x => new ApplicantDegreeListDto
					{
						DegreeId = x.DegreeId,
						Description = x.Description
					}).FirstOrDefault());
			}
			return opResult;
		}

		IOpResult<IEnumerable<ApplicantDegreeListDto>> IApplicantTrackingService.GetApplicantDegreeList()
		{
			var opResult = new OpResult<IEnumerable<ApplicantDegreeListDto>>();
			var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
			var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantTrackingEnabled || isSystemAdmin || accessRights.UserId == 0)
			{
				opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantDegreeListQuery()
					.ExecuteQueryAs(x => new ApplicantDegreeListDto
					{
						DegreeId = x.DegreeId,
						Description = x.Description
					}));
			}
			return opResult;
		}

		IOpResult<ApplicantDegreeListDto> IApplicantTrackingService.UpdateApplicantDegreeList(ApplicantDegreeListDto dto)
		{
			var opResult = new OpResult<ApplicantDegreeListDto>();
			var isApplicantAdmin = _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).Success;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantAdmin || isSystemAdmin)
			{
				var applicantDegree = new ApplicantDegreeList();
				int applicantDegreeId = dto.DegreeId;
				if (applicantDegreeId == 0)
				{
					applicantDegree = new ApplicantDegreeList()
					{
						Description = dto.Description
					};
					_session.UnitOfWork.RegisterNew(applicantDegree);
				}
				else
				{
					applicantDegree = _session.UnitOfWork.ApplicantTrackingRepository
												.ApplicantDegreeListQuery()
												.ByDegreeId(dto.DegreeId)
												.FirstOrDefault();

					applicantDegree.DegreeId = dto.DegreeId;
					applicantDegree.Description = dto.Description;

					_session.UnitOfWork.RegisterModified(applicantDegree);
				}
				_session.UnitOfWork.Commit().MergeInto(opResult);

				if (dto.DegreeId == 0)
				{
					dto.DegreeId = applicantDegree.DegreeId;
				}

				opResult.Data = dto;
			}

			return opResult;
		}

		IOpResult<IEnumerable<ApplicantDegreeListDto>> IApplicantTrackingService.DeleteApplicantDegreeList(int applicantDegreeListId, int modifiedBy)
		{
			var opResult = new OpResult<IEnumerable<ApplicantDegreeListDto>>();
			var isApplicantAdmin = _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).Success;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantAdmin || isSystemAdmin)
			{
				var applicantDegreeToDelete = _session.UnitOfWork.ApplicantTrackingRepository
											.ApplicantDegreeListQuery()
											.ByDegreeId(applicantDegreeListId)
											.FirstOrDefault();

				_session.UnitOfWork.RegisterDeleted(applicantDegreeToDelete);
				_session.UnitOfWork.Commit().MergeInto(opResult);

				var applicantDegrees = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantDegreeListQuery()
					.ExecuteQueryAs(x => new ApplicantDegreeListDto
					{
						DegreeId = x.DegreeId,
						Description = x.Description
					}).ToList();
				opResult.Data = applicantDegrees;
			}
			return opResult;
		}

		#endregion

        IOpResult<ApplicantDetailDto> IApplicantTrackingService.GetHiredApplicantApplicationHeader(int applicantId)
        {
            var opResult = new OpResult<ApplicantDetailDto>();

            var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
            var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
            var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;

            if (isApplicantTrackingEnabled || isSystemAdmin)
            {
                ApplicantDetailDto data = null;

                // Get application for applicant with hired status
                List <ApplicantDetailDto> lst = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantApplicationHeaderQuery()
                    .ByApplicantId(applicantId)
                    .ByApplicantStatusTypeId(ApplicantStatusType.Hired)
                    .ViewDenied(false) // Not blocked
                    .ExecuteQueryAs(new ApplicantApplicationHeaderMaps.ToApplicantDetailDto())
                    .ToList();

                // Make sure the rejection reason logic is applied to choose the header record that maps to correct posting
                lst = lst.Where(x => !x.ApplicantRejectionReasonId.HasValue).OrderByDescending(x => x.ApplicationHeaderId).ToList();

                if (lst.Count > 0)
                {
                    // filter by no rejection reasons
                    data = lst.OrderByDescending(x=>x.ApplicationHeaderId).FirstOrDefault();

                    // Add Employment information if any
                    if (data!=null && data.EmployeeId.HasValue)
                    {
                        data.HireInfo = _session.UnitOfWork.EmployeeRepository.QueryEmployees()
                            .ByEmployeeId(data.EmployeeId.Value)
                            .ExecuteQueryAs(new ApplicantApplicationHeaderMaps.ToEmployeeHireInfoDto())
                            .FirstOrDefault();

                        if (data.HireInfo.HiredBy.HasValue)
                        {
                            data.HireInfo.Hirer = _session.UnitOfWork.UserRepository.QueryUsers()
                                .ByUserId(data.HireInfo.HiredBy.Value)
                                .ExecuteQueryAs(x => x.FirstName + " " + x.LastName)
                                .FirstOrDefault();
                        }

                        opResult.SetDataOnSuccess(data);
                    }
                    else
                    {
                        return opResult.SetToFail("No application information found");
                    }
                }
                else
                {
                    return opResult.SetToFail("No application information found");
                }
            }
            return opResult;
        }

        IOpResult<ApplicantDetailDto> IApplicantTrackingService.GetQualifiedApplicant(int postingId, int applicantId )
		{
		    var opResult = new OpResult<ApplicantDetailDto>();

		    var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
		    var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
		    var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;

		    if (isApplicantTrackingEnabled || isSystemAdmin)
		    { 
		        opResult.Data = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantApplicationHeaderQuery()
		            .ByPostingId(postingId)
		            .ByApplicantId(applicantId)
		            .ExecuteQueryAs(new ApplicantApplicationHeaderMaps.ToApplicantDetailDto())
		            .FirstOrDefault();

                ApplicantDetailDto data = opResult.Data;
                // Add Employment information if any
                if (data.EmployeeId.HasValue)
                {
                    data.HireInfo = _session.UnitOfWork.EmployeeRepository.QueryEmployees()
                        .ByEmployeeId(data.EmployeeId.Value)
                        .ExecuteQueryAs(new ApplicantApplicationHeaderMaps.ToEmployeeHireInfoDto())
                        .FirstOrDefault();

                    if (data.HireInfo.HiredBy.HasValue)
                    {
                        data.HireInfo.Hirer = _session.UnitOfWork.UserRepository.QueryUsers()
                            .ByUserId(data.HireInfo.HiredBy.Value)
                            .ExecuteQueryAs(x => x.FirstName + " " + x.LastName)
                            .FirstOrDefault();
                    }
                }
		    }
		    return opResult;
		}
		
		IOpResult<IEnumerable<ApplicantDetailDto>> IApplicantTrackingService.GetQualifiedApplicants(ApplicantFilterDto dto)
		{
			var opResult = new OpResult<IEnumerable<ApplicantDetailDto>>();

			var isApplicantAdmin = _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).Success;
			var isExternalApplicant = _session.CanPerformAction(ApplicantTrackingActionType.ExternalApplicant).Success;
			var isInternalApplicant = _session.CanPerformAction(ApplicantTrackingActionType.InternalApplicant).Success;

			if (isApplicantAdmin || isExternalApplicant || isInternalApplicant)
			{
				_session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, dto.ClientId.GetValueOrDefault()).MergeInto(opResult);
				if (opResult.HasError)
					return opResult;

				var qry = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantApplicationHeaderQuery()
					.ByDateSubmitted(dto.StartDate, dto.EndDate)
					.ByApplicationCompleted(true)
					.ByPostingId(dto.PostingId)
					//.ByApplicantStatusTypeId(dto.StatusId)
					.ByApplicantPostingEnabled(true)
					//.ByApplicantPostingClosed(dto.IsClosed)
					.ByJobTypeId(dto.JobTypeId)
					.ByJobProfileId(dto.JobProfileId)
					.ByPostingCategoryId(dto.PostingCategoryId)
					.ByPostingOwnerId(dto.PostingOwnerId)
					.ByDivisionId(dto.DivisionId)
					.ByDepartmentId(dto.DepartmentId)
					.ViewDenied(dto.ViewDenied)
					.ByKeyword(dto.Keyword)
					.ByName(dto.NameSearch)
					.ByPostingNumber(dto.PostingNumber)
					.ByApplicantPostingClientId(dto.ClientId)
                    .ByRatingSelection(dto.RatingSelection)
                    .IgnoreNullStatus();

				if ((dto.ApplicantRejectionReasonId ?? 0) > 0)
					qry.ByApplicantRejectionReasonId(dto.ApplicantRejectionReasonId);

                List<ApplicantDetailDto> data = qry.ExecuteQueryAs(new ApplicantApplicationHeaderMaps.ToApplicantDetailDto()).ToList();

                // Add Employment information if any
                IEnumerable<int> employeeIds = data.Select(x => x.EmployeeId.HasValue ? x.EmployeeId.Value : 0).Distinct();
                IDictionary<int, EmployeeHireInfoDto> hires = _session.UnitOfWork.EmployeeRepository.QueryEmployees()
                    .ByEmployeeIds(employeeIds)
                    .ExecuteQueryAs(new ApplicantApplicationHeaderMaps.ToEmployeeHireInfoDto())
                    .ToDictionary<EmployeeHireInfoDto,int>( x => x.EmployeeId);
                IEnumerable<int> hirerIds = hires.Values.Select(x => (x.HiredBy.HasValue ? x.HiredBy.Value : 0)).Distinct();

                IDictionary<int, UserUsernameDto> adminUsers = _session.UnitOfWork.UserRepository.QueryUsers()
                            .ByUserIds(hirerIds.ToArray())
                            .ExecuteQueryAs(x => new UserUsernameDto() { UserId = x.UserId, UserName = x.FirstName + " " + x.LastName })
                            .ToDictionary<UserUsernameDto, int>(x => x.UserId);

                // Map employment information with hirer information
                foreach (ApplicantDetailDto dtoTmp in data)
                {
                    if (dtoTmp.EmployeeId.HasValue && hires.ContainsKey(dtoTmp.EmployeeId.Value))
                    {
                        dtoTmp.HireInfo = hires[dtoTmp.EmployeeId.Value];
                        if (dtoTmp.HireInfo.HiredBy.HasValue)
                            dtoTmp.HireInfo.Hirer = adminUsers[dtoTmp.HireInfo.HiredBy.Value].UserName;
                    }
                }

                opResult.Data = data;
			}
			return opResult;
		}

		IOpResult<IEnumerable<ApplicantsCountDto>> IApplicantTrackingService.GetQualifiedApplicantsCount(ApplicantFilterDto dto)
		{
			var opResult = new OpResult<IEnumerable<ApplicantsCountDto>>();

			var isApplicantAdmin = _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).Success;
			var isExternalApplicant = _session.CanPerformAction(ApplicantTrackingActionType.ExternalApplicant).Success;
			var isInternalApplicant = _session.CanPerformAction(ApplicantTrackingActionType.InternalApplicant).Success;

			if (isApplicantAdmin || isExternalApplicant || isInternalApplicant)
			{
				_session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, dto.ClientId.GetValueOrDefault()).MergeInto(opResult);
				if (opResult.HasError)
					return opResult;

				var qry = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantApplicationHeaderQuery()
					.ByDateSubmitted(dto.StartDate, dto.EndDate)
					.ByApplicationCompleted(true)
					.ByPostingId(dto.PostingId)
					.ByApplicantPostingEnabled(true)
					//.ByApplicantPostingClosed(dto.IsClosed)
					.ByJobTypeId(dto.JobTypeId)
					.ByJobProfileId(dto.JobProfileId)
					.ByPostingCategoryId(dto.PostingCategoryId)
					.ByPostingOwnerId(dto.PostingOwnerId)
					.ByDivisionId(dto.DivisionId)
					.ByDepartmentId(dto.DepartmentId)
					.ViewDenied(dto.ViewDenied)
					.ByKeyword(dto.Keyword)
					.ByName(dto.NameSearch)
					.ByPostingNumber(dto.PostingNumber)
					.ByApplicantPostingClientId(dto.ClientId)
					.IgnoreNullStatus();

				var data = qry.ExecuteQueryAs(x => x.ApplicantStatusTypeId ?? ApplicantStatusType.Applicant).GroupBy(x => x).Select(x => new ApplicantsCountDto
				{
					StatusId = x.Key,
					Count = x.Count()
				}).ToList();

				opResult.Data = data;
			}
			return opResult;
		}
        IOpResult<IEnumerable<ApplicantApplicationEmailHistoryDto>> IApplicantTrackingService.GetApplicantCorrespondencesByApplicationHeaderId(int applicationHeaderId)
		{
            var opResult = new OpResult<IEnumerable<ApplicantApplicationEmailHistoryDto>>();

			_session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

            int clientId = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantApplicationHeaderQuery()
            .ByApplicationHeaderId(applicationHeaderId).ExecuteQueryAs(x => x.Applicant.ClientId).FirstOrDefault();

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(opResult);
            if (opResult.HasError)
                return opResult;

            IEnumerable<ApplicantApplicationEmailHistoryDto> history = _session.UnitOfWork.ApplicantTrackingRepository
                                .ApplicantApplicationEmailHistoryQuery()
                                .ByApplicationHeaderId(applicationHeaderId)
                                .ExecuteQueryAs(x => new ApplicantApplicationEmailHistoryDto
								{
                                    ApplicantApplicationEmailHistoryId = x.ApplicantApplicationEmailHistoryId,
                                    ApplicationHeaderId = x.ApplicationHeaderId,
                                    ApplicantCompanyCorrespondenceId = x.ApplicantCompanyCorrespondenceId,
                                    CorrespondenceTemplateName = (x.ApplicantCompanyCorrespondenceId  != null) ?  x.ApplicantCompanyCorrespondence.Description : "Custom",
                                    CorrespondenceSubject = (x.ApplicantCompanyCorrespondenceId != null) ? x.ApplicantCompanyCorrespondence.Subject : 
                                                                (!string.IsNullOrEmpty(x.Subject) ? x.Subject : ""),
                                    ApplicantStatusTypeId = x.ApplicantStatusTypeId,
                                    SenderName = (x.User != null) ? x.User.LastName + ", " + x.User.FirstName : "",
                                    SentDate = x.SentDate.HasValue ? x.SentDate.Value : default(DateTime),
                                    SenderEmail = x.SenderEmail,
                                    // If subject does'nt exist then it is a text message
                                    isText = (x.ApplicantCompanyCorrespondenceId != null) ? x.ApplicantCompanyCorrespondence.IsText :
                                            string.IsNullOrEmpty(x.Subject),                                            
                                }).ToList();

            opResult.Data = history;
            return opResult;
        }
        IOpResult<string> IApplicantTrackingService.GetApplicantCorrespondenceEmailByApplicationEmailHistoryId(int emailHistoryId)
        {
            var opResult = new OpResult<string>();

            _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

            var qry = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantApplicationEmailHistoryQuery()
            .ByEmailHistoryId(emailHistoryId);

			var data = qry.ExecuteQueryAs(x => new ApplicantTrackingCorrespondenceReplacementInfoDto
			{
                ApplicantClientId = x.ApplicantApplicationHeader.Applicant.ClientId,
                TemplateBody = !string.IsNullOrEmpty(x.Body) ? x.Body : x.ApplicantCompanyCorrespondence.Body ,
                ApplicantFirstName = x.ApplicantApplicationHeader.Applicant.FirstName,
                ApplicantLastName = x.ApplicantApplicationHeader.Applicant.LastName,
                UserName = x.User != null ? x.User.UserName : "",
                Posting =  x.ApplicantApplicationHeader.ApplicantPosting.Description,
				Date = DateTime.Now,
                Address = x.ApplicantApplicationHeader.Applicant.AddressLine2 != "" ? x.ApplicantApplicationHeader.Applicant.Address + "<br/>" + x.ApplicantApplicationHeader.Applicant.AddressLine2 + "<br/>" + x.ApplicantApplicationHeader.Applicant.City + ", " + x.ApplicantApplicationHeader.Applicant.StateDetails.Name + " " + x.ApplicantApplicationHeader.Applicant.Zip : x.ApplicantApplicationHeader.Applicant.Address + "<br/>" + x.ApplicantApplicationHeader.Applicant.City + ", " + x.ApplicantApplicationHeader.Applicant.StateDetails.Name + " " + x.ApplicantApplicationHeader.Applicant.Zip,
                Phone = x.ApplicantApplicationHeader.Applicant.PhoneNumber,
                CompanyAddress = x.ApplicantApplicationHeader.Applicant.Client.AddressLine2 != "" ? x.ApplicantApplicationHeader.Applicant.Client.AddressLine1 + "<br/>" + x.ApplicantApplicationHeader.Applicant.Client.AddressLine2 + "<br/>" + x.ApplicantApplicationHeader.Applicant.Client.City + ", " + x.ApplicantApplicationHeader.Applicant.Client.State.Name + " " + x.ApplicantApplicationHeader.Applicant.Client.PostalCode : x.ApplicantApplicationHeader.Applicant.Client.AddressLine1 + "<br/>" + x.ApplicantApplicationHeader.Applicant.Client.City + ", " + x.ApplicantApplicationHeader.Applicant.Client.State.Name + " " + x.ApplicantApplicationHeader.Applicant.Client.PostalCode

			}).FirstOrDefault();

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, data.ApplicantClientId).MergeInto(opResult);
            if (opResult.HasError)
                return opResult;

			opResult.Data = string.Empty;
			if (data.TemplateBody != null)
			{
				// Company Logo
                AzureViewDto imgSource = _clientAzureService.GetAzureClientResource(ResourceSourceType.AzureClientImage, data.ApplicantClientId, "logo").Data;
				data.CompanyLogo = imgSource?.Source;
				// Company Name
                data.CompanyName = _session.UnitOfWork.ClientRepository.QueryClients().ByClientId(data.ApplicantClientId).ExecuteQueryAs(x =>
				(x.ApplicantClient.JobBoardTitle == null ? x.ClientName : x.ApplicantClient.JobBoardTitle)).FirstOrDefault();

				opResult.Data = GetEmailBody(data);
			}

			return opResult;
		}

		IOpResult<EmailBodyDto> IApplicantTrackingService.GetApplicantCompanyCorrespondenceEmailBodyByCorrespondenceId(int applicantCompanyCorrespondenceId, int? applicationHeaderId, bool isApplicantAdmin)
		{
			var opResult = new OpResult<EmailBodyDto>();
            //if (isApplicantAdmin)
            //{
            //    _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
            //}
            //else
            //{
            //    _session.CanPerformAction(OnboardingActionType.OnboardingAdministrator).MergeInto(opResult);
            //}

            var appResult = _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin);
            var onbResult = _session.CanPerformAction(OnboardingActionType.OnboardingAdministrator);
            if (appResult.HasError && onbResult.HasError)
            {
                if (appResult.HasError)
                {
                    _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
                }
                else
                {
                    _session.CanPerformAction(OnboardingActionType.OnboardingAdministrator).MergeInto(opResult);
                }
            }

            if (opResult.HasError)
				return opResult;

			var qry = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantCompanyCorrespondenceQuery()
				.ByCorrespondenceId(applicantCompanyCorrespondenceId);

            // Retrieve template
			var clientTemplate = qry.ExecuteQueryAs(x => new { x.ClientId, x.Body, x.Subject }).FirstOrDefault();
            int clientId = clientTemplate.ClientId;

            ApplicantTrackingCorrespondenceReplacementInfoDto data = null;
            if (applicationHeaderId.HasValue && applicationHeaderId.Value > 0 )
            {
                data = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantApplicationHeaderQuery()
                    .ByApplicationHeaderId(applicationHeaderId.Value)
                    .ExecuteQueryAs(x => new ApplicantTrackingCorrespondenceReplacementInfoDto
                    {
                        ApplicantFirstName = x.Applicant.FirstName,
                        ApplicantLastName = x.Applicant.LastName,
                        ApplicantEmail = x.Applicant.EmailAddress,
                        UserName = x.Applicant.User != null ? x.Applicant.User.UserName : "",
                        Posting = x.ApplicantPosting.Description,
                        Date = DateTime.Now,
                        Address = x.Applicant.AddressLine2 != ""
                        ? x.Applicant.Address + "<br/>" + x.Applicant.AddressLine2 + "<br/>" + x.Applicant.City + ", " +
                          x.Applicant.StateDetails.Name + " " + x.Applicant.Zip
                        : x.Applicant.Address + "<br/>" + x.Applicant.City + ", " + x.Applicant.StateDetails.Name +
                          " " + x.Applicant.Zip,
                        Phone = x.Applicant.PhoneNumber,
                    })
                    .FirstOrDefault();
            }
            else
            {
                data = new ApplicantTrackingCorrespondenceReplacementInfoDto()
                {
                    ApplicantFirstName = "Applicant's First Name",
                    ApplicantLastName = "Applicant's Last Name",
                    UserName = "Applicant's User Name",
                    Password = "Applicant's Password",
                    Posting = "Example Posting",
                    Date = DateTime.Now,
                    Address = "Applicant's Street<br />Applicant's City, Applicant's State Applicant's Zip",
                    Phone = "Applicant's Phone Number",
                };
            }

			_session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;


			if (clientTemplate.Body != null)
			{
                // Email Template
                data.TemplateBody = clientTemplate.Body;
				// Company Logo
				AzureViewDto imgSource = _clientAzureService.GetAzureClientResource(ResourceSourceType.AzureClientImage, clientId, "logo").Data;
				data.CompanyLogo = imgSource?.Source;
				// Company Name
				data.CompanyName = _session.UnitOfWork.ClientRepository.QueryClients().ByClientId(clientId).ExecuteQueryAs(x =>
				(x.ApplicantClient.JobBoardTitle == null ? x.ClientName : x.ApplicantClient.JobBoardTitle)).FirstOrDefault();
				// Company Address
				data.CompanyAddress = _session.UnitOfWork.ClientRepository.QueryClients().ByClientId(clientId).ExecuteQueryAs(x =>
				(x.AddressLine2 != "" ? x.AddressLine1 + "<br/>" + x.AddressLine2 + "<br/>" + x.City + ", " + x.State.Name + " " + x.PostalCode
					: x.AddressLine1 + "<br/>" + x.City + ", " + x.State.Name + " " + x.PostalCode)).FirstOrDefault();

				opResult.Data = new EmailBodyDto()
				{
					Body = GetEmailBody(data),
                    Subject = clientTemplate.Subject,
                    TemplateBody = clientTemplate.Body,
                };
			}

			return opResult;
		}

		IOpResult<ApplicantOnBoardingHeaderDto> IApplicantTrackingService.GetApplicantOnBoardingHeaderByApplicationHeaderIdAndOnboardingProcessId(int applicantApplicationHeaderId, int applicantOnboardingProcessId)
		{
			var opResult = new OpResult<ApplicantOnBoardingHeaderDto>();
			var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
			var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantTrackingEnabled || isSystemAdmin || accessRights.UserId == 0)
			{
				opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantOnBoardingHeaderQuery()
					.ByApplicantApplicationHeaderId(applicantApplicationHeaderId)
					.ByApplicantOnBoardingProcessId(applicantOnboardingProcessId)
					.ExecuteQueryAs(x => new ApplicantOnBoardingHeaderDto
					{
						ApplicantOnBoardingHeaderId = x.ApplicantOnBoardingHeaderId,
						ApplicantApplicationHeaderId = x.ApplicantApplicationHeaderId,
						ApplicantOnBoardingProcessId = x.ApplicantOnBoardingProcessId,
						ApplicantId = x.ApplicantId,
						ClientId = x.ClientId,
						OnBoardingStarted = x.OnBoardingStarted,
						OnBoardingEnded = x.OnBoardingEnded,
						IsHired = x.IsHired,
						IsRejected = x.IsRejected,
						ModifiedBy = x.ModifiedBy,
						Modified = x.Modified
					}).FirstOrDefault());
			}

			return opResult;
		}
		#region ApplicantOnboardingDefaultDetail

		IOpResult<ApplicantOnBoardingDefaultDetailDto> IApplicantTrackingService.GetApplicantOnBoardingDefaultDetailByDetailId(int detailId)
		{
			var opResult = new OpResult<ApplicantOnBoardingDefaultDetailDto>();
			var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
			var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantTrackingEnabled || isSystemAdmin || accessRights.UserId == 0)
			{
				opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantOnBoardingDefaultDetailQuery()
					.ByApplicantOnBoardingDefaultDetailId(detailId)
					.ExecuteQueryAs(x => new ApplicantOnBoardingDefaultDetailDto
					{
						ApplicantOnBoardingDefaultDetailId = x.ApplicantOnBoardingDefaultDetailId,
						ClientId = x.ClientId,
						ApplicantPostingId = x.ApplicantPostingId,
						ApplicantOnBoardingProcessId = x.ApplicantOnBoardingProcessId,
						ApplicantOnBoardingTaskId = x.ApplicantOnBoardingTaskId,
						AssignedToUserId = x.AssignedToUserId,
						IsEmailRequired = x.IsEmailRequired,
						DaysToComplete = x.DaysToComplete,
						SpecialInstructions = x.SpecialInstructions,
						ModifiedBy = x.ModifiedBy,
						IsAutoStart = x.IsAutoStart
					}).FirstOrDefault());
			}

			return opResult;
		}

		IOpResult<IEnumerable<ApplicantOnBoardingDefaultDetailDto>> IApplicantTrackingService.GetApplicantOnBoardingDefaultDetailByProcessIdAndPostingId(int processId, int postingId)
		{
			var opResult = new OpResult<IEnumerable<ApplicantOnBoardingDefaultDetailDto>>();
			var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
			var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantTrackingEnabled || isSystemAdmin || accessRights.UserId == 0)
			{
				opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantOnBoardingDefaultDetailQuery()
					.ByApplicantOnBoardingProcessId(processId)
					.ByApplicantPostingId(postingId)
					.ExecuteQueryAs(x => new ApplicantOnBoardingDefaultDetailDto
					{
						ApplicantOnBoardingDefaultDetailId = x.ApplicantOnBoardingDefaultDetailId,
						ClientId = x.ClientId,
						ApplicantPostingId = x.ApplicantPostingId,
						ApplicantOnBoardingProcessId = x.ApplicantOnBoardingProcessId,
						ApplicantOnBoardingTaskId = x.ApplicantOnBoardingTaskId,
						AssignedToUserId = x.AssignedToUserId,
						IsEmailRequired = x.IsEmailRequired,
						DaysToComplete = x.DaysToComplete,
						SpecialInstructions = x.SpecialInstructions,
						ModifiedBy = x.ModifiedBy,
						IsAutoStart = x.IsAutoStart
					}).ToList());
			}

			return opResult;
		}

		#endregion

		#region ApplicantOnboardingDetail

		IOpResult<ApplicantOnBoardingDetailDto> IApplicantTrackingService.GetApplicantOnBoardingDetailByDetailId(int detailId)
		{
			var opResult = new OpResult<ApplicantOnBoardingDetailDto>();
			var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
			var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantTrackingEnabled || isSystemAdmin || accessRights.UserId == 0)
			{
				opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantOnBoardingDetailQuery()
					.ByApplicantOnBoardingDetailId(detailId)
					.ExecuteQueryAs(x => new ApplicantOnBoardingDetailDto
					{
						ApplicantOnBoardingDetailId = x.ApplicantOnBoardingDetailId,
						ApplicantOnBoardingHeaderId = x.ApplicantOnBoardingHeaderId,
						ApplicantOnBoardingTaskId = x.ApplicantOnBoardingTaskId,
						AssignedToUserId = x.AssignedToUserId,
						IsEmailRequired = x.IsEmailRequired,
						DaysToComplete = x.DaysToComplete,
						SpecialInstructions = x.SpecialInstructions,
						DateStarted = x.DateStarted,
						DateCompleted = x.DateCompleted,
						IsCompleted = x.IsCompleted,
						ModifiedBy = x.ModifiedBy,
						ApplicantCorrespondenceId = x.ApplicantCorrespondenceId,
						Modified = x.Modified
					}).FirstOrDefault());
			}
			return opResult;
		}

		IOpResult<ApplicantOnBoardingDetailDto> IApplicantTrackingService.UpdateApplicantOnBoardingDetail(ApplicantOnBoardingDetailDto dto)
		{
			var opResult = new OpResult<ApplicantOnBoardingDetailDto>();
			var isApplicantAdmin = _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).Success;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantAdmin || isSystemAdmin)
			{
				var applicantOnboardingDetail = new ApplicantOnBoardingDetail();
				int applicantOnboardingDetailId = dto.ApplicantOnBoardingDetailId;
				if (applicantOnboardingDetailId == 0)
				{
					applicantOnboardingDetail = new ApplicantOnBoardingDetail()
					{
						ApplicantOnBoardingDetailId = dto.ApplicantOnBoardingDetailId,
						ApplicantOnBoardingHeaderId = dto.ApplicantOnBoardingHeaderId,
						ApplicantOnBoardingTaskId = dto.ApplicantOnBoardingTaskId,
						AssignedToUserId = dto.AssignedToUserId,
						IsEmailRequired = dto.IsEmailRequired,
						DaysToComplete = dto.DaysToComplete,
						SpecialInstructions = dto.SpecialInstructions,
						DateStarted = dto.DateStarted,
						DateCompleted = dto.DateCompleted,
						IsCompleted = dto.IsCompleted,
						ModifiedBy = dto.ModifiedBy,
						ApplicantCorrespondenceId = dto.ApplicantCorrespondenceId,
						Modified = dto.Modified
					};
					_session.UnitOfWork.RegisterNew(applicantOnboardingDetail);
				}
				else
				{
					applicantOnboardingDetail = _session.UnitOfWork.ApplicantTrackingRepository
												.ApplicantOnBoardingDetailQuery()
												.ByApplicantOnBoardingDetailId(dto.ApplicantOnBoardingDetailId)
												.FirstOrDefault();

					applicantOnboardingDetail.ApplicantOnBoardingDetailId = dto.ApplicantOnBoardingDetailId;
					applicantOnboardingDetail.ApplicantOnBoardingHeaderId = dto.ApplicantOnBoardingHeaderId;
					applicantOnboardingDetail.ApplicantOnBoardingTaskId = dto.ApplicantOnBoardingTaskId;
					applicantOnboardingDetail.AssignedToUserId = dto.AssignedToUserId;
					applicantOnboardingDetail.IsEmailRequired = dto.IsEmailRequired;
					applicantOnboardingDetail.DaysToComplete = dto.DaysToComplete;
					applicantOnboardingDetail.SpecialInstructions = dto.SpecialInstructions;
					applicantOnboardingDetail.DateStarted = dto.DateStarted;
					applicantOnboardingDetail.DateCompleted = dto.DateCompleted;
					applicantOnboardingDetail.IsCompleted = dto.IsCompleted;
					applicantOnboardingDetail.ModifiedBy = dto.ModifiedBy;
					applicantOnboardingDetail.ApplicantCorrespondenceId = dto.ApplicantCorrespondenceId;
					applicantOnboardingDetail.Modified = dto.Modified;

					_session.UnitOfWork.RegisterModified(applicantOnboardingDetail);
				}
				_session.UnitOfWork.Commit().MergeInto(opResult);

				if (dto.ApplicantOnBoardingDetailId == 0)
				{
					dto.ApplicantOnBoardingDetailId = applicantOnboardingDetail.ApplicantOnBoardingDetailId;
				}

				opResult.Data = dto;
			}

			return opResult;
		}

		IOpResult IApplicantTrackingService.UpdateApplicantsStatus(List<ApplicantDetailDto> applicantsList, int clientId, bool flag)
		{
			var opResult = new OpResult();

			_session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			_session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			foreach (ApplicantDetailDto applicant in applicantsList)
			{
				var existingApplicant = _session.UnitOfWork.ApplicantTrackingRepository
												.ApplicantsQuery()
												.ByApplicantId(applicant.ApplicantId)
												.FirstOrDefault();
				existingApplicant.IsDenied = flag;
				_session.UnitOfWork.RegisterModified(existingApplicant);

			}
			_session.UnitOfWork.Commit().MergeInto(opResult);

			return opResult;
		}

		IOpResult IApplicantTrackingService.TransferApplications(List<ApplicantDetailDto> applicantsList, int toPostingId, ApplicantStatusType toStatusId, int userId)
		{
			var opResult = new OpResult();

			_session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			var posting = _session.UnitOfWork.ApplicantTrackingRepository
								.ApplicantPostingsQuery()
								.ByPostingId(toPostingId)
								.ExecuteQueryAs(x => new
								{
									x.ClientId,
								})
								.FirstOrDefault();

			_session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, posting.ClientId).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			foreach (ApplicantDetailDto applicant in applicantsList)
			{
				//Insert ApplicantApplicationHeader
				var newApplicantApplicationHeader = new ApplicantApplicationHeader()
				{
					ApplicantId = applicant.ApplicantId,
					PostingId = toPostingId,
					DateSubmitted = DateTime.Now,
					OrigPostingId = applicant.PostingId,
					IsApplicationCompleted = true,
					IsRecommendInterview = false,
					ApplicantResumeId = applicant.ApplicantResumeId,
					ApplicantRejectionReasonId = (int?)null,
                    ApplicantStatusTypeId = toStatusId,
                    CoverLetter = applicant.CoverLetter,
                    CoverLetterId = applicant.CoverLetterId,
                    AddedByAdmin = applicant.AddedByAdmin,
                    Score = applicant.Score,
					AddedBy = _session.LoggedInUserInformation.UserId,
                    DisclaimerId = applicant.DisclaimerId,
				};
				_session.UnitOfWork.RegisterNew(newApplicantApplicationHeader);
				_session.UnitOfWork.Commit().MergeInto(opResult);

				//Select ApplicantApplicationDetails
				var applicantApplicationDetailList = this.Self.GetApplicantApplicationDetailsByHeaderId(applicant.ApplicationHeaderId).Data;
				//Insert ApplicantApplicationDetails
				foreach (ApplicantApplicationDetailDto applicantApplicationDetailDto in applicantApplicationDetailList)
				{
					var applicantApplicationDetail = new ApplicantApplicationDetail();
					applicantApplicationDetail = new ApplicantApplicationDetail()
					{
						ApplicationHeaderId = newApplicantApplicationHeader.ApplicationHeaderId,
						QuestionId = applicantApplicationDetailDto.QuestionId,
						Response = applicantApplicationDetailDto.Response,
						IsFlagged = applicantApplicationDetailDto.IsFlagged,
						SectionId = applicantApplicationDetailDto.SectionId
					};
					_session.UnitOfWork.RegisterNew(applicantApplicationDetail);
				}

				//Select ApplicantPosting
				var newApplicantPosting = this.Self.GetApplicantPosting(toPostingId).Data;

                var remark = _remarkProvider.RegisterRemarkChanges(new RemarkDto
                {
                    Description = "Transferred application from " + applicant.Posting + " to " + newApplicantPosting.PostingNumber + " - " + newApplicantPosting.Description,
                    AddedBy = userId,
                    AddedDate = DateTime.Now,
                    ModifiedBy = userId,
                    Modified = DateTime.Now
                })
                .MergeInto(opResult)
                .Data;

                //Insert ApplicantNote
                var applicantNote = new ApplicantNote();
				applicantNote = new ApplicantNote()
				{
					ApplicantId = applicant.ApplicantId,
                    Remark = remark
				};
				_session.UnitOfWork.RegisterNew(applicantNote);

			}
			_session.UnitOfWork.Commit().MergeInto(opResult);

			return opResult;
		}

		#endregion

		#region ApplicantOnboardingTask

		IOpResult<ApplicantOnBoardingTaskDto> IApplicantTrackingService.GetApplicantOnboardingTask(
			int applicantOnboardingTaskId)
		{
			var result = new OpResult<ApplicantOnBoardingTaskDto>();

			_session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(result);
			if (result.HasError) return result;

			result.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository
				.ApplicantOnBoardingTaskQuery()
				.ByApplicantOnBoardingTaskId(applicantOnboardingTaskId)
				.ExecuteQueryAs(x => new ApplicantOnBoardingTaskDto
				{
					ApplicantOnboardingTaskId = x.ApplicantOnboardingTaskId,
					Description = x.Description,
					ClientId = x.ClientId,
					DefaultDaysToComplete = x.DefaultDaysToComplete,
					ProcessPhaseId = x.ProcessPhaseId,
					IsEnabled = x.IsEnabled,
					DefaultSpecialInstructions = x.DefaultSpecialInstructions,
                    ApplicantStatusTypeId = x.ApplicantStatusTypeId,
                    //AttachmentCount = x.ApplicantOnBoardingTaskAttachment.Count()
                    ApplicantOnboardingTaskAttachment = x.ApplicantOnBoardingTaskAttachment
						.Select(y => new ApplicantOnboardingTaskAttachmentDto()
						{
							ApplicantOnBoardingTaskAttachmentId = y.ApplicantOnBoardingTaskAttachmentId,
							ApplicantOnBoardingTaskId = y.ApplicantOnBoardingTaskId,
							ClientId = y.ClientId,
							Description = y.Description,
							FileType = y.FileType
						}).ToList()
				})
				.FirstOrDefault());

			return result;
		}

		IOpResult<IEnumerable<ApplicantOnBoardingTaskDto>> IApplicantTrackingService.GetApplicantOnboardingTasks(int clientId)
		{
			var opResult = new OpResult<IEnumerable<ApplicantOnBoardingTaskDto>>();
			_session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			_session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantOnBoardingTaskQuery()
					.ByClientId(clientId)
					.ExecuteQueryAs(x => new ApplicantOnBoardingTaskDto
					{
						ApplicantOnboardingTaskId = x.ApplicantOnboardingTaskId,
						Description = x.Description,
						ClientId = x.ClientId,
						DefaultDaysToComplete = x.DefaultDaysToComplete,
						ProcessPhaseId = x.ProcessPhaseId,
						IsEnabled = x.IsEnabled,
						DefaultSpecialInstructions = x.DefaultSpecialInstructions,
                        ApplicantStatusTypeId = x.ApplicantStatusTypeId,
						//AttachmentCount = x.ApplicantOnBoardingTaskAttachment.Count()
						ApplicantOnboardingTaskAttachment = x.ApplicantOnBoardingTaskAttachment
                                                                .Select(y => new ApplicantOnboardingTaskAttachmentDto()
                                                                {
                                                                    ApplicantOnBoardingTaskAttachmentId = y.ApplicantOnBoardingTaskAttachmentId,
                                                                    ApplicantOnBoardingTaskId = y.ApplicantOnBoardingTaskId,
                                                                    ClientId = y.ClientId,
                                                                    Description = y.Description,
                                                                    FileType = y.FileType
                                                                }).ToList()

					}).ToList());

			return opResult;
		}

		IOpResult<ApplicantOnBoardingTaskDto> IApplicantTrackingService.SaveApplicantOnboardingTask(ApplicantOnBoardingTaskDto dto)
		{
			var opResult = new OpResult<ApplicantOnBoardingTaskDto>();

			// check user has permission to save task
			_session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
			_session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, dto.ClientId).MergeInto(opResult);
			if (opResult.HasError) return opResult;

			if (dto.IsNewEntity(x => x.ApplicantOnboardingTaskId))
			{
				var newtask = new ApplicantOnBoardingTask()
				{
					ClientId = dto.ClientId,
					Description = dto.Description,
					DefaultDaysToComplete = dto.DefaultDaysToComplete,
					DefaultSpecialInstructions = dto.DefaultSpecialInstructions,

					DefaultAssignedToUserId = 0,// _session.LoggedInUserInformation.UserId;
					IsDefaultIsEmailRequired = true,
					ProcessPhaseId = 1,
                    ApplicantStatusTypeId = dto.ApplicantStatusTypeId,

					IsEnabled = true,
					ModifiedBy = _session.LoggedInUserInformation.UserId,
					Modified = DateTime.Now,
				};
				_session.SetModifiedProperties(newtask);
				_session.UnitOfWork.RegisterNew(newtask);
				_session.UnitOfWork.RegisterPostCommitAction(() => dto.ApplicantOnboardingTaskId = newtask.ApplicantOnboardingTaskId);
			}
			else
			{
				var existingTask = _session.UnitOfWork.ApplicantTrackingRepository
									.ApplicantOnBoardingTaskQuery()
									.ByApplicantOnBoardingTaskId(dto.ApplicantOnboardingTaskId)
									.FirstOrDefault();
				existingTask.Description = dto.Description;
				existingTask.DefaultDaysToComplete = dto.DefaultDaysToComplete;
				existingTask.DefaultSpecialInstructions = dto.DefaultSpecialInstructions;
                existingTask.ApplicantStatusTypeId = dto.ApplicantStatusTypeId;

				_session.SetModifiedProperties(existingTask);
				_session.UnitOfWork.RegisterModified(existingTask);

			}

			_session.UnitOfWork.Commit().MergeInto(opResult);
			dto.IsEnabled = true;
			opResult.SetDataOnSuccess(dto);

			return opResult;
		}

		IOpResult<ApplicantOnboardingTaskAttachmentDto> IApplicantTrackingService.SaveApplicantOnboardingTaskAttachment(ApplicantOnboardingTaskAttachmentDto dto)
		{
			var opResult = new OpResult<ApplicantOnboardingTaskAttachmentDto>();

			_session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
			_session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, dto.ClientId).MergeInto(opResult);
			if (opResult.HasError) return opResult;

			var entity = new ApplicantOnBoardingTaskAttachment
			{
				ApplicantOnBoardingTaskId = dto.ApplicantOnBoardingTaskId,
				ClientId = dto.ClientId,
				Description = dto.Description,
				FileLocation = dto.FileLocation,
				FileType = dto.FileType
			};

			_session.SetModifiedProperties(entity);
			_session.UnitOfWork.RegisterNewOrModified(entity, e => e.ApplicantOnBoardingTaskAttachmentId == 0);
			_session.UnitOfWork.RegisterPostCommitAction(() => dto.ApplicantOnBoardingTaskAttachmentId = entity.ApplicantOnBoardingTaskAttachmentId);
			_session.UnitOfWork.Commit().MergeInto(opResult);

			opResult.SetDataOnSuccess(dto);

			return opResult;
		}

		IOpResult IApplicantTrackingService.DeleteApplicantOnboardingTaskAttachment(int applicantOnBoardingTaskAttachmentId)
		{
			var opResult = new OpResult();
			_session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			var existingAttachment = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantOnboardingTaskAttachmentQuery()
					.ByApplicantOnBoardingTaskAttachmentId(applicantOnBoardingTaskAttachmentId).FirstOrDefault();

			_session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, existingAttachment.ClientId).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			if (existingAttachment != null)
			{
				if (File.Exists(existingAttachment.FileLocation))
				{
					File.Delete(existingAttachment.FileLocation);
				}
				_session.UnitOfWork.RegisterDeleted(existingAttachment);
				_session.UnitOfWork.Commit().MergeInto(opResult);

			}
			return opResult;
		}

		IOpResult<IEnumerable<ApplicantOnboardingTaskAttachmentDto>> IApplicantTrackingService.GetTaskAttachments(int applicantOnboardingTaskId)
		{
			var result = new OpResult<IEnumerable<ApplicantOnboardingTaskAttachmentDto>>();

			_session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(result);
			if (result.HasError) return result;

			return result.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository
				.ApplicantOnboardingTaskAttachmentQuery()
				.ByApplicantOnBoardingTaskId(applicantOnboardingTaskId)
				.ExecuteQueryAs(x => new ApplicantOnboardingTaskAttachmentDto
				{
					ApplicantOnBoardingTaskAttachmentId = x.ApplicantOnBoardingTaskAttachmentId,
					ApplicantOnBoardingTaskId = x.ApplicantOnBoardingTaskId,
					ClientId = x.ClientId,
					Description = x.Description,
					FileLocation = x.FileLocation,
					FileType = x.FileType
				})
				.ToList());
		}

		IOpResult<FileStreamDto> IApplicantTrackingService.GetTaskAttachment(int applicantOnBoardingTaskAttachmentId)
		{
			var opResult = new OpResult<FileStreamDto>();

			_session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
			if (opResult.HasError) return opResult;

			var attachment = _session.UnitOfWork.ApplicantTrackingRepository
				.ApplicantOnboardingTaskAttachmentQuery()
				.ByApplicantOnBoardingTaskAttachmentId(applicantOnBoardingTaskAttachmentId)
				.FirstOrDefault();

			_session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, attachment.ClientId).MergeInto(opResult);
			if (opResult.HasError) return opResult;

			if (attachment == null)
			{
				opResult.SetToFail();
				return opResult;
			}

			var ext = Path.GetExtension(attachment.FileLocation);
			var mimeType = "";
			switch (ext)
			{
				case ".pdf":
					mimeType = "application/pdf";
					break;
				case ".doc":
					mimeType = "application/msword";
					break;
				case ".docx":
					mimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
					break;
				case ".txt":
					mimeType = "text/plain";
					break;
				case ".rtf":
					mimeType = "application/rtf";
					break;
				default:
					mimeType = "text/plain";
					break;
			}

			var fileName = Path.GetFileName(attachment.FileLocation);
			var fs = File.Open(attachment.FileLocation, FileMode.Open, FileAccess.Read);
			Stream fileStream = fs;

			opResult.TrySetData(() => new FileStreamDto
			{
				FileExtension = ext,
				FileName = fileName,
				FileStream = fileStream,
				MimeType = mimeType
			});

			return opResult;
		}

		IOpResult IApplicantTrackingService.DeleteApplicantOnboardingTask(int applicantOnBoardingTaskId)
		{
			var opResult = new OpResult();
			_session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			var existingTask = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantOnBoardingTaskQuery()
					.ByApplicantOnBoardingTaskId(applicantOnBoardingTaskId).FirstOrDefault();

			_session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, existingTask.ClientId).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			if (existingTask != null)
			{
				existingTask.IsEnabled = false;
				_session.UnitOfWork.RegisterModified(existingTask);
				_session.UnitOfWork.Commit().MergeInto(opResult);
			}
			return opResult;
		}
		IOpResult<ApplicantOnBoardingTaskDto> IApplicantTrackingService.CopyApplicantOnboardingTask(int applicantOnBoardingTaskId)
		{
			var opResult = new OpResult<ApplicantOnBoardingTaskDto>();
			_session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			var existingTask = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantOnBoardingTaskQuery()
					.ByApplicantOnBoardingTaskId(applicantOnBoardingTaskId).FirstOrDefault();

			_session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, existingTask.ClientId).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			var newTask = new ApplicantOnBoardingTask();
			if (existingTask != null)
			{

				newTask.ClientId = existingTask.ClientId;
				newTask.Description = "Copy of :" + existingTask.Description;
				newTask.DefaultDaysToComplete = existingTask.DefaultDaysToComplete;
				newTask.IsEnabled = true;
				newTask.ModifiedBy = _session.LoggedInUserInformation.UserId;
				newTask.Modified = DateTime.Now;
				newTask.DefaultAssignedToUserId = existingTask.DefaultAssignedToUserId;
				newTask.IsDefaultIsEmailRequired = existingTask.IsDefaultIsEmailRequired;
				newTask.DefaultSpecialInstructions = existingTask.DefaultSpecialInstructions;
				newTask.ProcessPhaseId = existingTask.ProcessPhaseId;
                newTask.ApplicantStatusTypeId = existingTask.ApplicantStatusTypeId;
				_session.UnitOfWork.RegisterNew(newTask);
				_session.UnitOfWork.Commit().MergeInto(opResult);
				if (opResult.Success)
				{
					var existingAttachments = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantOnboardingTaskAttachmentQuery()
					.ByApplicantOnBoardingTaskId(applicantOnBoardingTaskId)
					.ExecuteQuery()
					.ToList();

					var baseFolder = "";
					foreach (var existingAttachment in existingAttachments)
					{
						var newAttachment = new ApplicantOnBoardingTaskAttachment();
						baseFolder = Path.Combine(@"C:\upload\", _session.LoggedInUserInformation.ClientId.GetValueOrDefault().ToString(), "TaskAttachments", newTask.ApplicantOnboardingTaskId.ToString());
						newAttachment.ApplicantOnBoardingTaskId = newTask.ApplicantOnboardingTaskId;
						newAttachment.ClientId = existingAttachment.ClientId;
						newAttachment.Description = existingAttachment.Description;
						newAttachment.FileLocation = Path.Combine(baseFolder, existingAttachment.Description);
						newAttachment.Modified = DateTime.Now;
						newAttachment.ModifiedBy = _session.LoggedInUserInformation.UserId;
						newAttachment.FileType = existingAttachment.FileType;
						_session.UnitOfWork.RegisterNew(newAttachment);

						if (!Directory.Exists(baseFolder))
						{
							Directory.CreateDirectory(baseFolder);
						}
						if (File.Exists(existingAttachment.FileLocation))
						{
							File.Copy(existingAttachment.FileLocation, newAttachment.FileLocation, true);
						}

					}
					_session.UnitOfWork.Commit().MergeInto(opResult);
					if (opResult.Success)
					{
						opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantOnBoardingTaskQuery()
										.ByApplicantOnBoardingTaskId(newTask.ApplicantOnboardingTaskId)
										.ExecuteQueryAs(x => new ApplicantOnBoardingTaskDto
										{
											ApplicantOnboardingTaskId = x.ApplicantOnboardingTaskId,
											Description = x.Description,
											ClientId = x.ClientId,
											DefaultDaysToComplete = x.DefaultDaysToComplete,
											ProcessPhaseId = x.ProcessPhaseId,
											IsEnabled = x.IsEnabled,
											DefaultSpecialInstructions = x.DefaultSpecialInstructions,
                                            ApplicantStatusTypeId = x.ApplicantStatusTypeId,
											//AttachmentCount = x.ApplicantOnBoardingTaskAttachment.Count()
											ApplicantOnboardingTaskAttachment = x.ApplicantOnBoardingTaskAttachment
																					.Select(y => new ApplicantOnboardingTaskAttachmentDto()
																					{
																						ApplicantOnBoardingTaskAttachmentId = y.ApplicantOnBoardingTaskAttachmentId,
																						ApplicantOnBoardingTaskId = y.ApplicantOnBoardingTaskId,
																						ClientId = y.ClientId,
																						Description = y.Description,
																						FileType = y.FileType
																					}).ToList()

										}).FirstOrDefault());
					}
				}
			}
			return opResult;
		}

		#endregion

		IOpResult<DateTime> IApplicantTrackingService.GetMinimumPublishedDate(int clientId, bool openPostingsOnly)
		{
			var opResult = new OpResult<DateTime>();
			var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
			var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantTrackingEnabled || isSystemAdmin)
			{
				var query = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
					.ByClientId(clientId)
					.ByIsActive(true);

				if (openPostingsOnly)
				{
					query.ByIsClosed(false);
				}

				DateTime minPublishedDate = query
					.ExecuteQueryAs(x => x.PublishedDate)
					.DefaultIfEmpty(new DateTime(2006, 1, 1)).Min();

				opResult.Data = minPublishedDate;
			}

			return opResult;
		}

		IOpResult<FileStreamDto> IApplicantTrackingService.GetApplicantResumeToDownload(int applicantResumeId)
		{
            IOpResult<FileStreamDto> result =  new OpResult<FileStreamDto>();
            IOpResult<ApplicantResumeDto> resumeResult = this.Self.GetApplicantResume(applicantResumeId);
            if (!resumeResult.Success)
            {
                result.AddMessage(new GenericMsg("Cannot find the resume."));
                result.SetToFail();
                return result;
            }

            ApplicantResumeDto resume = resumeResult.Data;
            ApplicantDto applicant = this.Self.GetApplicantByApplicantId(resume.ApplicantId).Data;
            FileStreamDto fileStreamDto = new FileStreamDto();
            IDocxToPdfConverter converter = new DocxToPdfConverter();
            string fileName = "";
            string ext = "";
            byte[] contnt = null;

            if (resume.LinkLocation.Contains(@"C:\"))
			{
                fileName = Path.GetFileName(resume.LinkLocation);
                ext = Path.GetExtension(resume.LinkLocation);
                contnt = File.ReadAllBytes(resume.LinkLocation);

                if(ext.ToLower() == ".doc" | ext.ToLower() == ".docx" | ext.ToLower() == ".rtf")
                {
                    byte[] convResult = converter.Convert(contnt);
                    contnt = convResult;
                    fileName = fileName.Replace(ext, ".pdf");
                    ext = ".pdf";                    
                }

                if (contnt != null && contnt.Length > 0 && !string.IsNullOrEmpty(ConfigValues.AzureFile))
                    SaveResumeToAzure(applicant.ClientId, applicant.ApplicantId, applicantResumeId, fileName, contnt).MergeInto(result);
            }
            else
            {
                FileStreamDto blob = _azureResourceProvider.GetBlob(resume.LinkLocation, ConfigValues.AzureFile).Data;
                fileName = blob.FileName;
                ext = blob.FileExtension;
                contnt = blob.FileStream.ToByteArray();

                if (ext.ToLower() == ".doc" | ext.ToLower() == ".docx" | ext.ToLower() == ".rtf")
				{
                    byte[] convResult = converter.Convert(contnt);
                    contnt = convResult;
                    fileName = fileName.Replace(ext, ".pdf");
                    ext = ".pdf";

                    if (contnt != null && contnt.Length > 0 && !string.IsNullOrEmpty(ConfigValues.AzureFile))
                        SaveResumeToAzure(applicant.ClientId, applicant.ApplicantId, applicantResumeId, fileName, contnt).MergeInto(result);
                }
            }

            if (result.Success)
            {
                result.Data = new FileStreamDto {
                    FileName = fileName,
					FileExtension = ext,
					MimeType = MimeTypeMap.GetMimeType(ext),
                    FileStream = new MemoryStream(contnt)
                };
            }
            return result;
        }

        public IOpResult<ApplicantResumeDto> SaveResumeToAzure(int clientId, int applicantId, int? applicantResumeId, string fileName, byte[] fileData)
        {
            var result = new OpResult<ApplicantResumeDto>();

            var resumeResource = _azureResourceProvider.CreateOrUpdateApplicantAzureFile(clientId, applicantId, fileName, fileData,
                ConfigValues.AzureFile, true).Data;
            if (string.IsNullOrEmpty(resumeResource))
            {
                result.AddMessage(new GenericMsg("Azure file failed to create."));
                result.SetToFail();
                return result;
            }

            ApplicantResume resume = new ApplicantResume()
            {
                ApplicantResumeId = applicantResumeId ?? 0,
                ApplicantId = applicantId,
                DateAdded = DateTime.Now,
                LinkLocation = resumeResource,
                };
            _session.UnitOfWork.RegisterNewOrModified(resume, e => e.ApplicantResumeId == 0);
            _session.UnitOfWork.Commit().MergeInto(result);

            result.SetDataOnSuccess(new ApplicantResumeDto()
            {
                ApplicantResumeId = resume.ApplicantResumeId,
                ApplicantId = applicantId,
                DateAdded = resume.DateAdded,
                LinkLocation = resumeResource
			});
            return result;
		}

        IOpResult<FileStreamDto> IApplicantTrackingService.GetApplicantApplicationToDownload(int applicationHeaderId, string cssUrl, Func<int, CorrespondenceType, IOpResult<DisclaimerDetailDto>> getDisclaimerFactory)
		{
			OpResult<FileStreamDto> opResult = new OpResult<FileStreamDto>();

            //_session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
            //if (opResult.HasError)
            //    return opResult;


			_session.CanPerformAction(ApplicantTrackingActionType.ReadApplicantInfo).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			ApplicantApplicationHeaderDto applicationHeader = this.Self.GetApplicantApplicationHeader(applicationHeaderId).Data;
			ApplicantDto applicant = this.Self.GetApplicantByApplicantId(applicationHeader.ApplicantId).Data;
			IEnumerable<ApplicantSkillDto> skills = this.Self.GetApplicantSkillsByApplicantId(applicationHeader.ApplicantId).Data;
			IEnumerable<ApplicantEducationHistoryDto> education = this.Self.GetApplicantEducationHistoryByApplicantId(applicationHeader.ApplicantId).Data;
			IEnumerable<ApplicantEmploymentHistoryDto> employment = this.Self.GetApplicantEmploymentHistoryByApplicantId(applicationHeader.ApplicantId).Data;
			IEnumerable<ApplicantReferenceDto> references = this.Self.GetApplicantReferencesByApplicantId(applicationHeader.ApplicantId).Data;
			IEnumerable<ApplicationSectionWithQuestionsDto> sections = this.Self.GetQuestionsForApplication(applicationHeaderId).Data;
			IEnumerable<ApplicationQuestionResponseDto> responses = this.Self.GetApplicantQuestionResponses(applicationHeaderId).Data;
            DisclaimerDetailDto disclaimerDetails = getDisclaimerFactory(applicationHeaderId, CorrespondenceType.Disclaimer).Data;

			var applicationTemplate = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantCompanyCorrespondenceQuery()
				.ByIsActive(false).ByCorrespondenceTypeId((ApplicantCorrespondenceType)1).ExecuteQueryAs(x => new { x.Description, x.Body })
				.Where(y => y.Description == "Applicant Application").FirstOrDefault();

			ApplicantApplicationPdf pdf = new ApplicantApplicationPdf(applicant, applicationHeader, education, employment,
				skills, references, sections, responses, disclaimerDetails);

			pdf.Initialize(applicationTemplate.Body, cssUrl);

			return new OpResult<FileStreamDto>().TrySetData(() =>
			{
				return new FileStreamDto
				{
					FileName = applicant.FirstName + " Application.pdf",
					FileExtension = ".pdf",
					MimeType = MimeTypeMap.GetMimeType(".pdf"),
					FileStream = new MemoryStream(pdf.GetPdf())
				};
			});
		}
        
		IOpResult<ApplicantsStatusChangeDto> IApplicantTrackingService.ChangeApplicantsStatus(ApplicantsStatusChangeDto dto)
		{
			var opResult = new OpResult<ApplicantsStatusChangeDto>();

			_session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			_session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, dto.ClientId).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			foreach (var applicant in dto.Applicants)
			{
				var applicationHeader = new ApplicantApplicationHeader();
				int applicationHeaderId = applicant.ApplicationHeaderId;
				applicationHeader = _session.UnitOfWork.ApplicantTrackingRepository
											.ApplicantApplicationHeaderQuery()
											.ByApplicationHeaderId(applicationHeaderId)
											.FirstOrDefault();

				applicationHeader.ApplicantStatusTypeId = dto.ToStatusId;

				applicationHeader.ApplicantRejectionReasonId = (int?)null;

				//For now, We are not updating any Rejection Reason by default. It is handled as a seperate call.
					if (dto.ToStatusId == ApplicantStatusType.Rejected)
				{
					//applicationHeader.ApplicantRejectionReasonId = 3;
				}

					if (dto.ToStatusId == ApplicantStatusType.Applicant)
				{
					applicationHeader.IsRecommendInterview = false;
				}

					if (dto.ToStatusId == ApplicantStatusType.Candidate)
				{
					applicationHeader.IsRecommendInterview = true;
				}

				//Update ApplicantApplicationHeader
				_session.UnitOfWork.RegisterModified(applicationHeader);


					if (dto.ToStatusId == ApplicantStatusType.Candidate && dto.IsApplicantHiringWorkflowEnabled && applicant.ApplicantOnBoardingProcessId > 0)
				{
					var applicantOnBoardingHeaderDto = this.Self.GetApplicantOnBoardingHeaderByApplicationHeaderIdAndOnboardingProcessId(applicationHeaderId, applicant.ApplicantOnBoardingProcessId).Data;
					//Ensure that an entry in ApplicantOnBoardingHeader doesn't already exists
					if (applicantOnBoardingHeaderDto == null)
					{
						var applicantOnBoardingHeader = new ApplicantOnBoardingHeader();
						applicantOnBoardingHeader.ApplicantOnBoardingHeaderId = 0;
						applicantOnBoardingHeader.ApplicantApplicationHeaderId = applicationHeaderId;
						applicantOnBoardingHeader.ApplicantOnBoardingProcessId = applicant.ApplicantOnBoardingProcessId;
						applicantOnBoardingHeader.ApplicantId = applicant.ApplicantId;
						applicantOnBoardingHeader.ClientId = dto.ClientId;
						applicantOnBoardingHeader.OnBoardingStarted = null;
						applicantOnBoardingHeader.OnBoardingEnded = null;
						applicantOnBoardingHeader.IsHired = false;
						applicantOnBoardingHeader.IsRejected = false;
						applicantOnBoardingHeader.ModifiedBy = dto.CurrentUserId;
						applicantOnBoardingHeader.Modified = DateTime.Now;

						_session.UnitOfWork.RegisterNew(applicantOnBoardingHeader);
						_session.UnitOfWork.Commit().MergeInto(opResult);


						var defaultTasks = this.Self.GetApplicantOnBoardingDefaultDetailByProcessIdAndPostingId(applicant.ApplicantOnBoardingProcessId, applicationHeader.PostingId).Data;
						foreach (var taskDto in defaultTasks)
						{
							if (taskDto.IsAutoStart)
							{
								applicantOnBoardingHeader.OnBoardingStarted = DateTime.Now;
								applicantOnBoardingHeader.Modified = DateTime.Now;
								applicantOnBoardingHeader.ModifiedBy = dto.CurrentUserId;

								_session.UnitOfWork.RegisterModified(applicantOnBoardingHeader);
								_session.UnitOfWork.Commit().MergeInto(opResult);
							}

							var applicantOnboardingDetail = new ApplicantOnBoardingDetail();
							applicantOnboardingDetail.ApplicantOnBoardingDetailId = 0;
							applicantOnboardingDetail.ApplicantOnBoardingTaskId = taskDto.ApplicantOnBoardingTaskId;
							applicantOnboardingDetail.AssignedToUserId = taskDto.AssignedToUserId;
							applicantOnboardingDetail.IsEmailRequired = taskDto.IsEmailRequired;
							applicantOnboardingDetail.DaysToComplete = taskDto.DaysToComplete;
							applicantOnboardingDetail.SpecialInstructions = taskDto.SpecialInstructions;
							applicantOnboardingDetail.DateCompleted = null;
							applicantOnboardingDetail.IsCompleted = false;
							applicantOnboardingDetail.ModifiedBy = dto.CurrentUserId;
							applicantOnboardingDetail.Modified = DateTime.Now;
							applicantOnboardingDetail.ApplicantCorrespondenceId = 0;
							applicantOnboardingDetail.ApplicantOnBoardingHeaderId =
								applicantOnBoardingHeader.ApplicantOnBoardingHeaderId;
							applicantOnboardingDetail.DateStarted = (taskDto.IsAutoStart) ? DateTime.Now : (DateTime?)null;

							_session.UnitOfWork.RegisterNew(applicantOnboardingDetail);
							_session.UnitOfWork.Commit().MergeInto(opResult);
						}
					}
				}
			}
			_session.UnitOfWork.Commit().MergeInto(opResult);
			opResult.Data = new ApplicantsStatusChangeDto();

			return opResult;
		}

		IOpResult IApplicantTrackingService.SendEmailByApplicationHeaderIdsAndCorrespondenceId(
			List<ApplicantDetailDto> applicantsList, int? correspondenceId, string subject, string body, bool? isText)
		{
			var opResult = new OpResult();

			_session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

            ApplicantCompanyCorrespondenceDto dataCorrespondence = null;
            if (correspondenceId.HasValue)
            {
                var qryCorrespondence = _session.UnitOfWork.ApplicantTrackingRepository
                    .ApplicantCompanyCorrespondenceQuery()
                    .ByCorrespondenceId(correspondenceId.Value);

                dataCorrespondence = qryCorrespondence.ExecuteQueryAs(x => new ApplicantCompanyCorrespondenceDto() 
                {
                    ApplicantCompanyCorrespondenceId = x.ApplicantCompanyCorrespondenceId,
                    Body = x.Body,
                    Subject = x.Subject,
                    Description = x.Description,
                    ClientId = x.ClientId,
                    IsText = x.IsText.HasValue ? x.IsText.Value : false,
                }).FirstOrDefault();
            }
            else if (!string.IsNullOrEmpty(body))
            {
                dataCorrespondence = new ApplicantCompanyCorrespondenceDto()
                {
                    ApplicantCompanyCorrespondenceId = -1, // Indication for customized messaage
                    Body = body,
                    Subject = subject,
                    Description = subject,
                    ClientId = _session.LoggedInUserInformation.ClientId.Value,
                    IsActive = true,
                    IsText = isText.HasValue ? isText.Value : false,
                };
            }

            string senderEmail = _session.UnitOfWork.UserRepository
                .QueryUsers()
                .ByUserId(_session.LoggedInUserInformation.UserId).ExecuteQueryAs(x => x.EmailAddress).FirstOrDefault();

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, dataCorrespondence.ClientId)
				.MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			var smtpSetting = _clientService.GetClientSMTPSetting(dataCorrespondence.ClientId);

			// Company Logo
			AzureViewDto imgSource = null;
			imgSource = _clientAzureService.GetAzureClientResource(ResourceSourceType.AzureClientImage, dataCorrespondence.ClientId, "logo").Data;
			string imgUrl = imgSource?.Source;

			// Company Name
			string companyName = _session.UnitOfWork.ClientRepository.QueryClients().ByClientId(dataCorrespondence.ClientId).ExecuteQueryAs(x =>
					(x.ApplicantClient.JobBoardTitle == null ? x.ClientName : x.ApplicantClient.JobBoardTitle)).FirstOrDefault();


			foreach (ApplicantDetailDto applicant in applicantsList)
			{
				var qry = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantApplicationHeaderQuery()
					.ByApplicationHeaderId(applicant.ApplicationHeaderId);

				var replacementData = qry.ExecuteQueryAs(x => new ApplicantTrackingCorrespondenceReplacementInfoDto
				{
					ApplicantClientId = x.Applicant.ClientId,
                    ApplicantFirstName = x.Applicant.FirstName,
                    ApplicantLastName = x.Applicant.LastName,
					ApplicantEmail = x.Applicant.EmailAddress,
                    UserName = x.Applicant.User != null ? x.Applicant.User.UserName : "",
                    Posting = x.ApplicantPosting.Description,
					Date = DateTime.Now,
					Address = x.Applicant.AddressLine2 != ""
						? x.Applicant.Address + "<br/>" + x.Applicant.AddressLine2 + "<br/>" + x.Applicant.City + ", " +
						  x.Applicant.StateDetails.Name + " " + x.Applicant.Zip
						: x.Applicant.Address + "<br/>" + x.Applicant.City + ", " + x.Applicant.StateDetails.Name +
						  " " + x.Applicant.Zip,
					Phone = x.Applicant.PhoneNumber,
					CompanyAddress = x.Applicant.Client.AddressLine2 != ""
						? x.Applicant.Client.AddressLine1 + "<br/>" + x.Applicant.Client.AddressLine2 + "<br/>" +
						  x.Applicant.Client.City + ", " + x.Applicant.Client.State.Name + " " +
						  x.Applicant.Client.PostalCode
						: x.Applicant.Client.AddressLine1 + "<br/>" + x.Applicant.Client.City + ", " +
						  x.Applicant.Client.State.Name + " " + x.Applicant.Client.PostalCode,
					CompanyName = x.Applicant.Client.ApplicantClient.JobBoardTitle == null ? x.Applicant.Client.ClientName : x.Applicant.Client.ApplicantClient.JobBoardTitle,
					CompanyLogo = imgUrl
				}).FirstOrDefault();

				if (dataCorrespondence.Body != null)
				{

					// Send correspondence email and handle notifications
					var emailResult = _applicantTrackingNotificationService.ProcessCorrespondenceNotification(applicant,
						correspondenceId, dataCorrespondence.Subject , dataCorrespondence.Body, replacementData, isText);
					if (emailResult.HasError)
					{
						opResult
                            .AddMessage(new GenericMsg("Email notification not sent to: " + replacementData.ApplicantFirstName + " " + replacementData.ApplicantLastName + " Reason: " +
													   emailResult.MsgObjects[0].Msg))
							.SetToFail();
						return opResult;
					}

                    var applicationEmail = new ApplicantApplicationEmailHistory();
                    applicationEmail.ApplicationHeaderId = applicant.ApplicationHeaderId;
                    applicationEmail.ApplicantCompanyCorrespondenceId = correspondenceId;
                    applicationEmail.ApplicantStatusTypeId = applicant.ApplicantStatusTypeId;
                    applicationEmail.SenderId = _session.LoggedInUserInformation.UserId;
                    applicationEmail.SentDate = DateTime.Now;
                    applicationEmail.Subject = dataCorrespondence.Subject;
                    applicationEmail.Body = dataCorrespondence.Body;
                    applicationEmail.SenderEmail = senderEmail;

                    _session.UnitOfWork.RegisterNew(applicationEmail);
					_session.UnitOfWork.Commit().MergeInto(opResult);

				}

			}

			return opResult;
		}
	

		IOpResult IApplicantTrackingService.UpdateApplicantsRejectionReason(List<ApplicantDetailDto> applicantsList, int rejectionReasonId)
		{
			var opResult = new OpResult();

			_session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			foreach (ApplicantDetailDto applicant in applicantsList)
			{
				var applicationHeader = _session.UnitOfWork.ApplicantTrackingRepository
												.ApplicantApplicationHeaderQuery()
												.ByApplicationHeaderId(applicant.ApplicationHeaderId)
												.FirstOrDefault();

				applicationHeader.ApplicantRejectionReasonId = rejectionReasonId;
				_session.UnitOfWork.RegisterModified(applicationHeader);
			}
			_session.UnitOfWork.Commit().MergeInto(opResult);

			return opResult;
		}


		#endregion

		#region Company settings
		IOpResult<ApplicantClientDto> IApplicantTrackingService.GetApplicantClientByID(int? clientId)
		{
			var opResult = new OpResult<ApplicantClientDto>();
			if (!clientId.HasValue)
				clientId = _session.LoggedInUserInformation.ClientId;

			opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository
				.QueryApplicantClient()
				.ByClientId(clientId.Value)
				.ExecuteQueryAs(x => new ApplicantClientDto
				{
					ClientId = x.ClientId,
					ClientName = x.Client.ClientName,
					CorrespondenceEmailAddress = x.CorrespondenceEmailAddress,
					JobBoardTitle = x.JobBoardTitle == null ? x.Client.ClientName : x.JobBoardTitle,
					ShowAboutPage = x.ShowAboutPage == null ? false : x.ShowAboutPage.Value,
					CompanyURL  = x.CompanyURL,
					PhotoCaption = x.PhotoCaption,
					AboutUs = x.AboutUs,
					Logo = new AzureViewDto(),
					Photo = new AzureViewDto()
				})
				.FirstOrDefault());

			if (!opResult.HasData) return opResult;

			IOpResult<AzureViewDto> img = new OpResult<AzureViewDto>();
			img = _clientAzureService.GetAzureClientResource(ResourceSourceType.AzureClientImage, (int)clientId, "logo");
            if (img.Success) opResult.Data.Logo = img.Data;

			img = _clientAzureService.GetAzureClientResource(ResourceSourceType.AzureClientImage, (int)clientId, "hero");
            if (img.Success) opResult.Data.Photo = img.Data;

			return opResult;
		}

		IOpResult<IEnumerable<ClientJobSiteDto>> IApplicantTrackingService.GetClientJobSitesByID(int? clientId)
		{
			var opResult = new OpResult<IEnumerable<ClientJobSiteDto>>();
			if (!clientId.HasValue)
				clientId = _session.LoggedInUserInformation.ClientId;

			_session
				.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin)
				.MergeInto(opResult);

			_session
				.ResourceAccessChecks
				.CheckAccessById(ResourceOwnership.Client, clientId.Value)
				.MergeInto(opResult);

			if (opResult.HasError)
				return opResult;

			opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ClientJobSiteQuery()
					.ByClientId(clientId.Value)
					.ExecuteQueryAs(x => new ClientJobSiteDto
					{
						ClientId = x.ClientId,
						ClientJobSiteId = x.ClientJobSiteId,
						ApplicantJobSiteId = x.ApplicantJobSiteId,
						JobSiteDescription = x.ApplicantJobSite.JobSiteDescription,
						Code = x.Code,
						Counter = x.Counter,
						SharePosts = x.SharePosts,
						Email = x.Email
					}).ToList());

			return opResult;
		}

        IOpResult<ClientJobSiteDto> IApplicantTrackingService.GetClientJobSiteByClientIdAndApplicantJobSite(int? clientId, ApplicantJobSiteEnum applicantJobSite)
        {
            var opResult = new OpResult<ClientJobSiteDto>();
            if (!clientId.HasValue)
                clientId = _session.LoggedInUserInformation.ClientId;

            _session
                .CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin)
                .MergeInto(opResult);

            _session
                .ResourceAccessChecks
                .CheckAccessById(ResourceOwnership.Client, clientId.Value)
                .MergeInto(opResult);

            if (opResult.HasError)
                return opResult;

            opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ClientJobSiteQuery()
                    .ByClientId(clientId.Value)
                    .ByApplicantJobSiteId(Dto.Enums.ApplicantJobSiteEnum.Indeed)
                    .ExecuteQueryAs(x => new ClientJobSiteDto
                    {
                        ClientId = x.ClientId,
                        ClientJobSiteId = x.ClientJobSiteId,
                        ApplicantJobSiteId = x.ApplicantJobSiteId,
                        JobSiteDescription = x.ApplicantJobSite.JobSiteDescription,
                        Code = x.Code,
                        Counter = x.Counter,
                        SharePosts = x.SharePosts,
                        Email = x.Email
                    }).FirstOrDefault());

            return opResult;
        }

        /// <summary>
        /// Gets all ClientJobSite records by id, loops through them and sets SharePosts to false.
        /// Currently Indeed is the only vendor being used but method will accomodate any new vendors.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IOpResult<IEnumerable<ClientJobSiteDto>> IApplicantTrackingService.UpdateClientJobSiteSharePost(int? clientId)
		{
			var opResult = new OpResult<IEnumerable<ClientJobSiteDto>>();

			var currentClientJobSites = _session.UnitOfWork.ApplicantTrackingRepository.ClientJobSiteQuery()
				.ByClientId(clientId.Value)
				.ExecuteQueryAs(dto => new
				{
					dto.ClientJobSiteId
				}).ToList();

			foreach (var x in currentClientJobSites)
			{
				var changedProperties = new PropertyList<ClientJobSite>();

				var entity = new ClientJobSite
				{
					ClientJobSiteId = x.ClientJobSiteId,
					SharePosts = false
				};

				_session.SetModifiedProperties(entity);

				changedProperties.Add(y => y.SharePosts);
				changedProperties.Add(y => y.Modified);
				changedProperties.Add(y => y.ModifiedBy);

				_session.UnitOfWork.RegisterModified(entity, changedProperties);
			}

			_session.UnitOfWork.Commit().MergeInto(opResult);

			return opResult;
		}

		IOpResult IApplicantTrackingService.SaveApplicantClient(ApplicantClientDto dto)
		{
			var opResult = new OpResult();

			_session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			_session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, dto.ClientId).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			var existingSettings = _session.UnitOfWork.ApplicantTrackingRepository.QueryApplicantClient()
											.ByClientId(dto.ClientId)
											.FirstOrDefault();
			if (dto.JobBoardTitle != null && dto.JobBoardTitle.Trim().Length == 0)
				dto.JobBoardTitle = null;

			if (existingSettings == null)
			{
				ApplicantClient newSettings = new ApplicantClient();
				newSettings.ClientId = dto.ClientId;
				newSettings.CorrespondenceEmailAddress = dto.CorrespondenceEmailAddress;
				newSettings.JobBoardTitle = dto.JobBoardTitle;

				newSettings.ShowAboutPage = dto.ShowAboutPage;
				newSettings.CompanyURL = dto.CompanyURL;

				if (dto.Photo != null)
				{
					//TODO: Save the photo byte stream to azure along with file name
				}
				if (dto.Logo != null)
				{
					//TODO: Save the logo byte stream to azure along with file name
				}

				newSettings.PhotoCaption = dto.PhotoCaption;
				newSettings.AboutUs = dto.AboutUs;
				_session.SetModifiedProperties(newSettings);
				_session.UnitOfWork.RegisterNew(newSettings);
			}
			else
			{
				existingSettings.CorrespondenceEmailAddress = dto.CorrespondenceEmailAddress;
				existingSettings.JobBoardTitle = dto.JobBoardTitle;

				existingSettings.ShowAboutPage = dto.ShowAboutPage;
				existingSettings.CompanyURL = dto.CompanyURL;

				if (dto.Photo != null)
				{
					//TODO: Save the photo byte stream to azure along with file name
				}
				if (dto.Logo != null)
				{
					//TODO: Save the logo byte stream to azure along with file name
				}

				existingSettings.PhotoCaption = dto.PhotoCaption;
				existingSettings.AboutUs = dto.AboutUs;

				_session.SetModifiedProperties(existingSettings);
				_session.UnitOfWork.RegisterModified(existingSettings);
			}
			_session.UnitOfWork.Commit().MergeInto(opResult);

			return opResult;
		}

		IOpResult<IEnumerable<ApplicantJobSiteDto>> IApplicantTrackingService.GetApplicantJobSites()
		{
			var opResult = new OpResult<IEnumerable<ApplicantJobSiteDto>>();
			_session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantJobSiteQuery()
					.ExecuteQueryAs(x => new ApplicantJobSiteDto
					{
						ApplicantJobSiteId = x.ApplicantJobSiteId,
						JobSiteDescription = x.JobSiteDescription

					}).ToList());

			return opResult;
		}

		IOpResult<ClientJobSiteDto> IApplicantTrackingService.SaveClientJobSite(ClientJobSiteDto dto)
		{
			var opResult = new OpResult<ClientJobSiteDto>();

			_session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			_session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, dto.ClientId).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;


			if (dto.IsNewEntity(x => x.ClientJobSiteId))
			{
				var newClientJobSite = new ClientJobSite()
				{
					ApplicantJobSiteId = dto.ApplicantJobSiteId,
					ClientId = dto.ClientId,
					Code = dto.Code,
					Counter = 0,
					SharePosts = dto.SharePosts,
					Email = dto.Email
				};
				_session.SetModifiedProperties(newClientJobSite);
				_session.UnitOfWork.RegisterNew(newClientJobSite);
				_session.UnitOfWork.RegisterPostCommitAction(() => dto.ClientJobSiteId = newClientJobSite.ClientJobSiteId);
			}
			else
			{
				var existingClientJobSite = _session.UnitOfWork.ApplicantTrackingRepository.ClientJobSiteQuery()
										.ByClientJobSiteId(dto.ClientJobSiteId)
										.FirstOrDefault();

				opResult.CheckForNotFound(existingClientJobSite);
				if (opResult.HasError)
					return opResult;

				existingClientJobSite.Code = dto.Code;
				existingClientJobSite.SharePosts = dto.SharePosts;
				existingClientJobSite.Email = dto.Email;

				_session.SetModifiedProperties(existingClientJobSite);
				_session.UnitOfWork.RegisterModified(existingClientJobSite);
			}
			_session.UnitOfWork.Commit().MergeInto(opResult);
			if (opResult.Success)
			{
				opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ClientJobSiteQuery()
				.ByClientId(dto.ClientId)
				.ByClientJobSiteId(dto.ClientJobSiteId)
				.ExecuteQueryAs(x => new ClientJobSiteDto
				{
					ClientId = x.ClientId,
					ClientJobSiteId = x.ClientJobSiteId,
					ApplicantJobSiteId = x.ApplicantJobSiteId,
					JobSiteDescription = x.ApplicantJobSite.JobSiteDescription,
					Code = x.Code,
					Counter = x.Counter,
					SharePosts = x.SharePosts,
					Email = x.Email
				}).FirstOrDefault());
			}
			//opResult.SetDataOnSuccess(dto);
			return opResult;
		}

		IOpResult IApplicantTrackingService.DeleteClientJobSite(int clientJobSiteId)
		{
			var opResult = new OpResult();

			_session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			var existingClientJobSite = _session.UnitOfWork.ApplicantTrackingRepository.ClientJobSiteQuery()
										.ByClientJobSiteId(clientJobSiteId)
										.FirstOrDefault();

			opResult.CheckForNotFound(existingClientJobSite);
			if (opResult.HasError)
				return opResult;

			_session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, existingClientJobSite.ClientId).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			_session.UnitOfWork.RegisterDeleted(existingClientJobSite);
			_session.UnitOfWork.Commit().MergeInto(opResult);
			return opResult;
		}

		#endregion

		IOpResult<IEnumerable<ApplicationSectionWithQuestionsDto>> IApplicantTrackingService.GetQuestionsForApplication(int applicationHeaderId)
		{
			var result = new OpResult<IEnumerable<ApplicationSectionWithQuestionsDto>>();

			var isAdmin = _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin);
			var isApplicant = _session.CanPerformAction(ApplicantTrackingActionType.ReadApplicantInfo);

			if (isAdmin.HasError && isApplicant.HasError)
			{
				isAdmin.MergeInto(result);
				isApplicant.MergeInto(result);
				return result;
			}



            return result.TrySetData(() =>
            {
                //load application question info for the application applied to the posting
                var applicationInfo = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantApplicationHeaderQuery()
                    .ByApplicationHeaderId(applicationHeaderId)
                    .ExecuteQueryAs(x => new
                    {
                        x.ApplicantId,
                        x.ApplicantPosting.ClientId,
                        x.ApplicantPosting.ApplicationId,
                        x.ApplicantPosting.ApplicantCompanyApplication.IsFlagReferenceCheck,
                        Questions = x.ApplicantPosting.ApplicantCompanyApplication.ApplicantQuestionSets.Select(q => new ApplicationQuestionDto
                        {
                            DisplayOrder = q.OrderId,
                            QuestionId = q.QuestionId,
                            SectionId = q.Question.SectionId,
                            QuestionText = q.Question.Question,
                            ResponseTitle = q.Question.ResponseTitle,
                            IsEnabled = q.Question.IsEnabled,
                            IsFlagged = q.Question.IsFlagged,
                            FieldTypeId = q.Question.FieldTypeId,
                            IsRequired = q.Question.IsRequired,
                            FlaggedResponse = q.Question.FlaggedResponse,
                            SelectionCount = q.Question.SelectionCount,
                            Items = q.Question.ApplicantQuestionDdlItem.Select(i => new QuestionItemOptionDto
                            {
                                ItemId = i.ApplicantQuestionDdlItemId,
                                Value = i.Value,
                                Description = i.Description,
                                IsDefault = i.IsDefault,
                                FlaggedResponse = i.FlaggedResponse,
                                IsEnabled = i.IsEnabled
                            })
                        }),
                        AdditionalQuestions = x.ApplicantApplicationDetail
                            .Where(d => d.SectionId == (int)ApplicationSectionSystemType.AdditionalQuestions)
                            .Select(d => new ApplicationQuestionDto
                            {
                                DisplayOrder = d.ApplicationDetailId,
                                QuestionId = d.QuestionId ?? 0,
                                SectionId = d.ApplicantQuestionControl.SectionId,
                                QuestionText = d.ApplicantQuestionControl.Question,
                                ResponseTitle = d.ApplicantQuestionControl.ResponseTitle,
                                IsEnabled = d.ApplicantQuestionControl.IsEnabled,
                                IsFlagged = d.ApplicantQuestionControl.IsFlagged,
                                FieldTypeId = d.ApplicantQuestionControl.FieldTypeId,
                                IsRequired = d.ApplicantQuestionControl.IsRequired,
                                FlaggedResponse = d.ApplicantQuestionControl.FlaggedResponse,
                                SelectionCount = d.ApplicantQuestionControl.SelectionCount,
                                Items = d.ApplicantQuestionControl.ApplicantQuestionDdlItem.Select(i => new QuestionItemOptionDto
                                {
                                    ItemId = i.ApplicantQuestionDdlItemId,
                                    Value = i.Value,
                                    Description = i.Description,
                                    IsDefault = i.IsDefault,
                                    FlaggedResponse = i.FlaggedResponse,
                                    IsEnabled = i.IsEnabled
                                })
                            })
                    })
                    .FirstOrDefault();

                result.CheckForNotFound(applicationInfo);
                if (result.HasError)
                    return null;

                if (isAdmin.Success)
                    _session.ResourceAccessChecks.CheckAccessByAccessibleClientIds(applicationInfo.ClientId).MergeInto(result);
                else if (isApplicant.Success)
                    _session.ResourceAccessChecks.HasAccessToApplicant(applicationInfo.ApplicantId).MergeInto(result);

                if (result.HasError)
                    return null;

                //load all sections available to the client
                var sections = _session.UnitOfWork.ApplicantTrackingRepository.ApplicationQuestionSectionQuery()
                    .ByClientIdWithDefaultClient(applicationInfo.ClientId)
                    .HasQuestionsByClientId(applicationInfo.ClientId)
                    .ExecuteQueryAs(x => new ApplicationSectionWithQuestionsDto
                    {
                        SectionId = x.SectionId,
                        Description = x.Description,
                        Instruction = x.ApplicationSectionInstruction.Where(y => y.ClientId == applicationInfo.ClientId).Select(z => z.Instruction).FirstOrDefault(),
                        DisplayOrder = x.DisplayOrder,
                        IsEnabled = x.IsEnabled
                    })
                    .OrderBy(s => s.DisplayOrder)
                    .ToList();

                //assign questions to each section
                sections = sections.Select(s =>
                {
                    var questions = applicationInfo.Questions.Where(q => q.SectionId == s.SectionId).ToList();

                    if (s.SectionId == (int)ApplicationSectionSystemType.AdditionalQuestions)
                    {
                        // check if additional questions already added
                        List<int> qIds = questions.Select(x => x.QuestionId).ToList<int>();
                        foreach (ApplicationQuestionDto k in applicationInfo.AdditionalQuestions)
                        {
                            if(!qIds.Contains(k.QuestionId)) questions.Add(k);
                        }
                    }

                    s.Questions = questions;
                    return s;
                }).ToList();

                return sections;
            });
        }

        IOpResult<IEnumerable<ApplicationQuestionResponseDto>> IApplicantTrackingService.GetApplicantQuestionResponses(int applicationHeaderId)
        {
            var result = new OpResult<IEnumerable<ApplicationQuestionResponseDto>>();

            var isAdmin = _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin);
            var isApplicant = _session.CanPerformAction(ApplicantTrackingActionType.ReadApplicantInfo);

            if (isAdmin.HasError && isApplicant.HasError)
            {
                isAdmin.MergeInto(result);
                isApplicant.MergeInto(result);
                return result;
            }

            var applicationInfo = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantApplicationHeaderQuery()
                .ByApplicationHeaderId(applicationHeaderId)
                .ExecuteQueryAs(x => new
                {
                    x.ApplicantId,
                    x.ApplicantPosting.ClientId,
                    x.ApplicantPosting.ApplicationId,
                    x.ApplicantPosting.ApplicantCompanyApplication.IsFlagReferenceCheck
                })
                .FirstOrDefault();

            result.CheckForNotFound(applicationInfo);
            if (result.HasError)
                return result;

            if (isAdmin.Success)
                _session.ResourceAccessChecks.CheckAccessByAccessibleClientIds(applicationInfo.ClientId).MergeInto(result);
            else if (isApplicant.Success)
                _session.ResourceAccessChecks.HasAccessToApplicant(applicationInfo.ApplicantId).MergeInto(result);

            if (result.HasError)
                return result;

            return result.TrySetData(() =>
            {
                //get the specific responses to questions for this particular applicant
                var responses = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantApplicationDetailQuery()
                    .ByApplicationHeaderId(applicationHeaderId)
                    .WhereQuestionIdIsNotNull()
                    .ExecuteQueryAs(x => new ApplicationQuestionResponseDto
                    {
                        ApplicationDetailId = x.ApplicationDetailId,
                        IsFlagged = x.IsFlagged,
                        QuestionId = x.QuestionId,
                        ResponseValue = x.Response,
                        SectionId = x.SectionId
                    })
                    .ToList();

                //if an admin we need to exclude EEOC responses for legal reasons
                if (isAdmin.Success)
                {
                    responses = responses.Where(r => r.SectionId != (int)ApplicationSectionSystemType.EqualOpportunityEmployer).ToList();
                }

                return responses;
            });
        }

        IOpResult<ApplicationQuestionResponseNumbersDto> IApplicantTrackingService.GetApplicantQuestionResponses(ApplicationQuestionFilterDto dto)
        {
            var result = new OpResult<ApplicationQuestionResponseNumbersDto>();

            var isAdmin = _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin);

            if (isAdmin.Success)
                _session.ResourceAccessChecks.CheckAccessByAccessibleClientIds(dto.ClientId).MergeInto(result);

            if (result.HasError)
                return result;

            var questionQuery = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantQuestionControlQuery();

            if (dto.QuestionId.HasValue)    questionQuery.ByQuestionId(dto.QuestionId.Value);
            else                            questionQuery.ByQuestionText(dto.QuestionText);

            ApplicationQuestionDto question = questionQuery.FirstOrDefaultAs(q => new ApplicationQuestionDto
            {
                QuestionId = q.QuestionId,
                QuestionText = q.Question,
                ResponseTitle = q.ResponseTitle,
                IsEnabled = q.IsEnabled,
                IsFlagged = q.IsFlagged,
                FieldTypeId = q.FieldTypeId,
                IsRequired = q.IsRequired,
                FlaggedResponse = q.FlaggedResponse,
                SelectionCount = q.SelectionCount,
                Items = q.ApplicantQuestionDdlItem.Select(i => new QuestionItemOptionDto
                {
                    ItemId = i.ApplicantQuestionDdlItemId,
                    Value = i.Value,
                    Description = i.Description,
                    IsDefault = i.IsDefault,
                    FlaggedResponse = i.FlaggedResponse,
                    IsEnabled = i.IsEnabled
                })
            });

            result.CheckForNotFound(question);
            if (result.HasError)
                return result;

            IApplicantApplicationDetailQuery questionDetailQuery = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantApplicationDetailQuery()
                .ByQuestionId(dto.QuestionId.Value)
                .ByApplicationDateRange(dto.StartDate.Value, dto.EndDate.Value);

            // If posting Id information is passed apply posting based filter
            if (dto.PostingId.HasValue && dto.PostingId.Value != 0)
            {
                IApplicantPostingsQuery applicationQuery = _session.UnitOfWork.ApplicantTrackingRepository
                  .ApplicantPostingsQuery()
                  .ByPublishDateRange(dto.StartDate.Value, dto.EndDate.Value); //<-- common to all queries

                if (dto.PostingId.Value > 0)
                {
                    //add posting ID filter
                    applicationQuery.ByPostingId(dto.PostingId.Value);
                }
                else
                {
                    //common filter to non-posting-id specific queries
                    applicationQuery
                      .ByClientId(dto.ClientId)
                      .ByIsActive(true);

                    if (dto.PostingId.Value == -1)
                        applicationQuery.ByIsClosed(false);
                }

                //now we can execute the query knowing the appropriate filters have been applied
                IEnumerable<int> applicationIds = applicationQuery.ExecuteQueryAs(x => x.ApplicantCompanyApplication.CompanyApplicationId).Distinct<int>();

                //filter only the applications that applies to selected posting
                questionDetailQuery.ByApplicationIds(applicationIds);
            }

            IEnumerable<ApplicationQuestionResponseDto> rawData = questionDetailQuery
                .ExecuteQueryAs(x => new ApplicationQuestionResponseDto
                {
                    QuestionId = x.QuestionId.Value,
                    ResponseValue = x.Response,
                    IsFlagged = x.ApplicantQuestionControl.IsFlagged,
                }).ToList();

            ApplicationQuestionResponseNumbersDto responses = new ApplicationQuestionResponseNumbersDto();
            responses.QuestionId    = question.QuestionId;
            responses.QuestionText  = question.QuestionText;
            responses.Responses     = new Dictionary<string,int>();

            if ( question.FieldTypeId == FieldType.List || question.FieldTypeId == FieldType.MultipleSelection)
            {
                IDictionary<int, int> selectionCountMap = new Dictionary<int, int>();
                foreach (QuestionItemOptionDto item in question.Items)
                    selectionCountMap.Add( int.Parse(item.Value) , 0);

                foreach (ApplicationQuestionResponseDto resp in rawData)
                {
                    IEnumerable<int> selection = resp.ResponseValue.Split(',').Select(y => int.Parse(y));
                    foreach (int k in selection)
                        selectionCountMap[k]++;
                }

                foreach (QuestionItemOptionDto item in question.Items)
                {
                    responses.Responses.Add(item.Description, selectionCountMap[int.Parse(item.Value)]);
                }
            }
            else
            {
                string k = "";
                foreach (ApplicationQuestionResponseDto resp in rawData)
                {
                    if (string.IsNullOrEmpty(resp.ResponseValue))   k = "";
                    else                                            k = resp.ResponseValue.ToLower();

                    if (responses.Responses.ContainsKey(k))     responses.Responses[k]++;
                    else                                        responses.Responses.Add(k, 1);
                }
            }

            result.Data = responses;
            return result;
        }

        public IOpResult<ICollection<ApplicationQuestionSectionWithQuestionDetailDto>> GetApplicationSectionsByApplicationId(
            int applicationId, int applicationHeaderId)
        {
            var result = new OpResult<ICollection<ApplicationQuestionSectionWithQuestionDetailDto>>();
            var internalApp = _session.CanPerformAction(ApplicantTrackingActionType.InternalApplicant);
            var externalApp = _session.CanPerformAction(ApplicantTrackingActionType.ExternalApplicant);
            var isApplicantAdmin = _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin);

            int? clientId = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantCompanyApplicationQuery()
                .ByApplicationId(applicationId).ExecuteQuery().FirstOrDefault()?.ClientId;
            var hasResourceAccess = _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId.HasValue ? clientId.Value : 0);

            if (internalApp.Success || externalApp.Success || isApplicantAdmin.Success || hasResourceAccess.Success)
            {
                result.Data = _appTrackProvider
                    .GetApplicationSectionsByApplicationId(applicationId, applicationHeaderId).Data.ToList();

            }
            else
            {
                internalApp.MergeInto(result);
                externalApp.MergeInto(result);
                isApplicantAdmin.MergeInto(result);
                hasResourceAccess.MergeInto(result);
            }
            return result;
        }

        public IOpResult<IEnumerable<ApplicantApplicationDetailDto>> SaveQuestionSection(IEnumerable<ApplicantApplicationDetailDto> details)
        {
            var result = new OpResult<IEnumerable<ApplicantApplicationDetailDto>>();
            // No data needs to be saved
            if (details == null || details.Count() == 0) return result;

            var internalApp = _session.CanPerformAction(ApplicantTrackingActionType.InternalApplicant).Success;
            var externalApp = _session.CanPerformAction(ApplicantTrackingActionType.ExternalApplicant).Success;
            var isApplicantAdmin = _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).Success;

            int? clientId = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantApplicationHeaderQuery()
                .ByApplicationHeaderId(details.First().ApplicationHeaderId ).ExecuteQueryAs(x=>x.Applicant.ClientId).FirstOrDefault();
            var hasResourceAccess = _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId.HasValue ? clientId.Value : 0).Success;

            if (!internalApp && !externalApp && !isApplicantAdmin && !hasResourceAccess)
            {
                result.AddMessage(new GenericMsg("No sufficient permissions to retrieve section.")).SetToFail();
                return result;
            }

            foreach (var detail in details)
            {
                var entity = new ApplicantApplicationDetail
                {
                    ApplicationDetailId = detail.ApplicationDetailId,
                    ApplicationHeaderId = detail.ApplicationHeaderId,
                    SectionId           = detail.SectionId,
                    IsFlagged           = detail.IsFlagged,
                    QuestionId          = detail.QuestionId,
                    Response            = detail.Response?.Trim() ?? CommonConstants.EMPTY_STRING
                };

                var existingDetail = _session.UnitOfWork.ApplicantTrackingRepository
                    .ApplicantApplicationDetailQuery()
                    .ByApplicationHeaderId(detail.ApplicationHeaderId)
                    .BySectionId(detail.SectionId)
                    .ByQuestionId(detail.QuestionId)
                    .FirstOrDefaultAs(x => new { x.ApplicationDetailId, x.Response });

                //make sure only one response per question per applicant-application is created
                if (existingDetail != null)
                {
                    entity.ApplicationDetailId = existingDetail.ApplicationDetailId;
                    detail.ApplicationDetailId = existingDetail.ApplicationDetailId;
                }

                if (detail.ApplicationDetailId == CommonConstants.NEW_ENTITY_ID)
                {
                    _session.UnitOfWork.RegisterNew(entity);
                }
                else
                {
                    //check for changes
                    if (entity.Response != existingDetail.Response)
                    {
                        _session.UnitOfWork.RegisterModified(entity, (new PropertyList<ApplicantApplicationDetail>())
                            .Include(x => x.Response)
                            .Include(x => x.IsFlagged));
                    }
                }

                _session.UnitOfWork.RegisterPostCommitAction(() =>
                {
                    detail.ApplicationDetailId = entity.ApplicationDetailId;
                    detail.Response            = entity.Response;
                });
            }

            if (result.Success)
                _session.UnitOfWork.Commit().MergeInto(result);

            return result.SetDataOnSuccess(details);
        }

        IOpResult<bool> IApplicantTrackingService.CheckEmployeeNumber(int clientId, int employeeId, string employeeNumber)
        {
            var opResult = new OpResult<bool>();

            _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
            if (opResult.HasError)
                return opResult;

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(opResult);
            if (opResult.HasError)
                return opResult;

            var employeeNumberExists = _session.UnitOfWork.EmployeeRepository.QueryEmployees()
                    .ByClientId(clientId)
                    .ByNotEmployeeId(employeeId)
                    .ByEmployeeNumbers(new string[] { employeeNumber })
                    .ExecuteQuery()
                    .Any();

            opResult.Data = employeeNumberExists;

            return opResult;
        }

        IOpResult<bool> IApplicantTrackingService.CheckSSN(int clientId, int employeeId, string ssn)
        {
            var opResult = new OpResult<bool>();

            _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
            if (opResult.HasError)
                return opResult;

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(opResult);
            if (opResult.HasError)
                return opResult;

            var ssnExists = _session.UnitOfWork.EmployeeRepository.QueryEmployees()
                    .ByClientId(clientId)
                    .ByNotEmployeeId(employeeId)
                    .BySocialSecurityNumber(ssn)
                    .ExecuteQuery()
                    .Any();

            opResult.Data = ssnExists;

            return opResult;
        }

        IOpResult IApplicantTrackingService.TransferApplicantToEmployee(ApplicantToEmployeePostDto dto,
            ISecurityService securityService)
        {
            var opResult = new OpResult<bool>();

            _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
            if (opResult.HasError)
                return opResult;

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, dto.ClientId).MergeInto(opResult);
            if (opResult.HasError)
                return opResult;

           // var folder = _formProvider.GetRegisteredApplicantTrackingAttachmentFolder(dto.ClientId, dto.ApplicationHeaderId).MergeInto(opResult).Data;

            var applicantApplicantPosting = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantApplicationHeaderQuery()
                .ByApplicationHeaderId(dto.ApplicationHeaderId)
                .ExecuteQueryAs(x => new ApplicantApplicantPostingDto()
                {
                    ApplicantId         = x.ApplicantId,
                    FirstName           = x.Applicant.FirstName,
                    MiddleInitial       = x.Applicant.MiddleInitial,
                    LastName            = x.Applicant.LastName,
                    Address = x.Applicant.Address ?? String.Empty,
                    AddressLine2 = x.Applicant.AddressLine2 ?? String.Empty,
                    City = x.Applicant.City ?? String.Empty,
                    State               = (x.Applicant.State.HasValue && x.Applicant.State.Value > 0) ? x.Applicant.State.Value : 1,
                    Zip = x.Applicant.Zip ?? String.Empty,
                    CountryId           = (x.Applicant.CountryId > 0) ? x.Applicant.CountryId : 1,
                    PhoneNumber         = x.Applicant.PhoneNumber,
                    CellPhoneNumber     = x.Applicant.CellPhoneNumber,
                    EmailAddress        = x.Applicant.EmailAddress,
                    Dob                 = x.Applicant.Dob,
                    JobTitle            = x.ApplicantPosting.JobProfile.Description,
                    JobProfileId        = x.ApplicantPosting.JobProfileId,
                    ClientDivisionId    = x.ApplicantPosting.ClientDivisionId   ?? x.ApplicantPosting.JobProfile.JobProfileClassifications.ClientDivisionId,
                    ClientDepartmentId  = x.ApplicantPosting.ClientDepartmentId ?? x.ApplicantPosting.JobProfile.JobProfileClassifications.ClientDepartmentId,
                    ClientGroupId       = x.ApplicantPosting.JobProfile.JobProfileClassifications.ClientGroupId,
                    JobClass            = x.ApplicantPosting.JobProfile.JobProfileClassifications.JobClass,
                    EeocLocationId      = x.ApplicantPosting.JobProfile.JobProfileClassifications.EeocLocationId,
                    EeocJobCategoryId   = x.ApplicantPosting.JobProfile.JobProfileClassifications.EeocJobCategoryId,
                    ClientWorkersCompId = x.ApplicantPosting.JobProfile.JobProfileClassifications.ClientWorkersCompId,
                    ClientCostCenterId  = x.ApplicantPosting.JobProfile.JobProfileClassifications.ClientCostCenterId,
                    ClientShiftId       = x.ApplicantPosting.JobProfile.JobProfileClassifications.ClientShiftId,
                    IsBenefitEligible   = x.ApplicantPosting.JobProfile.JobProfileCompensation.BenefitsEligibility ?? true,
                    IsExternalApplicant = x.ExternalApplicationIdentity != null ? true : false,
                    JobSiteName = (x.ExternalApplicationIdentity != null) ? x.ExternalApplicationIdentity.ApplicantJobSite.JobSiteDescription : "",
                }).FirstOrDefault();

            bool isIndeedApplicant = applicantApplicantPosting.IsExternalApplicant && (applicantApplicantPosting.JobSiteName.ToLower() == "indeed");

            if (!dto.EmployeeId.HasValue || dto.EmployeeId == 0) //Not an Employee already
            {
                var employee = new Employee()
                {
                    FirstName            = applicantApplicantPosting.FirstName,
                    LastName             = applicantApplicantPosting.LastName,
                    MiddleInitial        = applicantApplicantPosting.MiddleInitial ?? String.Empty,
                    AddressLine1         = applicantApplicantPosting.Address,
                    AddressLine2         = applicantApplicantPosting.AddressLine2,
                    City                 = applicantApplicantPosting.City,
                    StateId              = applicantApplicantPosting.State,
                    PostalCode = applicantApplicantPosting.Zip ?? "",
                    CountryId            = applicantApplicantPosting.CountryId ?? 1, // 1 is the id of the US
                    HomePhoneNumber      = applicantApplicantPosting.PhoneNumber,
                    SocialSecurityNumber = dto.SocialSecurityNumber ?? "",
                    Gender               = dto.Gender,
                    BirthDate            = applicantApplicantPosting.Dob,
                    EmployeeNumber       = dto.EmployeeNumber.ToString(),
                    JobTitle             = applicantApplicantPosting.JobTitle,
                    JobProfileId         = applicantApplicantPosting.JobProfileId,
                    ClientDivisionId     = applicantApplicantPosting.ClientDivisionId,
                    ClientDepartmentId   = applicantApplicantPosting.ClientDepartmentId,
                    JobClass             = applicantApplicantPosting.JobClass,
                    ClientCostCenterId   = applicantApplicantPosting.ClientCostCenterId,
                    ClientGroupId        = applicantApplicantPosting.ClientGroupId,
                    ClientWorkersCompId  = applicantApplicantPosting.ClientWorkersCompId,
                    IsW2Pension          = false,
                    HireDate             = dto.HireDate,
                    SeparationDate       = null,
                    AnniversaryDate      = null,
                    RehireDate           = null,
                    EligibilityDate      = null,
                    IsActive             = true,
                    EmailAddress         = applicantApplicantPosting.EmailAddress,
                    PayStubOption        = 0,
                    Notes                = string.Empty,
                    CostCenterType       = 1,
                    //LastModifiedDate   = DateTime.Now,
                    ClientId             = dto.ClientId,
                    MaritalStatusId      = null,
                    EeocRaceId           = null,
                    EeocJobCategoryId    = applicantApplicantPosting.EeocJobCategoryId,
                    EeocLocationId       = applicantApplicantPosting.EeocLocationId,
                    CellPhoneNumber      = applicantApplicantPosting.CellPhoneNumber,
                    IsInOnboarding       = dto.StartedOnboarding,
                    CountyId             = null,
                    DirectSupervisorID   = dto.DirectSupervisorId


                };

                _session.SetModifiedProperties(employee);

                _session.UnitOfWork.RegisterNew(employee);
                //_session.UnitOfWork.Commit().MergeInto(opResult);
                //dto.EmployeeId = employee.EmployeeId;

                var employeePay = new EmployeePay()
                {
                    //EmployeeId = employee.EmployeeId,
                    PayFrequencyId              = (Core.Dto.Payroll.PayFrequencyType) dto.PayFrequencyId,
                    Type                        = (Core.Dto.Employee.PayType) dto.EmployeePayTypeId,
                    ContractAmount              = 0.00,
                    ContractAmountEffectiveDate = new DateTime(1900, 1, 1),
                    SalaryAmount = 
                        (Core.Dto.Employee.PayType) dto.EmployeePayTypeId == Core.Dto.Employee.PayType.Salary
                            ? (double) dto.Salary
                            : 0.00,
                    SalaryAmountEffectiveDate =
                        ((Core.Dto.Employee.PayType) dto.EmployeePayTypeId == Core.Dto.Employee.PayType.Salary)
                            ? dto.HireDate
                            : null,
                    Hours                       = 0,
                    EmployeeStatusId            = (Core.Dto.Employee.EmployeeStatusType) dto.EmployeeStatusId,
                    ClientShiftId               = applicantApplicantPosting.ClientShiftId,
                    IsTippedEmployee            = false,
                    TempAgencyPercent           = null,
                    TempAgencyPercentDtOverride = null,
                    TempAgencyPercentOtOverride = null,
                    ClientId                    = dto.ClientId,
                    IsExcludeFromAca            = false,
                    AcaNote                     = string.Empty,
                    IsCobraParticipant          = false,
                    Employee                    = employee,
                    ContractNote                = String.Empty,
                    IsC1099Exempt               = false,
                    IsFicaExempt                = false,
                    IsFutaExempt                = false,
                    IsHireActQualified          = false,
                    IsIncomeTaxExempt           = false,
                    IsSocSecExempt              = false,
                    IsSutaExempt                = false,
                    Modified                    = DateTime.Now,
                    ModifiedBy                  = _session.LoggedInUserInformation.UserId,
                    HireActStartDate            = Convert.ToDateTime("3/19/2010"),
                    SalaryNote                  = String.Empty,
                    IsArpExempt                 = false,
					ClientTaxId = dto.SutaState?.LegacyTaxId?? null
                };
                _session.UnitOfWork.RegisterNew(employeePay);

                if (dto.HourlyRateTypeId.HasValue)
                {
                    var employeeClientRate = new EmployeeClientRate()
                    {
                        //EmployeeId = employee.EmployeeId,
                        ClientRateId = dto.HourlyRateTypeId ?? 0,
                        Rate = 
                            (Core.Dto.Employee.PayType) dto.EmployeePayTypeId == Core.Dto.Employee.PayType.Hourly
                                ? (double) dto.HourlyRate
                                : 0.00,
                        IsDefaultRate     = true,
                        RateEffectiveDate = dto.HireDate,
                        ClientId          = dto.ClientId,
                        Modified          = DateTime.Now,
                        ModifiedBy        = _session.LoggedInUserInformation.UserId,
                        Employee          = employee
                    };
                    _session.UnitOfWork.RegisterNew(employeeClientRate);
                }

                if (applicantApplicantPosting.JobProfileId != null)
                {
                    var jpas = _session.UnitOfWork.ClientRepository.
                        JobProfileAccrualsQuery()
                        .ByJobProfileId(applicantApplicantPosting.JobProfileId.Value)
                        .ExecuteQuery()
                        .ToList();
                    foreach (var jpa in jpas)
                    {
                        var employeeAccrual                    = new EmployeeAccrual();
                        employeeAccrual.Employee               = employee;
                        employeeAccrual.ClientAccrualId        = jpa.ClientAccrualId;
                        employeeAccrual.IsAllowScheduledAwards = true;
                        _session.SetModifiedProperties(employeeAccrual);
                        _session.UnitOfWork.RegisterNew(employeeAccrual);
                    }
                }

                _session.UnitOfWork.Commit()
                    .MergeInto(opResult); //save all 3 new employee entries at 1 time incase of failure

				if (opResult.Success)
                {
					// notify employee navigator
					_employeeService.EmployeeNavigatorNotifier(dto.ClientId, employee.EmployeeId);
                }


                dto.EmployeeId = employee.EmployeeId;

                var benefits                  = new EmployeeBenefitInfo();
                benefits.Employee             = employee;
                benefits.IsEligible           = applicantApplicantPosting.IsBenefitEligible;
                benefits.IsRetirementEligible = true;
                benefits.IsTobaccoUser        = false;

				_session.SetModifiedProperties(benefits);
				_session.UnitOfWork.RegisterNew(benefits);

				var applicant = _session.UnitOfWork.ApplicantTrackingRepository
					.ApplicantsQuery()
					.ByApplicantId(dto.ApplicantId)
					.FirstOrDefault();
				applicant.EmployeeId = employee.EmployeeId;
				applicant.ClientId = dto.ClientId;
				_session.UnitOfWork.RegisterModified(applicant);

				if (applicant.UserId.HasValue)
				{
					var user = _session.UnitOfWork.UserRepository
						.QueryUsers()
						.ByUserId(applicant.UserId.Value)
						.FirstOrDefault();

					user.UserTypeId = UserType.Employee;
					user.EmployeeId = employee.EmployeeId;
					user.IsPayrollAccessBlocked = false;
					user.IsHrBlocked = false;
					user.ViewEmployeePayTypes = UserViewEmployeePayType.None;
					user.ViewEmployeeRateTypes = UserViewEmployeePayType.None;
					user.IsEmployeeSelfServiceViewOnly = true;
					user.IsEmployeeSelfServiceOnly = true;
					_session.UnitOfWork.RegisterModified(user);

					_session.UnitOfWork.Commit().MergeInto(opResult);

					var convertApplicantToEmployeeDto = new ConvertApplicantToEmployeeDto()
					{
						AuthUserId = user.AuthUserId.GetValueOrDefault(),
						AuthUserTypeId = Authentication.Dto.Enums.AuthUserTypeId.Employee,
						EmployeeId = employee.EmployeeId,
						ClientId = dto.ClientId,
						ModifiedById = _session.LoggedInUserInformation.AuthUserId
					};

					opResult.TryCatch(
						() => { _loginService.ConvertApplicantToEmployee(convertApplicantToEmployeeDto); });

					//add user Mfa data here
					var clientMfaResult = securityService.GetClientMfaSettings(dto.ClientId);
					var clientMfaSetting = clientMfaResult?.Data?.FirstOrDefault(x =>
						x.AuthUserTypeId == (byte) Authentication.Dto.Enums.AuthUserTypeId.Employee);

					securityService.SetUserMfaSettings(applicant.UserId.Value, clientMfaSetting.Required,
						clientMfaSetting.Questions, user.AuthUserId.GetValueOrDefault());
				}

		   }
			else
			{
				//Already an Employee
				var employeeOriginalInfo = _session.UnitOfWork.PayrollRepository.QueryEmployeePay()
					.ByEmployeeId(dto.EmployeeId ?? 0)
					.ExecuteQueryAs(x => new
					{
						OrigClientTax = x.ClientTaxId,
						OrigJobProfile = x.Employee.JobProfileId,
						OrigDivision = x.Employee.ClientDivisionId,
						OrigDepartment = x.Employee.ClientDepartmentId,
						OrigEmployeeStatus = x.EmployeeStatusId,
						OrigEmployeeType = x.Type,
						OrigSalaryAmount = x.SalaryAmount,
						OrigPayFrequency = x.PayFrequencyId,
						OrigSalaryEffectiveDate = x.SalaryAmountEffectiveDate,
						EmployeePayId = x.EmployeePayId,
						OrigClientShiftId = x.ClientShiftId,
						OrigDirectSupervisorId= x.Employee.DirectSupervisorID
					}).FirstOrDefault();

				var employeeNewInfo = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
					.ByPostingId(dto.PostingId)
					.ExecuteQueryAs(x => new
					{
						NewJobProfile = x.JobProfileId,
						NewDivision = x.ClientDivisionId,
						NewDepartment = x.ClientDepartmentId
					}).FirstOrDefault();

				var effectiveDateDto = new EffectiveDateDto();

				var effectiveIdForJobProfileInsert = 1;
				if (employeeOriginalInfo.OrigJobProfile != employeeNewInfo.NewJobProfile)
				{
					effectiveDateDto = new EffectiveDateDto()
					{
						EffectiveDate = dto.HireDate ?? DateTime.Now,
						AppliedBy = string.Empty,
						AppliedOn = null,
						CreatedBy = dto.UserId.ToString(),
						CreatedOn = DateTime.Now,
						Type = 1,
						TablePkId = dto.EmployeeId ?? 0,
						Accepted = null,
						EmployeeId = dto.EmployeeId ?? 0,
						Table = "Employee",
						Column = "JobProfileId",
						OldValue = employeeOriginalInfo.OrigJobProfile.ToString(),
						NewValue = employeeNewInfo.NewJobProfile.ToString(),
						FriendlyView = "Job Profile",
						Datatype = "__JobProfile.Description"
					};
					effectiveIdForJobProfileInsert =
						this.Self.UpdateEffectiveDate(effectiveDateDto).Data.EffectiveId;
				}

				var effectiveIdForDivisionInsert = 1;
				if (employeeOriginalInfo.OrigDivision != employeeNewInfo.NewDivision)
				{
					effectiveDateDto = new EffectiveDateDto()
					{
						EffectiveDate = dto.HireDate ?? DateTime.Now,
						AppliedBy = string.Empty,
						AppliedOn = null,
						CreatedBy = dto.UserId.ToString(),
						CreatedOn = DateTime.Now,
						Type = 1,
						TablePkId = dto.EmployeeId ?? 0,
						Accepted = null,
						EmployeeId = dto.EmployeeId ?? 0,
						Table = "Employee",
						Column = "ClientDivisionID",
						OldValue = employeeOriginalInfo.OrigDivision.ToString(),
						NewValue = employeeNewInfo.NewDivision.ToString(),
						FriendlyView = "Division",
						Datatype = "__ClientDivision.Name"
					};
					effectiveIdForDivisionInsert = this.Self.UpdateEffectiveDate(effectiveDateDto).Data.EffectiveId;
				}

				var effectiveIdForDepartmentInsert = 1;
				if (employeeOriginalInfo.OrigDepartment != employeeNewInfo.NewDepartment)
				{
					effectiveDateDto = new EffectiveDateDto()
					{
						EffectiveDate = dto.HireDate ?? DateTime.Now,
						AppliedBy = string.Empty,
						AppliedOn = null,
						CreatedBy = dto.UserId.ToString(),
						CreatedOn = DateTime.Now,
						Type = 1,
						TablePkId = dto.EmployeeId ?? 0,
						Accepted = null,
						EmployeeId = dto.EmployeeId ?? 0,
						Table = "Employee",
						Column = "ClientDepartmentID",
						OldValue = employeeOriginalInfo.OrigDepartment.ToString(),
						NewValue = employeeNewInfo.NewDepartment.ToString(),
						FriendlyView = "Department",
						Datatype = "__ClientDepartment.Name"
					};
					effectiveIdForDepartmentInsert =
						this.Self.UpdateEffectiveDate(effectiveDateDto).Data.EffectiveId;
				}

				var effectiveIdForPayFrequencyInsert = 1;
				if (employeeOriginalInfo.OrigPayFrequency != (PayFrequencyType) dto.PayFrequencyId)
				{
					effectiveDateDto = new EffectiveDateDto()
					{
						EffectiveDate = dto.HireDate ?? DateTime.Now,
						AppliedBy = string.Empty,
						AppliedOn = null,
						CreatedBy = dto.UserId.ToString(),
						CreatedOn = DateTime.Now,
						Type = 1,
						TablePkId = employeeOriginalInfo.EmployeePayId,
						Accepted = 1,
						EmployeeId = dto.EmployeeId ?? 0,
						Table = "EmployeePay",
						Column = "PayFrequencyID",
						OldValue = employeeOriginalInfo.OrigPayFrequency.ToString(),
						NewValue = dto.PayFrequencyId.ToString(),
						FriendlyView = "Pay Frequency",
						Datatype = "__PayFrequency.PayFrequency"
					};
					effectiveIdForPayFrequencyInsert =
						this.Self.UpdateEffectiveDate(effectiveDateDto).Data.EffectiveId;

					// update the modified value
					EmployeePay modified = _session.UnitOfWork.EmployeeRepository.GetEmployee(dto.EmployeeId.Value)
						.EmployeePayInfo.First();
					modified.PayFrequencyId = (PayFrequencyType) dto.PayFrequencyId;
					_session.UnitOfWork.RegisterModified<EmployeePay>(modified);
				}

				var effectiveIdForPayTypeInsert = 1;
				if (employeeOriginalInfo.OrigEmployeeType != (Core.Dto.Employee.PayType) dto.EmployeePayTypeId)
				{
					effectiveDateDto = new EffectiveDateDto()
					{
						EffectiveDate = dto.HireDate ?? DateTime.Now,
						AppliedBy = string.Empty,
						AppliedOn = null,
						CreatedBy = dto.UserId.ToString(),
						CreatedOn = DateTime.Now,
						Type = 1,
						TablePkId = employeeOriginalInfo.EmployeePayId,
						Accepted = 1,
						EmployeeId = dto.EmployeeId ?? 0,
						Table = "EmployeePay",
						Column = "Type",
						OldValue = employeeOriginalInfo.OrigEmployeeType.ToString(),
						NewValue = dto.EmployeePayTypeId.ToString(),
						FriendlyView = "Pay Payroll Type",
						Datatype = "**1,Hourly;2,Salary"
					};
					effectiveIdForPayTypeInsert = this.Self.UpdateEffectiveDate(effectiveDateDto).Data.EffectiveId;

					// update the modified value
					EmployeePay modified = _session.UnitOfWork.EmployeeRepository.GetEmployee(dto.EmployeeId.Value)
						.EmployeePayInfo.First();
					modified.Type = (Core.Dto.Employee.PayType) dto.EmployeePayTypeId;
					_session.UnitOfWork.RegisterModified<EmployeePay>(modified);
				}

				
				if (employeeOriginalInfo.OrigClientShiftId != applicantApplicantPosting.ClientShiftId)
				{
					EmployeePay modified = _session.UnitOfWork.EmployeeRepository.GetEmployee(dto.EmployeeId.Value).EmployeePayInfo.First();
					modified.ClientShiftId = applicantApplicantPosting.ClientShiftId;
					_session.UnitOfWork.RegisterModified<EmployeePay>(modified);
				}

				var effectiveIdForSalaryAmountInsert = 1;
				var effectiveIdForSalaryAmountEffectiveDateInsert = 1;
				if ((Core.Dto.Employee.PayType) dto.EmployeePayTypeId == Core.Dto.Employee.PayType.Salary &&
					employeeOriginalInfo.OrigSalaryAmount != (double) dto.Salary)
				{
					effectiveDateDto = new EffectiveDateDto()
					{
						EffectiveDate = dto.HireDate ?? DateTime.Now,
						AppliedBy = string.Empty,
						AppliedOn = null,
						CreatedBy = dto.UserId.ToString(),
						CreatedOn = DateTime.Now,
						Type = 1,
						TablePkId = employeeOriginalInfo.EmployeePayId,
						Accepted = 1,
						EmployeeId = dto.EmployeeId ?? 0,
						Table = "EmployeePay",
						Column = "SalaryAmount",
						OldValue = employeeOriginalInfo.OrigSalaryAmount.ToString(),
						NewValue = (dto.Salary ?? 0).ToString(),
						FriendlyView = "Pay Salary Amount",
						Datatype = "5"
					};
					effectiveIdForSalaryAmountInsert =
						this.Self.UpdateEffectiveDate(effectiveDateDto).Data.EffectiveId;


					effectiveDateDto = new EffectiveDateDto()
					{
						EffectiveDate = dto.HireDate ?? DateTime.Now,
						AppliedBy = string.Empty,
						AppliedOn = null,
						CreatedBy = dto.UserId.ToString(),
						CreatedOn = DateTime.Now,
						Type = 1,
						TablePkId = employeeOriginalInfo.EmployeePayId,
						Accepted = 1,
						EmployeeId = dto.EmployeeId ?? 0,
						Table = "EmployeePay",
						Column = "SalaryAmountEffectiveDate",
						OldValue = employeeOriginalInfo.OrigSalaryEffectiveDate.ToString(),
						NewValue = (dto.HireDate ?? DateTime.Now).ToString(),
						FriendlyView = "Pay Salary Amount Effective Date",
						Datatype = "3"
					};
					effectiveIdForSalaryAmountEffectiveDateInsert =
						this.Self.UpdateEffectiveDate(effectiveDateDto).Data.EffectiveId;

					// update the modified value
					EmployeePay modified = _session.UnitOfWork.EmployeeRepository.GetEmployee(dto.EmployeeId.Value)
						.EmployeePayInfo.First();
                    //modified.ClientTaxId = dto.SutaState.LegacyTaxId ?? null;
					modified.SalaryAmount = (double) (dto.Salary ?? 0);
					modified.SalaryAmountEffectiveDate = (dto.HireDate ?? DateTime.Now);
					_session.UnitOfWork.RegisterModified<EmployeePay>(modified);
				}


				var effectiveIdForEmployeeStatusInsert = 1;
				if (employeeOriginalInfo.OrigEmployeeStatus !=
					(Core.Dto.Employee.EmployeeStatusType) dto.EmployeeStatusId)
				{
					effectiveDateDto = new EffectiveDateDto()
					{
						EffectiveDate = dto.HireDate ?? DateTime.Now,
						AppliedBy = string.Empty,
						AppliedOn = null,
						CreatedBy = dto.UserId.ToString(),
						CreatedOn = DateTime.Now,
						Type = 1,
						TablePkId = employeeOriginalInfo.EmployeePayId,
						Accepted = 1,
						EmployeeId = dto.EmployeeId ?? 0,
						Table = "EmployeePay",
						Column = "EmployeeStatusID",
						OldValue = employeeOriginalInfo.OrigEmployeeStatus.ToString(),
						NewValue = dto.EmployeeStatusId.ToString(),
						FriendlyView = "Pay Status",
						Datatype = "__EmployeeStatus.EmployeeStatus"
					};
					effectiveIdForEmployeeStatusInsert =
						this.Self.UpdateEffectiveDate(effectiveDateDto).Data.EffectiveId;

					// update the modified value
					EmployeePay modified = _session.UnitOfWork.EmployeeRepository.GetEmployee(dto.EmployeeId.Value)
						.EmployeePayInfo.First();
                    modified.EmployeeStatusId = (Core.Dto.Employee.EmployeeStatusType) dto.EmployeeStatusId;
					_session.UnitOfWork.RegisterModified<EmployeePay>(modified);
				}

				//EmployeeClientRate Info
				if ((Core.Dto.Employee.PayType)dto.EmployeePayTypeId == Core.Dto.Employee.PayType.Hourly)
				{
					var effectiveIdForClientRateIdInsert = 1;
					var effectiveIdForHourlyRateInsert = 1;
					var effectiveIdForHourlyRateEffectiveDateInsert = 1;
					decimal origHourlyRate = 0;
					int origClientRateId = 0;
					DateTime? origHourlyRateEffectiveDate = null;

					EmployeeClientRate origEmployeeClientRate = null;
					IEnumerable<EmployeeClientRate> origEmployeeClientRates = _session.UnitOfWork.EmployeeRepository.EmployeeClientRateQuery()
						.ByClientId(dto.ClientId)
						.ByEmployeeId(dto.EmployeeId ?? 0)
						.ExecuteQuery()
						.ToList();

					//Check if an entry with the new ClentRate already exists
					if ((dto.HourlyRateTypeId ?? 0) > 0 && origEmployeeClientRates.Count() > 0)
					{
						origEmployeeClientRate = origEmployeeClientRates.FirstOrDefault(x => dto.HourlyRateTypeId.HasValue && dto.HourlyRateTypeId > 0 && x.ClientRateId == dto.HourlyRateTypeId);
					}

					//No entry with the new ClentRate already exists, Check if a default Clientrate entry exists
					if (origEmployeeClientRate == null)
					{
						origEmployeeClientRate = origEmployeeClientRates.FirstOrDefault(x => x.IsDefaultRate == true);
					}

					//No entry with the new ClentRate or a default Clientrate entry exists, check if atleast any entry exists
					if (origEmployeeClientRate == null && origEmployeeClientRates.Count() > 0)
					{
						origEmployeeClientRate = origEmployeeClientRates.FirstOrDefault();
					}


					if (origEmployeeClientRate != null)
					{
						origClientRateId = origEmployeeClientRate.ClientRateId;
						origHourlyRate = (decimal)origEmployeeClientRate.Rate;
						origHourlyRateEffectiveDate = origEmployeeClientRate.RateEffectiveDate;
					}

					//Old value(s) are different from the new ones. So, update with new values.
					if (dto.HourlyRate != origHourlyRate || dto.HireDate != origHourlyRateEffectiveDate || dto.HourlyRateTypeId != origClientRateId)
					{
						var clientRatesForThisClient = _session.UnitOfWork.ClientRepository.ClientRateQuery().ByClientId(dto.ClientId).ExecuteQuery().ToList();
						effectiveDateDto = new EffectiveDateDto()
						{
							EffectiveDate = dto.HireDate ?? DateTime.Now,
							AppliedBy = string.Empty,
							AppliedOn = null,
							CreatedBy = dto.UserId.ToString(),
							CreatedOn = DateTime.Now,
							Type = 1,
							TablePkId = dto.EmployeeId ?? 0,
							Accepted = 1,
							EmployeeId = dto.EmployeeId ?? 0,
							Table = "EmployeeClientRate",
							Column = "ClientRateId",
							OldValue = origClientRateId.ToString(),
							NewValue = (dto.HourlyRateTypeId ?? clientRatesForThisClient[0].ClientRateId).ToString(),
							FriendlyView = "Pay " + (dto.HourlyRateTypeId ?? 0).ToString() + " Client Rate Id",
							Datatype = "__ClientRate.Description"
						};
						effectiveIdForClientRateIdInsert =
							this.Self.UpdateEffectiveDate(effectiveDateDto).Data.EffectiveId;

						effectiveDateDto = new EffectiveDateDto()
						{
							EffectiveDate = dto.HireDate ?? DateTime.Now,
							AppliedBy = string.Empty,
							AppliedOn = null,
							CreatedBy = dto.UserId.ToString(),
							CreatedOn = DateTime.Now,
							Type = 1,
							TablePkId = dto.EmployeeId ?? 0,
							Accepted = 1,
							EmployeeId = dto.EmployeeId ?? 0,
							Table = "EmployeeClientRate",
							Column = "Rate",
							OldValue = origHourlyRate.ToString(),
							NewValue = (dto.HourlyRate ?? 0).ToString(),
							FriendlyView = "Pay " + (dto.HourlyRate ?? 0).ToString() + " Rate",
							Datatype = "4"
						};
						effectiveIdForHourlyRateInsert =
							this.Self.UpdateEffectiveDate(effectiveDateDto).Data.EffectiveId;

						effectiveDateDto = new EffectiveDateDto()
						{
							EffectiveDate = dto.HireDate ?? DateTime.Now,
							AppliedBy = string.Empty,
							AppliedOn = null,
							CreatedBy = dto.UserId.ToString(),
							CreatedOn = DateTime.Now,
							Type = 1,
							TablePkId = dto.EmployeeId ?? 0,
							Accepted = 1,
							EmployeeId = dto.EmployeeId ?? 0,
							Table = "EmployeeClientRate",
							Column = "RateEffectiveDate",
							OldValue = origHourlyRateEffectiveDate.ToString(),
							NewValue = (dto.HireDate ?? DateTime.Now).ToString(),
							FriendlyView = "Rate Effective Date",
							Datatype = "3"
						};
						effectiveIdForHourlyRateEffectiveDateInsert =
							this.Self.UpdateEffectiveDate(effectiveDateDto).Data.EffectiveId;

						//Chances are there, another default entry exists, So mark that as not default.
						if (origEmployeeClientRate != null && origEmployeeClientRate.IsDefaultRate == false)
						{
							var origDefaultEmployeeClientRate = origEmployeeClientRates.FirstOrDefault(x => x.IsDefaultRate == true);
							if (origDefaultEmployeeClientRate != null)
							{
								origDefaultEmployeeClientRate.IsDefaultRate = false;
								_session.UnitOfWork.RegisterModified<EmployeeClientRate>(origDefaultEmployeeClientRate);
							}

						}

						// Add/Update the modified values
						if (origEmployeeClientRate != null)
						{
							origEmployeeClientRate.IsDefaultRate = true;
							origEmployeeClientRate.ClientRateId = dto.HourlyRateTypeId ?? clientRatesForThisClient[0].ClientRateId;
							origEmployeeClientRate.Rate = (double)(dto.HourlyRate ?? 0);
							origEmployeeClientRate.RateEffectiveDate = (dto.HireDate ?? DateTime.Now);
							_session.UnitOfWork.RegisterModified<EmployeeClientRate>(origEmployeeClientRate);
						}
						else //Chances are there, the Employee's previous PayType was 'Salary' & no entries on EmployeeClientRate
						{
							origEmployeeClientRate = new EmployeeClientRate();
							origEmployeeClientRate.ClientId = dto.ClientId;
							origEmployeeClientRate.EmployeeId = dto.EmployeeId ?? 0;
							origEmployeeClientRate.ClientRateId = dto.HourlyRateTypeId ?? clientRatesForThisClient[0].ClientRateId;
							origEmployeeClientRate.Rate = (double)(dto.HourlyRate ?? 0);
							origEmployeeClientRate.IsDefaultRate = true;
							origEmployeeClientRate.RateEffectiveDate = (dto.HireDate ?? DateTime.Now);
							origEmployeeClientRate.ModifiedBy = _session.LoggedInUserInformation.UserId;
							origEmployeeClientRate.Modified = DateTime.Now;
							_session.UnitOfWork.RegisterNew<EmployeeClientRate>(origEmployeeClientRate);
						}
					}
				}

				var effectiveIdForClientTaxInsert = 1;
                if (employeeOriginalInfo.OrigClientTax != dto.SutaState?.LegacyTaxId)
				{
					effectiveDateDto = new EffectiveDateDto()
					{
						EffectiveDate = dto.HireDate ?? DateTime.Now,
						AppliedBy = string.Empty,
						AppliedOn = null,
						CreatedBy = dto.UserId.ToString(),
						CreatedOn = DateTime.Now,
						Type = 1,
						TablePkId = employeeOriginalInfo.EmployeePayId,
						Accepted = 1,
						EmployeeId = dto.EmployeeId ?? 0,
						Table = "EmployeePay",
						Column = "ClientTaxID",
						OldValue = employeeOriginalInfo.OrigClientTax.ToString(),
                        NewValue = dto.SutaState?.LegacyTaxId.ToString(),
						FriendlyView = "Client Tax",
						Datatype = "__ClientTax.Description"
					};
					effectiveIdForClientTaxInsert =
						this.Self.UpdateEffectiveDate(effectiveDateDto).Data.EffectiveId;

					// update the modified value
					EmployeePay modified = _session.UnitOfWork.EmployeeRepository.GetEmployee(dto.EmployeeId.Value)
						.EmployeePayInfo.First();
                    modified.ClientTaxId = dto.SutaState?.LegacyTaxId ?? null;
					_session.UnitOfWork.RegisterModified<EmployeePay>(modified);
				}

				Employee modifiedProps = _session.UnitOfWork.EmployeeRepository.GetEmployee(dto.EmployeeId.Value);

				// In case if hired for different position from previous
				if(modifiedProps.JobProfileId.HasValue && modifiedProps.JobProfileId != applicantApplicantPosting.JobProfileId)
				{
				    IEnumerable<ApplicantApplicationHeader> oldHeaders = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantApplicationHeaderQuery()
				        .ByApplicantId(dto.ApplicantId)
				        .ByJobProfileId(modifiedProps.JobProfileId)
				        .ExecuteQuery();

				    int reasonId = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantRejectionReasonQuery()
				        .ByClientId(dto.ClientId).ByIsActive(true).ByDescription("hired for")
				        .ExecuteQueryAs(x=>x.ApplicantRejectionReasonId).FirstOrDefault();
				    if (reasonId > 0 && oldHeaders.Count() > 0)
				    {
				        oldHeaders.ForEach(x => {
				            if (!x.ApplicantRejectionReasonId.HasValue)
				            {
				                x.ApplicantRejectionReasonId = reasonId;
				                _session.UnitOfWork.RegisterModified<ApplicantApplicationHeader>(x);
				            }
				        });
				    }
				}

				// Employee Personal Info update
				modifiedProps.SocialSecurityNumber = dto.SocialSecurityNumber;
				modifiedProps.HireDate = dto.HireDate;
				modifiedProps.ClientGroupId = applicantApplicantPosting.ClientGroupId;
				modifiedProps.JobClass = applicantApplicantPosting.JobClass;
				modifiedProps.JobTitle = applicantApplicantPosting.JobTitle;
				modifiedProps.JobProfileId = applicantApplicantPosting.JobProfileId;
				modifiedProps.EeocLocationId = applicantApplicantPosting.EeocLocationId;
				modifiedProps.EeocJobCategoryId = applicantApplicantPosting.EeocJobCategoryId;
				modifiedProps.ClientWorkersCompId = applicantApplicantPosting.ClientWorkersCompId;
				modifiedProps.ClientCostCenterId = applicantApplicantPosting.ClientCostCenterId;
				modifiedProps.ClientDivisionId = applicantApplicantPosting.ClientDivisionId;
				modifiedProps.ClientDepartmentId = applicantApplicantPosting.ClientDepartmentId;
				modifiedProps.DirectSupervisorID = dto.DirectSupervisorId;
				modifiedProps.IsInOnboarding = dto.StartedOnboarding;
				_session.SetModifiedProperties(modifiedProps);
				
				// Pay Info update
				if (modifiedProps.EmployeePayInfo!=null && modifiedProps.EmployeePayInfo.Count > 0)
				{
				    EmployeePay ePay = modifiedProps.EmployeePayInfo.First();
				    ePay.PayFrequencyId = (Core.Dto.Payroll.PayFrequencyType)dto.PayFrequencyId;
				    ePay.Type = (Core.Dto.Employee.PayType)dto.EmployeePayTypeId;
				    ePay.SalaryAmount = (Core.Dto.Employee.PayType)dto.EmployeePayTypeId == Core.Dto.Employee.PayType.Salary
				                            ? (double)dto.Salary
				                            : 0.00;
				    ePay.SalaryAmountEffectiveDate =
				                            ((Core.Dto.Employee.PayType)dto.EmployeePayTypeId == Core.Dto.Employee.PayType.Salary)
				                                ? dto.HireDate
				                                : null;
				    ePay.EmployeeStatusId = (Core.Dto.Employee.EmployeeStatusType)dto.EmployeeStatusId;
				    ePay.ClientTaxId = dto.SutaState?.LegacyTaxId ?? null;
				}

				//if (modifiedProps.EmployeeClientRates != null && modifiedProps.EmployeeClientRates.Count > 0 &&
				//	(Core.Dto.Employee.PayType)dto.EmployeePayTypeId == Core.Dto.Employee.PayType.Hourly)
				//{
				//	//EmployeeClientRate eRate = modifiedProps.EmployeeClientRates.First();
				//	EmployeeClientRate eRate = modifiedProps.EmployeeClientRates.FirstOrDefault(x => x.ClientRateId.Equals(dto.HourlyRateTypeId)) ?? modifiedProps.EmployeeClientRates.First();
				//	eRate.ClientRateId = dto.HourlyRateTypeId ?? 0;
				//	eRate.Rate = (Core.Dto.Employee.PayType)dto.EmployeePayTypeId == Core.Dto.Employee.PayType.Hourly
				//			? (double)dto.HourlyRate
				//			: 0.00;
				//	eRate.IsDefaultRate = true;
				//	eRate.RateEffectiveDate = dto.HireDate;
				//}

				_session.UnitOfWork.RegisterModified<Employee>(modifiedProps);

				EmployeeBenefitInfo benefitInfo = _session.UnitOfWork.EmployeeRepository.EmployeeBenefitInfoQuery().ByEmployeeId(dto.EmployeeId.Value).FirstOrDefault();
				if (benefitInfo != null)
				{
					benefitInfo.IsEligible = applicantApplicantPosting.IsBenefitEligible;
					_session.UnitOfWork.RegisterModified(benefitInfo);
				}
				else
				{
					var benefits = new EmployeeBenefitInfo();
					benefits.Employee = modifiedProps;
					benefits.IsEligible = applicantApplicantPosting.IsBenefitEligible;
					benefits.IsRetirementEligible = true;
					benefits.IsTobaccoUser = false;
					_session.SetModifiedProperties(benefits);
					_session.UnitOfWork.RegisterNew(benefits);
				}

				_session.UnitOfWork.Commit().MergeInto(opResult);

				if (opResult.Success)
                {
					// notify employee navigator
					_employeeService.EmployeeNavigatorNotifier(dto.ClientId, dto.EmployeeId ?? 0);
                }
				

				if (applicantApplicantPosting.JobProfileId != null)
				{
					var employeeAccruals = _session.UnitOfWork.EmployeeRepository
						.EmployeeAccrualQuery()
						.ByEmployeeId(dto.EmployeeId.Value)
						.ExecuteQuery()
						.ToList();

					foreach (var employeeAccrual in employeeAccruals)
					{
						_session.UnitOfWork.RegisterDeleted(employeeAccrual);
					}

					var jpas = _session.UnitOfWork.ClientRepository.
						JobProfileAccrualsQuery()
						.ByJobProfileId(applicantApplicantPosting.JobProfileId.Value)
						.ExecuteQuery()
						.ToList();

					foreach (var jpa in jpas)
					{
						var employeeAccrual = new EmployeeAccrual();
						employeeAccrual.EmployeeId = dto.EmployeeId.Value;
						employeeAccrual.ClientAccrualId = jpa.ClientAccrualId;
						employeeAccrual.IsAllowScheduledAwards = true;
						_session.SetModifiedProperties(employeeAccrual);
						_session.UnitOfWork.RegisterNew(employeeAccrual);
					}
				}
				_session.UnitOfWork.Commit().MergeInto(opResult);
			}

			//Update ApplicantOnBoardingHeader
			var applicantOnBoardingHeaders = _session.UnitOfWork.ApplicantTrackingRepository
				.ApplicantOnBoardingHeaderQuery()
				.ByApplicantApplicationHeaderId(dto.ApplicationHeaderId)
				.ExecuteQuery()
				.ToList();

			foreach (var applicantOnBoardingHeader in applicantOnBoardingHeaders)
			{
				applicantOnBoardingHeader.IsHired = true;
				applicantOnBoardingHeader.ModifiedBy = _session.LoggedInUserInformation.UserId;
				applicantOnBoardingHeader.Modified = DateTime.Now;
				applicantOnBoardingHeader.OnBoardingEnded = DateTime.Now;
				_session.UnitOfWork.RegisterModified(applicantOnBoardingHeader);
			}

			//Update ApplicantApplicationheader
			var applicantApplicationHeader = _session.UnitOfWork.ApplicantTrackingRepository
				.ApplicantApplicationHeaderQuery()
				.ByApplicationHeaderId(dto.ApplicationHeaderId)
				.ExecuteQuery()
				.FirstOrDefault();
			applicantApplicationHeader.ApplicantStatusTypeId = ApplicantStatusType.Hired;
			_session.UnitOfWork.RegisterModified(applicantApplicationHeader);


			//Update ApplicantPosting
			ApplicantPosting applicantPosting = _session.UnitOfWork.ApplicantTrackingRepository
				.ApplicantPostingsQuery()
				.ByPostingId(dto.PostingId)
				.ExecuteQuery()
				.FirstOrDefault();
			applicantPosting.FilledDate = DateTime.Today;
			applicantPosting.StaffHired =
				applicantApplicantPosting.FirstName + " " + applicantApplicantPosting.LastName;
			applicantPosting.ModifiedBy = _session.LoggedInUserInformation.UserId;
			applicantPosting.Modified = DateTime.Now;
			_session.UnitOfWork.RegisterModified(applicantPosting);

            if (dto.DirectSupervisorId.HasValue && dto.DirectSupervisorId.Value > 0)
			{
				_supervisorProvider.CreateSupervisorEmployeeAccessIfNotExist(dto.EmployeeId.Value, dto.DirectSupervisorId.Value).MergeInto(opResult);
			}

			//Insert to EmployeeOnboarding
			if (dto.StartedOnboarding)
			{
                bool obRecordExists = true;
				// if only onboarding not yet initiated
				EmployeeOnboarding onboard =
					_session.UnitOfWork.EmployeeRepository.GetEmployeeOnboarding(dto.EmployeeId.Value);

				if (onboard == null)
				{
                    onboard = new EmployeeOnboarding();
					_session.UnitOfWork.RegisterNew(onboard);
                    obRecordExists = false;
                }
                else
                {
                    _session.UnitOfWork.RegisterModified(onboard);
                }

                onboard.EmployeeId = dto.EmployeeId ?? 0;
                onboard.ClientId = dto.ClientId;
                onboard.OnboardingInitiated = DateTime.Now;
                onboard.EmployeeStarted = null;
                onboard.OnboardingEnd = null;
                onboard.EmployeeSignoff = null;
                onboard.ESSActivated = null;
                onboard.UserAddedDuringOnboarding = isIndeedApplicant;
                onboard.InvitationSent = null;
                onboard.Modified = DateTime.Now;
                onboard.ModifiedBy = _session.LoggedInUserInformation.UserId;

                // For the jobprofile choose assign default onboarding tasks to employee
                if( applicantApplicantPosting.JobProfileId.HasValue)
                {
                    int jobProfileId = applicantApplicantPosting.JobProfileId.Value;

                    // Profile work flow items
                    var profileWorkflowItems = _session.UnitOfWork.ClientRepository
                    .JobProfileOnboardingWorkflowQuery()
                    .ByJobProfileId(jobProfileId)
                    .ExecuteQueryAs(x => new EmployeeOnboardingWorkflowDto
                    {
                        EmployeeId = dto.EmployeeId.Value,
                        OnboardingWorkflowTaskId = x.OnboardingWorkflowTaskId,
                        FormTypeId = x.FormTypeId,
                        Modified = DateTime.Now,
                        ModifiedBy = _session.LoggedInUserInformation.UserId,
                    }).ToList();

                    // Profile admin task items
                    var profileAdminTaskListId = _session.UnitOfWork.ClientRepository
                        .JobProfileQuery()
                        .ByJobProfileId(jobProfileId)
                        .ExecuteQueryAs(x => x.OnboardingAdminTaskListId)
                        .FirstOrDefault();

                    // Retrieve the already assigned tasks
                    int eowItems = 0;
                    int eadmintaskItems = 0;
                    if (obRecordExists)
                    {
                        eowItems = _session.UnitOfWork.OnboardingRepository.EmployeeOnboardingWorkflowQuery()
                            .ByEmployeeId(dto.EmployeeId.Value)
                            .OrderBySequence().ExecuteQueryAs(x => x.EmployeeId).Count();

                        eadmintaskItems = _session.UnitOfWork.OnboardingRepository.EmployeeOnboardingTasksQuery()
                            .ByEmployeeId(dto.EmployeeId.Value)
                            .ExecuteQueryAs(x => x.EmployeeId).Count();
                    }

                    // If no onboarding workflow tasks yet assigned to the employee
                    if(eowItems == 0 && profileWorkflowItems.Count > 0)
                    {
                        _onboardingService.AddWorkflowEmployee(dto.HireDate.Value, profileWorkflowItems);
                    }

                    // if there is only 1 set of admin tasks available for profile and not yet assigned
                    if (eadmintaskItems == 0 && profileAdminTaskListId.HasValue )
                    {
                        // Profile admin work flow taks
                        var profileAdminTasks = _onboardingService.GetOnboardingTaskListByTaskListId(profileAdminTaskListId.Value).Data;
                        _onboardingService.ImportTaskLists(dto.EmployeeId.Value, profileAdminTasks );
                    }
                }
            }

			_session.UnitOfWork.Commit().MergeInto(opResult);

            var folder = _formProvider.GetRegisteredApplicantTrackingAttachmentFolder(dto.ClientId, dto.ApplicationHeaderId).MergeInto(opResult).Data;

            if (opResult.Success)
			{
				if (applicantPosting.NumOfPositions.HasValue)
				{
					opResult.Data =
						_session.UnitOfWork.ApplicantTrackingRepository.ApplicantApplicationHeaderQuery()
							.ByPostingId(applicantPosting.PostingId)
							.ByApplicantStatusTypeId(ApplicantStatusType.Hired)
							.ExecuteQuery().Count() >= applicantPosting.NumOfPositions.Value;
				}
				else
				{
					opResult.Data = true;
				}

			}

			return opResult;
		}
		
		IOpResult<ApplicantToEmployeePostDto> IApplicantTrackingService.GetApplicantEmployeeByApplicantId(int applicantId, int postingId)
		{
			var opResult = new OpResult<ApplicantToEmployeePostDto>();

            var empHeader = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantApplicationHeaderQuery()
                .ByApplicantId(applicantId)
                .ByPostingId(postingId)
                .ExecuteQueryAs(x => new
                {
                    x.Applicant.EmployeeId,
                    x.ApplicationHeaderId,
                    x.PostingId,
                    x.Applicant.ClientId,
                    x.Applicant.UserId,
                    x.ApplicantPosting.JobProfileId,
                    x.ApplicantPosting.Salary,
                    EmployeeStatusId = (x.ApplicantPosting.EmployeeStatus != null) ? x.ApplicantPosting.EmployeeStatus.EmployeeStatusId :
                        (x.ApplicantPosting.JobProfile != null && x.ApplicantPosting.JobProfile.JobProfileClassifications != null)
                            ? x.ApplicantPosting.JobProfile.JobProfileClassifications.EmployeeStatusId 
                            : (Core.Dto.Employee.EmployeeStatusType?)null,
                    PayFrequencyID = 
                        (x.ApplicantPosting.JobProfile != null && x.ApplicantPosting.JobProfile.JobProfileCompensation != null)
                            ? x.ApplicantPosting.JobProfile.JobProfileCompensation.PayFrequencyID 
                            : (PayFrequencyType?)null,
                    EmployeeTypeID = x.ApplicantPosting.JobProfile != null && x.ApplicantPosting.JobProfile.JobProfileCompensation != null ? x.ApplicantPosting.JobProfile.JobProfileCompensation.EmployeeTypeID : null,
                    DirectSupervisorID = 
                        (x.ApplicantPosting.JobProfile != null && x.ApplicantPosting.JobProfile.JobProfileClassifications != null)
                            ? x.ApplicantPosting.JobProfile.JobProfileClassifications.DirectSupervisorId 
                            : (int?)null,

				}).FirstOrDefault();

			//Is already an Employee
			if (empHeader != null && empHeader.EmployeeId.HasValue && empHeader.EmployeeId.Value > 0)
			{
				ApplicantToEmployeePostDto dto = new ApplicantToEmployeePostDto();
				int employeeId = empHeader.EmployeeId.HasValue ? empHeader.EmployeeId.Value : 0;

				// 1. Employee info
				Employee emp = _session.UnitOfWork.EmployeeRepository.GetEmployee(employeeId);

				dto.PostingId = empHeader.PostingId;
				dto.ApplicationHeaderId = empHeader.ApplicationHeaderId;
				dto.EmployeeId = employeeId;
				dto.EmployeeNumber = int.Parse(emp.EmployeeNumber);
				dto.Gender = emp.Gender;
				dto.SocialSecurityNumber = emp.SocialSecurityNumber;
				dto.HireDate = emp.HireDate;
				dto.StartedOnboarding = emp.IsInOnboarding;
				dto.ClientId = empHeader.ClientId;
				dto.UserId = empHeader.UserId;
				dto.ApplicantId = applicantId;

				var payInfo = _session.UnitOfWork.PayrollRepository.QueryEmployeePay()
					.ByEmployeeId(employeeId)
					.ExecuteQueryAs(x => new
					{
						Status = x.EmployeeStatusId,
						PayType = x.Type,
						SalaryAmount = x.SalaryAmount,
						PayFrequency = x.PayFrequencyId,
						SalaryEffectiveDate = x.SalaryAmountEffectiveDate,
						ClientTaxId = x.ClientTaxId
					}).FirstOrDefault();

				// 2. Pay info

				dto.PayFrequencyId = (((int?)payInfo?.PayFrequency).HasValue ? ((int?)payInfo?.PayFrequency).Value : (empHeader.PayFrequencyID.HasValue ? (int)empHeader.PayFrequencyID.Value :  0));
				dto.EmployeePayTypeId = (((int?)payInfo?.PayType).HasValue ? ((int?)payInfo?.PayType).Value : (empHeader.EmployeeTypeID.HasValue ? (int)empHeader.EmployeeTypeID.Value : 0));
				dto.EmployeeStatusId = (((int?)payInfo?.Status).HasValue ? ((int?)payInfo?.Status).Value : (empHeader.EmployeeStatusId.HasValue ? (int)empHeader.EmployeeStatusId.Value : 0));
				dto.DirectSupervisorId = (((int?)emp?.DirectSupervisorID).HasValue ? ((int?)emp?.DirectSupervisorID).Value : (empHeader.DirectSupervisorID.HasValue ? (int)empHeader.DirectSupervisorID.Value : 0));
				dto.Salary = (decimal?)payInfo?.SalaryAmount;


				var clientRate = _session.UnitOfWork.EmployeeRepository.EmployeeClientRateQuery()
				.ByEmployeeId(employeeId)
				.FirstOrDefault();

				// 3. Client Rate
				if (clientRate != null)
				{
					dto.HourlyRateTypeId = clientRate.ClientRateId;
					dto.HourlyRate = (decimal)clientRate.Rate;
				}

				// 4. SUTA State
				if (((int?)payInfo?.ClientTaxId).HasValue)
				{
				    dto.SutaState = _session.UnitOfWork.ClientRepository.QueryClientTaxes()
				    .ByClientTaxId(((int?)payInfo?.ClientTaxId).Value)
				    .ExecuteQueryAs(x => new BasicTaxDto()
				    {
				        Name = x.LegacyStateTax.State.Name,
				        StateId = x.LegacyStateTax.State.StateId,
				        LegacyTaxId = x.ClientTaxId
				    }).FirstOrDefault();
				}

				opResult.Data = dto;
			}
			else
			{
				ApplicantToEmployeePostDto dto = new ApplicantToEmployeePostDto();

				dto.PostingId = empHeader.PostingId;
				dto.ApplicationHeaderId = empHeader.ApplicationHeaderId;
				dto.ClientId = empHeader.ClientId;
				dto.UserId = empHeader.UserId;
				dto.ApplicantId = applicantId;

				dto.PayFrequencyId = empHeader.PayFrequencyID.HasValue ? (int)empHeader.PayFrequencyID : 0;
				dto.EmployeePayTypeId = empHeader.EmployeeTypeID.HasValue ? (int)empHeader.EmployeeTypeID : 0;
				dto.EmployeeStatusId = empHeader.EmployeeStatusId.HasValue ? (int)empHeader.EmployeeStatusId : 0;
				dto.DirectSupervisorId = empHeader.DirectSupervisorID.HasValue ? (int)empHeader.DirectSupervisorID : 0;
				opResult.Data = dto;
			}

			return opResult;
		}

        IOpResult<TerminatedEmployeeInfoDto> IApplicantTrackingService.GetTerminationInformation(int clientId, DateTime startDate, DateTime endDate, IEnumerable<int> employeeIds, int userId, int userTypeId)
        {
            var opResult = new OpResult<TerminatedEmployeeInfoDto>();

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(opResult);

            var employeeList = _session.UnitOfWork.PayrollRepository
                .QueryEmployeePay()
                .ByClient(clientId)
                .ByEmployeeIds(employeeIds)
                .ExecuteQueryAs(EmployeeHireMapper.MapToTerminationDto);

            var terminatedEmployeees = new List<TerminatedEmployeeDto>();
            var newHires = new List<TerminatedEmployeeDto>();

            int startCount = 0;
            int endCount = 0;
            int totalCount = 0;

            int headcountBoth = 0;
            int headcountStart = 0;
            int headcountEnd = 0;
            int terminatedCount = 0;

			startDate = startDate.Date;
			endDate = endDate.Date;

            List<int> list = new List<int>();
            foreach (var item in employeeList)
            {
                list.Add(item.EmployeeId);
            }
            var filterEmployees = _supervisorProvider.FilterEmployeesBySupervisor(list, userId, clientId);
            var supervisorEmployees = filterEmployees.Data.ToList();

			// 1 Rehire = 2 Employees Hired (except they worked on different periods)
			List<TerminatedEmployeeDto> rehires = new List<TerminatedEmployeeDto>();
			employeeList.Where(x => EmployeeHireMapper.IsEmployeeReHired(x)).ForEach(y => rehires.Add(EmployeeHireMapper.CopyTerminationDto(y)));
			var employeeList2 = EmployeeHireMapper.SeparateRehires<TerminatedEmployeeDto>(employeeList, rehires);

			foreach (var datum in employeeList2)
            {
                if (datum.EmployeeStatusId != EmployeeStatusType.FullTimeTemp && datum.EmployeeStatusId != EmployeeStatusType.PartTimeTemp)
                {
                    //Supervisors can only see their employees
                    if (userTypeId == 4)
                    {
                        //Add Terminated Employees
                        if (EmployeeHireMapper.IsTerminatedOnInterval(datum,startDate.Date, endDate.Date ) && supervisorEmployees.Contains(datum.EmployeeId))
                        {
                            terminatedEmployeees.Add(datum);
                            terminatedCount++;
                        }
                        //The overall turnover percentage should not change according to user permissions
                        if (EmployeeHireMapper.IsTerminatedOnInterval(datum, startDate.Date, endDate.Date) && !supervisorEmployees.Contains(datum.EmployeeId))
                        {
                            terminatedCount++;
                        }
                    }
                    else
                    {
                        //Add Terminated Employees
                        if (EmployeeHireMapper.IsTerminatedOnInterval(datum, startDate.Date, endDate.Date))
                        {
                            terminatedEmployeees.Add(datum);
                            terminatedCount++;
                        }
                    }

                    //Add New Hires
                    if (EmployeeHireMapper.IsNewHireOnInterval(datum, startDate.Date, endDate.Date))
					{
                        newHires.Add(datum);
                    }

                    //Start Count
                    if (EmployeeHireMapper.IsEmployedAsOn(datum,startDate.Date))
					{
                        startCount++;
                    }

                    //End Count
                    if (EmployeeHireMapper.IsEmployedAsOn(datum, endDate.Date))
					{
                        endCount++;
                    }
                }
            }

            newHires = newHires.OrderBy(s => s.LastName).ToList();
            terminatedEmployeees = terminatedEmployeees.OrderBy(s => s.LastName).ToList();

            totalCount = startCount + endCount;

            TerminatedEmployeeInfoDto dto = new TerminatedEmployeeInfoDto()
            {
                TotalCount = totalCount,
				TurnoverRate = totalCount == 0 ? 0 : (((float)terminatedCount) / ((float)totalCount / 2)) * 100,
				RetentionRate = startCount == 0 ? 100 : (((float)startCount - (float)terminatedCount) / ((float)startCount)) * 100,
				GrowthRate = startCount == 0 ? 100 : (((float)endCount - (float)startCount) / ((float)startCount)) * 100,
				TerminatedEmployees = terminatedEmployeees,
                NewHires = newHires
            };

            opResult.TrySetData(() =>
                dto
            );

            return opResult;
        }

        IOpResult<IEnumerable<TerminatedEmployeeInfoDto>> IApplicantTrackingService.GetHistoryInformation(int clientId)
		{
			var opResult = new OpResult<IEnumerable<TerminatedEmployeeInfoDto>>();

			_session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(opResult);

			var employeeList = _session.UnitOfWork.PayrollRepository
				.QueryEmployeePay()
				.ByClient(clientId)
				.ExecuteQueryAs(x => new TerminatedEmployeeDto
				{
					EmployeeId = x.EmployeeId,
					EmployeeStatusId = x.EmployeeStatusId,
					FirstName = x.Employee.FirstName,
					LastName = x.Employee.LastName,
					Department = x.Employee.Department.Name,
					CostCenter = x.Employee.CostCenter.Description,
					HireDate = x.Employee.HireDate,
					SeparationDate = x.Employee.SeparationDate,
					ReHireDate = x.Employee.RehireDate ,
					TerminationReason = x.Employee.EmployeePayInfo.Select(y => y.EmployeeTerminationReasonE.Description).FirstOrDefault()
				});

            ClientDto companyData = _clientService.GetClient(clientId).MergeInto(opResult).Data;
            DateTime? companyStartDate = companyData.StartDate;

			List<TerminatedEmployeeInfoDto> historyInfo = new List<TerminatedEmployeeInfoDto>();
			DateTime now = DateTime.Now;
			DateTime tempStart = new DateTime(now.Year, now.Month, 1);
			DateTime tempEnd = new DateTime(now.Year, now.Month, 1);
			tempEnd = tempEnd.AddMonths(1).AddDays(-1);
			var yearsWithData = new List<int>();

			List<TerminatedEmployeeDto> rehires = new List<TerminatedEmployeeDto>();
			employeeList.Where(x => EmployeeHireMapper.IsEmployeeReHired(x)).ForEach(y => rehires.Add(EmployeeHireMapper.CopyTerminationDto(y)));
			var employeeList2 = EmployeeHireMapper.SeparateRehires<TerminatedEmployeeDto>(employeeList, rehires);

			while ((tempEnd.Year == now.Year) || (yearsWithData.Contains(tempEnd.Year+1)))
            {
                if (companyStartDate.HasValue && ( (companyStartDate.Value.Year * 100 + companyStartDate.Value.Month) > 
                    (tempStart.Year * 100 + tempStart.Month) ) ) break;

				var terminatedEmployeees = new List<TerminatedEmployeeDto>();
				var newHires = new List<TerminatedEmployeeDto>();

				int startCount = 0;
				int endCount = 0;
				int terminatedCount = 0;
				int totalCount = 0;

				foreach (var datum in employeeList2)
				{
					if (datum.EmployeeStatusId != EmployeeStatusType.FullTimeTemp && datum.EmployeeStatusId != EmployeeStatusType.PartTimeTemp)
					{
						//Add Terminated Employees
						if (EmployeeHireMapper.IsTerminatedOnInterval(datum, tempStart.Date, tempEnd.Date))
						{
							terminatedEmployeees.Add(datum);
							terminatedCount++;
						}

						//Add New Hires
						if (EmployeeHireMapper.IsNewHireOnInterval(datum, tempStart.Date, tempEnd.Date))
						{
							newHires.Add(datum);
						}

						//Start Count
						if (EmployeeHireMapper.IsEmployedAsOn(datum, tempStart))
						{
							startCount++;
						}

						//End Count
						if (EmployeeHireMapper.IsEmployedAsOn(datum, tempEnd))
						{
							endCount++;
						}
					}
				}

				if ((terminatedEmployeees.Count > 0 || newHires.Count > 0) && !yearsWithData.Contains(tempEnd.Year))
					yearsWithData.Add(tempEnd.Year);
					

				totalCount = (startCount + endCount) ;

				TerminatedEmployeeInfoDto dto = new TerminatedEmployeeInfoDto()
				{
                    MonthStartDate = tempStart,
					TotalCount = totalCount,
					TurnoverRate = totalCount == 0 ? 0 : (((float)terminatedCount) / ((float)totalCount / 2)) * 100,
					RetentionRate = startCount == 0 ? 100 : (((float)startCount - (float)terminatedCount) / ((float)startCount)) * 100,
					GrowthRate = startCount == 0 ? 100 : (((float)endCount - (float)startCount) / ((float)startCount)) * 100,
					TerminatedEmployees = terminatedEmployeees,
					NewHires = newHires
				};

				historyInfo.Add(dto);

				tempEnd = (new DateTime(tempStart.Year,tempStart.Month,1)).AddDays(-1);
				tempStart = tempStart.AddMonths(-1);
			}

			for (int i = historyInfo.Count; i <= 12; i++)
			{
				historyInfo.Add(new TerminatedEmployeeInfoDto()
				{
					TotalCount = 0,
					TurnoverRate = 0,
					RetentionRate = 0,
					GrowthRate = 0,
					TerminatedEmployees = null,
					NewHires = null
				});
			}

			bool empty = true;
			foreach (var d in historyInfo)
            {
				if (d.TotalCount != 0)
					empty = false;
				if (!empty)
					break;
            }

			if (!empty)
				opResult.TrySetData(() => historyInfo);
			else
				opResult = new OpResult<IEnumerable<TerminatedEmployeeInfoDto>>();

			return opResult;
		}

			IOpResult<EffectiveDateDto> IApplicantTrackingService.UpdateEffectiveDate(EffectiveDateDto dto)
		{
			var opResult = new OpResult<EffectiveDateDto>();
			var isApplicantAdmin = _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).Success;
			var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
			if (isApplicantAdmin || isSystemAdmin)
			{
				var effectiveDate = new EffectiveDate();
				if (dto.EffectiveId == 0)
				{
					effectiveDate = new EffectiveDate
					{
						EmployeeId = dto.EmployeeId,
						Table = dto.Table,
						Column = dto.Column,
						TablePkId = dto.TablePkId,
						OldValue = dto.OldValue,
						NewValue = dto.NewValue,
						Datatype = dto.Datatype,
						DateEffective = dto.EffectiveDate,
						DateAppliedOn = dto.AppliedOn,
						AppliedBy = dto.AppliedBy,
						FriendlyView = dto.FriendlyView,
						Accepted = dto.Accepted,
						EffectiveDateType = (EffectiveDateTypes)dto.Type,
						CreatedBy = dto.CreatedBy,
						DateCreatedOn = dto.CreatedOn
					};
					_session.UnitOfWork.RegisterNew(effectiveDate);
				}
				else
				{
					effectiveDate = _session.UnitOfWork.MiscRepository.EffectiveDateQuery()
						.ByEffectiveId(dto.EffectiveId).FirstOrDefault();

					if (effectiveDate != null)
					{
						effectiveDate.EmployeeId = dto.EmployeeId;
						effectiveDate.Table = dto.Table;
						effectiveDate.Column = dto.Column;
						effectiveDate.TablePkId = dto.TablePkId;
						effectiveDate.OldValue = dto.OldValue;
						effectiveDate.NewValue = dto.NewValue;
						effectiveDate.Datatype = dto.Datatype;
						effectiveDate.DateEffective = dto.EffectiveDate;
						effectiveDate.DateAppliedOn = dto.AppliedOn;
						effectiveDate.AppliedBy = dto.AppliedBy;
						effectiveDate.FriendlyView = dto.FriendlyView;
						effectiveDate.Accepted = dto.Accepted;
						effectiveDate.EffectiveDateType = (EffectiveDateTypes)dto.Type;
					}
					_session.UnitOfWork.RegisterModified(effectiveDate);
				}
				_session.UnitOfWork.Commit().MergeInto(opResult);

				if (dto.EffectiveId == 0)
				{
					dto.EffectiveId = effectiveDate.EffectiveId;
				}

				opResult.Data = dto;
			}

			return opResult;
		}

		#region ApplicantOnboardingProcess
		IOpResult<IEnumerable<ApplicantOnBoardingProcessDto>> IApplicantTrackingService.GetApplicantOnboardingProcess(int? clientId)
		{
			var opResult = new OpResult<IEnumerable<ApplicantOnBoardingProcessDto>>();
			if (!clientId.HasValue)
				clientId = _session.LoggedInUserInformation.ClientId;

			_session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			_session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId.Value).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantOnBoardingProcessQuery()
					.ByClientId(clientId.Value)
					.ByIsEnabled(true)
					.ExecuteQueryAs(ApplicantTrackingMaps.FromApplicantOnboardingProcess.ToApplicantOnboardingProcessDto.Instance).ToList());

			return opResult;
		}

		IOpResult<IEnumerable<ApplicantOnboardingProcessTypeDto>> IApplicantTrackingService.GetApplicantOnBoardingProcessType()
		{
			var opResult = new OpResult<IEnumerable<ApplicantOnboardingProcessTypeDto>>();

			_session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;


			opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantOnboardingProcessTypeQuery()
					.OrderByDescription()
					.ExecuteQueryAs(x => new ApplicantOnboardingProcessTypeDto
					{
						ApplicantOnBoardingProcessTypeId = x.ApplicantOnBoardingProcessTypeId,
						Description = x.Description

					}).ToList());

			return opResult;
		}

		IOpResult<IEnumerable<ApplicantOnBoardingTaskDto>> IApplicantTrackingService.GetAvailableProcessTasks(int processId, int clientId)
		{
			var opResult = new OpResult<IEnumerable<ApplicantOnBoardingTaskDto>>();

			_session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			_session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantOnBoardingTaskQuery()
				.ByClientId(clientId)
				.IsActive(true)
				.ExcludeTasksByProcessId(processId)
				.ExecuteQueryAs(x => new ApplicantOnBoardingTaskDto
				{
					ApplicantOnboardingTaskId = x.ApplicantOnboardingTaskId,
					Description = x.Description

				}).ToList());

			return opResult;
		}

        IOpResult<ApplicantOnBoardingProcessDto> IApplicantTrackingService.SaveApplicantOnboardingProcess(ApplicantOnBoardingProcessDto dto)
        {
            var opResult = new OpResult<ApplicantOnBoardingProcessDto>(dto);
            var newId = dto.ApplicantOnboardingProcessId;

            _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, dto.ClientId).MergeInto(opResult);

            if (opResult.HasError) return opResult;

            if (dto.IsNewEntity(x => x.ApplicantOnboardingProcessId))
            {
                var aop = new ApplicantOnBoardingProcess()
                {
                    ClientId = dto.ClientId,
                    Description = dto.Description,
                    CustomToPostingId = dto.CustomToPostingId,
                    IsEnabled = true,
                    ApplicantOnBoardingProcessTypeId = dto.ApplicantOnBoardingProcessTypeId
                };
                _session.SetModifiedProperties(aop);
                _session.UnitOfWork.RegisterPostCommitAction(() => newId = aop.ApplicantOnboardingProcessId);
                _session.UnitOfWork.RegisterNew(aop);
			}
			else
			{
				var editaop = _session.UnitOfWork.ApplicantTrackingRepository
										.ApplicantOnBoardingProcessQuery()
										.ByApplicantOnboardingProcessId(dto.ApplicantOnboardingProcessId)
										.FirstOrDefault();
				editaop.Description = dto.Description;
				editaop.ApplicantOnBoardingProcessTypeId = dto.ApplicantOnBoardingProcessTypeId;
				_session.SetModifiedProperties(editaop);
				_session.UnitOfWork.RegisterModified(editaop);

			}

			var existingProcessSets = _session.UnitOfWork.ApplicantTrackingRepository
				.ApplicantOnBoardingProcessSetQuery()
				.ByApplicantOnboardingProcessId(dto.ApplicantOnboardingProcessId)
				.ExecuteQuery()
				.ToList();

			opResult.CheckForNotFound(existingProcessSets);
			if (!opResult.HasError)
			{
				var deleteItems = existingProcessSets.Where(x => !dto.ApplicantOnBoardingProcessSets.Any(y => y.ApplicantOnboardingTaskId == x.ApplicantOnboardingTaskId));

				foreach (ApplicantOnBoardingProcessSet aops in deleteItems)
				{
					_session.UnitOfWork.RegisterDeleted(aops);

				}
			}
			foreach (ApplicantOnBoardingProcessSetDto aopd in dto.ApplicantOnBoardingProcessSets)
			{
				var isNew = true;
				var eaops = new ApplicantOnBoardingProcessSet();
				foreach (ApplicantOnBoardingProcessSet aops in existingProcessSets)
				{
					if (aopd.ApplicantOnboardingTaskId == aops.ApplicantOnboardingTaskId)
					{
						isNew = false;
						eaops = aops;
					}
				}
				if (isNew)
				{
					var newItem = new ApplicantOnBoardingProcessSet()
					{
						ApplicantOnboardingProcessId = dto.ApplicantOnboardingProcessId,
						ApplicantOnboardingTaskId = aopd.ApplicantOnboardingTaskId,
						OrderId = aopd.OrderId
					};
                    _session.UnitOfWork.RegisterPostCommitAction(() => aopd.ApplicantOnboardingProcessId = newItem.ApplicantOnboardingProcessId);
                    _session.UnitOfWork.RegisterNew(newItem);
				}
				else
				{
					eaops.OrderId = aopd.OrderId;
					_session.UnitOfWork.RegisterModified(eaops);
				}

			}

			_session.UnitOfWork.Commit().MergeInto(opResult);

            if (opResult.HasError)
                return opResult;

            opResult.Data.ApplicantOnboardingProcessId = newId;

            return opResult;
        }

		IOpResult IApplicantTrackingService.DeleteApplicantOnboardingProcess(int onboardingProcessId)
		{
			var opResult = new OpResult();
			_session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;


			var aop = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantOnBoardingProcessQuery()
						.ByApplicantOnboardingProcessId(onboardingProcessId).FirstOrDefault();

			opResult.CheckForNotFound(aop);
			if (opResult.HasError)
				return opResult;

			_session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, aop.ClientId).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			aop.IsEnabled = false;
			_session.SetModifiedProperties(aop);
			_session.UnitOfWork.RegisterModified(aop);
			_session.UnitOfWork.Commit().MergeInto(opResult);
			return opResult;
		}

		IOpResult IApplicantTrackingService.SaveApplicantOnboardingProcessSets(List<ApplicantOnBoardingProcessSetDto> dto, int applicantOnboardingProcessId)
		{
			var opResult = new OpResult();

			_session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			var aop = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantOnBoardingProcessQuery()
						.ByApplicantOnboardingProcessId(applicantOnboardingProcessId).FirstOrDefault();

			opResult.CheckForNotFound(aop);
			if (opResult.HasError)
				return opResult;

			_session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, aop.ClientId).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			var existingProcessSets = _session.UnitOfWork.ApplicantTrackingRepository
				.ApplicantOnBoardingProcessSetQuery()
				.ByApplicantOnboardingProcessId(applicantOnboardingProcessId)
				.ExecuteQuery()
				.ToList();

			var deleteItems = existingProcessSets.Where(x => !dto.Any(y => y.ApplicantOnboardingTaskId == x.ApplicantOnboardingTaskId));

			foreach (ApplicantOnBoardingProcessSet aops in deleteItems)
			{
				_session.UnitOfWork.RegisterDeleted(aops);

			}

			foreach (ApplicantOnBoardingProcessSetDto aopd in dto)
			{
				var isNew = true;
				var eaops = new ApplicantOnBoardingProcessSet();
				foreach (ApplicantOnBoardingProcessSet aops in existingProcessSets)
				{
					if (aopd.ApplicantOnboardingTaskId == aops.ApplicantOnboardingTaskId)
					{
						isNew = false;
						eaops = aops;
					}
				}
				if (isNew)
				{
					var newItem = new ApplicantOnBoardingProcessSet()
					{
						ApplicantOnboardingProcessId = aopd.ApplicantOnboardingProcessId,
						ApplicantOnboardingTaskId = aopd.ApplicantOnboardingTaskId,
						OrderId = aopd.OrderId
					};
					_session.UnitOfWork.RegisterNew(newItem);
				}
				else
				{
					eaops.OrderId = aopd.OrderId;
					_session.UnitOfWork.RegisterModified(eaops);
				}

			}
			_session.UnitOfWork.Commit().MergeInto(opResult);

			return opResult;
		}

		IOpResult<ApplicantOnBoardingProcessDto> IApplicantTrackingService.CopyApplicantOnboardingProcess(int applicantOnBoardingProcessId)
		{
			var opResult = new OpResult<ApplicantOnBoardingProcessDto>();

			_session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;


			var existingProcess = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantOnBoardingProcessQuery()
					.ByApplicantOnboardingProcessId(applicantOnBoardingProcessId).FirstOrDefault();
			opResult.CheckForNotFound(existingProcess);
			if (opResult.HasError)
				return opResult;

			_session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, existingProcess.ClientId).MergeInto(opResult);
			if (opResult.HasError)
				return opResult;

			var newProcess = new ApplicantOnBoardingProcess();
			if (existingProcess != null)
			{

				newProcess.ClientId = existingProcess.ClientId;
                                newProcess.Description = "Copy of: " + existingProcess.Description;
				newProcess.CustomToPostingId = existingProcess.CustomToPostingId;
				newProcess.ApplicantOnBoardingProcessTypeId = existingProcess.ApplicantOnBoardingProcessTypeId;
				newProcess.ProcessPhaseId = existingProcess.ProcessPhaseId;
				newProcess.IsEnabled = true;
				_session.SetModifiedProperties(newProcess);
				_session.UnitOfWork.RegisterNew(newProcess);
				_session.UnitOfWork.Commit().MergeInto(opResult);
				if (opResult.Success)
				{
					var existingProcessSets = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantOnBoardingProcessSetQuery()
					.ByApplicantOnboardingProcessId(applicantOnBoardingProcessId)
					.ExecuteQuery()
					.ToList();

					foreach (var existingProcessSet in existingProcessSets)
					{
						var newProcessSet = new ApplicantOnBoardingProcessSet();
						newProcessSet.ApplicantOnboardingProcessId = newProcess.ApplicantOnboardingProcessId;
						newProcessSet.ApplicantOnboardingTaskId = existingProcessSet.ApplicantOnboardingTaskId;
						newProcessSet.OrderId = existingProcessSet.OrderId;

						_session.UnitOfWork.RegisterNew(newProcessSet);
					}
					_session.UnitOfWork.Commit().MergeInto(opResult);

					if (opResult.Success)
					{
						opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantOnBoardingProcessQuery()
					 .ByApplicantOnboardingProcessId(newProcess.ApplicantOnboardingProcessId)
					 .ExecuteQueryAs(ApplicantTrackingMaps.FromApplicantOnboardingProcess.ToApplicantOnboardingProcessDto.Instance).FirstOrDefault());
					}
				}

			}

			return opResult;
		}

		IOpResult IApplicantTrackingService.UpdateClientJobSite(ClientJobSiteDto dto)
		{
			var result = new OpResult();

			var entity = new ClientJobSite
			{
				ClientJobSiteId = dto.ClientJobSiteId,
				ClientId = dto.ClientId,
				ApplicantJobSiteId = dto.ApplicantJobSiteId,
				Code = dto.Code,
				Counter = dto.Counter,
				SharePosts = dto.SharePosts,
				Email = dto.Email
			};

			_session.UnitOfWork.RegisterModified(entity);
			_session.UnitOfWork.Commit().MergeInto(result);

			return result;
		}

		public IOpResult<IEnumerable<ApplicantReferenceDto>> GetApplicantReferencesForCurrentUser()
		{
			IOpResult<IEnumerable<ApplicantReferenceDto>> result;
			IApplicantTrackingService service = this;
			var applicant = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantsQuery()
				.ByUserId(_session.LoggedInUserInformation.UserId).ExecuteQuery().FirstOrDefault();
			if (applicant != null)
			{
				result = service.GetApplicantReferencesByApplicantId(applicant.ApplicantId);
			}
			else
			{
				result = new OpResult<IEnumerable<ApplicantReferenceDto>>();
			}

			return result;
		}

		//IOpResult<IEnumerable<ApplicantOnBoardingProcessSetDto>> IApplicantTrackingService.GetSelectedProcessTasks(int processId)
		//{
		//    var opResult = new OpResult<IEnumerable<ApplicantOnBoardingProcessSetDto>>();
		//    var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
		//    var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
		//    var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
		//    if (isApplicantTrackingEnabled || isSystemAdmin || accessRights.UserId == 0)
		//    {

		//        opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantOnBoardingProcessSetQuery()
		//             .ByApplicantOnboardingProcessId(processId)
		//            .ExecuteQueryAs(x => new ApplicantOnBoardingProcessSetDto
		//            {
		//                ApplicantOnboardingProcessId = x.ApplicantOnboardingProcessId,
		//                ApplicantOnboardingTaskId = x.ApplicantOnboardingTaskId,
		//                Description = x.ApplicantOnBoardingProcess.Description

		//            }).ToList());
		//    }
		//    return opResult;
		//}

		#endregion

		IOpResult<AddApplicantDto> IApplicantTrackingService.AddNewUser(AddApplicantDto dto)
		{

			//WARNING NO SECURITY MEASURES IMPLEMENTED

			return _appTrackProvider.AddNewUser(dto);
		}

        IOpResult<bool> IApplicantTrackingService.GetClientLegacySecuritySettingForAnonymousUser(int clientId)
        {
            return _securityService.GetClientLegacySecuritySettingForAnonymousUser(clientId);
        }

        /// <summary>
        /// Utility method used in placing data to emails
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public string GetEmailBody(ApplicantTrackingCorrespondenceReplacementInfoDto dto)
		{
            var mainLink = ConfigurationManager.AppSettings.Get("MainRedirectRootUrl");

            StringBuilder mailBody = new StringBuilder(dto.TemplateBody);
            mailBody.Replace("{*Applicant}", dto.ApplicantFirstName + " " + dto.ApplicantLastName);
            mailBody.Replace("{*ApplicantFirstName}", dto.ApplicantFirstName );
            mailBody.Replace("{*UserName}", dto.UserName);
            mailBody.Replace("{*Password}", string.IsNullOrEmpty( dto.Password) ? "********" : dto.Password);
            mailBody.Replace("{*OnboardingUrl}", "<a href='" + mainLink + "'>ONBOARDING</a>");
			mailBody.Replace("{*Posting}", dto.Posting);
			mailBody.Replace("{*Date}", dto.Date.ToShortDateString());
			mailBody.Replace("{*ApplicantAddress}", dto.Address);
			mailBody.Replace("{*ApplicantPhoneNumber}", dto.Phone);
			mailBody.Replace("{*CompanyAddress}", dto.CompanyAddress);
			if(! string.IsNullOrEmpty(dto.CompanyLogo))
				mailBody.Replace("{*CompanyLogo}", "<img src='" + dto.CompanyLogo + "'/>");
			else
				mailBody.Replace("{*CompanyLogo}", "");

			mailBody.Replace("{*CompanyName}", dto.CompanyName);

			return mailBody.ToString();
		}
		[Obsolete("GetGrowthRate has count information, which can be utilized for Turnover rate calculations.")]
		IOpResult<TurnoverRateDto> IApplicantTrackingService.GetTurnoverRate(int clientId, DateTime startDate, DateTime endDate, IEnumerable<int> employeeIds, int userId, int userTypeId)
        {
            var result = new OpResult<TurnoverRateDto>();

            //filter entries by clientid so that we avoid making 3 calls to the db (one call for each result set with unique constraints--see for each loop below).
            var employeePay = _session.UnitOfWork.PayrollRepository.QueryEmployeePay().ByClient(clientId);
            var employee = _session.UnitOfWork.EmployeeRepository.QueryEmployees();
            if (employeeIds != null) employee = employee.ByEmployeeIds(employeeIds);
			var allData = _session.UnitOfWork.EmployeeRepository.EmployeeStatusQuery().JoinEmployeePayEmployeeStatusAndEmployee(employeePay, employee, clientId).Execute();

            var startCount = 0;
            var endCount = 0;
            var termedCount = 0;

            startDate = startDate.AddDays(1);
            startDate = startDate.AddHours(endDate.Hour);
            startDate = startDate.AddMinutes(endDate.Minute);
            startDate = startDate.AddSeconds(endDate.Second);

            //Go through all data returned and extract: 
            //the amount of active employees hired before the start date
            //the amount of active employees hired before the end date
            //and the amount of deactivated employees who were 'separated' between the start date and the end date or who were not 'separated' but whose employeepay record was modified between the start date and end date
            foreach (var datum in allData)
            {
                if (datum.HireDate < startDate.Date && datum.Active)
                {
                    startCount++;
                }

                if (datum.HireDate <= endDate.Date && datum.Active)
                {
                    endCount++;
                }

                if (((datum.SeparationDate >= startDate && datum.SeparationDate <= endDate) ||
                    (!(datum.SeparationDate >= DateTime.Now || datum.SeparationDate <= DateTime.Now) &&
                     datum.Modified >= startDate && datum.Modified <= endDate)) && !datum.Active)
                {
                    termedCount++;

					// This employee is also part of start count
					startCount++;
				}
            }

            result.Data = new TurnoverRateDto()
            {
                EndCount = endCount,
                StartCount = startCount,
                TermedCount = termedCount
            };

            return result;
        }

        IOpResult<TurnoverRateDto> IApplicantTrackingService.GetGrowthRate(int clientId, DateTime startDate, DateTime endDate, IEnumerable<int> employeeIds, int userId, int userTypeId)
        {
            var result = new OpResult<TurnoverRateDto>();

            var employees = new OpResult<IEnumerable<ActiveEmployeeDto>>();

            _session.CanPerformAction(EmployeeManagerActionType.EmployeeView).MergeInto(employees);
            if (employees.Success)
            {
                employees.TrySetData(() =>
                    _session.UnitOfWork.EmployeeRepository
                        .QueryEmployees()
                        .ByClientId(_session.LoggedInUserInformation.ClientId.Value)
                        .ByEmployeeIds(employeeIds)
                        .ExecuteQueryAs(EmployeeHireMapper.MapToActiveEmployeeDto)
                        .ToList());
            }

            employees.Data = employees.Data.Where(s => s.EmployeeStatus != null && s.EmployeeStatus != "Full Time Temp" && s.EmployeeStatus != "Part Time Temp");
			//employees.Data = employees.Data.Where(s => s.IsActive == true);

			List<ActiveEmployeeDto> rehires = new List<ActiveEmployeeDto>();
			employees.Data.Where(x => EmployeeHireMapper.IsEmployeeReHired(x)).ForEach(y => rehires.Add(EmployeeHireMapper.CopyActiveEmployeeDto(y)));
			var employeeList2 = EmployeeHireMapper.SeparateRehires<ActiveEmployeeDto>(employees.Data, rehires);

			var allData = new List<ActiveEmployeeDto>();
            allData = employeeList2.ToList<ActiveEmployeeDto>();

            var startCount = 0;
            var endCount = 0;
            var termedCount = 0;
			var hiredAndTermed = 0;
			var newCount = 0;
			endDate = endDate.Date;
			bool newE, termedE;

            //Go through all data returned and extract: 
            //the amount of active employees hired before the start date
            //the amount of active employees hired before the end date
            //and the amount of deactivated employees who were 'separated' between the start date and the end date or who were not 'separated' but whose employeepay record was modified between the start date and end date
            foreach (var datum in allData)
            {
                if (EmployeeHireMapper.IsEmployedAsOn(datum, startDate.Date))
                {
                    startCount++;
                }

                if (EmployeeHireMapper.IsEmployedAsOn(datum, endDate.Date))
                {
                    endCount++;
				}

				newE = false;termedE = false;
				if (EmployeeHireMapper.IsNewHireOnInterval(datum,startDate.Date, endDate.Date))
				{
					newCount++;
					newE = true;
				}

				if (EmployeeHireMapper.IsTerminatedOnInterval(datum, startDate.Date, endDate.Date))
				{
					termedCount++;
					termedE = true;
				}

				if (newE && termedE)
				{
					// This employee is hired and terminated within this date range.
					hiredAndTermed++;
				}
			}

            result.Data = new TurnoverRateDto()
            {
                EndCount = endCount,
                StartCount = startCount,
                TermedCount = termedCount,
				NewCount = newCount,
				HiredAndTermedCount = hiredAndTermed,
            };

            return result;
        }

        IOpResult<IEnumerable<EmployeePayAndEmployeeStatusAndEmployeeWithReason>> IApplicantTrackingService.GetTerminatedEmployeesDetailFn(int clientId, DateTime startDate, DateTime endDate, IEnumerable<int> employeeIds, int userId, int userTypeId)
        {
            var opResult = new OpResult<IEnumerable<EmployeePayAndEmployeeStatusAndEmployeeWithReason>>();

            var employeePay = _session.UnitOfWork.PayrollRepository.QueryEmployeePay().ByClient(clientId);
            var employee = _session.UnitOfWork.EmployeeRepository.QueryEmployees();
            if (employeeIds != null) employee = employee.ByEmployeeIds(employeeIds);
			var allData = _session.UnitOfWork.EmployeeRepository.EmployeeStatusQuery().JoinEmployeePayEmployeeStatusAndEmployeeAndReason(employeePay, employee, clientId).Execute();
			allData = allData.Where(s => s.EmployeeStatus != null && s.EmployeeStatus != "Full Time Temp" && s.EmployeeStatus != "Part Time Temp");

            var resultList = new List<EmployeePayAndEmployeeStatusAndEmployeeWithReason>();

			startDate = startDate.Date;
			endDate = endDate.Date;

			List<int> list = new List<int>();
            foreach (var item in allData)
            {
                list.Add(item.EmployeeId);
            }
            var filterEmployees = _supervisorProvider.FilterEmployeesBySupervisor(list, userId, clientId);
            var supervisorEmployees = filterEmployees.Data.ToList();

			List<EmployeePayAndEmployeeStatusAndEmployeeWithReason> rehires = new List<EmployeePayAndEmployeeStatusAndEmployeeWithReason>();
			allData.Where(x => EmployeeHireMapper.IsEmployeeReHired(x)).ForEach(y => rehires.Add(EmployeeHireMapper.CopyEmployeePayAndEmployeeStatusAndEmployeeWithReason(y)));
			var employeeList2 = EmployeeHireMapper.SeparateRehires<EmployeePayAndEmployeeStatusAndEmployeeWithReason>(allData, rehires);

			foreach (var datum in employeeList2)
            {
                if (EmployeeHireMapper.IsTerminatedOnInterval(datum,startDate.Date,endDate.Date) && supervisorEmployees.Contains(datum.EmployeeId))
                {
					if (EmployeeHireMapper.IsNewHireOnInterval(datum, startDate.Date, endDate.Date))
					{
						// This employee is hired and terminated within this date range.
					}
					else
					{
						resultList.Add(datum);
					}
                }
            }

            resultList = resultList.OrderBy(s => s.FullName).ToList();

            opResult.Data = resultList;

            return opResult;
        }
		[Obsolete("GetGrowthRate has count information, which can be utilized for Retention rate calculations.")]
		IOpResult<TurnoverRateDto> IApplicantTrackingService.GetRetentionRate(int clientId, DateTime startDate, DateTime endDate, IEnumerable<int> employeeIds, int userId, int userTypeId)
        {
            var result = new OpResult<TurnoverRateDto>();

            //filter entries by clientid so that we avoid making 3 calls to the db (one call for each result set with unique constraints--see for each loop below).
            var employeePay = _session.UnitOfWork.PayrollRepository.QueryEmployeePay().ByClient(clientId);
            var employee = _session.UnitOfWork.EmployeeRepository.QueryEmployees();
            if (employeeIds != null) employee = employee.ByEmployeeIds(employeeIds);
			var allData = _session.UnitOfWork.EmployeeRepository.EmployeeStatusQuery().JoinEmployeePayEmployeeStatusAndEmployee(employeePay, employee, clientId).Execute();

            var bothCount = 0;
            var startCount = 0;
            var termedCount = 0;

            allData = allData.Where(s => s.EmployeeStatus != "Full Time Temp" || s.EmployeeStatus != "Part Time Temp");

            //Go through all data returned and extract: 
            //the amount of active employees hired before the start date
            //the amount of active employees hired before the end date
            //and the amount of deactivated employees who were 'separated' between the start date and the end date or who were not 'separated' but whose employeepay record was modified between the start date and end date
            foreach (var datum in allData)
            {
                if ((datum.HireDate <= startDate && datum.Active) && (datum.HireDate <= endDate && datum.Active) && (datum.SeparationDate == null && datum.Active))
                {
                    bothCount++;
                }

                if (datum.HireDate < startDate && datum.Active)
                {
                    startCount++;
                }

                if (((datum.SeparationDate >= startDate && datum.SeparationDate <= endDate) ||
                     (!(datum.SeparationDate >= endDate || datum.SeparationDate <= endDate) &&
                      datum.Modified >= startDate && datum.Modified <= endDate)) && !datum.Active)
                {
                    termedCount++;
                }
            }

            result.Data = new TurnoverRateDto()
            {
                EndCount = startCount,
                StartCount = bothCount,
                TermedCount = termedCount
            };

            return result;
        }

        IOpResult<IEnumerable<EmployeePayAndEmployeeStatusAndEmployeeWithReason>> IApplicantTrackingService.GetNewHiredEmployeesDetailFn(int clientId, DateTime startDate, DateTime endDate, IEnumerable<int> employeeIds, int userId, int userTypeId)
        {
            var opResult = new OpResult<IEnumerable<EmployeePayAndEmployeeStatusAndEmployeeWithReason>>();

            var employeePay = _session.UnitOfWork.PayrollRepository.QueryEmployeePay().ByClient(clientId);
            var employee = _session.UnitOfWork.EmployeeRepository.QueryEmployees();
            if (employeeIds != null) employee = employee.ByEmployeeIds(employeeIds);
			var allData = _session.UnitOfWork.EmployeeRepository.EmployeeStatusQuery().JoinEmployeePayEmployeeStatusAndEmployeeAndReason(employeePay, employee, clientId).Execute();
			allData = allData.Where(s => s.EmployeeStatus != null && s.EmployeeStatus != "Full Time Temp" && s.EmployeeStatus != "Part Time Temp");

            var resultList = new List<EmployeePayAndEmployeeStatusAndEmployeeWithReason>();

            startDate = startDate.Date;
			endDate = endDate.Date;

            List<int> list = new List<int>();
            foreach (var item in allData)
            {
                list.Add(item.EmployeeId);
            }
            var filterEmployees = _supervisorProvider.FilterEmployeesBySupervisor(list, userId, clientId);
            var supervisorEmployees = filterEmployees.Data.ToList();

			List<EmployeePayAndEmployeeStatusAndEmployeeWithReason> rehires = new List<EmployeePayAndEmployeeStatusAndEmployeeWithReason>();
			allData.Where(x => EmployeeHireMapper.IsEmployeeReHired(x)).ForEach(y => rehires.Add(EmployeeHireMapper.CopyEmployeePayAndEmployeeStatusAndEmployeeWithReason(y)));
			var employeeList2 = EmployeeHireMapper.SeparateRehires<EmployeePayAndEmployeeStatusAndEmployeeWithReason>(allData, rehires);

			foreach (var datum in employeeList2)
            {
                if (EmployeeHireMapper.IsNewHireOnInterval(datum, startDate.Date, endDate.Date) && supervisorEmployees.Contains(datum.EmployeeId))
                {
					if (EmployeeHireMapper.IsTerminatedOnInterval(datum, startDate.Date, endDate.Date))
					{
						// This employee is hired and terminated within this date range.
					}
					else
					{
						resultList.Add(datum);
					}
				}
            }

            resultList = resultList.OrderBy(s => s.FullName).ToList();

            opResult.Data = resultList;

            return opResult;
        }
    }
}