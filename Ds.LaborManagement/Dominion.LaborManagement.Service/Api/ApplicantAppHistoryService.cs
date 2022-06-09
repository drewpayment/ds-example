using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Services.Api;
using Dominion.Core.Services.Interfaces;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.LaborManagement.Dto.ApplicantTracking;
using Dominion.LaborManagement.Service.Internal.Security;
using Dominion.LaborManagement.Service.Mapping;
using Dominion.Utility.Mapping;
using Dominion.Utility.OpResult;

namespace Dominion.LaborManagement.Service.Api
{
    public class ApplicantAppHistoryService : IApplicantAppHistoryService
    {
        private readonly IBusinessApiSession _session;
        private readonly AppHistoryMaps.ToApplicantEmploymentHistoryDto _toEmploymentHistoryMapper;
        private readonly ApplicantLicenseMapper.ToApplicantLicense _toLicenseMapper;
        private readonly ApplicantLicenseMapper.ToApplicantLicenseDto _toLicenseDtoMapper;
        private readonly ApplicantSkillMapper.ToApplicantSkill _toSkillMapper;
        private readonly ApplicantSkillMapper.ToApplicantSkillDto _toSkillDtoMapper;

        private readonly ApplicantEducationHistoryMapper.ToApplicantEducationHistoryDto _toApplicantEducationHistoryDto;
        private readonly ISecurityManager _securityManager;
        public ApplicantAppHistoryService(IBusinessApiSession session, ISecurityManager securityManager)
        {
            _securityManager = securityManager;
            _session = session;
            _toEmploymentHistoryMapper = new AppHistoryMaps.ToApplicantEmploymentHistoryDto();
            _toLicenseMapper = new ApplicantLicenseMapper.ToApplicantLicense();
            _toLicenseDtoMapper = new ApplicantLicenseMapper.ToApplicantLicenseDto();
            _toSkillMapper = new ApplicantSkillMapper.ToApplicantSkill();
            _toSkillDtoMapper = new ApplicantSkillMapper.ToApplicantSkillDto();
            _toApplicantEducationHistoryDto = new ApplicantEducationHistoryMapper.ToApplicantEducationHistoryDto();
        }

        public OpResult<InitializeAppHistoryDto> getRequirements(int applicationId, int applicationHeaderId)
        {
            var result = new OpResult<InitializeAppHistoryDto>();
           var internalApp =  _session.CanPerformAction(ApplicantTrackingActionType.InternalApplicant);
            var externalApp = _session.CanPerformAction(ApplicantTrackingActionType.ExternalApplicant);
            if (internalApp.Success || externalApp.Success)
            {
                InitializeAppHistoryDto data = new InitializeAppHistoryDto();
                //get the education requirements
                var schoolNoOrder = this._session.UnitOfWork.ApplicantTrackingRepository
                    .ApplicantApplicationHeaderQuery()
                    .ByApplicationHeaderId(applicationHeaderId).ExecuteQueryAs(x =>
                        new ApplicantPostingSchoolRequirementDto()
                        {
                            IsForceMinSchoolingMatch = x.ApplicantPosting.IsForceMinSchoolingMatch,
                            MinSchooling = x.ApplicantPosting.MinSchooling
                        }).FirstOrDefault();

                if (schoolNoOrder?.MinSchooling > 0)
                {
                    schoolNoOrder.Order = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantSchoolTypeQuery()
                        .BySchoolTypeId(schoolNoOrder.MinSchooling.Value).ExecuteQuery().First().ApplicationOrder;
                }

                data.SchoolRequirementDto = schoolNoOrder;

                var employment = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantCompanyApplicationQuery()
                    .ByApplicationId(applicationId).ExecuteQueryAs(x=> new
                    {
                        yoe=x.YearsOfEmployment,
                        isExperience=x.IsExperience
                    }

                    ).First();

                //data.YearsOfEmployment = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantCompanyApplicationQuery()
                //    .ByApplicationId(applicationId).ExecuteQuery().First().YearsOfEmployment;

                data.YearsOfEmployment = employment.yoe;
                data.isExperience = employment.isExperience;
                result.Data = data;
            }
            else
            {
                internalApp.MergeInto(result);
                externalApp.MergeInto(result);
            }

            return result;
        }

