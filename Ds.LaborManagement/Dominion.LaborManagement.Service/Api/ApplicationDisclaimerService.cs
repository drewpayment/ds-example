using Dominion.Core.Dto.Core;
using Dominion.Core.Dto.Labor;
using Dominion.Core.Services.Interfaces;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.LaborManagement.Dto.ApplicantTracking;
using Dominion.LaborManagement.Service.Mapping;
using Dominion.Utility.OpResult;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Dominion.Authentication.Intermediate.Util;
using Dominion.Core.Dto.Client;
using Dominion.Core.Dto.Core;
using Dominion.Core.Dto.Location;
using Dominion.Core.Dto.User;
using Dominion.Core.Services.Api;
using Dominion.Core.Services.Emails;
using Dominion.Core.Services.Internal.Providers.Resources;
using Dominion.LaborManagement.Dto.ApplicantTracking.IndeedApply;
using Dominion.LaborManagement.Dto.Notification;
using Dominion.LaborManagement.Service.Api.Notification;
using Dominion.LaborManagement.Service.Internal;
using Dominion.LaborManagement.Service.Internal.Security;
using Dominion.Utility.Configs;
using Dominion.Utility.Constants;
using Dominion.Utility.Security;
using Dominion.Utility.ExtensionMethods;
using Dominion.Utility.Io;
using Dominion.Utility.Web;
using GenericMsg = Dominion.Utility.Msg.Specific.GenericMsg;
using Dominion.Domain.Entities.Core;

namespace Dominion.LaborManagement.Service.Api
{
    public class ApplicationDisclaimerService : IApplicationDisclaimerService
    {
        private readonly IBusinessApiSession _session;
        private readonly IApplicantTrackingService _applicantTrackingService;
        private readonly ISecurityManager _securityManager;
        private readonly IApplicantTrackingEmail _applicantTrackingEmail;
        private readonly IApplicationDisclaimerService _self;
        private readonly IApplicantTrackingNotificationService _applicantTrackingNotificationService;
        private readonly IAzureResourceProvider _azureResourceProvider;
        private readonly IClientAzureService _azureService;
        private readonly IClientService _clientService;

        public ApplicationDisclaimerService(
            IBusinessApiSession session,
            IApplicantTrackingService applicantTrackingService,
            ISecurityManager securityManager,
            IApplicantTrackingEmail applicantTrackingEmail,
            IApplicantTrackingNotificationService applicantTrackingNotificationService,
            IAzureResourceProvider azureResourceProvider,
            IClientAzureService clientAzureService,
            IClientService clientService
        )
        {
            _session = session;
            _applicantTrackingService = applicantTrackingService;
            _securityManager = securityManager;
            _applicantTrackingEmail = applicantTrackingEmail;
            _self = this;
            _applicantTrackingNotificationService = applicantTrackingNotificationService;
            _azureResourceProvider = azureResourceProvider;
            _azureService = clientAzureService;
            _clientService = clientService;
        }

