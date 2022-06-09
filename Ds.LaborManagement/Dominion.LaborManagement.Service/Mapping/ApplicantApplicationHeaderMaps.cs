using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Misc;
using Dominion.LaborManagement.Dto.ApplicantTracking;
using Dominion.Utility.Mapping;
using System;
using System.Linq.Expressions;
using System.Linq;
using Dominion.Core.Dto.Client;
using Dominion.Core.Dto.Location;
using System.Linq;
using Dominion.Core.Dto.Labor;
using Dominion.Domain.Entities.Employee;
using Dominion.LaborManagement.Dto.EmployeeLaborManagement;

namespace Dominion.LaborManagement.Service.Mapping
{
    public class ApplicantApplicationHeaderMaps
    {
        public class
            ToApplicantApplicationHeaderDto : ExpressionMapper<ApplicantApplicationHeader, ApplicantApplicationHeaderDto
            >
        {
            public override Expression<Func<ApplicantApplicationHeader, ApplicantApplicationHeaderDto>> MapExpression
            {
                get
                {
                    return x => new ApplicantApplicationHeaderDto
                    {
                        ApplicationHeaderId = x.ApplicationHeaderId,
                        ApplicantId = x.ApplicantId,
                        PostingId = x.PostingId,
                        IsApplicationCompleted = x.IsApplicationCompleted,
                        IsRecommendInterview = x.IsRecommendInterview,
                        DateSubmitted = x.DateSubmitted,
                        ApplicantResumeId = x.ApplicantResumeId,
                        ApplicantRejectionReasonId = x.ApplicantRejectionReasonId,
                        OrigPostingId = x.OrigPostingId,
                        ApplicantStatusTypeId = x.ApplicantStatusTypeId,
                        CoverLetter=x.CoverLetter,
                        CoverLetterId=x.CoverLetterId,
                        IsExternalApplicant = x.ExternalApplicationIdentity != null ? true : false,
                        JobSiteName = (x.ExternalApplicationIdentity != null) ? x.ExternalApplicationIdentity.ApplicantJobSite.JobSiteDescription : "",
                        AddedByAdmin = x.AddedByAdmin,
                        AddedByUserName = (x.AddedByUser != null) ? (x.AddedByUser.FirstName + " " + x.AddedByUser.LastName) : "",
                        Score = x.Score.HasValue ? x.Score.Value : 0,
                        DisclaimerId = x.DisclaimerId,
                        ApplicantPosting = x.ApplicantPosting != null ? new ApplicantPostingDto
                        {
                            PostingId = x.ApplicantPosting.PostingId,
                            Description = x.ApplicantPosting.Description,
                            PostingTypeId = x.ApplicantPosting.PostingTypeId,
                            PostingNumber = x.ApplicantPosting.PostingNumber,
                            ApplicationId = x.ApplicantPosting.ApplicationId,
                            ClientDivisionId = x.ApplicantPosting.ClientDivisionId,
                            ClientDepartmentId = x.ApplicantPosting.ClientDepartmentId,
                            PostingCategoryId = x.ApplicantPosting.PostingCategoryId,
                            JobTypeId = (int)x.ApplicantPosting.JobTypeId,
                            JobRequirements = x.ApplicantPosting.JobRequirements,
                            Location = (x.ApplicantPosting.ClientDivision != null) ?
                                        (x.ApplicantPosting.ClientDivision.City ?? "") + ", " +
                                        (x.ApplicantPosting.ClientDivision.State != null ? x.ApplicantPosting.ClientDivision.State.Abbreviation ?? "" : "") : "",
                            Salary = x.ApplicantPosting.Salary,
                            HoursPerWeek = x.ApplicantPosting.HoursPerWeek,
                            StartDate = x.ApplicantPosting.StartDate,
                            FilledDate = x.ApplicantPosting.FilledDate,
                            IsEnabled = x.ApplicantPosting.IsEnabled,
                            ClientId = x.ApplicantPosting.ClientId,
                            PublishedDate = x.ApplicantPosting.PublishedDate,
                            JobProfileId = x.ApplicantPosting.JobProfileId,
                            PublishStart = x.ApplicantPosting.PublishStart,
                            PublishEnd = x.ApplicantPosting.PublishEnd,
                            StaffHired = x.ApplicantPosting.StaffHired,
                            RejectionCorrespondence = x.ApplicantPosting.RejectionCorrespondence,
                            ApplicationCompletedCorrespondence = x.ApplicantPosting.ApplicationCompletedCorrespondence,
                            ApplicantResumeRequiredId = x.ApplicantPosting.ApplicantResumeRequiredId,
                            MinSchooling = x.ApplicantPosting.MinSchooling,
                            ApplicantOnBoardingProcessId = x.ApplicantPosting.ApplicantOnBoardingProcessId,
                            IsPublished = x.ApplicantPosting.IsPublished,
                            ModifiedBy = x.ApplicantPosting.ModifiedBy,
                            Modified = x.ApplicantPosting.Modified,
                            DisabledDate = x.ApplicantPosting.DisabledDate,
                            IsForceMinSchoolingMatch = x.ApplicantPosting.IsForceMinSchoolingMatch,
                            IsClosed = x.ApplicantPosting.IsClosed,
                            //PostingOwnerId = x.ApplicantPosting.PostingOwnerId,
                            PostingOwners = x.ApplicantPosting.ApplicantPostingOwners.Select(z => new ApplicantPostingOwnerDto
                            {
                                PostingId = z.PostingId,
                                UserId = z.UserId
                            }).ToList(),
                            OwnerNotifications = x.ApplicantPosting.OwnerNotifications
                        } : default(ApplicantPostingDto),
                        ApplicantCompanyCorrespondence = x.ApplicantApplicationEmailHistory.Count > 0 ? 
                        x.ApplicantApplicationEmailHistory.OrderByDescending(h => h.SentDate).Select( y => 
                        new ApplicantCompanyCorrespondenceDto()
                        {
                            ClientId = x.Applicant.ClientId,
                            ApplicantCompanyCorrespondenceId    = (y.ApplicantCompanyCorrespondenceId.HasValue ? y.ApplicantCompanyCorrespondenceId.Value : 0),
                            ApplicantCorrespondenceTypeId       = (y.ApplicantCompanyCorrespondenceId.HasValue ? y.ApplicantCompanyCorrespondence.ApplicantCorrespondenceTypeId : default(ApplicantCorrespondenceType?)),
                            IsActive                            = (y.ApplicantCompanyCorrespondenceId.HasValue ? y.ApplicantCompanyCorrespondence.IsActive : true),
                            Description     = !string.IsNullOrEmpty(y.Subject) ? y.Subject : 
                                                                  (y.ApplicantCompanyCorrespondenceId.HasValue ? y.ApplicantCompanyCorrespondence.Description : ""),
                            Body            = !string.IsNullOrEmpty(y.Body) ? y.Body :
                                                                  (y.ApplicantCompanyCorrespondenceId.HasValue ? y.ApplicantCompanyCorrespondence.Body : ""),
                        }).FirstOrDefault() : default(ApplicantCompanyCorrespondenceDto),
                        Applicant = x.Applicant != null ? new ApplicantDto
                        {
                            ApplicantId = x.Applicant.ApplicantId,
                            FirstName = x.Applicant.FirstName,
                            MiddleInitial = x.Applicant.MiddleInitial,
                            LastName = x.Applicant.LastName,
                            Address = x.Applicant.Address,
                            AddressLine2 = x.Applicant.AddressLine2,
                            City = x.Applicant.City,
                            State = x.Applicant.State ?? default(int),
                            Zip = x.Applicant.Zip,
                            PhoneNumber = x.Applicant.PhoneNumber,
                            CellPhoneNumber = x.Applicant.CellPhoneNumber,
                            EmailAddress = x.Applicant.EmailAddress,
                            Dob = x.Applicant.Dob,
                            EmployeeId = x.Applicant.EmployeeId,
                            UserId = x.Applicant.UserId,
                            ClientId = x.Applicant.ClientId,
                            IsDenied = x.Applicant.IsDenied,
                            WorkPhoneNumber = x.Applicant.WorkPhoneNumber,
                            WorkExtension = x.Applicant.WorkExtension,
                            CountryId = x.Applicant.CountryId,
                            IsTextEnabled = x.Applicant.IsTextEnabled.HasValue ? x.Applicant.IsTextEnabled.Value : false,
                            Client = x.Applicant.Client != null ? new ClientDto
                            {
                                ClientId = x.Applicant.Client.ClientId,
                                ClientName = x.Applicant.Client.ClientName,
                                AddressLine1 = x.Applicant.Client.AddressLine1,
                                AddressLine2 = x.Applicant.Client.AddressLine2,
                                City = x.Applicant.Client.City,
                                State = x.Applicant.Client.State != null ? new StateDto
                                {
                                    StateId = x.Applicant.Client.State.StateId,
                                    CountryId = x.Applicant.Client.State.CountryId,
                                    Name = x.Applicant.Client.State.Name,
                                    Code = x.Applicant.Client.State.Code,
                                    Abbreviation = x.Applicant.Client.State.Abbreviation,
                                    PostalNumericCode = x.Applicant.Client.State.PostalNumericCode
                                } : default(StateDto),
                                PostalCode = x.Applicant.Client.PostalCode
                            } : default(ClientDto),
                            StateDetails = x.Applicant.StateDetails != null ? new StateDto
                            {
                                StateId = x.Applicant.StateDetails.StateId,
                                CountryId = x.Applicant.StateDetails.CountryId,
                                Name = x.Applicant.StateDetails.Name,
                                Code = x.Applicant.StateDetails.Code,
                                Abbreviation = x.Applicant.StateDetails.Abbreviation,
                                PostalNumericCode = x.Applicant.StateDetails.PostalNumericCode
                            } : default(StateDto)
                        } : default(ApplicantDto)
                    };
                }
            }

        }
        public class ToApplicantDetailDto : ExpressionMapper<ApplicantApplicationHeader, ApplicantDetailDto>
        {
            public override Expression<Func<ApplicantApplicationHeader, ApplicantDetailDto>> MapExpression
            {
                get
                {
                    return x => new ApplicantDetailDto()
                    {
                        ApplicationHeaderId = x.ApplicationHeaderId,
                        ApplicationSubmittedOn = x.DateSubmitted,
                        OrigPostingId = x.OrigPostingId,
                        ApplicantResumeId = x.ApplicantResumeId,
                        ApplicantRejectionReasonId = x.ApplicantRejectionReasonId,
                        ApplicantStatusTypeId = x.ApplicantStatusTypeId,
                        IsRecommended = (x.ApplicantStatusTypeId == ApplicantStatusType.Candidate || x.IsRecommendInterview),// ? true : false,
                        ApplicantName = x.Applicant.FirstName + " " + x.Applicant.LastName,
                        ApplicantNameFlipped = x.Applicant.LastName + ", " + x.Applicant.FirstName,
                        IsApplicantDenied = x.Applicant.IsDenied,
                        IsTextEnabled = x.Applicant.IsTextEnabled.HasValue ? x.Applicant.IsTextEnabled.Value : false,
                        ApplicantId = x.ApplicantId,
                        Username = x.Applicant.User.UserName,
                        EmployeeId = x.Applicant.EmployeeId,
                        Posting = x.ApplicantPosting.PostingNumber + " " + x.ApplicantPosting.Description,
                        PostingId = x.ApplicantPosting.PostingId,
                        PostingStartDate = x.ApplicantPosting.StartDate,
                        FilledOn = x.ApplicantPosting.FilledDate,
                        ApplicantResumeRequiredId = x.ApplicantPosting.ApplicantResumeRequiredId,
                        ApplicantOnBoardingProcessId = x.ApplicantPosting.ApplicantOnBoardingProcessId,
                        RejectionReason = x.ApplicantRejectionReasonId.HasValue ? x.ApplicantRejectionReason.Description : string.Empty,
                        HasViewed = x.ApplicationViewed.Any(y => y.UserId == 1),
                        IsFlagged = x.ApplicantApplicationDetail.Any(y => y.IsFlagged),
                        Note = x.Applicant.ApplicantNote.Any() ? x.Applicant.ApplicantNote.OrderByDescending(y => y.Remark.AddedDate).FirstOrDefault().Remark.Description : "",

                        ApplicantCorrespondenceTypeId = (Dto.ApplicantTracking.ApplicantCorrespondenceType)(x.ApplicantApplicationEmailHistory.Count > 0 ?
                        (x.ApplicantApplicationEmailHistory.OrderByDescending(h => h.SentDate).FirstOrDefault().ApplicantCompanyCorrespondenceId ?? default(int?)) : default(int?)),
                        SentEmailsCount = x.ApplicantApplicationEmailHistory.Count,

                        NoteCount = x.Applicant.ApplicantNote.Count,
                        ResumeLinkLocation = x.Applicant.ApplicantResume.Any() ? x.Applicant.ApplicantResume.OrderByDescending(Y => Y.DateAdded).FirstOrDefault().LinkLocation : "",
                        IsExternalApplicant = x.ExternalApplicationIdentity != null ? true : false,
                        CoverLetter = x.CoverLetter,
                        CoverLetterId = x.CoverLetterId,
                        JobSiteName = (x.ExternalApplicationIdentity != null) ? x.ExternalApplicationIdentity.ApplicantJobSite.JobSiteDescription : "",
                        AddedByAdmin = x.AddedByAdmin,
						AddedByUserName = (x.AddedByUser != null) ? (x.AddedByUser.FirstName + " " + x.AddedByUser.LastName) : "",
						Score = x.Score.HasValue ? x.Score.Value : 0,
                        DisclaimerId = x.DisclaimerId,
                        PostingNo = x.ApplicantPosting.PostingNumber,
                        DocumentCount = x.ApplicantDocument.Count
                    };
                }
            }
        }

        public class ToEmployeeHireInfoDto : ExpressionMapper<Employee, EmployeeHireInfoDto>
        {
            public override Expression<Func<Employee, EmployeeHireInfoDto>> MapExpression
            {
                get
                {
                    return x => new EmployeeHireInfoDto()
                    {
                        ClientCostCenterId = x.ClientCostCenterId,
                        ClientDepartmentId = x.ClientDepartmentId,
                        ClientDivisionId = x.ClientDivisionId,
                        ClientId = x.ClientId,
                        EmployeeId = x.EmployeeId,
                        EmployeeNumber = x.EmployeeNumber,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        MiddleInitial = x.MiddleInitial,
                        JobProfileId = x.JobProfileId,
                        JobTitle = x.JobTitle,
                        HiredOn = x.Modified,
                        HiredBy = x.ModifiedBy,
                    };
                }
            }
        }
    }
}