        public IOpResult<ApplicantEmploymentHistoryDto> SaveApplicantEmploymentHistory(
            ApplicantEmploymentHistoryDto dto)
        {

            var opResult = new OpResult<ApplicantEmploymentHistoryDto>();

            var applicantQuery = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantsQuery()
                .ByUserId(_session.LoggedInUserInformation.UserId);

            if(dto.ApplicantId > 0)
                applicantQuery = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantsQuery()
                .ByApplicantId(dto.ApplicantId);

            var applicant = applicantQuery.ExecuteQuery().First();

            var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
            var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
            var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
            if (isApplicantTrackingEnabled || isSystemAdmin)
            {
                var applicantEmploymentHistory = new ApplicantEmploymentHistory();
                if (dto.ApplicantEmploymentId == 0)
                {
                    applicantEmploymentHistory = new ApplicantEmploymentHistory()
                    {
                        ApplicantEmploymentId = dto.ApplicantEmploymentId,
                        ApplicantId = applicant.ApplicantId,
                        Company = dto.Company,
                        City = dto.City,
                        StateId = dto.StateId,
                        Zip = dto.Zip,
                        Title = dto.Title,
                        StartDate = dto.StartDate,
                        EndDate = dto.IsVoluntaryResign.HasValue ? dto.EndDate : "Present",
                        IsContactEmployer = dto.IsContactEmployer,
                        IsVoluntaryResign = dto.IsVoluntaryResign,
                        IsEnabled = true,
                        Responsibilities = dto.Responsibilities,
                        CountryId = dto.CountryId
                    };
                    _session.UnitOfWork.RegisterNew(applicantEmploymentHistory);
                }
                else
                {
                    applicantEmploymentHistory = _session.UnitOfWork.ApplicantTrackingRepository
                                                .ApplicantEmploymentHistoryQuery()
                                                .ByApplicantEmploymentHistoryId(dto.ApplicantEmploymentId)
                                                .FirstOrDefault();
                    applicantEmploymentHistory.ApplicantId = applicant.ApplicantId;
                    applicantEmploymentHistory.Company = dto.Company;
                    applicantEmploymentHistory.City = dto.City;
                    applicantEmploymentHistory.StateId = dto.StateId;
                    applicantEmploymentHistory.Zip = dto.Zip;
                    applicantEmploymentHistory.Title = dto.Title;
                    applicantEmploymentHistory.StartDate = dto.StartDate;
                    applicantEmploymentHistory.EndDate = dto.IsVoluntaryResign.HasValue ? dto.EndDate : "Present";
                    applicantEmploymentHistory.IsContactEmployer = dto.IsContactEmployer;
                    applicantEmploymentHistory.IsVoluntaryResign = dto.IsVoluntaryResign;
                    applicantEmploymentHistory.IsEnabled = dto.IsEnabled;
                    applicantEmploymentHistory.Responsibilities = dto.Responsibilities;
                    applicantEmploymentHistory.CountryId = dto.CountryId;

                    _session.UnitOfWork.RegisterModified(applicantEmploymentHistory);
                }
                _session.UnitOfWork.Commit().MergeInto(opResult);

                if (dto.ApplicantEmploymentId == 0)
                {
                    dto.ApplicantEmploymentId = applicantEmploymentHistory.ApplicantEmploymentId;
                    dto.ApplicantId = applicant.ApplicantId;
                    dto.IsEnabled = true;
                }

                opResult.Data = dto;
            }

            return opResult;
        }