        IOpResult<ResumeRequiredType?> IApplicationDisclaimerService.GetPostingResumeOption(int headId)
        {
            var result = new OpResult<ResumeRequiredType?>
            {
                Data = (ResumeRequiredType?)_session.UnitOfWork.ApplicantTrackingRepository
                    .ApplicantPostingsQuery()
                    .ByApplicantApplicationHeaderId(headId)
                    .Result
                    .FirstOrDefault()
                    .ApplicantResumeRequired?
                    .ApplicantResumeRequiredId
            };

            if (result.Data == null)
            {
                result.SetToFail();
            }
            else
            {
                result.SetToSuccess();
            }

            return result;
        }
        IOpResult<DisclaimerDetailDto> IApplicationDisclaimerService.GetDisclaimerTemplate(int clientId)
        {
            var result = new OpResult<DisclaimerDetailDto>();

            // Company Name , if dto was null the result should of already failed and return so this should be safe for clientId
            var company = _session.UnitOfWork.ClientRepository.QueryClients().ByClientId(clientId)
                .ExecuteQueryAs(x => new
                { CompanyName = x.ApplicantClient.JobBoardTitle == null ? x.ClientName : x.ApplicantClient.JobBoardTitle,
                  CompanyAddress = x.AddressLine2 != "" ? x.AddressLine1 + "<br/>" + x.AddressLine2 + "<br/>" + x.City + ", " + x.State.Name + " " + x.PostalCode :
                x.AddressLine1 + "<br/>" + x.City + ", " + x.State.Name + " " + x.PostalCode
                })
                .FirstOrDefault();

            var corr = _session.UnitOfWork.ApplicantTrackingRepository
                .ApplicantCompanyCorrespondenceQuery()
                .ByClientId(Convert.ToInt32(clientId))
                .ByIsActive(true)
                .ByCorrespondenceTypeId(ApplicantCorrespondenceType.ApplicationDisclaimer)
                .ExecuteQueryAs(c => new ApplicantCompanyCorrespondenceDto
                {
                    ApplicantCompanyCorrespondenceId = c.ApplicantCompanyCorrespondenceId,
                    ClientId = c.ClientId,
                    Description = c.Description,
                    ApplicantCorrespondenceTypeId = c.ApplicantCorrespondenceTypeId,
                    Body = c.Body,
                    IsActive = c.IsActive
                })
                .FirstOrDefault();

            ApplicantTrackingCorrespondenceReplacementInfoDto data = new ApplicantTrackingCorrespondenceReplacementInfoDto();
            data.ApplicantFirstName = "Applicant's First Name";
            data.ApplicantLastName = "Applicant's Last Name";
            data.UserName = "Applicant's User Name";
            data.Password = "Applicant's Password";
            data.Posting = "Posting Name";
            data.Date = DateTime.Now;
            data.Address = "Applicant's Address";
            data.Phone = "Applicant's Phone";
            data.CompanyAddress = company.CompanyAddress;
            data.CompanyName = company.CompanyName;
            data.TemplateBody = corr?.Body;

            // Actual content from mail template
            string body = _applicantTrackingService.GetEmailBody(data);

            result.TrySetData(() => new DisclaimerDetailDto
            {
                ApplicantId = 0,
                ApplicantApplicationHeaderId = 0,
                ApplicantPostingId = 0,
                ClientId = clientId,
                ClientName = company.CompanyName,
                Body = body,
            });

            return result;
        }

        IOpResult<ApplicantPostingDto> IApplicationDisclaimerService.GetPostingResumeAndCoverLetterOption(int headId)
        {
            var result = new OpResult<ApplicantPostingDto>()
                          .TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository
                              .ApplicantPostingsQuery()
                              .ByApplicantApplicationHeaderId(headId)
                              .FirstOrDefaultAs(x => new ApplicantPostingDto
                              {
                                  ApplicantResumeRequiredId = x.ApplicantResumeRequiredId,
                                  IsCoverLetterRequired = x.IsCoverLetterRequired
                              })
                          );

            return result.CheckForData();
        }

        IOpResult<DisclaimerDetailDto> IApplicationDisclaimerService.GetDisclaimer(int headId, CorrespondenceType correspondenceType)
        {
            var result = new OpResult<DisclaimerDetailDto>();

            var dto = _session.UnitOfWork.ApplicantTrackingRepository
                .ApplicantApplicationHeaderQuery()
                .ByApplicationHeaderId(headId)
                .ExecuteQueryAs(new ApplicantApplicationHeaderMaps.ToApplicantApplicationHeaderDto())
                .FirstOrDefault();

            if (dto == null) result.SetToFail();
            if (result.HasError) return result;

            var clientId = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery().ByPostingId(dto.PostingId).ExecuteQueryAs(x => x.ClientId).FirstOrDefault();

            var disclaimerQry = _session.UnitOfWork.ApplicantTrackingRepository
                .ApplicantCompanyCorrespondenceQuery()
                .ByClientId(clientId)
                .ByCorrespondenceTypeId(ApplicantCorrespondenceType.ApplicationDisclaimer);

            if (dto.IsApplicationCompleted)
            {
                if (dto.DisclaimerId.HasValue && dto.DisclaimerId.Value != 0)
                    disclaimerQry = disclaimerQry.ByCorrespondenceId(dto.DisclaimerId.Value);
                else
                    disclaimerQry = disclaimerQry.ByCorrespondenceId(-1);// Since the user has accepted default disclaimer
            }
            else
                disclaimerQry = disclaimerQry.ByIsActive(true);

            var corr = disclaimerQry.ExecuteQueryAs(c => new ApplicantCompanyCorrespondenceDto
                {
                    ApplicantCompanyCorrespondenceId = c.ApplicantCompanyCorrespondenceId,
                    ClientId = c.ClientId,
                    Description = c.Description,
                    ApplicantCorrespondenceTypeId = c.ApplicantCorrespondenceTypeId,
                    Body = c.Body,
                    IsActive = c.IsActive
                })
                .FirstOrDefault();

            ApplicantTrackingCorrespondenceReplacementInfoDto data = new ApplicantTrackingCorrespondenceReplacementInfoDto();
            data.ApplicantFirstName = dto?.Applicant.FirstName;
            data.ApplicantLastName = dto?.Applicant.LastName;
            data.UserName = dto?.Applicant.UserName != null ? dto?.Applicant.UserName : "";
            data.Posting = dto?.ApplicantPosting.Description;
            data.Date = DateTime.Now;
            data.Address = dto?.Applicant.AddressLine2 != "" ? dto?.Applicant.Address + "<br/>" + dto?.Applicant.AddressLine2 + "<br/>" + dto?.Applicant.City + ", " + dto?.Applicant.StateDetails?.Name + " " + dto?.Applicant.Zip :
                dto?.Applicant.Address + "<br/>" + dto?.Applicant.City + ", " + dto?.Applicant.StateDetails?.Name + " " + dto?.Applicant.Zip;
            data.Phone = dto?.Applicant.PhoneNumber;
            data.CompanyAddress = dto?.Applicant.Client.AddressLine2 != "" ? dto?.Applicant.Client.AddressLine1 + "<br/>" + dto?.Applicant.Client.AddressLine2 + "<br/>" + dto?.Applicant.Client.City + ", " + dto?.Applicant.Client.State.Name + " " + dto?.Applicant.Client.PostalCode :
                dto?.Applicant.Client.AddressLine1 + "<br/>" + dto?.Applicant.Client.City + ", " + dto?.Applicant.Client.State.Name + " " + dto?.Applicant.Client.PostalCode;

            // Company Name , if dto was null the result should of already failed and return so this should be safe for clientId
            data.CompanyName = _session.UnitOfWork.ClientRepository.QueryClients().ByClientId(corr?.ClientId ?? Convert.ToInt32(dto?.Applicant.ClientId)).ExecuteQueryAs(x =>
            (x.ApplicantClient.JobBoardTitle == null ? x.ClientName : x.ApplicantClient.JobBoardTitle)).FirstOrDefault();
            data.TemplateBody = corr?.Body;

            // Actual content from mail template
            string body = _applicantTrackingService.GetEmailBody(data);

            result.TrySetData(() => new DisclaimerDetailDto
            {
                ApplicantId = dto?.ApplicantId ?? 0,
                ApplicantApplicationHeaderId = dto?.ApplicationHeaderId ?? 0,
                ApplicantPostingId = dto?.ApplicantPosting.PostingId ?? 0,
                ClientId = dto?.Applicant.ClientId ?? 0,
                ClientName = dto?.Applicant.Client.ClientName,
                Body = body,
                CoverLetter = dto?.CoverLetter,
                CoverLetterId = dto?.CoverLetterId,
                FirstName = dto?.Applicant.FirstName,
                LastName = dto?.Applicant.LastName,
                IsApplicationSubmitted = dto?.IsApplicationCompleted ?? false,
                ApplicationSubmittedOn = dto?.DateSubmitted,
                Posting = dto?.ApplicantPosting.Description,
                Date = new DateTime(),
                ApplicantAddress1 = dto?.Applicant.Address,
                ApplicantAddress2 = dto?.Applicant.AddressLine2,
                ApplicantCity = dto?.Applicant.City,
                ApplicantState = dto?.Applicant.StateDetails,
                ApplicantPostalCode = dto?.Applicant.Zip,
                ApplicantPhoneNumber = dto?.Applicant.PhoneNumber,
                CompanyAddress1 = dto?.Applicant.Client.AddressLine1,
                CompanyAddress2 = dto?.Applicant.Client.AddressLine2,
                CompanyCity = dto?.Applicant.Client.City,
                CompanyState = dto?.Applicant.Client.State,
                CompanyPostalCode = dto?.Applicant.Client.PostalCode,
            });

            return result;
        }

        IOpResult<ApplicantResumeDto> IApplicationDisclaimerService.GetApplicantResume(int applicantId)
        {
            return new OpResult<ApplicantResumeDto>(_session.UnitOfWork.ApplicantTrackingRepository
                .ApplicantResumeQuery()
                .ByApplicantId(applicantId)
                .ExecuteQueryAs(x => new ApplicantResumeDto
                {
                    ApplicantId = x.ApplicantId,
                    ApplicantResumeId = x.ApplicantResumeId,
                    DateAdded = x.DateAdded,
                    LinkLocation = x.LinkLocation
                })
                .FirstOrDefault());
        }