        public IOpResult<ApplicantEmploymentHistoryDto> DeleteApplicantEmploymentHistory(int applicantEmploymentHistoryId)
        {
            var opResult = new OpResult<ApplicantEmploymentHistoryDto>();
            var isApplicantAdmin = _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).Success;
            var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
            var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
            var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
            if (isApplicantTrackingEnabled || isSystemAdmin)
            {
                var employmentHistoryToDelete = _session.UnitOfWork.ApplicantTrackingRepository
                                            .ApplicantEmploymentHistoryQuery()
                                            .ByApplicantEmploymentHistoryId(applicantEmploymentHistoryId)
                                            .FirstOrDefault();

                employmentHistoryToDelete.IsEnabled = false;
                _session.UnitOfWork.RegisterModified(employmentHistoryToDelete);
                _session.UnitOfWork.Commit().MergeInto(opResult);
                var dto = new ApplicantEmploymentHistoryDto()
                {
                    ApplicantEmploymentId = employmentHistoryToDelete.ApplicantEmploymentId,
                    ApplicantId = employmentHistoryToDelete.ApplicantId,
                    Company = employmentHistoryToDelete.Company,
                    City = employmentHistoryToDelete.City,
                    StateId = employmentHistoryToDelete.StateId,
                    Zip = employmentHistoryToDelete.Zip,
                    Title = employmentHistoryToDelete.Title,
                    StartDate = employmentHistoryToDelete.StartDate,
                    EndDate = employmentHistoryToDelete.EndDate,
                    IsContactEmployer = employmentHistoryToDelete.IsContactEmployer,
                    IsVoluntaryResign = employmentHistoryToDelete.IsVoluntaryResign,
                    IsEnabled = employmentHistoryToDelete.IsEnabled,
                    Responsibilities = employmentHistoryToDelete.Responsibilities,
                    CountryId = employmentHistoryToDelete.CountryId
                };
                opResult.Data = dto;
            }
            return opResult;
        }