        IOpResult<ApplicantResumeDto> IApplicationDisclaimerService.SaveApplicantResume(ApplicantResumeDto dto)
        {
            var result = new OpResult<ApplicantResumeDto>();

            // check action types

            var entity = new ApplicantResume
            {
                ApplicantResumeId = dto.ApplicantResumeId,
                ApplicantId = dto.ApplicantId,
                LinkLocation = dto.LinkLocation,
                DateAdded = dto.DateAdded
            };

            _session.UnitOfWork.RegisterNewOrModified(entity, e => e.ApplicantResumeId == 0);
            _session.UnitOfWork.Commit().MergeInto(result);

            if (result.HasError) return result;

            result.TrySetData(() => new ApplicantResumeDto
            {
                ApplicantResumeId = entity.ApplicantResumeId,
                ApplicantId = entity.ApplicantId,
                LinkLocation = entity.LinkLocation,
                DateAdded = entity.DateAdded
            });

            return result;
        }

        IOpResult<ApplicantApplicationHeaderDto> IApplicationDisclaimerService.AttachResume(int applicationHeaderId,
            int applicantResumeId)
        {
            var result = new OpResult<ApplicantApplicationHeaderDto>();

            var entity = _session.UnitOfWork.ApplicantTrackingRepository
                .ApplicantApplicationHeaderQuery()
                .ByApplicationHeaderId(applicationHeaderId)
                .Result
                .FirstOrDefault();

            entity.ApplicantResumeId = applicantResumeId;

            _session.UnitOfWork.RegisterModified(entity);
            _session.UnitOfWork.Commit().MergeInto(result);

            if (result.HasError) return result;

            result.Data = new ApplicantApplicationHeaderDto
            {
                ApplicationHeaderId = entity.ApplicationHeaderId,
                ApplicantResumeId = entity.ApplicantResumeId,
                ApplicantId = entity.ApplicantId,
                PostingId = entity.PostingId,
                IsApplicationCompleted = entity.IsApplicationCompleted,
                IsRecommendInterview = entity.IsRecommendInterview,
                DateSubmitted = entity.DateSubmitted,
                ApplicantRejectionReasonId = entity.ApplicantRejectionReasonId,
                OrigPostingId = entity.OrigPostingId,
                ApplicantStatusTypeId = entity.ApplicantStatusTypeId,
                AddedByAdmin = entity.AddedByAdmin,
            };

            return result;
        }


        IOpResult<FileStreamDto> IApplicationDisclaimerService.GetResumePreview(int applicantResumeId)
        {
            var result = new OpResult<FileStreamDto>();
            ApplicantResumeDto dto = (_session.UnitOfWork.ApplicantTrackingRepository
                .ApplicantResumeQuery()
                .ByApplicantResumeId(applicantResumeId)
                .ExecuteQueryAs(r => new ApplicantResumeDto
                {
                    ApplicantResumeId = r.ApplicantResumeId,
                    ApplicantId = r.ApplicantId,
                    DateAdded = r.DateAdded,
                    LinkLocation = r.LinkLocation
                })
                .FirstOrDefault());
            if (dto == null)
            {
                return result.SetToFail(() => new GenericMsg("Unable to find specified resume."));
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
                    return result.SetToFail(() => new GenericMsg("Unable to find specified resume."));
                }
            }
            else
            {
                _azureResourceProvider.GetBlob(dto.LinkLocation, ConfigValues.AzureFile).MergeAll(result);
            }

            return result;
        }

        IOpResult<ApplicantApplicationHeaderDto> IApplicationDisclaimerService.CompleteApplication(
            int applicantApplicationHeaderId, int applicantResumeId, bool addedByAdmin)
        {
            var result = new OpResult<ApplicantApplicationHeaderDto>();

            var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
            var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
            var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;

            if (!isApplicantTrackingEnabled && !isSystemAdmin) result.SetToFail();

            if (result.HasError) return result;

            int? currentDisclaimerId = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantCompanyCorrespondenceQuery()
                .ByCorrespondenceTypeId(ApplicantCorrespondenceType.ApplicationDisclaimer)
                .ByClientId(_session.LoggedInUserInformation.ClientId.GetValueOrDefault())
                .ByIsActive(true)
                .ExecuteQueryAs(x => x.ApplicantCompanyCorrespondenceId).FirstOrDefault();

            currentDisclaimerId = currentDisclaimerId == 0 ? default(int?) : currentDisclaimerId;

            var entity = _session.UnitOfWork.ApplicantTrackingRepository
                .ApplicantApplicationHeaderQuery()
                .ByApplicationHeaderId(applicantApplicationHeaderId)
                .Result
                .FirstOrDefault();

            entity.IsApplicationCompleted = true;
            entity.ApplicantStatusTypeId = 0;
            entity.ApplicantResumeId = applicantResumeId;
            entity.AddedByAdmin = addedByAdmin;
            entity.AddedBy = _session.LoggedInUserInformation.UserId;
            entity.DisclaimerId = currentDisclaimerId;

            _session.UnitOfWork.RegisterModified(entity);
            _session.UnitOfWork.Commit().MergeInto(result);

            result.TrySetData(_session.UnitOfWork.ApplicantTrackingRepository
                .ApplicantApplicationHeaderQuery()
                .ByApplicationHeaderId(applicantApplicationHeaderId)
                .ExecuteQueryAs(new ApplicantApplicationHeaderMaps.ToApplicantApplicationHeaderDto())
                .FirstOrDefault);

            return result;
        }

        IOpResult<ApplicantApplicationDetailDto> IApplicationDisclaimerService.SaveApplicantApplicationDetail(
            ApplicantApplicationDetailDto dto)
        {
            var result = new OpResult<ApplicantApplicationDetailDto>();

            var entity = new ApplicantApplicationDetail
            {
                ApplicationDetailId = dto.ApplicationDetailId,
                ApplicationHeaderId = dto.ApplicationHeaderId,
                IsFlagged = dto.IsFlagged,
                QuestionId = dto.QuestionId,
                Response = dto.Response,
                SectionId = dto.SectionId
            };

            _session.UnitOfWork.RegisterNewOrModified(entity, e => e.ApplicationDetailId == 0);
            _session.UnitOfWork.Commit().MergeInto(result);

            if (result.HasError) return result;

            result.TrySetData(() => new ApplicantApplicationDetailDto
            {
                ApplicationDetailId = entity.ApplicationDetailId,
                ApplicationHeaderId = entity.ApplicationHeaderId,
                IsFlagged = entity.IsFlagged,
                QuestionId = entity.QuestionId,
                Response = entity.Response,
                SectionId = entity.SectionId
            });

            return result;
        }

        IOpResult IApplicationDisclaimerService.SendPostingOwnerNotification(ApplicantPostingDetailDto postDetail,
            ApplicantDetailDto applicantDetail, Uri uri)
        {
            var emailResult =
                _applicantTrackingNotificationService.ProcessPostingOwnerNotification(postDetail, applicantDetail, uri);

            return emailResult;
        }

        IOpResult IApplicationDisclaimerService.CheckResumeRequired(ResumeRequiredType applicantResumeRequiredType,
            int applicationHeaderId)
        {
            var result = new OpResult();
            var existing = _session.UnitOfWork.ApplicantTrackingRepository
                .ApplicantApplicationDetailQuery()
                .ByApplicationHeaderId(applicationHeaderId)
                .Result
                .Any();

            if (!existing && applicantResumeRequiredType == ResumeRequiredType.SubmitResumeOnly)
                _self.SaveApplicantApplicationDetail(new ApplicantApplicationDetailDto
                {
                    ApplicationHeaderId = applicationHeaderId,
                    QuestionId = 0,
                    IsFlagged = false,
                    Response = "",
                    SectionId = 0
                })
                .MergeInto(result);

            return result;
        }

        IOpResult<string> IApplicationDisclaimerService.UpdateJobSiteTrackers(int clientId)
        {
            var result = new OpResult<string>();

            var trackers = _applicantTrackingService.GetClientJobSitesByID(clientId).MergeInto(result).Data;
            var jobSites = _applicantTrackingService.GetApplicantJobSites().Data;
            if (trackers != null && trackers.Any())
            {
                foreach (var t in trackers)
                {
                    var jobSite = jobSites.FirstOrDefault(j => j.ApplicantJobSiteId == t.ApplicantJobSiteId);
                    var code = t.ApplicantJobSiteId;
                    var url = jobSite?.JobSiteBaseUrl ?? "";
                    if (url.Length == 0) continue;
                    result.TrySetData(() => url.Replace("*", code.ToString())).MergeInto(result);

                    t.Counter = t.Counter + 1;
                    _applicantTrackingService.UpdateClientJobSite(t).MergeInto(result);
                }
            }
            else
            {
                result.SetToSuccess();
            }

            return result;
        }

        IOpResult<ApplicantDetailDto> IApplicationDisclaimerService.GetApplicantDetail(int headerId)
        {
            var result = new OpResult<ApplicantDetailDto>();

            var clientId = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantApplicationHeaderQuery()
                    .ByApplicationHeaderId(headerId)
                    .ExecuteQueryAs(x => x.Applicant.ClientId)
                    .FirstOrDefault();

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(result);

            result.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantApplicationHeaderQuery()
                    .ByApplicationHeaderId(headerId)
                    .ExecuteQueryAs(new ApplicantApplicationHeaderMaps.ToApplicantDetailDto())
                    .FirstOrDefault()).MergeInto(result);

            return result;
        }

        IOpResult<ApplicantDetailDto> IApplicationDisclaimerService.SendPostApplicantNotification(ApplicantApplicationHeaderDto header)
        {
            var result = new OpResult<ApplicantDetailDto>();

            var postingResult = _applicantTrackingService.GetApplicantPosting(header.PostingId).MergeInto(result);

            if (!postingResult.HasData)
            {
                result.SetToFail();
                return result;
            }
            

            var applicantDetail = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantApplicationHeaderQuery()
                    .ByApplicationHeaderId(header.ApplicationHeaderId)
                    .ExecuteQueryAs(new ApplicantApplicationHeaderMaps.ToApplicantDetailDto())
                    .FirstOrDefault();

            var hasResourceAccess = _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, postingResult.Data.ClientId).Success;
            var canWrite = _session.CanPerformAction(ApplicantTrackingActionType.WriteApplicantInfo).Success;

            if (!canWrite && !hasResourceAccess)
            {
                result.AddMessage(new GenericMsg("No sufficient permissions for notification.")).SetToFail();
                return result;
            }

            var correspondence = _session.UnitOfWork.ApplicantTrackingRepository
                .ApplicantCompanyCorrespondenceQuery()
                .ByCorrespondenceId(postingResult.Data.ApplicationCompletedCorrespondence)
                .ExecuteQueryAs(x => new
                {
                    Body = x.Body,
                    Subject = x.Subject,
                    ClientId = x.ClientId,
                    Type = x.ApplicantCorrespondenceTypeId
                })
                .FirstOrDefault();

            if (correspondence?.Body != null)
            {
                // Company logo
                AzureViewDto imgSource = _azureService.GetAzureClientResource(ResourceSourceType.AzureClientImage, postingResult.Data.ClientId, "logo").Data;

                var client = _session.UnitOfWork.ClientRepository
                    .QueryClients()
                    .ByClientId(postingResult.Data.ClientId)
                    .ExecuteQueryAs(x => new ClientDto
                    {
                        AddressLine1 = x.AddressLine1,
                        AddressLine2 = x.AddressLine2,
                        City = x.City,
                        State = new StateDto
                        {
                            Name = x.State.Name
                        },
                        PostalCode = x.PostalCode,
                        ClientName = x.ApplicantClient.JobBoardTitle == null ? x.ClientName : x.ApplicantClient.JobBoardTitle
                    })
                    .FirstOrDefault();

                var replacementInfo = new ApplicantTrackingCorrespondenceReplacementInfoDto
                {
                    ApplicantClientId = postingResult.Data.ClientId,
                    ApplicantFirstName = header.Applicant.FirstName,
                    ApplicantLastName = header.Applicant.LastName,
                    ApplicantEmail = header.Applicant.EmailAddress,
                    UserName = header.Applicant.UserName != null ? header.Applicant.UserName : "",
                    Posting = postingResult.Data.Description,
                    Date = DateTime.Now,
                    Address = header.Applicant.AddressLine2 != "" ? header.Applicant.Address + "<br/>" + header.Applicant.AddressLine2 + "<br/>" + header.Applicant.City + ", " + header.Applicant.StateDetails.Name + " " + header.Applicant.Zip : header.Applicant.Address + "<br/>" + header.Applicant.City + ", " + header.Applicant.StateDetails.Name + " " + header.Applicant.Zip,
                    Phone = header.Applicant.PhoneNumber,
                    CompanyAddress = client?.AddressLine2 != "" ? client?.AddressLine1 + "<br/>" + client?.AddressLine2 + "<br/>" + client?.City + ", " + client?.State.Name + " " + client?.PostalCode : client?.AddressLine1 + "<br/>" + client?.City + ", " + client?.State.Name + " " + client?.PostalCode,
                    CompanyName = client?.ClientName,
                    CompanyLogo = imgSource?.Source
                };

                string textCorrespondence = null;
                if (postingResult.Data.ApplicationReceivedTextCorrespondence != null)
                {
                    textCorrespondence = _session.UnitOfWork.ApplicantTrackingRepository
                    .ApplicantCompanyCorrespondenceQuery()
                    .ByCorrespondenceId(postingResult.Data.ApplicationReceivedTextCorrespondence.Value)
                    .ExecuteQueryAs(x =>x.Body)
                    .FirstOrDefault();

                }

                var emailResult = _applicantTrackingNotificationService.ProcessSubmittedApplicationNotification(
                    applicantDetail, postingResult.Data.ApplicationCompletedCorrespondence, correspondence.Subject, correspondence.Body,
                    replacementInfo, textCorrespondence);

                if (emailResult.HasError)
                {
                    result
                        .AddMessage(new GenericMsg("Email notification not sent to: " + header.Applicant.EmailAddress + " Reason: " + emailResult.MsgObjects[0].Msg))
                        .SetToFail();
                    return result;
                }

                var applicationEmail = new ApplicantApplicationEmailHistory();
                applicationEmail.ApplicationHeaderId = header.ApplicationHeaderId;
                applicationEmail.ApplicantCompanyCorrespondenceId = postingResult.Data.ApplicationCompletedCorrespondence;
                applicationEmail.ApplicantStatusTypeId = header.ApplicantStatusTypeId;
                applicationEmail.SenderId = (postingResult.Data.OwnerNotifications && postingResult.Data.PostingOwners?.Count > 0) ? postingResult.Data.PostingOwners.FirstOrDefault().UserId : default(int?);
                applicationEmail.SentDate = DateTime.Now;
                applicationEmail.Subject = correspondence.Subject;
                applicationEmail.Body = correspondence.Body;
                if (applicationEmail.SenderId.HasValue)
                {
                    //applicationEmail.SenderEmail = _session.UnitOfWork.UserRepository.QueryUsers()
                    //.ByUserId(applicationEmail.SenderId.Value).ExecuteQueryAs(x => x.EmailAddress).FirstOrDefault();
                }

                _session.UnitOfWork.RegisterNew(applicationEmail);
                _session.UnitOfWork.Commit().MergeInto(result);

                // Update CorrespondenceType
                applicantDetail.ApplicantCorrespondenceTypeId = correspondence.Type;
            }

            if (result.HasError) return result;

            result.TrySetData(() => applicantDetail).MergeInto(result);

            return result;
        }

        public async Task<OpResult<ApplicantResumeDto>> SaveResume(int applicantId, int? applicantResumeId, HttpRequestMessage request)
        {
            var result = new OpResult<ApplicantResumeDto>();
            var existingResume = _self.GetApplicantResume(applicantId).MergeInto(result).Data;
            var clientid = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantsQuery().ByApplicantId(applicantId)
                .FirstOrDefault().ClientId;


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
                        
                        if (existingResume!=null && existingResume.ApplicantResumeId!=CommonConstants.NEW_ENTITY_ID && existingResume.LinkLocation.Contains(@"C:\"))
                        {
                            DsIo.DeleteLocalFile(existingResume.LinkLocation);
                        }

                        if (!string.IsNullOrEmpty(ConfigValues.AzureFile))
                            SaveResumeToAzure(clientid, applicantId, applicantResumeId, filename, fileData).MergeAll(result);
                        else
                            WriteResumeToApplicantResumes(applicantId, filename, result, string.Format( @"C:\Upload\{0}\ApplicantResumes", clientid) , 
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

        public OpResult<ApplicantResumeDto> SaveResume(int applicantId, IndeedFilePropertyBag file, int clientId, int headerId)
        {
            var result = new OpResult<ApplicantResumeDto>();
            try
            {
                var header = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantApplicationHeaderQuery()
                    .ByApplicationHeaderId(headerId).ExecuteQuery().FirstOrDefault();
                var existingResume = _self.GetApplicantResume(applicantId).MergeInto(result).Data;

                var resume = SaveResumeToAzure(clientId,applicantId,(existingResume?.ApplicantResumeId??0),file.FileName, Convert.FromBase64String(file.Data));

                header.ApplicantResumeId = resume.Data.ApplicantResumeId;

                _session.UnitOfWork.RegisterModified(header);
                _session.UnitOfWork.Commit().MergeInto(result);
            }
            catch (Exception e)
            {
                result.AddExceptionMessage(e);
            }

            return result;
        }

        public OpResult<ApplicantResumeDto> SaveResumeToAzure(int clientId, int applicantId, int? applicantResumeId,string fileName, byte[] fileData)
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

        private IOpResult<ApplicantResumeDto> WriteResumeToApplicantResumes(int applicantId, string providedFileName, OpResult<ApplicantResumeDto> result, string baseFolder, ApplicantResumeDto existingResume, string fileContent = null)
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


            // update existing resume
            var dto = new ApplicantResumeDto()
            {
                ApplicantResumeId = existingResume?.ApplicantResumeId ?? 0,
                ApplicantId = existingResume?.ApplicantId ?? applicantId,
                DateAdded = DateTime.Now,
                LinkLocation = file
            };

            return _self.SaveApplicantResume(dto).MergeAll(result);
        }

        IOpResult IApplicationDisclaimerService.UpdateCoverLetter(DisclaimerDetailDto dto)
        {
            IOpResult opResult = new OpResult();
            var appHeader = _session.UnitOfWork.ApplicantTrackingRepository
                .ApplicantApplicationHeaderQuery()
                .ByApplicationHeaderId(dto.ApplicantApplicationHeaderId)
                .FirstOrDefault();
            if(appHeader!=null)
            {
                appHeader.CoverLetter = dto.CoverLetter;
                appHeader.CoverLetterId = null;
                _session.UnitOfWork.RegisterModified(appHeader);
                _session.UnitOfWork.Commit().MergeInto(opResult);
            }
            return opResult;
        }


        public async Task<OpResult> SaveCoverLetter(int appHeaderid, int? coverLetterId, HttpRequestMessage request)
        {
            var result = new OpResult();
            var appHeader = _session.UnitOfWork.ApplicantTrackingRepository
                .ApplicantApplicationHeaderQuery()
                .ByApplicationHeaderId(appHeaderid)
                .FirstOrDefault();

            var clientId = appHeader.Applicant.ClientId;

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

                        SaveCoverLetterToAzure(clientId, appHeader.ApplicantId, coverLetterId, filename, fileData, appHeader);
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

        public OpResult SaveCoverLetterToAzure(int clientId, int applicantId, int? coverLetterId, string fileName, byte[] fileData, ApplicantApplicationHeader applicationHeader)
        {
            var result = new OpResult();
            Resource coverLetterResource;

            if (!string.IsNullOrEmpty(ConfigValues.AzureFile))
                coverLetterResource = _azureResourceProvider.CreateAzureResource(clientId, null, coverLetterId, null, fileName, fileData, ConfigValues.AzureFile, AzureDirectoryType.ApplicantFile, ResourceSourceType.LocalServerFile, true).MergeInto(result).Data;
            else
            {
                string coverLetterFileDir = string.Format(@"C:\Upload\{0}\CoverLetters\{1}\", clientId, applicantId );
                Directory.CreateDirectory(coverLetterFileDir);
                File.WriteAllBytes(coverLetterFileDir + fileName, fileData);

                coverLetterResource = new Resource
                {
                    ClientId = clientId,        EmployeeId = null,
                    AddedDate = DateTime.Now,   AddedById = _session.LoggedInUserInformation.UserId,
                    AzureAccount = null,        Name = fileName.Trim(),
                    SourceType = ResourceSourceType.LocalServerFile,
                    Source = coverLetterFileDir + fileName,
                };
                _session.SetModifiedProperties(coverLetterResource, coverLetterResource.AddedDate);
                _session.UnitOfWork.RegisterNew(coverLetterResource);
                _session.UnitOfWork.Commit().MergeInto(result);
            }

            if (result.Success)
            {
                applicationHeader.CoverLetter = null;
                applicationHeader.CoverLetterId = coverLetterResource.ResourceId;
                _session.UnitOfWork.RegisterModified(applicationHeader);
                _session.UnitOfWork.Commit().MergeInto(result);

            }

            return result;
        }

        IOpResult<FileStreamDto> IApplicationDisclaimerService.GetCoverLetterPreview(int coverLetterId)
        {
            var result = new OpResult<FileStreamDto>();
            var dto = (_session.UnitOfWork.CoreRepository
                .QueryResources()
                .ByResource(coverLetterId)
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
                return result.SetToFail(() => new GenericMsg("Unable to find the cover letter."));
            }
            else if (dto.Source.Contains(@"C:\"))
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
                    return result.SetToFail(() => new GenericMsg("Unable to find the cover letter."));
                }
            }
            else
            {
                _azureResourceProvider.GetBlob(dto.Source, ConfigValues.AzureFile).MergeAll(result);
            }

            return result;
        }
    }
}