        IOpResult<ApplicantEducationHistoryDto> IApplicantAppHistoryService.GetApplicantEducationHistory(int educationHistoryId)
        {
            var opResult = new OpResult<ApplicantEducationHistoryDto>();
            var accessRights = _securityManager.GetLegacyAccessRightsForCurrentUser();
            var isApplicantTrackingEnabled = accessRights.IsApplicantTrackingEnabled;
            var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
            if (isApplicantTrackingEnabled || isSystemAdmin || accessRights.UserId == 0)
            {

                opResult.TrySetData(() => _session.UnitOfWork.ApplicantTrackingRepository.ApplicantEducationHistoryQuery()
                    .ByApplicantEducationId(educationHistoryId)
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
                    }).FirstOrDefault());
            }

            return opResult;
        }

        IOpResult<ApplicantEducationHistoryDto> IApplicantAppHistoryService.UpdateApplicantEducationHistory(ApplicantEducationHistoryDto dto)
        {
            var opResult = new OpResult<ApplicantEducationHistoryDto>();
            var applicantQuery = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantsQuery()
                .ByUserId(_session.LoggedInUserInformation.UserId);

            if (dto.ApplicantId > 0)
                applicantQuery = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantsQuery()
                .ByApplicantId(dto.ApplicantId);

            var applicant = applicantQuery.ExecuteQuery().First();

            var isApplicantAdmin = _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).Success;
            var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
            var internalApp = _session.CanPerformAction(ApplicantTrackingActionType.InternalApplicant);
            var externalApp = _session.CanPerformAction(ApplicantTrackingActionType.ExternalApplicant);
            if (isApplicantAdmin || isSystemAdmin || internalApp.Success || externalApp.Success)
            {
                var applicantEducationHistory = new ApplicantEducationHistory();
                int historyId = dto.ApplicantEducationId;
                if (historyId == 0)
                {
                    applicantEducationHistory = new ApplicantEducationHistory()
                    {
                        ApplicantId = applicant.ApplicantId,
                        Description = dto.Description,
                        DateStarted = dto.DateStarted,
                        DateEnded = dto.DateEnded,
                        HasDegree = dto.HasDegree,
                        DegreeId = dto.DegreeId,
                        Studied = dto.Studied,
                        IsEnabled = true,
                        YearsCompleted = dto.YearsCompleted,
                        ApplicantSchoolTypeId = dto.ApplicantSchoolTypeId
                    };
                    _session.UnitOfWork.RegisterNew(applicantEducationHistory);
                }
                else
                {
                    applicantEducationHistory = _session.UnitOfWork.ApplicantTrackingRepository
                                                .ApplicantEducationHistoryQuery()
                                                .ByApplicantEducationId(dto.ApplicantEducationId)
                                                .FirstOrDefault();

                    applicantEducationHistory.ApplicantEducationId = dto.ApplicantEducationId;
                    applicantEducationHistory.ApplicantId = dto.ApplicantId;
                    applicantEducationHistory.Description = dto.Description;
                    applicantEducationHistory.DateStarted = dto.DateStarted;
                    applicantEducationHistory.DateEnded = dto.DateEnded;
                    applicantEducationHistory.HasDegree = dto.HasDegree;
                    applicantEducationHistory.DegreeId = dto.DegreeId;
                    applicantEducationHistory.Studied = dto.Studied;
                    applicantEducationHistory.IsEnabled = dto.IsEnabled;
                    applicantEducationHistory.YearsCompleted = dto.YearsCompleted;
                    applicantEducationHistory.ApplicantSchoolTypeId = dto.ApplicantSchoolTypeId;

                    _session.UnitOfWork.RegisterModified(applicantEducationHistory);
                }
                _session.UnitOfWork.Commit().MergeInto(opResult);

                if (dto.ApplicantEducationId == 0)
                {
                    dto.ApplicantEducationId = applicantEducationHistory.ApplicantEducationId;
                }

                dto.ApplicantId = applicantEducationHistory.ApplicantId;
                dto.IsEnabled = applicantEducationHistory.IsEnabled;
                dto.ApplicationOrder = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantSchoolTypeQuery()
                    .BySchoolTypeId(dto.ApplicantSchoolTypeId.GetValueOrDefault()).FirstOrDefault().ApplicationOrder;

                if (dto.DegreeId.HasValue)
                {
                    dto.Degree = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantDegreeListQuery()
                        .ByDegreeId(dto.DegreeId.Value).ExecuteQuery().First().Description;
                }

                opResult.Data = dto;
            }

            return opResult;
        }

        IOpResult<ApplicantEducationHistoryDto> IApplicantAppHistoryService.DeleteApplicantEducationHistory(int educationHistoryId)
        {
            var opResult = new OpResult<ApplicantEducationHistoryDto>();
            var isApplicantAdmin = _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).Success;
            var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
            var internalApp = _session.CanPerformAction(ApplicantTrackingActionType.InternalApplicant);
            var externalApp = _session.CanPerformAction(ApplicantTrackingActionType.ExternalApplicant);
            if (isApplicantAdmin || isSystemAdmin || internalApp.Success || externalApp.Success)
            {
                var applicantEducationToDelete = _session.UnitOfWork.ApplicantTrackingRepository
                                            .ApplicantEducationHistoryQuery()
                                            .ByApplicantEducationId(educationHistoryId)
                                            .FirstOrDefault();

                applicantEducationToDelete.IsEnabled = false;
                _session.UnitOfWork.RegisterModified(applicantEducationToDelete);
                _session.UnitOfWork.Commit().MergeInto(opResult);
                opResult.Data = _toApplicantEducationHistoryDto.Map(applicantEducationToDelete);

            }
            return opResult;
        }

        public IOpResult<ApplicantLicenseDto> SaveApplicantLicense(ApplicantLicenseDto dto)
        {
            var result = new OpResult<ApplicantLicenseDto>();

            var applicant = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantsQuery()
                .ByUserId(_session.LoggedInUserInformation.UserId).ExecuteQuery().First();

            var internalApp = _session.CanPerformAction(ApplicantTrackingActionType.InternalApplicant);
            var externalApp = _session.CanPerformAction(ApplicantTrackingActionType.ExternalApplicant);
            ApplicantLicense license;

            if (internalApp.Success || externalApp.Success)
            {
                license = _toLicenseMapper.Map(dto);
                license.Country = _session.UnitOfWork.LocationRepository.CountryQuery().ExecuteQuery()
                    .FirstOrDefault(x => x.CountryId == license.CountryId);

                if (license.StateId.HasValue)
                {
                    license.State = _session.UnitOfWork.LocationRepository.StateQuery().ByStateId(license.StateId.Value).FirstOrDefault();
                }

                if (dto.ApplicantLicenseId == 0)
                {
                    
                    
                    license.IsEnabled = true;
                    license.ApplicantId = applicant.ApplicantId;
                    _session.UnitOfWork.RegisterNew(license);
                }
                else
                {
                    license = _toLicenseMapper.Map(dto);
                    license.ApplicantId = applicant.ApplicantId;
                    _session.UnitOfWork.RegisterModified(license);
                }

                _session.UnitOfWork.Commit().MergeInto(result);
                result.Data = _toLicenseDtoMapper.Map(license);
            }
            else
            {
                internalApp.MergeInto(result);
                externalApp.MergeInto(result);
            }

            return result;
        }

        public IOpResult<ApplicantLicenseDto> DeleteApplicantLicense(int applicantLicenseId)
        {
            var opResult = new OpResult<ApplicantLicenseDto>();
            var isApplicantAdmin = _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).Success;
            var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
            var internalApp = _session.CanPerformAction(ApplicantTrackingActionType.InternalApplicant);
            var externalApp = _session.CanPerformAction(ApplicantTrackingActionType.ExternalApplicant);
            if (isApplicantAdmin || isSystemAdmin || internalApp.Success || externalApp.Success)
            {
                var license = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantLicenseQuery()
                    .ByApplicantLicenseId(applicantLicenseId).ExecuteQuery().First();
                license.IsEnabled = false;
                _session.UnitOfWork.RegisterModified(license);
                _session.UnitOfWork.Commit().MergeInto(opResult);
                if (opResult.Success)
                {
                    opResult.Data = new ApplicantLicenseDto(){ApplicantLicenseId = applicantLicenseId};
                }
            }
            else
            {
                internalApp.MergeInto(opResult);
                externalApp.MergeInto(opResult);
            }

            return opResult;
        }

        public IOpResult<IEnumerable<ApplicantLicenseDto>> GetLicensesForApplicant()
        {
            var result = new OpResult<IEnumerable<ApplicantLicenseDto>>();
            var applicantId = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantsQuery()
                .ByUserId(_session.LoggedInUserInformation.UserId).ExecuteQuery().First().ApplicantId;
            var internalApp = _session.CanPerformAction(ApplicantTrackingActionType.InternalApplicant);
            var externalApp = _session.CanPerformAction(ApplicantTrackingActionType.ExternalApplicant);

            if (internalApp.Success || externalApp.Success)
            {
                result.Data = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantLicenseQuery()
                    .ByApplicantId(applicantId).ByEnabled(true).ExecuteQueryAs(_toLicenseDtoMapper);
            }
            else
            {
                internalApp.MergeInto(result);
                externalApp.MergeInto(result);
            }

            return result;
        }

        public IOpResult<ApplicantSkillDto> SaveApplicantSkill(ApplicantSkillDto dto)
        {
            var result = new OpResult<ApplicantSkillDto>();

            var applicant = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantsQuery()
                .ByUserId(_session.LoggedInUserInformation.UserId).ExecuteQuery().First();

            var internalApp = _session.CanPerformAction(ApplicantTrackingActionType.InternalApplicant);
            var externalApp = _session.CanPerformAction(ApplicantTrackingActionType.ExternalApplicant);
            ApplicantSkill skill;

            if (internalApp.Success || externalApp.Success)
            {
                skill = _toSkillMapper.Map(dto);
                
                if (dto.ApplicantSkillId == 0)
                {
                    skill.IsEnabled = true;
                    skill.ApplicantId = applicant.ApplicantId;
                    _session.UnitOfWork.RegisterNew(skill);
                }
                else
                {
                    skill = _toSkillMapper.Map(dto);
                    skill.ApplicantId = applicant.ApplicantId;
                    _session.UnitOfWork.RegisterModified(skill);
                }

                _session.UnitOfWork.Commit().MergeInto(result);
                result.Data = _toSkillDtoMapper.Map(skill);
            }
            else
            {
                internalApp.MergeInto(result);
                externalApp.MergeInto(result);
            }

            return result;
        }

        public IOpResult<ApplicantSkillDto> DeleteApplicantSkill(int applicantSkillId)
        {
            var opResult = new OpResult<ApplicantSkillDto>();
            var isApplicantAdmin = _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).Success;
            var isSystemAdmin = _session.CanPerformAction(SystemActionType.SystemAdministrator).Success;
            var internalApp = _session.CanPerformAction(ApplicantTrackingActionType.InternalApplicant);
            var externalApp = _session.CanPerformAction(ApplicantTrackingActionType.ExternalApplicant);
            if (isApplicantAdmin || isSystemAdmin || internalApp.Success || externalApp.Success)
            {
                var skill = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantSkillQuery()
                    .ByApplicantSkillId(applicantSkillId).ExecuteQuery().First();
                skill.IsEnabled = false;
                _session.UnitOfWork.RegisterModified(skill);
                _session.UnitOfWork.Commit().MergeInto(opResult);
                if (opResult.Success)
                {
                    opResult.Data = new ApplicantSkillDto() { ApplicantSkillId = applicantSkillId };
                }
            }
            else
            {
                internalApp.MergeInto(opResult);
                externalApp.MergeInto(opResult);
            }

            return opResult;
        }

        public IOpResult<IEnumerable<ApplicantSkillDto>> GetSkillsForApplicant()
        {
            var result = new OpResult<IEnumerable<ApplicantSkillDto>>();
            var applicantId = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantsQuery()
                .ByUserId(_session.LoggedInUserInformation.UserId).ExecuteQuery().First().ApplicantId;
            var internalApp = _session.CanPerformAction(ApplicantTrackingActionType.InternalApplicant);
            var externalApp = _session.CanPerformAction(ApplicantTrackingActionType.ExternalApplicant);

            if (internalApp.Success || externalApp.Success)
            {
                result.Data = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantSkillQuery()
                    .ByApplicantId(applicantId).ByEnabled(true).ExecuteQueryAs(_toSkillDtoMapper);
            }
            else
            {
                internalApp.MergeInto(result);
                externalApp.MergeInto(result);
            }

            return result;
        }

        public IOpResult<IEnumerable<ApplicantEducationHistoryDto>> GetEducationForApplicant()
        {
            var result = new OpResult<IEnumerable<ApplicantEducationHistoryDto>>();
            var applicantId = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantsQuery()
                .ByUserId(_session.LoggedInUserInformation.UserId).ExecuteQuery().First().ApplicantId;
            var educationHistory = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantEducationHistoryQuery()
                .ByApplicantId(applicantId).ByIsActive(true);

            var internalApp = _session.CanPerformAction(ApplicantTrackingActionType.InternalApplicant);
            var externalApp = _session.CanPerformAction(ApplicantTrackingActionType.ExternalApplicant);

            if (internalApp.Success || externalApp.Success)
            {
                result.Data = educationHistory.ExecuteQueryAs(x =>
                    new ApplicantEducationHistoryDto()
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
                        ApplicationOrder = x.ApplicantSchoolType.ApplicationOrder
                    });
            }
            else
            {
                internalApp.MergeInto(result);
                externalApp.MergeInto(result);
            }

            return result;
        }

        public IOpResult<IEnumerable<ApplicantEmploymentHistoryDto>> GetApplicantEmploymentHistoryForUsersApplicantId()
        {
            var opResult = new OpResult<IEnumerable<ApplicantEmploymentHistoryDto>>();
            var applicantId = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantsQuery()
                .ByUserId(_session.LoggedInUserInformation.UserId).ExecuteQuery().First().ApplicantId;
            var internalApp = _session.CanPerformAction(ApplicantTrackingActionType.InternalApplicant);
            var externalApp = _session.CanPerformAction(ApplicantTrackingActionType.ExternalApplicant);
            if (internalApp.Success || externalApp.Success)
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
            else
            {
                internalApp.MergeInto(opResult);
                externalApp.MergeInto(opResult);
            }

            return opResult;
        }
    }
}
