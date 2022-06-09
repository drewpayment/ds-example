using Dominion.Core.Dto.Labor;
using Dominion.Core.Services.Interfaces;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Entities.TimeClock;
using Dominion.LaborManagement.Service.Mapping.Clock;
using Dominion.Utility.Containers;
using Dominion.Utility.Msg.Specific;
using Dominion.Utility.OpResult;
using Dominion.Utility.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using Dominion.Core.Dto.Client;
using Dominion.Core.Dto.Common;
using Dominion.Core.Dto.Employee;
using Dominion.Core.Dto.Misc;
using Dominion.Core.Dto.TimeClock;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Entities;
using Dominion.LaborManagement.Dto.Clock;
using Dominion.LaborManagement.Dto.EmployeeLaborManagement;
using Dominion.LaborManagement.Service.Internal.Security;
using Dominion.LaborManagement.Service.Mapping;
using Dominion.Utility.Constants;
using ClockClientOvertimeSelected = Dominion.Domain.Entities.TimeClock.ClockClientOvertimeSelected;
using HolidayDto = Dominion.Core.Dto.Labor.HolidayDto;
using Dominion.Core.Dto.SftpUpload.CsvTemplates;
using Dominion.LaborManagement.Dto.Sproc;
using Dominion.Core.Dto.TimeCard;
using Dominion.Core.Dto.Sprocs;
using Dominion.LaborManagement.Dto.Enums;
using Dominion.Core.Dto.TimeCard.Result;
using Dominion.Utility.ExtensionMethods;

namespace Dominion.LaborManagement.Service.Internal.Providers
{
    internal class LaborManagementProvider : ILaborManagementProvider
    {

        private readonly IBusinessApiSession _session;

        public ILaborManagementProvider Self { get; set; }

        public LaborManagementProvider(IBusinessApiSession session)
        {
            _session = session;
            Self = this;
        }

        IOpResult<ClockClientNoteDto> ILaborManagementProvider.GetClockClientNote(int clockClientNoteId)
        {
            return new OpResult<ClockClientNoteDto>().TrySetData(_session.UnitOfWork.LaborManagementRepository.ClockClientNotesQuery()
                .ByClockClientNoteId(clockClientNoteId)
                .ExecuteQueryAs(new ClockClientNoteMaps.ToClockClientNoteDto()).FirstOrDefault);
        }

        IOpResult<ClockClientOvertimeDto> ILaborManagementProvider.GetClockClientOvertime(int clockClientOvertimeId)
        {
            return new OpResult<ClockClientOvertimeDto>().TrySetData(_session.UnitOfWork.LaborManagementRepository.ClockClientOvertimeQuery()
                .ByClockClientOvertimeId(clockClientOvertimeId)
                .ExecuteQueryAs(new ClockClientOvertimeMaps.ToClockClientOvertimeDto()).FirstOrDefault);
        }

        IOpResult ILaborManagementProvider.CheckUsageForClockClientOvertimeSelected(ClockClientOvertimeDto current)
        {
            var result = new OpResult();

            var isSelectedInUse = _session.UnitOfWork.LaborManagementRepository
                .ClockClientOvertimeSelectedQuery()
                .ByClockClientOvertime(current.ClockClientOvertimeId)
                .Result
                .Any();

            if (isSelectedInUse)
            {
                result
                    .AddMessage(new GenericMsg($"This overtime has been selected for use on an existing time policy."))
                    .SetToFail();
            }

            return result;
        }

        IOpResult ILaborManagementProvider.CheckUsageForClockClientHoliday(ClockClientHolidayDto curr)
        {
            var result = new OpResult();

            var inUse = _session.UnitOfWork.LaborManagementRepository
                .ClockClientTimePolicyQuery()
                .ByClientId(curr.ClientId)
                .ExecuteQueryAs(x => new ClockClientTimePolicyDtos.ClockClientTimePolicyDto
                {
                    ClockClientTimePolicyId = x.ClockClientTimePolicyId,
                    ClientId = x.ClientId,
                    ClockClientHolidayId = x.ClockClientHolidayId
                })
                .ToList()
                .Any(x => x.ClockClientHolidayId == curr.ClockClientHolidayId);

            var details = _session.UnitOfWork.LaborManagementRepository
                .ClockClientHolidayDetailQuery()
                .ByClockClientHolidayId(curr.ClockClientHolidayId)
                .ExecuteQueryAs(x => new ClockClientHolidayDetailDto
                {
                    ClockClientHolidayDetailId = x.ClockClientHolidayDetailId
                });

            foreach (var d in details)
            {
                if (inUse) continue;
                inUse = _session.UnitOfWork.LaborManagementRepository
                    .ClockEmployeeBenefitQuery()
                    .ByClockClientHolidayDetail(d.ClockClientHolidayDetailId)
                    .ExecuteQueryAs(x => new ClockEmployeeBenefitMaps.ToClockEmployeeBenefitDto())
                    .ToList()
                    .Any();
            }

            if (inUse)
            {
                result
                    .AddMessage(new GenericMsg($"This holiday rule has been selected for use on an existing time policy."))
                    .SetToFail();
            }

            return result;
        }

        IOpResult ILaborManagementProvider.CheckUsageForClockClientHolidayDetail(ClockClientHolidayDetailDto curr)
        {
            var result = new OpResult();

            var inUse = _session.UnitOfWork.LaborManagementRepository
                .ClockClientHolidayQuery()
                .ByClockClientHolidayId(curr.ClockClientHolidayId)
                .Result
                .Any();

            if (inUse)
            {
                result
                    .AddMessage(new GenericMsg($"These holiday details can only be deleted if their rule has been deleted first."))
                    .SetToFail();
            }

            return result;
        }

        IOpResult ILaborManagementProvider.DeleteRelatedClockEmployeeBenefitRecords(int clockClientHolidayDetailId, bool holdCommit)
        {
            var result = new OpResult();

            var benefits = _session.UnitOfWork.LaborManagementRepository
                    .ClockEmployeeBenefitQuery()
                    .ByClockClientHolidayDetail(clockClientHolidayDetailId)
                    .ExecuteQueryAs(new ClockEmployeeBenefitMaps.ToClockEmployeeBenefitDto())
                    .ToList()
                    .Where(x => x.EventDate > DateTime.Today)
                    .ToList();

            foreach (var b in benefits)
            {
                var entity = new ClockEmployeeBenefit
                {
                    ClockEmployeeBenefitId = Convert.ToInt32(b.ClockEmployeeBenefitId),
                    ClockClientHolidayDetailId = clockClientHolidayDetailId
                };
                _session.SetModifiedProperties(entity);
                _session.UnitOfWork.RegisterDeleted(entity);

                var changeEntity = new ClockEmployeeBenefitChangeHistory
                {
                    ClockEmployeeBenefitId = Convert.ToInt32(b.ClockEmployeeBenefitId),
                    ClientId = b.ClientId,
                    EmployeeId = b.EmployeeId,
                    ClockClientHolidayDetailId = b.ClockClientHolidayDetailId,
                    EventDate = b.EventDate,
                    ClientEarningId = b.ClientEarningId,
                    Hours = b.Hours,
                    ChangeId = CommonConstants.NEW_ENTITY_ID,
                    ClientDepartmentId = b.ClientDepartmentId,
                    ChangeMode = ChangeModeType.Deleted,
                    ApprovedBy = b.ApprovedBy,
                    IsApproved = b.IsApproved,
                    ClientCostCenterId = b.ClientCostCenterId,
                    ClientDivisionId = b.ClientDivisionId,
                    ClientJobCostingAssignmentId1 = b.ClientJobCostingAssignmentId1,
                    ClientJobCostingAssignmentId2 = b.ClientJobCostingAssignmentId2,
                    ClientJobCostingAssignmentId3 = b.ClientJobCostingAssignmentId3,
                    ClientJobCostingAssignmentId4 = b.ClientJobCostingAssignmentId4,
                    ClientJobCostingAssignmentId5 = b.ClientJobCostingAssignmentId5,
                    ClientJobCostingAssignmentId6 = b.ClientJobCostingAssignmentId6,
                    ClientShiftId = b.ClientShiftId,
                    Comment = b.Comment,
                    EmployeeBenefitPay = b.EmployeeBenefitPay,
                    EmployeeClientRateId = b.EmployeeClientRateId,
                    EmployeeComment = b.EmployeeComment,
                    IsWorkedHours = b.IsWorkedHours,
                    RequestTimeOffDetailId = b.RequestTimeOffDetailId,
                    Subcheck = b.Subcheck
                };

                _session.SetModifiedProperties(changeEntity);
                _session.UnitOfWork.RegisterNew(changeEntity);
            }

            if (!holdCommit) _session.UnitOfWork.Commit().MergeInto(result);

            return result;
        }

        IOpResult ILaborManagementProvider.CheckUsageForClockClientNote(ClockClientNoteDto current)
        {
            var result = new OpResult();

            // Check to see if the note is in use on ClockEmployeeApproveDate table
            var isNoteInUse = _session.UnitOfWork.LaborManagementRepository
                .ClockEmployeeApproveDateQuery()
                .ByClockClientNoteId(current.ClockClientNoteId)
                .Result
                .Any();

            if (isNoteInUse)
            {
                result
                    .AddMessage(new GenericMsg($"This note is in use and cannot be deleted."))
                    .SetToFail();
            }

            return result;
        }

        IOpResult ILaborManagementProvider.CheckUsageForClockClientException(ClockClientExceptionDto current)
        {
            var result = new OpResult();

            var inUse = _session.UnitOfWork.LaborManagementRepository
                .ClockClientTimePolicyQuery()
                .ByClockClientExceptionId(current.ClockClientExceptionId)
                .Result
                .Any();

            if (inUse)
            {
                result
                    .AddMessage(new GenericMsg($"This clock exception is in use on an active time policy."))
                    .SetToFail();
            }

            return result;
        }

        IOpResult ILaborManagementProvider.CheckUsageForClockClientExceptionDetail(
            ClockClientExceptionDetailDto current)
        {
            var result = new OpResult();

            var inUse = _session.UnitOfWork.LaborManagementRepository
                .ClockClientExceptionQuery()
                .ByClockClientException(Convert.ToInt32(current.ClockClientExceptionId))
                .Result
                .Any();

            if (inUse)
            {
                result
                    .AddMessage(new GenericMsg($"This exception detail is in use and cannot be deleted."))
                    .SetToFail();
            }

            return result;
        }


        IOpResult ILaborManagementProvider.CheckUsageForClockClientRules(ClockClientRulesDto current)
        {
            var result = new OpResult();

            IEnumerable<string> policyNames = _session.UnitOfWork.LaborManagementRepository
                .ClockClientTimePolicyQuery()
                .ByClockClientRulesId(current.ClockClientRulesId)
                .ExecuteQueryAs(x => x.Name);

            if (policyNames.Count() > 0)
            {
                string message = policyNames.First() + " Time policy, and cannot be deleted.";
                if (policyNames.Count() > 1) message = " Time policies( " + string.Join(", ", policyNames) + "), and cannot be deleted.";

                return result.AddMessage(new GenericMsg("This Rule is assigned to " + message ))
                    .SetToFail();
            }

            return result;
        }

        IOpResult<ClockClientOvertimeDto> ILaborManagementProvider.RegisterNewClockClientOvertime(
            ClockClientOvertime entity, ClockClientOvertimeDto dto)
        {
            var result = new OpResult<ClockClientOvertimeDto>();

            _session.UnitOfWork.RegisterNew(entity);
            _session.UnitOfWork.RegisterPostCommitAction(() => dto.ClockClientOvertimeId = entity.ClockClientOvertimeId);

            return result;
        }

        IOpResult<ClockClientOvertimeDto> ILaborManagementProvider.RegisterExistingClockClientOvertime(
            ClockClientOvertime entity, ClockClientOvertimeDto dto)
        {
            var result = new OpResult<ClockClientOvertimeDto>();
            var current = Self.GetClockClientOvertime(dto.ClockClientOvertimeId).MergeInto(result).Data;

            if (result.CheckForNotFound(current).HasError) return result;

            if (current.ClientId != dto.ClientId)
            {
                _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, current.ClientId).MergeInto(result);
                if (result.Success)
                {
                    entity.ClientId = current.ClientId;
                    dto.ClientId = current.ClientId;
                }
            }

            if (result.Success)
            {
                entity.ClockClientOvertimeId = dto.ClockClientOvertimeId;
                if (result.Success)
                {
                    Self.RegisterModifiedClockClientOvertime(entity, current);
                }
            }

            return result;
        }

        IOpResult<ClockClientNoteDto> ILaborManagementProvider.RegisterNewClockClientNote(ClockClientNote entity,
            ClockClientNoteDto dto)
        {
            var result = new OpResult<ClockClientNoteDto>();

            _session.UnitOfWork.RegisterNew(entity);
            _session.UnitOfWork.RegisterPostCommitAction(() => dto.ClockClientNoteId = entity.ClockClientNoteId);

            return result;
        }

        IOpResult<ClockClientNoteDto> ILaborManagementProvider.RegisterExistingClockClientNote(ClockClientNote entity,
            ClockClientNoteDto dto)
        {
            var result = new OpResult<ClockClientNoteDto>();
            var current = Self.GetClockClientNote(dto.ClockClientNoteId).MergeInto(result).Data;

            // If not found, bail out.
            if (result.CheckForNotFound(current).HasError) return result;

            // if client IDs don't match, let's check permissions again
            if (current.ClientId != dto.ClientId)
            {
                _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, current.ClientId).MergeInto(result);
                if (result.Success)
                {
                    entity.ClientId = current.ClientId;
                    dto.ClientId = current.ClientId;
                }
            }

            if (result.Success)
            {
                entity.ClockClientNoteId = dto.ClockClientNoteId;
                if (result.Success)
                {
                    Self.RegisterModifiedClockClientNote(entity, current);
                }
            }

            return result;
        }

        IOpResult<ClockClientExceptionDto> ILaborManagementProvider.GetClockClientException(int clockClientExceptionId)
        {
            return new OpResult<ClockClientExceptionDto>().TrySetData(_session.UnitOfWork.LaborManagementRepository.ClockClientExceptionQuery()
                .ByClockClientException(clockClientExceptionId)
                .ExecuteQueryAs(new ClockClientExceptionMaps.ToClockClientExceptionDto()).FirstOrDefault);
        }

        IOpResult<List<ClockClientExceptionDto>> ILaborManagementProvider.GetClockClientExceptionsByClient(int clientId)
        {
            var result = new OpResult<List<ClockClientExceptionDto>>();

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(result);
            _session.CanPerformAction(LaborManagementActionType.ReadTimePolicy).MergeInto(result);

            if (result.HasError) return result;

            return result.TrySetData(() => _session.UnitOfWork.LaborManagementRepository
                .ClockClientExceptionQuery()
                .ByClientId(clientId)
                .ExecuteQueryAs(new ClockClientExceptionMaps.ToClockClientExceptionDto())
                .ToList());
        }

        IOpResult<ClockClientHolidayDto> ILaborManagementProvider.GetClockClientHoliday(int clockClientHolidayId)
        {
            return new OpResult<ClockClientHolidayDto>()
                .TrySetData(() => _session
                .UnitOfWork
                .LaborManagementRepository
                .ClockClientHolidayQuery()
                .ByClockClientHolidayId(clockClientHolidayId)
                .ExecuteQueryAs(x => new ClockClientHolidayDto
                {
                    ClockClientHolidayId = x.ClockClientHolidayId,
                    ClientId = x.ClientId,
                    ClientEarningId = x.ClientEarningId,
                    HolidayWaitingPeriodDateId = x.HolidayWaitingPeriodDateId,
                    HolidayWorkedClientEarningId = x.HolidayWorkedClientEarningId,
                    Hours = x.Hours,
                    Name = x.Name,
                    WaitingPeriod = x.WaitingPeriod
                })
                .FirstOrDefault());
        }

        IOpResult<List<ClockClientHolidayDto>> ILaborManagementProvider.GetClockClientHolidaysByClient(int clientId)
        {
            var result = new OpResult<List<ClockClientHolidayDto>>();

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(result);
            _session.CanPerformAction(LaborManagementActionType.ReadTimePolicy).MergeInto(result);

            if (result.HasError) return result;

            return result.TrySetData(() => _session.UnitOfWork.LaborManagementRepository
                .ClockClientHolidayQuery()
                .ByClient(clientId)
                .ExecuteQueryAs(new ClockClientHolidayMaps.ToClockClientHolidayDto())
                .ToList());
        }

        IOpResult<ClockClientRulesDto> ILaborManagementProvider.GetClockClientRules(int clockClientRulesId)
        {
            return new OpResult<ClockClientRulesDto>()
                .TrySetData(() => _session
                .UnitOfWork
                .TimeClockRepository
                .GetClockClientRules()
                .ByClockClientRulesId(clockClientRulesId)
                .ExecuteQueryAs(new ClockClientRulesMaps.ToClockClientRulesDto())
                .FirstOrDefault());
        }

        IOpResult<ClockClientHolidayDetailDto> ILaborManagementProvider.GetClockClientHolidayDetail(
            int clockClientHolidayDetailId)
        {
            return new OpResult<ClockClientHolidayDetailDto>()
                .TrySetData(() => _session
                .UnitOfWork
                .LaborManagementRepository
                .ClockClientHolidayDetailQuery()
                .ByClockClientHolidayDetailId(clockClientHolidayDetailId)
                .ExecuteQueryAs(x => new ClockClientHolidayDetailDto
                {
                    ClockClientHolidayDetailId = x.ClockClientHolidayDetailId,
                    ClockClientHolidayId = x.ClockClientHolidayId,
                    ClientHolidayName = x.ClientHolidayName,
                    EventDate = x.EventDate,
                    IsPaid = x.IsPaid,
                    OverrideHours = x.OverrideHours,
                    OverrideClientEarningId = x.OverrideClientEarningId,
                    OverrideHolidayWorkedClientEarningId = x.OverrideHolidayWorkedClientEarningId
                })
                .FirstOrDefault());
        }

        IOpResult<IEnumerable<ClockClientHolidayDetailDto>> ILaborManagementProvider.GetClockClientHolidayDetailList(int clockClientHolidayId)
        {
            return new OpResult<IEnumerable<ClockClientHolidayDetailDto>>()
                .TrySetData(() => _session
                    .UnitOfWork
                    .LaborManagementRepository
                    .ClockClientHolidayDetailQuery()
                    .ByClockClientHolidayId(clockClientHolidayId)
                    .ExecuteQueryAs(x => new ClockClientHolidayDetailDto
                    {
                        ClockClientHolidayDetailId = x.ClockClientHolidayDetailId,
                        ClockClientHolidayId = x.ClockClientHolidayId,
                        ClientHolidayName = x.ClientHolidayName,
                        EventDate = x.EventDate,
                        IsPaid = x.IsPaid,
                        OverrideHours = x.OverrideHours,
                        OverrideClientEarningId = x.OverrideClientEarningId,
                        OverrideHolidayWorkedClientEarningId = x.OverrideHolidayWorkedClientEarningId
                    })
                    .ToList());
        }

        IOpResult<ClockClientExceptionDto> ILaborManagementProvider.RegisterNewClockClientException(
            ClockClientException entity, ClockClientExceptionDto dto)
        {
            var result = new OpResult<ClockClientExceptionDto>();
            _session.UnitOfWork.RegisterNew(entity);
            _session.UnitOfWork.RegisterPostCommitAction(() => dto.ClockClientExceptionId = entity.ClockClientExceptionId);
            return result;
        }

        IOpResult<ClockClientExceptionDto> ILaborManagementProvider.RegisterExistingClockClientException(
            ClockClientException entity, ClockClientExceptionDto dto)
        {
            var result = new OpResult<ClockClientExceptionDto>();
            var current = Self.GetClockClientException(dto.ClockClientExceptionId).MergeInto(result).Data;
            // if not found, bail out.
            if (result.CheckForNotFound(current).HasError) return result;
            if (current.ClientId != dto.ClientId)
            {
                _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, current.ClientId).MergeInto(result);
                if (result.Success)
                {
                    entity.ClientId = current.ClientId;
                    dto.ClientId = current.ClientId;
                }
            }



            if (result.Success)
            {
                entity.ClockClientExceptionId = dto.ClockClientExceptionId;
                if (result.Success)
                {
                    Self.RegisterModifiedClockClientException(entity, current);
                }
            }

            return result;
        }

        IOpResult<ClockClientHolidayDto> ILaborManagementProvider.RegisterExistingClockClientHoliday(
            ClockClientHoliday entity, ClockClientHolidayDto dto)
        {
            var result = new OpResult<ClockClientHolidayDto>();
            var curr = Self.GetClockClientHoliday(dto.ClockClientHolidayId).MergeInto(result).Data;

            if (result.CheckForNotFound(curr).HasError) return result;
            if (curr.ClientId != dto.ClientId)
            {
                _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, curr.ClientId).MergeInto(result);
                if (result.Success)
                {
                    entity.ClientId = curr.ClientId;
                    dto.ClientId = curr.ClientId;
                }
            }

            if (result.Success)
            {
                entity.ClockClientHolidayId = dto.ClockClientHolidayId;
                if (result.Success)
                {
                    var changed = new PropertyList<ClockClientHoliday>();
                    if (dto.ClientId != curr.ClientId)
                        changed.Include(c => c.ClientId);
                    if (dto.ClientEarningId != curr.ClientEarningId)
                        changed.Include(c => c.ClientEarningId);
                    if (dto.HolidayWaitingPeriodDateId != curr.HolidayWaitingPeriodDateId)
                        changed.Include(c => c.HolidayWaitingPeriodDateId);
                    if (dto.HolidayWorkedClientEarningId != curr.HolidayWorkedClientEarningId)
                        changed.Include(c => c.HolidayWorkedClientEarningId);
                    if (dto.Hours != null && dto.Hours != curr.Hours)
                        changed.Include(c => c.Hours);
                    if (dto.Name != curr.Name)
                        changed.Include(c => c.Name);
                    if (dto.WaitingPeriod != curr.WaitingPeriod)
                        changed.Include(c => c.WaitingPeriod);

                    if (changed.Any())
                        _session.UnitOfWork.RegisterModified(entity, changed);
                }
            }

            return result;
        }

        IOpResult<ClockClientRulesDto> ILaborManagementProvider.RegisterExistingClockClientRules(
            ClockClientRules entity, ClockClientRulesDto dto)
        {
            var result = new OpResult<ClockClientRulesDto>();
            var curr = Self.GetClockClientRules(dto.ClockClientRulesId).MergeInto(result).Data;

            if (result.CheckForNotFound(curr).HasError) return result;
            if (curr.ClientId != dto.ClientId)
            {
                _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, curr.ClientId).MergeInto(result);
                if (result.Success)
                {
                    entity.ClientId = curr.ClientId;
                    dto.ClientId = curr.ClientId;
                }
            }

            if (result.Success)
            {
                entity.ClockClientRulesId = dto.ClockClientRulesId;
                if (result.Success)
                {
                    var changed = new PropertyList<ClockClientRules>();
                    if (dto.AllowInputPunches != curr.AllowInputPunches)
                        changed.Include(c => c.AllowInputPunches);
                    if (dto.IsAllowMobilePunch != curr.IsAllowMobilePunch)
                        changed.Include(c => c.IsAllowMobilePunch);
                    if (dto.IsEditBenefits != curr.IsEditBenefits)
                        changed.Include(c => c.IsEditBenefits);
                    if (dto.IsEditPunches != curr.IsEditPunches)
                        changed.Include(c => c.IsEditPunches);
                    if (dto.IsHideCostCenter != curr.IsHideCostCenter)
                        changed.Include(c => c.IsHideCostCenter);
                    if (dto.IsHideDepartment != curr.IsHideDepartment)
                        changed.Include(c => c.IsHideDepartment);
                    if (dto.IsHideEmployeeNotes != curr.IsHideEmployeeNotes)
                        changed.Include(c => c.IsHideEmployeeNotes);
                    if (dto.IsHideJobCosting != curr.IsHideJobCosting)
                        changed.Include(c => c.IsHideJobCosting);
                    if (dto.IsHideMultipleSchedules != curr.IsHideMultipleSchedules)
                        changed.Include(c => c.IsHideMultipleSchedules);
                    if (dto.IsHidePunchType != curr.IsHidePunchType)
                        changed.Include(c => c.IsHidePunchType);
                    if (dto.IsHideShift != curr.IsHideShift)
                        changed.Include(c => c.IsHideShift);
                    if (dto.IsImportBenefits != curr.IsImportBenefits)
                        changed.Include(c => c.IsImportBenefits);
                    if (dto.IsImportPunches != curr.IsImportPunches)
                        changed.Include(c => c.IsImportPunches);
                    if (dto.IsIpLockout != curr.IsIpLockout)
                        changed.Include(c => c.IsIpLockout);
                    if (dto.AllPunchesClockRoundingTypeId != curr.AllPunchesClockRoundingTypeId)
                        changed.Include(c => c.AllPunchesClockRoundingTypeId);
                    if (dto.WeeklyStartingDayOfWeekId != curr.WeeklyStartingDayOfWeekId)
                        changed.Include(c => c.WeeklyStartingDayOfWeekId);
                    if (dto.BiWeeklyStartingDayOfWeekId != curr.BiWeeklyStartingDayOfWeekId)
                        changed.Include(c => c.BiWeeklyStartingDayOfWeekId);
                    if (dto.SemiMonthlyStartingDayOfWeekId != curr.SemiMonthlyStartingDayOfWeekId)
                        changed.Include(c => c.SemiMonthlyStartingDayOfWeekId);
                    if (dto.MonthlyStartingDayOfWeekId != curr.MonthlyStartingDayOfWeekId)
                        changed.Include(c => c.MonthlyStartingDayOfWeekId);
                    if (dto.StartTime != curr.StartTime)
                        changed.Include(c => c.StartTime);
                    if (dto.StopTime != curr.StopTime)
                        changed.Include(c => c.StopTime);
                    if (dto.InEarlyClockRoundingTypeId != curr.InEarlyClockRoundingTypeId)
                        changed.Include(c => c.InEarlyClockRoundingTypeId);
                    if (dto.OutEarlyClockRoundingTypeId != curr.OutEarlyClockRoundingTypeId)
                        changed.Include(c => c.OutEarlyClockRoundingTypeId);
                    if (dto.InLateClockRoundingTypeId != curr.InLateClockRoundingTypeId)
                        changed.Include(c => c.InLateClockRoundingTypeId);
                    if (dto.OutLateClockRoundingTypeId != curr.OutLateClockRoundingTypeId)
                        changed.Include(c => c.OutLateClockRoundingTypeId);
                    if (dto.ClockTimeFormatId != curr.ClockTimeFormatId)
                        changed.Include(c => c.ClockTimeFormatId);
                    if (dto.InEarlyGraceTime != curr.InEarlyGraceTime)
                        changed.Include(c => c.InEarlyGraceTime);
                    if (dto.OutEarlyGraceTime != curr.OutEarlyGraceTime)
                        changed.Include(c => c.OutEarlyGraceTime);
                    if (dto.InLateGraceTime != curr.InLateGraceTime)
                        changed.Include(c => c.InLateGraceTime);
                    if (dto.OutLateGraceTime != curr.OutLateGraceTime)
                        changed.Include(c => c.OutLateGraceTime);
                    if (dto.ShiftInterval != curr.ShiftInterval)
                        changed.Include(c => c.ShiftInterval);
                    if (dto.MaxShift != curr.MaxShift)
                        changed.Include(c => c.MaxShift);
                    if (dto.PunchOption != curr.PunchOption)
                        changed.Include(c => c.PunchOption);
                    if (dto.InEarlyAllowPunchTime != curr.InEarlyAllowPunchTime)
                        changed.Include(c => c.InEarlyAllowPunchTime);
                    if (dto.OutEarlyAllowPunchTime != curr.OutEarlyAllowPunchTime)
                        changed.Include(c => c.OutEarlyAllowPunchTime);
                    if (dto.InLateAllowPunchTime != curr.InLateAllowPunchTime)
                        changed.Include(c => c.InLateAllowPunchTime);
                    if (dto.OutLateAllowPunchTime != curr.OutLateAllowPunchTime)
                        changed.Include(c => c.OutLateAllowPunchTime);
                    if (dto.AllPunchesClockRoundingTypeId != curr.AllPunchesClockRoundingTypeId)
                        changed.Include(c => c.AllPunchesClockRoundingTypeId);
                    if (dto.AllPunchesGraceTime != curr.AllPunchesGraceTime)
                        changed.Include(c => c.AllPunchesGraceTime);
                    if (dto.ApplyHoursOption != curr.ApplyHoursOption)
                        changed.Include(c => c.ApplyHoursOption);
                    if (dto.ClockAllocateHoursFrequencyId != curr.ClockAllocateHoursFrequencyId)
                        changed.Include(c => c.ClockAllocateHoursFrequencyId);
                    if (dto.ClockAllocateHoursOptionId != curr.ClockAllocateHoursOptionId)
                        changed.Include(c => c.ClockAllocateHoursOptionId);
                    if (dto.InEarlyOutsideGraceTimeClockRoundingTypeId != curr.InEarlyOutsideGraceTimeClockRoundingTypeId)
                        changed.Include(c => c.InEarly_OutsideGraceTimeClockRoundingTypeId);
                    if (dto.InLateOutsideGraceTimeClockRoundingTypeId != curr.InLateOutsideGraceTimeClockRoundingTypeId)
                        changed.Include(c => c.InLate_OutsideGraceTimeClockRoundingTypeId);
                    if (dto.OutEarlyOutsideGraceTimeClockRoundingTypeId != curr.OutEarlyOutsideGraceTimeClockRoundingTypeId)
                        changed.Include(c => c.OutEarly_OutsideGraceTimeClockRoundingTypeId);
                    if (dto.OutLateOutsideGraceTimeClockRoundingTypeId != curr.OutLateOutsideGraceTimeClockRoundingTypeId)
                        changed.Include(c => c.OutLate_OutsideGraceTimeClockRoundingTypeId);

                    if (changed.Any())
                        _session.UnitOfWork.RegisterModified(entity, changed);
                }
            }

            return result;
        }

        IOpResult<ClockClientHolidayDetailDto> ILaborManagementProvider.RegisterExistingClockClientHolidayDetail(
            ClockClientHolidayDetail entity, ClockClientHolidayDetailDto dto)
        {
            var result = new OpResult<ClockClientHolidayDetailDto>();
            var curr = Self.GetClockClientHolidayDetail(dto.ClockClientHolidayDetailId).MergeInto(result).Data;

            if (result.HasError || curr == null) return result;

            entity.ClockClientHolidayDetailId = dto.ClockClientHolidayDetailId;

            var changed = new PropertyList<ClockClientHolidayDetail>();

            if (dto.ClockClientHolidayId != curr.ClockClientHolidayId)
                changed.Include(c => c.ClockClientHolidayId);
            if (dto.ClientHolidayName != curr.ClientHolidayName)
                changed.Include(c => c.ClientHolidayName);
            if (dto.OverrideHours != curr.OverrideHours)
                changed.Include(c => c.OverrideHours);
            if (dto.OverrideClientEarningId != curr.OverrideClientEarningId)
                changed.Include(c => c.OverrideClientEarningId);
            if (dto.OverrideHolidayWorkedClientEarningId != curr.OverrideHolidayWorkedClientEarningId)
                changed.Include(c => c.OverrideHolidayWorkedClientEarningId);
            if (dto.EventDate != curr.EventDate)
                changed.Include(c => c.EventDate);
            if (dto.IsPaid != curr.IsPaid)
                changed.Include(c => c.IsPaid);

            if (changed.Any())
                _session.UnitOfWork.RegisterModified(entity, changed);

            return result;
        }

        IOpResult<ClockClientExceptionDetailDto> ILaborManagementProvider.RegisterExistingClockClientExceptionDetail(
            ClockClientExceptionDetail entity, ClockClientExceptionDetailDto dto)
        {
            var result = new OpResult<ClockClientExceptionDetailDto>();
            var current = Self.GetClockClientExceptionDetail(dto.ClockClientExceptionDetailId).MergeInto(result).Data;
            // if not found, bail out
            if (result.CheckForNotFound(current).HasError) return result;
            if (result.Success)
            {
                entity.ClockClientExceptionDetailId = dto.ClockClientExceptionDetailId;
                if (result.Success)
                {
                    Self.RegisterModifiedClockClientExceptionDetail(entity, current);
                }
            }

            return result;
        }

        void ILaborManagementProvider.RegisterModifiedClockClientNote(ClockClientNote entity,
            ClockClientNoteDto current)
        {
            var changed = new PropertyList<ClockClientNote>();
            if (current.Note != entity.Note)
                changed.Include(e => e.Note);
            if (current.IsHideFromEmployee != entity.IsHideFromEmployee)
                changed.Include(e => e.IsHideFromEmployee);
            if (current.IsActive != entity.IsActive)
                changed.Include(e => e.IsActive);

            // check if anything changed
            if (changed.Any())
            {
                _session.UnitOfWork.RegisterModified(entity, changed);
            }
        }

        void ILaborManagementProvider.RegisterModifiedClockClientOvertime(ClockClientOvertime entity,
            ClockClientOvertimeDto current)
        {
            var changed = new PropertyList<ClockClientOvertime>();
            if (current.ClientId != entity.ClientId)
                changed.Include(e => e.ClientId);
            if (current.ClientEarningId != entity.ClientEarningId)
                changed.Include(e => e.ClientEarningId);
            if (current.ClockOvertimeFrequencyId != entity.ClockOvertimeFrequencyId)
                changed.Include(e => e.ClockOvertimeFrequencyId);
            if (current.Name != entity.Name)
                changed.Include(e => e.Name);
            if (!current.Hours.Equals(entity.Hours))
                changed.Include(e => e.Hours);
            if (current.IsSunday != entity.IsSunday)
                changed.Include(e => e.IsSunday);
            if (current.IsMonday != entity.IsMonday)
                changed.Include(e => e.IsMonday);
            if (current.IsTuesday != entity.IsTuesday)
                changed.Include(e => e.IsTuesday);
            if (current.IsWednesday != entity.IsWednesday)
                changed.Include(e => e.IsWednesday);
            if (current.IsThursday != entity.IsThursday)
                changed.Include(e => e.IsThursday);
            if (current.IsFriday != entity.IsFriday)
                changed.Include(e => e.IsFriday);
            if (current.IsSaturday != entity.IsSaturday)
                changed.Include(e => e.IsSaturday);

            if (changed.Any())
            {
                _session.UnitOfWork.RegisterModified(entity, changed);
            }
        }

        void ILaborManagementProvider.RegisterModifiedClockClientException(ClockClientException entity,
            ClockClientExceptionDto current)
        {
            var changed = new PropertyList<ClockClientException>();
            if (current.ClientId != entity.ClientId)
                changed.Include(e => e.ClientId);
            if (current.Name != entity.Name)
                changed.Include(e => e.Name);

            if (changed.Any())
                _session.UnitOfWork.RegisterModified(entity, changed);
        }

        IOpResult<IEnumerable<ClientExceptionDetailDto>> ILaborManagementProvider.GetStandardExceptionsAsExceptionDetail()
        {
            return new OpResult<IEnumerable<ClientExceptionDetailDto>>().TrySetData(_session.UnitOfWork.LaborManagementRepository
                .ClockExceptionQuery()
                .ExecuteQueryAs(new ClockExceptionMaps.ToClientExceptionDetailDto())
                .ToList);
        }

        IOpResult<IEnumerable<ClockClientExceptionDetailDto>> ILaborManagementProvider.GetClientExceptionDetailByClientException(int clockClientExceptionId)
        {
            return new OpResult<IEnumerable<ClockClientExceptionDetailDto>>().TrySetData(_session.UnitOfWork.LaborManagementRepository
                .ClockClientExceptionDetailQuery()
                .ByClockClientException(clockClientExceptionId)
                .ExecuteQueryAs(new ClockClientExceptionDetailMaps.ToClockClientExceptionDetailDto())
                .ToList);
        }

        void ILaborManagementProvider.RegisterModifiedClockClientExceptionDetail(
            ClockClientExceptionDetail entity, ClockClientExceptionDetailDto current)
        {
            var changed = new PropertyList<ClockClientExceptionDetail>();
            if (current.ClockClientExceptionId != entity.ClockClientExceptionId)
                changed.Include(e => e.ClockClientExceptionId);
            if (current.ClockExceptionId != entity.ClockExceptionId)
                changed.Include(e => e.ClockExceptionId);
            if (current.Amount != entity.Amount)
                changed.Include(e => e.Amount);
            if (current.IsSelected != entity.IsSelected)
                changed.Include(e => e.IsSelected);
            if (current.ClockClientLunchId != entity.ClockClientLunchId)
                changed.Include(e => e.ClockClientLunchId);
            if (current.PunchTimeOption != entity.PunchTimeOption)
                changed.Include(e => e.PunchTimeOption);

            if (changed.Any())
                _session.UnitOfWork.RegisterModified(entity, changed);
        }

        IOpResult<ClockClientExceptionDetailDto> ILaborManagementProvider.RegisterNewClockClientExceptionDetail(
            ClockClientExceptionDetail entity, ClockClientExceptionDetailDto dto)
        {
            var result = new OpResult<ClockClientExceptionDetailDto>();
            _session.UnitOfWork.RegisterNew(entity);
            _session.UnitOfWork.RegisterPostCommitAction(() => dto.ClockClientExceptionDetailId = entity.ClockClientExceptionDetailId);
            return result;
        }

        IOpResult<ClockClientExceptionDetailDto> ILaborManagementProvider.GetClockClientExceptionDetail(
            int clockClientExceptionDetailId)
        {
            return new OpResult<ClockClientExceptionDetailDto>().TrySetData(_session.UnitOfWork.LaborManagementRepository.ClockClientExceptionDetailQuery()
                .ByClockClientExceptionDetail(clockClientExceptionDetailId)
                .ExecuteQueryAs(new ClockClientExceptionDetailMaps.ToClockClientExceptionDetailDto()).FirstOrDefault);
        }


        /// <inheritdoc />
        IOpResult<IEnumerable<HolidayDateDto>> ILaborManagementProvider.GetAvailableHolidayYears(int? startYear)
        {
            var result = new OpResult<IEnumerable<HolidayDateDto>>();
            var year = startYear ?? DateTime.Now.Year;

            result.TrySetData(() => _session.UnitOfWork.LaborManagementRepository
                .HolidayDateQuery()
                .ByYearsGreaterObserved(year)
                .ExecuteQueryAs(x => new HolidayDateDto()
                {
                    HolidayId = x.HolidayId,
                    HolidayDateId = x.HolidayDateId,
                    DateObserved = x.DateObserved,
                    Holiday = x.Holiday != null ? new HolidayDto
                    {
                        HolidayId = x.Holiday.HolidayId,
                        HolidayName = x.Holiday.HolidayName
                    } : default(HolidayDto)
                })
                .ToList());

            return result;
        }

        IOpResult<ClockClientHolidayChangeHistoryDto> ILaborManagementProvider.InsertClockClientHolidayChangeHistory(ClockClientHolidayDto hol, ChangeModeType changeMode)
        {
            var result = new OpResult<ClockClientHolidayChangeHistoryDto>();
            var dto = new ClockClientHolidayChangeHistoryDto
            {
                ClockClientHolidayId = hol.ClockClientHolidayId,
                ClientId = hol.ClientId,
                Name = hol.Name,
                ClientEarningId = hol.ClientEarningId,
                WaitingPeriod = hol.WaitingPeriod,
                HolidayWorkedClientEarningId = hol.HolidayWorkedClientEarningId,
                HolidayWaitingPeriodDateId = hol.HolidayWaitingPeriodDateId,
                Hours = hol.Hours,
                ChangeId = CommonConstants.NEW_ENTITY_ID
            };

            var entity = new ClockClientHolidayChangeHistory
            {
                ChangeId = dto.ChangeId,
                ClockClientHolidayId = dto.ClockClientHolidayId,
                ClientId = dto.ClientId,
                Name = dto.Name,
                ClientEarningId = dto.ClientEarningId,
                Hours = dto.Hours,
                HolidayWaitingPeriodDateId = dto.HolidayWaitingPeriodDateId,
                HolidayWorkedClientEarningId = dto.HolidayWorkedClientEarningId,
                WaitingPeriod = dto.WaitingPeriod
            };

            _session.SetChangeMode(entity, changeMode);
            _session.SetModifiedProperties(entity);
            _session.UnitOfWork.RegisterNew(entity);

            _session.UnitOfWork.RegisterPostCommitAction(() => { dto.ChangeId = entity.ChangeId; });
            _session.UnitOfWork.Commit().MergeInto(result);
            result.SetDataOnSuccess(dto);

            return result;
        }

        IOpResult ILaborManagementProvider.DeleteClockEmployeeBenefit(ClockEmployeeBenefitDto dto)
        {
            var result = new OpResult();

            var existing = _session.UnitOfWork.LaborManagementRepository
                .ClockEmployeeBenefitQuery()
                .ByClockEmployeeBenefit(dto.ClockEmployeeBenefitId)
                .ExecuteQueryAs(new ClockEmployeeBenefitMaps.ToClockEmployeeBenefitDto())
                .FirstOrDefault();

            if (existing == null)
                return result;

            var delete = new ClockEmployeeBenefit
            {
                ClockEmployeeBenefitId = existing.ClockEmployeeBenefitId,
                ClientId = existing.ClientId,
                EmployeeId = existing.EmployeeId
            };

            _session.UnitOfWork.RegisterDeleted(delete);

            return result;
        }

        void ILaborManagementProvider.DeleteClockEmployeeBenefits(IEnumerable<int> clockEmployeeBenefitIdList, int clientId)
        {
            foreach (var id in clockEmployeeBenefitIdList)
            {
                _session.UnitOfWork.RegisterDeleted(new ClockEmployeeBenefit
                {
                    ClientId = clientId,
                    ClockEmployeeBenefitId = id
                });
            }
        }

        IOpResult<ClockEmployeeBenefit> ILaborManagementProvider.SaveClockEmployeeBenefit(ClockEmployeeBenefitDto dto, bool holdCommit)
        {
            var result = new OpResult<ClockEmployeeBenefit>();
            var entity = new ClockEmployeeBenefit
            {
                ClockEmployeeBenefitId = CommonConstants.NEW_ENTITY_ID,
                ClockClientHolidayDetailId = dto.ClockClientHolidayDetailId,
                ClientId = dto.ClientId,
                ClientEarningId = Convert.ToInt32(dto.ClientEarningId),
                EventDate = dto.EventDate,
                EmployeeId = dto.EmployeeId,
                Hours = dto.Hours,
                IsApproved = false,
                IsWorkedHours = dto.IsWorkedHours,
                Comment = "Auto Generated Holiday"
            };

            _session.SetModifiedProperties(entity);

            _session.UnitOfWork.RegisterNew(entity);
            _session.UnitOfWork.RegisterPostCommitAction(() => { dto.ClockEmployeeBenefitId = entity.ClockEmployeeBenefitId; });

            if (!holdCommit) _session.UnitOfWork.Commit().MergeInto(result);

            result.SetDataOnSuccess(entity);

            return result;
        }

        IOpResult ILaborManagementProvider.RegisterExistingClockClientDailyRule(
            ClockClientDailyRules entity, ClockClientDailyRulesDto dto)
        {
            var result = new OpResult();

            var curr = _session.UnitOfWork.LaborManagementRepository
                .ClockClientDailyRulesQuery()
                .ByClockClientDailyRule(dto.ClockClientDailyRulesId)
                .ExecuteQueryAs(x => new ClockClientDailyRulesDto
                {
                    ClockClientDailyRulesId = x.ClockClientDailyRulesId,
                    ClockClientRulesId = x.ClockClientRulesId,
                    ClientEarningId = x.ClientEarningId,
                    DayOfWeekId = x.DayOfWeekId,
                    IsApplyOnlyIfMinHoursMetPrior = x.IsApplyOnlyIfMinHoursMetPrior,
                    MinHoursWorked = x.MinHoursWorked
                })
                .FirstOrDefault();

            if (curr != null)
            {
                var changed = new PropertyList<ClockClientDailyRules>();
                entity.ClockClientDailyRulesId = curr.ClockClientDailyRulesId;

                if (dto.ClientEarningId != curr.ClientEarningId)
                    changed.Include(c => c.ClientEarningId);
                if (dto.DayOfWeekId != curr.DayOfWeekId)
                    changed.Include(c => c.DayOfWeekId);
                if (dto.IsApplyOnlyIfMinHoursMetPrior != curr.IsApplyOnlyIfMinHoursMetPrior)
                    changed.Include(c => c.IsApplyOnlyIfMinHoursMetPrior);
                if (dto.MinHoursWorked != curr.MinHoursWorked)
                    changed.Include(c => c.MinHoursWorked);

                if (changed.Any()) changed.IncludeModifiedProperties();

                _session.UnitOfWork.RegisterModified(entity, changed);

                result.SetToSuccess();
            }
            else
            {
                result.SetToFail();
            }

            return result;
        }

        IOpResult<List<ClientCostCenterDto>> ILaborManagementProvider.GetClientCostCenterListByClient(int clientId)
        {
            var result = new OpResult<List<ClientCostCenterDto>>();

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(result);
            _session.CanPerformAction(LaborManagementActionType.ReadTimePolicy).MergeInto(result);

            if (result.HasError) return result;

            return result.TrySetData(() => _session.UnitOfWork.LaborManagementRepository
                .ClientCostCenterQuery()
                .ByClientId(clientId)
                .ExecuteQueryAs(new ClientCostCenterMaps.DefaultClientCostCenterMap())
                .ToList());
        }

        IOpResult<List<ClockRoundingTypeDto>> ILaborManagementProvider.GetClockRoundingTypeList()
        {
            var result = new OpResult<List<ClockRoundingTypeDto>>();

            _session.CanPerformAction(LaborManagementActionType.ReadTimePolicy).MergeInto(result);

            if (result.HasError) return result;

            return result.TrySetData(() => _session.UnitOfWork.TimeClockRepository
                .ClockRoundingTypeQuery()
                .ExecuteQueryAs(x => new ClockRoundingTypeDto
                {
                    ClockRoundingTypeId = x.ClockRoundingTypeId,
                    Description = x.Description,
                    RoundDirection = x.RoundDirection,
                    Minutes = x.Minutes
                })
                .ToList());
        }

        IOpResult<ClockClientLunchPaidOptionDto> ILaborManagementProvider.RegisterNewClockClientLunchPaidOption(ClockClientLunchPaidOptionDto dto)
        {
            var result = new OpResult<ClockClientLunchPaidOptionDto>(dto);

            if (dto.FromMinutes == null || dto.ToMinutes == null)
            {
                result.AddExceptionMessage(new Exception("From or To lunch paid option must be specified.")).SetToFail();
                return result;
            }

            var entity = new ClockClientLunchPaidOption
            {
                ClockClientLunchPaidOptionId = CommonConstants.NEW_ENTITY_ID,
                ClientId = dto.ClientId,
                ClockClientLunchId = dto.ClockClientLunchId,
                ClockClientLunchPaidOptionRulesId = dto.ClockClientLunchPaidOptionRulesId,
                FromMinutes = dto.FromMinutes,
                ToMinutes = dto.ToMinutes,
                OverrideMinutes = dto.OverrideMinutes ?? 0
            };

            _session.SetModifiedProperties(entity);
            _session.UnitOfWork.RegisterNew(entity);
            _session.UnitOfWork.RegisterPostCommitAction(() => result.Data.ClockClientLunchPaidOptionId = entity.ClockClientLunchPaidOptionId);
            _session.UnitOfWork.RegisterPostCommitAction(() => dto.OverrideMinutes = entity.OverrideMinutes);

            return result;
        }

        IOpResult<ClockClientLunchPaidOptionDto> ILaborManagementProvider.RegisterExistingClockClientLunchPaidOption(
            ClockClientLunchPaidOptionDto dto)
        {
            var result = new OpResult<ClockClientLunchPaidOptionDto>(dto);

            if (dto.FromMinutes == null || dto.ToMinutes == null)
            {
                result.AddExceptionMessage(new Exception("From minutes and To minutes must be specified for lunch paid options.")).SetToFail();
                return result;
            }

            var entity = new ClockClientLunchPaidOption
            {
                ClockClientLunchPaidOptionId = dto.ClockClientLunchPaidOptionId,
                ClockClientLunchPaidOptionRulesId = dto.ClockClientLunchPaidOptionRulesId,
                ClientId = dto.ClientId,
                ClockClientLunchId = dto.ClockClientLunchId,
                FromMinutes = dto.FromMinutes,
                ToMinutes = dto.ToMinutes,
                OverrideMinutes = dto.OverrideMinutes ?? 0
            };

            var curr = _session.UnitOfWork.LaborManagementRepository
                .ClockClientLunchPaidOptionQuery()
                .ByClockClientLunchPaidOptionId(dto.ClockClientLunchPaidOptionId)
                .ExecuteQueryAs(x => new ClockClientLunchPaidOptionDto
                {
                    ClockClientLunchPaidOptionId = x.ClockClientLunchPaidOptionId,
                    ClockClientLunchPaidOptionRulesId = x.ClockClientLunchPaidOptionRulesId,
                    ClientId = x.ClientId,
                    ClockClientLunchId = x.ClockClientLunchId,
                    FromMinutes = x.FromMinutes,
                    ToMinutes = x.ToMinutes,
                    OverrideMinutes = x.OverrideMinutes
                })
                .FirstOrDefault();

            if (curr == null)
            {
                result.SetToFail();
                return result;
            }

            _session.SetModifiedProperties(entity);

            var changed = new PropertyList<ClockClientLunchPaidOption>();
            entity.ClockClientLunchPaidOptionId = curr.ClockClientLunchPaidOptionId;

            if (dto.ClockClientLunchId != curr.ClockClientLunchId)
                changed.Include(c => c.ClockClientLunchId);
            if (dto.FromMinutes != curr.FromMinutes)
                changed.Include(c => c.FromMinutes);
            if (dto.ToMinutes != curr.ToMinutes)
                changed.Include(c => c.ToMinutes);
            if (dto.OverrideMinutes != curr.OverrideMinutes)
                changed.Include(c => c.OverrideMinutes);

            if (!changed.Any()) return result;

            changed.IncludeModifiedOptionalProperties();
            _session.UnitOfWork.RegisterModified(entity, changed);
            _session.UnitOfWork.RegisterPostCommitAction(() => dto.OverrideMinutes = entity.OverrideMinutes);

            return result;
        }

        IOpResult<ClockClientTimePolicyDtos.ClockClientTimePolicyDto> ILaborManagementProvider.
            RegisterNewClockClientTimePolicy(ClockClientTimePolicyDtos.ClockClientTimePolicyDto dto)
        {
            var result = new OpResult<ClockClientTimePolicyDtos.ClockClientTimePolicyDto>(dto);
            var entity = new ClockClientTimePolicy
            {
                ClockClientTimePolicyId = CommonConstants.NEW_ENTITY_ID,
                ClientId = dto.ClientId,
                Name = dto.Name,
                ClockClientExceptionId = dto.ClockClientExceptionId,
                ClockClientHolidayId = dto.ClockClientHolidayId,
                ClockClientRulesId = dto.ClockClientRulesId,
                ClientDepartmentId = dto.ClientDepartmentId,
                ClientShiftId = dto.ClientShiftId,
                ClientStatusId = dto.ClientStatusId,
                HasCombinedOtFrequencies = dto.HasCombinedOtFrequencies,
                IsAddToOtherPolicy = dto.IsAddToOtherPolicy,
                PayType = dto.PayType,
                TimeZoneId = dto.TimeZoneId,
                GeofenceEnabled = dto.GeofenceEnabled,
            };

            _session.SetModifiedProperties(entity);
            _session.UnitOfWork.RegisterNew(entity);
            _session.UnitOfWork.RegisterPostCommitAction(() => result.Data.ClockClientTimePolicyId = entity.ClockClientTimePolicyId);

            return result;
        }

        IOpResult<ClockClientTimePolicyDtos.ClockClientTimePolicyDto> ILaborManagementProvider.
            RegisterExistingClockClientTimePolicy(ClockClientTimePolicyDtos.ClockClientTimePolicyDto dto)
        {
            var result = new OpResult<ClockClientTimePolicyDtos.ClockClientTimePolicyDto>(dto);
            var entity = new ClockClientTimePolicy
            {
                ClockClientTimePolicyId = CommonConstants.NEW_ENTITY_ID,
                ClientId = dto.ClientId,
                Name = dto.Name,
                ClockClientExceptionId = dto.ClockClientExceptionId,
                ClockClientHolidayId = dto.ClockClientHolidayId,
                ClockClientRulesId = dto.ClockClientRulesId,
                ClientDepartmentId = dto.ClientDepartmentId,
                ClientShiftId = dto.ClientShiftId,
                ClientStatusId = dto.ClientStatusId,
                HasCombinedOtFrequencies = dto.HasCombinedOtFrequencies,
                IsAddToOtherPolicy = dto.IsAddToOtherPolicy,
                PayType = dto.PayType,
                TimeZoneId = dto.TimeZoneId,
                AutoPointsEnabled = dto.AutoPointsEnabled,
                ShowTCARatesEnabled = dto.ShowTCARatesEnabled,
                GeofenceEnabled = dto.GeofenceEnabled,
            };

            var curr = _session.UnitOfWork.LaborManagementRepository
                .ClockClientTimePolicyQuery()
                .ByClockClientTimePolicyId(dto.ClockClientTimePolicyId)
                .ExecuteQueryAs(new ClockClientTimePolicyMaps.ToClockClientTimePolicyDto())
                .FirstOrDefault();

            _session.SetModifiedProperties(entity);

            if (curr != null)
            {
                var changed = new PropertyList<ClockClientTimePolicy>();
                entity.ClockClientTimePolicyId = curr.ClockClientTimePolicyId;

                if (dto.ClientId != curr.ClientId)
                {
                    dto.ClientId = curr.ClientId;
                    _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, dto.ClientId).MergeInto(result);
                    if (result.HasError) return result;
                }

                var isRemovingHolidayRule = false;
                var isSwitchingHolidayRule = false;
                var isAddingHolidayRule = false;

                if (dto.Name != curr.Name)
                    changed.Include(c => c.Name);
                if (dto.ClockClientExceptionId != curr.ClockClientExceptionId)
                    changed.Include(c => c.ClockClientExceptionId);
                if (dto.ClockClientHolidayId != curr.ClockClientHolidayId)
                {
                    changed.Include(c => c.ClockClientHolidayId);

                    isRemovingHolidayRule = curr.ClockClientHolidayId.HasValue && !dto.ClockClientHolidayId.HasValue;
                    isAddingHolidayRule   = !curr.ClockClientHolidayId.HasValue && dto.ClockClientHolidayId.HasValue;
                    isSwitchingHolidayRule = curr.ClockClientHolidayId.HasValue && dto.ClockClientHolidayId.HasValue;
                }
                if (dto.ClockClientRulesId != curr.ClockClientRulesId)
                    changed.Include(c => c.ClockClientRulesId);
                if (dto.ClientDepartmentId != curr.ClientDepartmentId)
                    changed.Include(c => c.ClientDepartmentId);
                if (dto.ClientShiftId != curr.ClientShiftId)
                    changed.Include(c => c.ClientShiftId);
                if (dto.ClientStatusId != curr.ClientStatusId)
                    changed.Include(c => c.ClientStatusId);
                if (dto.HasCombinedOtFrequencies != curr.HasCombinedOtFrequencies)
                    changed.Include(c => c.HasCombinedOtFrequencies);
                if (dto.IsAddToOtherPolicy != curr.IsAddToOtherPolicy)
                    changed.Include(c => c.IsAddToOtherPolicy);
                if (dto.PayType != curr.PayType)
                    changed.Include(c => c.PayType);
                if (dto.TimeZoneId != curr.TimeZoneId)
                    changed.Include(c => c.TimeZoneId);
                if (dto.GeofenceEnabled != curr.GeofenceEnabled)
                    changed.Include(c => c.GeofenceEnabled);

                if (changed.Any()) changed.IncludeModifiedProperties();

                _session.UnitOfWork.RegisterModified(entity, changed);

                // If switching or removing a holiday rule, delete ClockEmployeeBenefit records for any future holidays that are tied to the old holiday rule.
                if (isSwitchingHolidayRule || isRemovingHolidayRule)
                    Self.DeleteFutureHolidayHours(curr.ClockClientTimePolicyId, curr.ClockClientHolidayId.Value,
                        DateTime.Now).MergeInto(result);

                // If switching or adding a holiday rule, insert ClockEmployeeBenefit records for any future holidays that are tied to the new holiday rule.
                if (isSwitchingHolidayRule || isAddingHolidayRule)
                {
                    Self.InsertFutureHolidayHours(dto.ClockClientTimePolicyId, dto.ClockClientHolidayId.Value, DateTime.Now).MergeInto(result);
                }
            }

            return result.SetDataOnSuccess(dto);
        }

        //        IOpResult ILaborManagementProvider.RegisterExistingClockClientLunch(ClockClientLunchDto curr)
        //        {
        //            var result = new OpResult();
        //            var entity = new ClockClientLunch
        //            {
        //                ClockClientLunchId = curr.ClockClientLunchId,
        //                ClientId = curr.ClientId,
        //                AllPunchesClockRoundingTypeId = curr.AllPunchesClockRoundingTypeId,
        //                AllPunchesGraceTime = curr.AllPunchesGraceTime,
        //                AutoDeductedWorkedHours = curr.AutoDeductedWorkedHours,
        //                ClientCostCenterId = curr.ClientCostCenterId,
        //                GraceTime = curr.GraceTime,
        //                Name = curr.Name,
        //                StopTime = curr.StopTime,
        //                InLateClockRoundingTypeId = curr.InLateClockRoundingTypeId,
        //                InEarlyClockRoundingTypeId = curr.InEarlyClockRoundingTypeId,
        //                InEarlyGraceTime = curr.InEarlyGraceTime,
        //                InLateGraceTime = curr.InLateGraceTime,
        //                IsAllowMultipleTimePeriods = curr.IsAllowMultipleTimePeriods,
        //                IsAutoDeducted = curr.IsAutoDeducted,
        //                IsDoEmployeesPunch = curr.IsDoEmployeesPunch,
        //                IsMonday = curr.IsMonday,
        //                IsTuesday = curr.IsTuesday,
        //                IsWednesday = curr.IsWednesday,
        //                IsThursday = curr.IsThursday,
        //                IsFriday = curr.IsFriday,
        //                IsSaturday = curr.IsSaturday,
        //                IsSunday = curr.IsSunday,
        //                IsShowPunches = curr.IsShowPunches,
        //                IsUseStartStopTimes = curr.IsUseStartStopTimes,
        //                Length = curr.Length,
        //                OutEarlyClockRoundingTypeId = curr.OutEarlyClockRoundingTypeId,
        //                OutLateGraceTime = curr.OutLateGraceTime,
        //                OutLateClockRoundingTypeId = curr.OutLateClockRoundingTypeId,
        //                OutEarlyGraceTime = curr.OutEarlyGraceTime,
        //                PunchType = curr.PunchType,
        //                StartTime = curr.StartTime,
        //                IsPaid = curr.IsPaid,
        //                IsMaxPaid = curr.IsMaxPaid
        //            };
        //            var changed = new PropertyList<ClockClientLunch>();
        //
        //            entity.ClockClientLunchId = curr.ClockClientLunchId;
        //
        //            if (entity.ClientId != curr.ClientId)
        //                entity.ClientId = curr.ClientId;
        //            if (entity.Name != curr.Name)
        //                changed.Include(c => c.Name);
        //            if (entity.Length != curr.Length)
        //                changed.Include(c => c.Length);
        //            if (entity.IsPaid != curr.IsPaid)
        //                changed.Include(c => c.IsPaid);
        //            if (entity.IsDoEmployeesPunch != curr.IsDoEmployeesPunch)
        //                changed.Include(c => c.IsDoEmployeesPunch);
        //            if (entity.IsAutoDeducted != curr.IsAutoDeducted)
        //                changed.Include(c => c.IsAutoDeducted);
        //            if (entity.AutoDeductedWorkedHours != curr.AutoDeductedWorkedHours)
        //                changed.Include(c => c.AutoDeductedWorkedHours);
        //            if (entity.GraceTime != curr.GraceTime)
        //                changed.Include(c => c.GraceTime);
        //            if (entity.StartTime != curr.StartTime)
        //                changed.Include(c => c.StartTime);
        //            if (entity.StopTime != curr.StopTime)
        //                changed.Include(c => c.StopTime);
        //            if (entity.ClientCostCenterId != curr.ClientCostCenterId)
        //                changed.Include(c => c.ClientCostCenterId);
        //            if (entity.PunchType != curr.PunchType)
        //                changed.Include(c => c.PunchType);
        //            if (entity.IsShowPunches != curr.IsShowPunches)
        //                changed.Include(c => c.IsShowPunches);
        //            if (entity.IsMaxPaid != curr.IsMaxPaid)
        //                changed.Include(c => c.IsMaxPaid);
        //            if (entity.InEarlyClockRoundingTypeId != curr.InEarlyClockRoundingTypeId)
        //                changed.Include(c => c.InEarlyClockRoundingTypeId);
        //            if (entity.OutEarlyClockRoundingTypeId != curr.OutEarlyClockRoundingTypeId)
        //                changed.Include(c => c.OutEarlyClockRoundingTypeId);
        //            if (entity.InLateClockRoundingTypeId != curr.InLateClockRoundingTypeId)
        //                changed.Include(c => c.InLateClockRoundingTypeId);
        //            if (entity.OutLateClockRoundingTypeId != curr.OutLateClockRoundingTypeId)
        //                changed.Include(c => c.OutLateClockRoundingTypeId);
        //            if (entity.InEarlyGraceTime != curr.InEarlyGraceTime)
        //                changed.Include(c => c.InEarlyGraceTime);
        //            if (entity.OutEarlyGraceTime != curr.OutEarlyGraceTime)
        //                changed.Include(c => c.OutEarlyGraceTime);
        //            if (entity.InLateGraceTime != curr.InLateGraceTime)
        //                changed.Include(c => c.InLateGraceTime);
        //            if (entity.OutLateGraceTime != curr.OutLateGraceTime)
        //                changed.Include(c => c.OutLateGraceTime);
        //            if (entity.AllPunchesClockRoundingTypeId != curr.AllPunchesClockRoundingTypeId)
        //                changed.Include(c => c.AllPunchesClockRoundingTypeId);
        //            if (entity.AllPunchesGraceTime != curr.AllPunchesGraceTime)
        //                changed.Include(c => c.AllPunchesGraceTime);
        //            if (entity.IsSunday != curr.IsSunday)
        //                changed.Include(c => c.IsSunday);
        //            if (entity.IsMonday != curr.IsMonday)
        //                changed.Include(c => c.IsMonday);
        //            if (entity.IsTuesday != curr.IsTuesday)
        //                changed.Include(c => c.IsTuesday);
        //            if (entity.IsWednesday != curr.IsWednesday)
        //                changed.Include(c => c.IsWednesday);
        //            if (entity.IsThursday != curr.IsThursday)
        //                changed.Include(c => c.IsThursday);
        //            if (entity.IsFriday != curr.IsFriday)
        //                changed.Include(c => c.IsFriday);
        //            if (entity.IsSaturday != curr.IsSaturday)
        //                changed.Include(c => c.IsSaturday);
        //            if (entity.IsUseStartStopTimes != curr.IsUseStartStopTimes)
        //                changed.Include(c => c.IsUseStartStopTimes);
        //            if (entity.IsAllowMultipleTimePeriods != curr.IsAllowMultipleTimePeriods)
        //                changed.Include(c => c.IsAllowMultipleTimePeriods);
        //
        //            if (changed.Any())
        //            {
        //                _session.UnitOfWork.RegisterModified(entity, changed);
        //                _session.SetModifiedProperties(entity);
        //                _session.UnitOfWork.Commit().MergeInto(result);
        //            }
        //            else
        //            {
        //                result.SetToFail();
        //            }
        //
        //            return result;
        //        }

        IOpResult<ClockClientLunchDto> ILaborManagementProvider.RegisterExistingClockClientLunch(ClockClientLunchDto dto)
        {
            var result = new OpResult<ClockClientLunchDto>(dto);
            var changed = new PropertyList<ClockClientLunch>();

            var curr = _session.UnitOfWork.LaborManagementRepository
                .ClockClientLunchQuery()
                .ByClockClientLunchId(dto.ClockClientLunchId)
                .ExecuteQueryAs(new ClockClientLunchMaps.ToClockClientLunchDto())
                .FirstOrDefault();

            var entity = new ClockClientLunch
            {
                ClockClientLunchId = dto.ClockClientLunchId,
                ClientId = dto.ClientId,
                Name = dto.Name,
                PunchType = dto.PunchType,
                AllPunchesClockRoundingTypeId = dto.AllPunchesClockRoundingTypeId,
                AllPunchesGraceTime = dto.AllPunchesGraceTime,
                IsPaid = dto.IsPaid,
                InLateGraceTime = dto.InLateGraceTime,
                InLateClockRoundingTypeId = dto.InLateClockRoundingTypeId,
                OutEarlyGraceTime = dto.OutEarlyGraceTime,
                OutEarlyClockRoundingTypeId = dto.OutEarlyClockRoundingTypeId,
                OutLateClockRoundingTypeId = dto.OutLateClockRoundingTypeId,
                OutLateGraceTime = dto.OutLateGraceTime,
                InEarlyGraceTime = dto.InEarlyGraceTime,
                InEarlyClockRoundingTypeId = dto.InEarlyClockRoundingTypeId,
                StartTime = dto.StartTime,
                StopTime = dto.StopTime,
                AutoDeductedWorkedHours = dto.AutoDeductedWorkedHours,
                ClientCostCenterId = dto.ClientCostCenterId,
                GraceTime = dto.GraceTime,
                IsAllowMultipleTimePeriods = dto.IsAllowMultipleTimePeriods,
                IsAutoDeducted = dto.IsAutoDeducted,
                IsDoEmployeesPunch = dto.IsDoEmployeesPunch,
                IsShowPunches = dto.IsShowPunches,
                Length = dto.Length,
                IsSunday = dto.IsSunday,
                IsMonday = dto.IsMonday,
                IsTuesday = dto.IsTuesday,
                IsWednesday = dto.IsWednesday,
                IsThursday = dto.IsThursday,
                IsFriday = dto.IsFriday,
                IsSaturday = dto.IsSaturday,
                IsMaxPaid = dto.IsMaxPaid,
                IsUseStartStopTimes = dto.IsUseStartStopTimes,
                MinutesToRestrictLunchPunch = dto.MinutesToRestrictLunchPunch
            };

            entity.ClockClientLunchId = curr.ClockClientLunchId;

            if (entity.ClientId != curr.ClientId)
                entity.ClientId = curr.ClientId;
            if (entity.Name != curr.Name)
                changed.Include(c => c.Name);
            if (entity.Length != curr.Length)
                changed.Include(c => c.Length);
            if (entity.IsPaid != curr.IsPaid)
                changed.Include(c => c.IsPaid);
            if (entity.IsDoEmployeesPunch != curr.IsDoEmployeesPunch)
                changed.Include(c => c.IsDoEmployeesPunch);
            if (entity.IsAutoDeducted != curr.IsAutoDeducted)
                changed.Include(c => c.IsAutoDeducted);
            if (entity.AutoDeductedWorkedHours != curr.AutoDeductedWorkedHours)
                changed.Include(c => c.AutoDeductedWorkedHours);
            if (entity.GraceTime != curr.GraceTime)
                changed.Include(c => c.GraceTime);
            if (entity.StartTime != curr.StartTime)
                changed.Include(c => c.StartTime);
            if (entity.StopTime != curr.StopTime)
                changed.Include(c => c.StopTime);
            if (entity.ClientCostCenterId != curr.ClientCostCenterId)
                changed.Include(c => c.ClientCostCenterId);
            if (entity.PunchType != curr.PunchType)
                changed.Include(c => c.PunchType);
            if (entity.IsShowPunches != curr.IsShowPunches)
                changed.Include(c => c.IsShowPunches);
            if (entity.IsMaxPaid != curr.IsMaxPaid)
                changed.Include(c => c.IsMaxPaid);
            if (entity.InEarlyClockRoundingTypeId != curr.InEarlyClockRoundingTypeId)
                changed.Include(c => c.InEarlyClockRoundingTypeId);
            if (entity.OutEarlyClockRoundingTypeId != curr.OutEarlyClockRoundingTypeId)
                changed.Include(c => c.OutEarlyClockRoundingTypeId);
            if (entity.InLateClockRoundingTypeId != curr.InLateClockRoundingTypeId)
                changed.Include(c => c.InLateClockRoundingTypeId);
            if (entity.OutLateClockRoundingTypeId != curr.OutLateClockRoundingTypeId)
                changed.Include(c => c.OutLateClockRoundingTypeId);
            if (entity.InEarlyGraceTime != curr.InEarlyGraceTime)
                changed.Include(c => c.InEarlyGraceTime);
            if (entity.OutEarlyGraceTime != curr.OutEarlyGraceTime)
                changed.Include(c => c.OutEarlyGraceTime);
            if (entity.InLateGraceTime != curr.InLateGraceTime)
                changed.Include(c => c.InLateGraceTime);
            if (entity.OutLateGraceTime != curr.OutLateGraceTime)
                changed.Include(c => c.OutLateGraceTime);
            if (entity.AllPunchesClockRoundingTypeId != curr.AllPunchesClockRoundingTypeId)
                changed.Include(c => c.AllPunchesClockRoundingTypeId);
            if (entity.AllPunchesGraceTime != curr.AllPunchesGraceTime)
                changed.Include(c => c.AllPunchesGraceTime);
            if (entity.IsSunday != curr.IsSunday)
                changed.Include(c => c.IsSunday);
            if (entity.IsMonday != curr.IsMonday)
                changed.Include(c => c.IsMonday);
            if (entity.IsTuesday != curr.IsTuesday)
                changed.Include(c => c.IsTuesday);
            if (entity.IsWednesday != curr.IsWednesday)
                changed.Include(c => c.IsWednesday);
            if (entity.IsThursday != curr.IsThursday)
                changed.Include(c => c.IsThursday);
            if (entity.IsFriday != curr.IsFriday)
                changed.Include(c => c.IsFriday);
            if (entity.IsSaturday != curr.IsSaturday)
                changed.Include(c => c.IsSaturday);
            if (entity.IsUseStartStopTimes != curr.IsUseStartStopTimes)
                changed.Include(c => c.IsUseStartStopTimes);
            if (entity.IsAllowMultipleTimePeriods != curr.IsAllowMultipleTimePeriods)
                changed.Include(c => c.IsAllowMultipleTimePeriods);
            if (entity.MinutesToRestrictLunchPunch != curr.MinutesToRestrictLunchPunch)
                changed.Include(c => c.MinutesToRestrictLunchPunch);

            if (!changed.Any()) return result;

            _session.UnitOfWork.RegisterModified(entity, changed);
            _session.SetModifiedProperties(entity);
            _session.UnitOfWork.Commit().MergeInto(result);

            return result;
        }

        IOpResult<ClockClientLunchDto> ILaborManagementProvider.RegisterNewClockClientLunch(ClockClientLunchDto dto)
        {
            var result = new OpResult<ClockClientLunchDto>(dto);
            var entity = new ClockClientLunch
            {
                ClockClientLunchId = dto.ClockClientLunchId,
                ClientId = dto.ClientId,
                AllPunchesClockRoundingTypeId = dto.AllPunchesClockRoundingTypeId,
                AllPunchesGraceTime = dto.AllPunchesGraceTime,
                AutoDeductedWorkedHours = dto.AutoDeductedWorkedHours,
                ClientCostCenterId = dto.ClientCostCenterId,
                GraceTime = dto.GraceTime,
                Name = dto.Name,
                StopTime = dto.StopTime,
                InLateClockRoundingTypeId = dto.InLateClockRoundingTypeId,
                InEarlyClockRoundingTypeId = dto.InEarlyClockRoundingTypeId,
                InEarlyGraceTime = dto.InEarlyGraceTime,
                InLateGraceTime = dto.InLateGraceTime,
                IsAllowMultipleTimePeriods = dto.IsAllowMultipleTimePeriods,
                IsAutoDeducted = dto.IsAutoDeducted,
                IsDoEmployeesPunch = dto.IsDoEmployeesPunch,
                IsMonday = dto.IsMonday,
                IsTuesday = dto.IsTuesday,
                IsWednesday = dto.IsWednesday,
                IsThursday = dto.IsThursday,
                IsFriday = dto.IsFriday,
                IsSaturday = dto.IsSaturday,
                IsSunday = dto.IsSunday,
                IsShowPunches = dto.IsShowPunches,
                IsUseStartStopTimes = dto.IsUseStartStopTimes,
                Length = dto.Length,
                OutEarlyClockRoundingTypeId = dto.OutEarlyClockRoundingTypeId,
                OutLateGraceTime = dto.OutLateGraceTime,
                OutLateClockRoundingTypeId = dto.OutLateClockRoundingTypeId,
                OutEarlyGraceTime = dto.OutEarlyGraceTime,
                PunchType = dto.PunchType,
                StartTime = dto.StartTime,
                IsPaid = dto.IsPaid,
                IsMaxPaid = dto.IsMaxPaid,
                MinutesToRestrictLunchPunch = dto.MinutesToRestrictLunchPunch
            };

            var hasConflictingName = _session.UnitOfWork.LaborManagementRepository
                .ClockClientLunchQuery()
                .ByClient(entity.ClientId)
                .ByClockClientLunchName(entity.Name)
                .Result
                .Any();

            if (hasConflictingName)
            {
                result
                    .AddExceptionMessage(new Exception("A Lunch/Break with that 'Name' already exists. Please change the name and resubmit."))
                    .SetToFail();
                return result;
            }


            entity.ClockClientLunchId = CommonConstants.NEW_ENTITY_ID;

            _session.UnitOfWork.RegisterNew(entity);
            _session.UnitOfWork.RegisterPostCommitAction(() => result.Data.ClockClientLunchId = entity.ClockClientLunchId);
            _session.SetModifiedProperties(entity);
            _session.UnitOfWork.Commit().MergeInto(result);

            return result;
        }

        IOpResult<ClockClientLunchDto> ILaborManagementProvider.RegisterNewClockClientLunch(ClockClientLunch entity)
        {

            var result = new OpResult<ClockClientLunchDto>(new ClockClientLunchDto
            {
                ClockClientLunchId = entity.ClockClientLunchId,
                ClientId = entity.ClientId,
                AllPunchesClockRoundingTypeId = entity.AllPunchesClockRoundingTypeId,
                AllPunchesGraceTime = entity.AllPunchesGraceTime,
                AutoDeductedWorkedHours = entity.AutoDeductedWorkedHours,
                ClientCostCenterId = entity.ClientCostCenterId,
                GraceTime = entity.GraceTime,
                Name = entity.Name,
                StopTime = entity.StopTime,
                InLateClockRoundingTypeId = entity.InLateClockRoundingTypeId,
                InEarlyClockRoundingTypeId = entity.InEarlyClockRoundingTypeId,
                InEarlyGraceTime = entity.InEarlyGraceTime,
                InLateGraceTime = entity.InLateGraceTime,
                IsAllowMultipleTimePeriods = entity.IsAllowMultipleTimePeriods,
                IsAutoDeducted = entity.IsAutoDeducted,
                IsDoEmployeesPunch = entity.IsDoEmployeesPunch,
                IsMonday = entity.IsMonday,
                IsTuesday = entity.IsTuesday,
                IsWednesday = entity.IsWednesday,
                IsThursday = entity.IsThursday,
                IsFriday = entity.IsFriday,
                IsSaturday = entity.IsSaturday,
                IsSunday = entity.IsSunday,
                IsShowPunches = entity.IsShowPunches,
                IsUseStartStopTimes = entity.IsUseStartStopTimes,
                Length = entity.Length,
                OutEarlyClockRoundingTypeId = entity.OutEarlyClockRoundingTypeId,
                OutLateGraceTime = entity.OutLateGraceTime,
                OutLateClockRoundingTypeId = entity.OutLateClockRoundingTypeId,
                OutEarlyGraceTime = entity.OutEarlyGraceTime,
                PunchType = entity.PunchType,
                StartTime = entity.StartTime,
                IsPaid = entity.IsPaid,
                IsMaxPaid = entity.IsMaxPaid,
                MinutesToRestrictLunchPunch = entity.MinutesToRestrictLunchPunch
            });

            var hasConflictingName = _session.UnitOfWork.LaborManagementRepository
                .ClockClientLunchQuery()
                .ByClient(entity.ClientId)
                .ByClockClientLunchName(entity.Name)
                .Result
                .Any();

            if (hasConflictingName)
            {
                result
                    .AddExceptionMessage(new Exception("A Lunch/Break with that 'Name' already exists. Please change the name and resubmit."))
                    .SetToFail();
                return result;
            }


            entity.ClockClientLunchId = CommonConstants.NEW_ENTITY_ID;

            _session.UnitOfWork.RegisterNew(entity);
            _session.UnitOfWork.RegisterPostCommitAction(() => result.Data.ClockClientLunchId = entity.ClockClientLunchId);
            _session.SetModifiedProperties(entity);
            _session.UnitOfWork.Commit().MergeInto(result);

            return result;
        }

        IOpResult<ClockClientAddHoursDto> ILaborManagementProvider.RegisterExistingClockClientAddHours(ClockClientAddHoursDto dto)
        {
            var result = new OpResult<ClockClientAddHoursDto>(dto);
            var changed = new PropertyList<ClockClientAddHours>();

            var curr = _session.UnitOfWork.LaborManagementRepository
                .ClockClientAddHoursQuery()
                .ByClockClientAddHoursId(dto.ClockClientAddHoursId)
                .ExecuteQueryAs(x => new ClockClientAddHoursDto
                {
                    ClockClientAddHoursId = x.ClockClientAddHoursId,
                    ClientId = x.ClientId,
                    Name = x.Name,
                    CalculationFrequency = x.CalculationFrequency,
                    TimeWorkedThreshold = x.TimeWorkedThreshold,
                    Award = x.Award,
                    ClientEarningId = x.ClientEarningId,
                    IsSunday = x.IsSunday,
                    IsMonday = x.IsMonday,
                    IsTuesday = x.IsTuesday,
                    IsWednesday = x.IsWednesday,
                    IsThursday = x.IsThursday,
                    IsFriday = x.IsFriday,
                    IsSaturday = x.IsSaturday,
                    Modified = x.Modified,
                    ModifiedBy = x.ModifiedBy,
                })
                .FirstOrDefault();

            var entity = new ClockClientAddHours
            {
                ClockClientAddHoursId = dto.ClockClientAddHoursId,
                ClientId = dto.ClientId,
                Name = dto.Name,
                CalculationFrequency = dto.CalculationFrequency,
                TimeWorkedThreshold = dto.TimeWorkedThreshold,
                Award = dto.Award,
                ClientEarningId = dto.ClientEarningId,
                IsSunday = dto.IsSunday,
                IsMonday = dto.IsMonday,
                IsTuesday = dto.IsTuesday,
                IsWednesday = dto.IsWednesday,
                IsThursday = dto.IsThursday,
                IsFriday = dto.IsFriday,
                IsSaturday = dto.IsSaturday,
                Modified = dto.Modified,
                ModifiedBy = dto.ModifiedBy
            };

            entity.ClockClientAddHoursId = curr.ClockClientAddHoursId;

            if (entity.ClientId != curr.ClientId)
                entity.ClientId = curr.ClientId;
            if (entity.Name != curr.Name)
                changed.Include(c => c.Name);
            if (entity.CalculationFrequency != curr.CalculationFrequency)
                changed.Include(c => c.CalculationFrequency);
            if (entity.TimeWorkedThreshold != curr.TimeWorkedThreshold)
                changed.Include(c => c.TimeWorkedThreshold);
            if (entity.Award != curr.Award)
                changed.Include(c => c.Award);
            if (entity.ClientEarningId != curr.ClientEarningId)
                changed.Include(c => c.ClientEarningId);
            if (entity.IsSunday != curr.IsSunday)
                changed.Include(c => c.IsSunday);
            if (entity.IsMonday != curr.IsMonday)
                changed.Include(c => c.IsMonday);
            if (entity.IsTuesday != curr.IsTuesday)
                changed.Include(c => c.IsTuesday);
            if (entity.IsWednesday != curr.IsWednesday)
                changed.Include(c => c.IsWednesday);
            if (entity.IsThursday != curr.IsThursday)
                changed.Include(c => c.IsThursday);
            if (entity.IsFriday != curr.IsFriday)
                changed.Include(c => c.IsFriday);
            if (entity.IsSaturday != curr.IsSaturday)
                changed.Include(c => c.IsSaturday);

            if (!changed.Any()) return result;

            _session.UnitOfWork.RegisterModified(entity, changed);
            _session.SetModifiedProperties(entity);
            _session.UnitOfWork.Commit().MergeInto(result);

            return result;
        }

        IOpResult<ClockClientAddHoursDto> ILaborManagementProvider.RegisterNewClockClientAddHours(ClockClientAddHoursDto dto)
        {
            var result = new OpResult<ClockClientAddHoursDto>(dto);
            var entity = new ClockClientAddHours
            {
                ClockClientAddHoursId = dto.ClockClientAddHoursId,
                ClientId = dto.ClientId,
                Name = dto.Name,
                CalculationFrequency = dto.CalculationFrequency,
                TimeWorkedThreshold = dto.TimeWorkedThreshold,
                Award = dto.Award,
                ClientEarningId = dto.ClientEarningId,
                IsSunday = dto.IsSunday,
                IsMonday = dto.IsMonday,
                IsTuesday = dto.IsTuesday,
                IsWednesday = dto.IsWednesday,
                IsThursday = dto.IsThursday,
                IsFriday = dto.IsFriday,
                IsSaturday = dto.IsSaturday,
                Modified = dto.Modified,
                ModifiedBy = dto.ModifiedBy
            };

            var hasConflictingName = _session.UnitOfWork.LaborManagementRepository
                .ClockClientLunchQuery()
                .ByClient(entity.ClientId)
                .ByClockClientLunchName(entity.Name)
                .Result
                .Any();

            if (hasConflictingName)
            {
                result
                    .AddExceptionMessage(new Exception("An Attendance Award with that 'Name' already exists. Please change the name and resubmit."))
                    .SetToFail();
                return result;
            }


            entity.ClockClientAddHoursId = CommonConstants.NEW_ENTITY_ID;

            _session.UnitOfWork.RegisterNew(entity);
            _session.UnitOfWork.RegisterPostCommitAction(() => result.Data.ClockClientAddHoursId = entity.ClockClientAddHoursId);
            _session.SetModifiedProperties(entity);
            _session.UnitOfWork.Commit().MergeInto(result);

            return result;
        }

        IOpResult<ClientShiftDto> ILaborManagementProvider.RegisterNewClientShift(ClientShiftDto dto)
        {
            var result = new OpResult<ClientShiftDto>(dto);
            var entity = new ClientShift
            {
                ClientShiftId = dto.ClientShiftId,
                ClientId = dto.ClientId,
                Description = dto.Description,
                AdditionalAmount = dto.AdditionalAmount,
                AdditionalAmountTypeId = dto.AdditionalAmountTypeId,
                AdditionalPremiumAmount = dto.AdditionalPremiumAmount,
                Limit = dto.Limit,
                Destination = dto.Destination,
                ShiftStartTolerance = dto.ShiftStartTolerance,
                ShiftEndTolerance = dto.ShiftEndTolerance,
                StartTime = dto.StartTime,
                StopTime = dto.StopTime,
                IsSunday = dto.IsSunday,
                IsMonday = dto.IsMonday,
                IsTuesday = dto.IsTuesday,
                IsWednesday = dto.IsWednesday,
                IsThursday = dto.IsThursday,
                IsFriday = dto.IsFriday,
                IsSaturday = dto.IsSaturday
            };

            var hasConflictingDescription = _session.UnitOfWork.LaborManagementRepository
                .ClientShiftQuery()
                .ByClientShiftDescription(dto.Description)
                .Result
                .Any();

            if (hasConflictingDescription)
            {
                result
                    .AddExceptionMessage(new Exception("A Client Shift with that 'Description' already exists. Please change the description and resubmit."))
                    .SetToFail();
                return result;
            }

            entity.ClientShiftId = CommonConstants.NEW_ENTITY_ID;

            _session.UnitOfWork.RegisterNew(entity);
            _session.UnitOfWork.RegisterPostCommitAction(() => result.Data.ClientShiftId = entity.ClientShiftId);
            _session.SetModifiedProperties(entity);
            _session.UnitOfWork.Commit().MergeInto(result);

            return result;
        }

        IOpResult ILaborManagementProvider.RegisterExistingClientShift(ClientShiftDto dto)
        {
            var result = new OpResult();
            var entity = new ClientShift
            {
                ClientShiftId = dto.ClientShiftId,
                ClientId = dto.ClientId,
                Description = dto.Description,
                AdditionalAmount = dto.AdditionalAmount,
                AdditionalAmountTypeId = dto.AdditionalAmountTypeId,
                AdditionalPremiumAmount = dto.AdditionalPremiumAmount,
                Limit = dto.Limit,
                Destination = dto.Destination,
                ShiftStartTolerance = dto.ShiftStartTolerance,
                ShiftEndTolerance = dto.ShiftEndTolerance,
                StartTime = dto.StartTime,
                StopTime = dto.StopTime,
                IsSunday = dto.IsSunday,
                IsMonday = dto.IsMonday,
                IsTuesday = dto.IsTuesday,
                IsWednesday = dto.IsWednesday,
                IsThursday = dto.IsThursday,
                IsFriday = dto.IsFriday,
                IsSaturday = dto.IsSaturday
            };
            var curr = _session.UnitOfWork.LaborManagementRepository
                .ClientShiftQuery()
                .ByClientShift(dto.ClientShiftId)
                .ExecuteQueryAs(x => new ClientShiftDto
                {
                    ClientShiftId = x.ClientShiftId,
                    ClientId = x.ClientId,
                    Description = x.Description,
                    AdditionalAmount = x.AdditionalAmount,
                    AdditionalAmountTypeId = x.AdditionalAmountTypeId,
                    AdditionalPremiumAmount = x.AdditionalPremiumAmount,
                    Limit = x.Limit,
                    Destination = x.Destination,
                    ShiftStartTolerance = x.ShiftStartTolerance,
                    ShiftEndTolerance = x.ShiftEndTolerance,
                    StartTime = x.StartTime,
                    StopTime = x.StopTime,
                    IsSunday = x.IsSunday,
                    IsMonday = x.IsMonday,
                    IsTuesday = x.IsTuesday,
                    IsWednesday = x.IsWednesday,
                    IsThursday = x.IsThursday,
                    IsFriday = x.IsFriday,
                    IsSaturday = x.IsSaturday
                })
                .FirstOrDefault();

            if (curr == null)
            {
                result.SetToFail();
                return result;
            }

            var changed = new PropertyList<ClientShift>();

            entity.ClientShiftId = curr.ClientShiftId;

            if (curr.ClientId != entity.ClientId)
                changed.Include(c => c.ClientId);
            if (curr.Description != entity.Description)
                changed.Include(c => c.Description);
            if (curr.AdditionalAmount != entity.AdditionalAmount)
                changed.Include(c => c.AdditionalAmount);
            if (curr.AdditionalAmountTypeId != entity.AdditionalAmountTypeId)
                changed.Include(c => c.AdditionalAmountTypeId);
            if (curr.AdditionalPremiumAmount != entity.AdditionalPremiumAmount)
                changed.Include(c => c.AdditionalPremiumAmount);
            if (curr.Limit != entity.Limit)
                changed.Include(c => c.Limit);
            if (curr.Destination != entity.Destination)
                changed.Include(c => c.Destination);
            if (curr.ShiftStartTolerance != entity.ShiftStartTolerance)
                changed.Include(c => c.ShiftStartTolerance);
            if (curr.ShiftEndTolerance != entity.ShiftEndTolerance)
                changed.Include(c => c.ShiftEndTolerance);
            if (curr.StartTime != entity.StartTime)
                changed.Include(c => c.StartTime);
            if (curr.StopTime != entity.StopTime)
                changed.Include(c => c.StopTime);
            if (curr.IsSunday != entity.IsSunday)
                changed.Include(c => c.IsSunday);
            if (curr.IsMonday != entity.IsMonday)
                changed.Include(c => c.IsMonday);
            if (curr.IsTuesday != entity.IsTuesday)
                changed.Include(c => c.IsTuesday);
            if (curr.IsWednesday != entity.IsWednesday)
                changed.Include(c => c.IsWednesday);
            if (curr.IsThursday != entity.IsThursday)
                changed.Include(c => c.IsThursday);
            if (curr.IsFriday != entity.IsFriday)
                changed.Include(c => c.IsFriday);
            if (curr.IsSaturday != entity.IsSaturday)
                changed.Include(c => c.IsSaturday);

            if (changed.Any())
            {
                changed.Include(c => c.Modified);
                changed.Include(c => c.ModifiedBy);
            }

            _session.SetModifiedProperties(entity);
            _session.UnitOfWork.RegisterModified(entity, changed);
            _session.UnitOfWork.Commit().MergeInto(result);

            return result;
        }

        IOpResult<ClockClientOvertimeDto> ILaborManagementProvider.RegisterNewClockClientOvertime(
            ClockClientOvertimeDto dto)
        {
            var result = new OpResult<ClockClientOvertimeDto>(dto);
            var entity = new ClockClientOvertime
            {
                ClockClientOvertimeId = CommonConstants.NEW_ENTITY_ID,
                ClientId = dto.ClientId,
                ClientEarningId = dto.ClientEarningId,
                ClockOvertimeFrequencyId = dto.ClockOvertimeFrequencyId,
                Hours = dto.Hours,
                Name = dto.Name,
                IsSunday = dto.IsSunday,
                IsMonday = dto.IsMonday,
                IsTuesday = dto.IsTuesday,
                IsWednesday = dto.IsWednesday,
                IsThursday = dto.IsThursday,
                IsFriday = dto.IsFriday,
                IsSaturday = dto.IsSaturday
            };

            var hasConflictingName = _session.UnitOfWork.LaborManagementRepository
                .ClockClientOvertimeQuery()
                .ByClientId(dto.ClientId)
                .ByClockClientOvertimeName(dto.Name)
                .Result
                .Any();

            if (hasConflictingName)
            {
                result
                    .AddExceptionMessage(new Exception("A Overtime rules with that 'Name' already exists. Please change the name and resubmit."))
                    .SetToFail();
                return result;
            }

            _session.UnitOfWork.RegisterNew(entity);
            _session.UnitOfWork.RegisterPostCommitAction(() => result.Data.ClockClientOvertimeId = entity.ClockClientOvertimeId);
            _session.SetModifiedProperties(entity);
            _session.UnitOfWork.Commit().MergeInto(result);

            return result;
        }

        IOpResult ILaborManagementProvider.RegisterExistingClockClientOvertime(ClockClientOvertimeDto dto)
        {
            var result = new OpResult();
            var entity = new ClockClientOvertime
            {
                ClockClientOvertimeId = dto.ClockClientOvertimeId,
                ClientId = dto.ClientId,
                ClientEarningId = dto.ClientEarningId,
                ClockOvertimeFrequencyId = dto.ClockOvertimeFrequencyId,
                Hours = dto.Hours,
                Name = dto.Name,
                IsSunday = dto.IsSunday,
                IsMonday = dto.IsMonday,
                IsTuesday = dto.IsTuesday,
                IsWednesday = dto.IsWednesday,
                IsThursday = dto.IsThursday,
                IsFriday = dto.IsFriday,
                IsSaturday = dto.IsSaturday
            };

            var curr = _session.UnitOfWork.LaborManagementRepository
                .ClockClientOvertimeQuery()
                .ByClockClientOvertimeId(dto.ClockClientOvertimeId)
                .ExecuteQueryAs(new ClockClientOvertimeMaps.ToClockClientOvertimeDto())
                .FirstOrDefault();

            if (curr == null)
            {
                result.SetToFail();
                return result;
            }

            var changed = new PropertyList<ClockClientOvertime>();

            entity.ClockClientOvertimeId = curr.ClockClientOvertimeId;

            if (entity.ClientId != curr.ClientId)
                changed.Include(c => c.ClientId);
            if (entity.ClientEarningId != curr.ClientEarningId)
                changed.Include(c => c.ClientEarningId);
            if (entity.ClockOvertimeFrequencyId != curr.ClockOvertimeFrequencyId)
                changed.Include(c => c.ClockOvertimeFrequencyId);
            if (entity.Hours != curr.Hours)
                changed.Include(c => c.Hours);
            if (entity.Name != curr.Name)
                changed.Include(c => c.Name);
            if (entity.IsSunday != curr.IsSunday)
                changed.Include(c => c.IsSunday);
            if (entity.IsMonday != curr.IsMonday)
                changed.Include(c => c.IsMonday);
            if (entity.IsTuesday != curr.IsTuesday)
                changed.Include(c => c.IsTuesday);
            if (entity.IsWednesday != curr.IsWednesday)
                changed.Include(c => c.IsWednesday);
            if (entity.IsThursday != curr.IsThursday)
                changed.Include(c => c.IsThursday);
            if (entity.IsFriday != curr.IsFriday)
                changed.Include(c => c.IsFriday);
            if (entity.IsSaturday != curr.IsSaturday)
                changed.Include(c => c.IsSaturday);

            if (changed.Any()) changed.IncludeModifiedProperties();

            _session.UnitOfWork.RegisterModified(entity, changed);
            _session.UnitOfWork.Commit().MergeInto(result);

            return result;
        }

        IOpResult<List<ClockClientTimePolicyDtos.ClockClientTimePolicyDto>> ILaborManagementProvider.
            GetClockClientTimePolicies(int clientId)
        {
            var result = new OpResult<List<ClockClientTimePolicyDtos.ClockClientTimePolicyDto>>();

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(result);
            _session.CanPerformAction(LaborManagementActionType.ReadTimePolicy).MergeInto(result);

            if (result.HasError) return result;

            return result.TrySetData(() => _session.UnitOfWork.LaborManagementRepository
                .ClockClientTimePolicyQuery()
                .ByClientId(clientId)
                .ExecuteQueryAs(new ClockClientTimePolicyMaps.ToClockClientTimePolicyDto())
                .ToList());
        }

        IOpResult<List<ClockClientRulesDto>> ILaborManagementProvider.GetClockClientRulesByClient(int clientId)
        {
            var result = new OpResult<List<ClockClientRulesDto>>();

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(result);
            _session.CanPerformAction(LaborManagementActionType.ReadTimePolicy).MergeInto(result);

            if (result.HasError) return result;

            return result.TrySetData(() => _session.UnitOfWork.LaborManagementRepository
                .ClockClientRulesQuery()
                .ByClient(clientId)
                .ExecuteQueryAs(new ClockClientRulesMaps.ToClockClientRulesDto())
                .ToList());
        }

        IOpResult<List<TimeZoneDto>> ILaborManagementProvider.GetTimeZones()
        {
            var result = new OpResult<List<TimeZoneDto>>();
            var zones = _session.UnitOfWork.TimeClockRepository
                .GetTimeZones()
                .Select(x => new TimeZoneDto
                {
                    TimeZoneId = x.TimeZoneId,
                    Description = x.Description,
                    FriendlyDescription = x.FriendlyDesc,
                    Utc = x.Utc
                })
                .ToList();

            result.TrySetData(() => zones);
            if (!result.HasData) result.SetToFail();
            return result;
        }

        IOpResult<List<ClockClientOvertimeSelectedDto>> ILaborManagementProvider.GetClockClientOvertimeSelectedList(int clockClientTimePolicyId, int clientId)
        {
            var result = new OpResult<List<ClockClientOvertimeSelectedDto>>();

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(result);
            _session.CanPerformAction(LaborManagementActionType.ReadTimePolicy).MergeInto(result);

            return result.TrySetData(() => _session.UnitOfWork.LaborManagementRepository
                .ClockClientOvertimeSelectedQuery()
                .ByClockClientTimePolicy(clockClientTimePolicyId)
                .ExecuteQueryAs(x => new ClockClientOvertimeSelectedDto()
                {
                    ClockClientOvertimeSelectedId = x.ClockClientOvertimeSelectedId,
                    ClockClientTimePolicyId = x.ClockClientTimePolicyId,
                    ClockClientOvertimeId = x.ClockClientOvertimeId
                })
                .ToList());
        }

        IOpResult<List<ClockClientOvertimeDto>> ILaborManagementProvider.GetClockClientOvertimeList(int clientId)
        {
            var result = new OpResult<List<ClockClientOvertimeDto>>();

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(result);
            _session.CanPerformAction(LaborManagementActionType.ReadTimePolicy).MergeInto(result);

            if (result.HasError) return result;

            return result.TrySetData(() => _session.UnitOfWork.LaborManagementRepository
                .ClockClientOvertimeQuery()
                .ByClientId(clientId)
                .ExecuteQueryAs(x => new ClockClientOvertimeDto
                {
                    ClockClientOvertimeId = x.ClockClientOvertimeId,
                    ClientId = x.ClientId,
                    ClientEarningId = x.ClientEarningId,
                    ClockOvertimeFrequencyId = x.ClockOvertimeFrequencyId,
                    Name = x.Name,
                    Hours = x.Hours,
                    IsSunday = x.IsSunday,
                    IsMonday = x.IsMonday,
                    IsTuesday = x.IsTuesday,
                    IsWednesday = x.IsWednesday,
                    IsThursday = x.IsThursday,
                    IsFriday = x.IsFriday,
                    IsSaturday = x.IsSaturday
                })
                .ToList());
        }

        IOpResult<List<ClockClientLunchDto>> ILaborManagementProvider.GetClockClientLunchBreakList(int clientId)
        {
            var result = new OpResult<List<ClockClientLunchDto>>();

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(result);
            _session.CanPerformAction(LaborManagementActionType.ReadTimePolicy).MergeInto(result);

            if (result.HasError) return result;

            return result.TrySetData(() => _session.UnitOfWork.LaborManagementRepository
                .ClockClientLunchQuery()
                .ByClient(clientId)
                .ExecuteQueryAs(new ClockClientLunchMaps.ToClockClientLunchDto())
                .ToList());
        }

        IOpResult<List<ClockClientAddHoursDto>> ILaborManagementProvider.GetClockClientAddHoursList(int clientId)
        {
            var result = new OpResult<List<ClockClientAddHoursDto>>();

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(result);
            _session.CanPerformAction(LaborManagementActionType.ReadTimePolicy).MergeInto(result);

            if (result.HasError) return result;

            return result.TrySetData(() => _session.UnitOfWork.LaborManagementRepository
                .ClockClientAddHoursQuery()
                .ByClient(clientId)
                .ExecuteQueryAs(a => new ClockClientAddHoursDto
                {
                    ClockClientAddHoursId = a.ClockClientAddHoursId,
                    ClientId = a.ClientId,
                    Name = a.Name,
                    CalculationFrequency = a.CalculationFrequency,
                    TimeWorkedThreshold = a.TimeWorkedThreshold,
                    Award = a.Award,
                    ClientEarningId = a.ClientEarningId,
                    IsSunday = a.IsSunday,
                    IsMonday = a.IsMonday,
                    IsTuesday = a.IsTuesday,
                    IsWednesday = a.IsWednesday,
                    IsThursday = a.IsThursday,
                    IsFriday = a.IsFriday,
                    IsSaturday = a.IsSaturday
                })
                .ToList());
        }

        IOpResult<List<ClientShiftDto>> ILaborManagementProvider.GetClientShiftList(int clientId)
        {
            var result = new OpResult<List<ClientShiftDto>>();

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(result);
            _session.CanPerformAction(LaborManagementActionType.ReadTimePolicy).MergeInto(result);

            if (result.HasError) return result;

            return result.TrySetData(() => _session.UnitOfWork.ClientRepository
                .ClientShiftQuery()
                .ByClientId(clientId)
                .ExecuteQueryAs(x => new ClientShiftDto
                {
                    ClientShiftId = x.ClientShiftId,
                    ClientId = x.ClientId,
                    Description = x.Description,
                    Destination = x.Destination,
                    AdditionalAmount = x.AdditionalAmount,
                    AdditionalAmountTypeId = x.AdditionalAmountTypeId,
                    AdditionalPremiumAmount = x.AdditionalPremiumAmount,
                    Limit = x.Limit,
                    ShiftEndTolerance = x.ShiftEndTolerance,
                    ShiftStartTolerance = x.ShiftStartTolerance,
                    StartTime = x.StartTime,
                    StopTime = x.StopTime,
                    IsSunday = x.IsSunday,
                    IsMonday = x.IsMonday,
                    IsTuesday = x.IsTuesday,
                    IsWednesday = x.IsWednesday,
                    IsThursday = x.IsThursday,
                    IsFriday = x.IsFriday,
                    IsSaturday = x.IsSaturday
                })
                .ToList());
        }

        IOpResult ILaborManagementProvider.DeleteRelatedClockClientLunches(int clientId, int clockClientTimePolicyId, List<ClockClientLunchSelectedDto> lunches)
        {
            var result = new OpResult();

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(result);
            _session.CanPerformAction(LaborManagementActionType.WriteTimePolicy).MergeInto(result);

            if (result.HasError) return result;

            foreach (var lunch in lunches)
            {
                _session.UnitOfWork.RegisterDeleted(new ClockClientLunchSelected
                {
                    ClockClientLunchId = lunch.ClockClientLunchId,
                    ClockClientTimePolicyId = lunch.ClockClientTimePolicyId
                });
            }

            if (lunches.Any())
                _session.UnitOfWork.Commit().MergeInto(result);

            return result;
        }

        IOpResult ILaborManagementProvider.DeleteRelatedClockClientLunches(int clientId, int clockClientTimePolicyId)
        {
            var result = new OpResult();

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(result);
            _session.CanPerformAction(LaborManagementActionType.WriteTimePolicy).MergeInto(result);

            if (result.HasError) return result;

            var lunches = _session.UnitOfWork.LaborManagementRepository
                .ClockClientTimePolicyQuery()
                .ByClockClientTimePolicyId(clockClientTimePolicyId)
                .ExecuteQueryAs(x => new ClockClientTimePolicyDtos.ClockClientTimePolicyDto
                {
                    ClockClientTimePolicyId = x.ClockClientTimePolicyId,
                    ClientId = x.ClientId,
                    Name = x.Name,
                    LunchSelected = x.LunchSelected.Select(l => new ClockClientLunchSelectedDto
                    {
                        ClockClientLunchId = l.ClockClientLunchId,
                        ClockClientTimePolicyId = l.ClockClientTimePolicyId
                    })
                })
                .FirstOrDefault()?
                .LunchSelected
                .ToList() ?? new List<ClockClientLunchSelectedDto>();

            foreach (var lunch in lunches)
            {
                _session.UnitOfWork.RegisterDeleted(new ClockClientLunchSelected
                {
                    ClockClientLunchId = lunch.ClockClientLunchId,
                    ClockClientTimePolicyId = lunch.ClockClientTimePolicyId
                });
            }

            if (lunches.Any())
                _session.UnitOfWork.Commit().MergeInto(result);

            return result;
        }

        IOpResult ILaborManagementProvider.DeleteRelatedClockClientAddHours(int clientId, int clockClientTimePolicyId, List<ClockClientAddHoursSelectedDto> addHoursSelected)
        {
            var result = new OpResult();

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(result);
            _session.CanPerformAction(LaborManagementActionType.WriteTimePolicy).MergeInto(result);

            if (result.HasError) return result;

            foreach (var addHours in addHoursSelected)
            {
                _session.UnitOfWork.RegisterDeleted(new ClockClientAddHoursSelected
                {
                    ClockClientAddHoursId = addHours.ClockClientAddHoursId,
                    ClockClientTimePolicyId = addHours.ClockClientTimePolicyId
                });
            }

            if (addHoursSelected.Any())
                _session.UnitOfWork.Commit().MergeInto(result);

            return result;
        }

        IOpResult ILaborManagementProvider.DeleteRelatedClockClientAddHours(int clientId, int clockClientTimePolicyId)
        {
            var result = new OpResult();

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(result);
            _session.CanPerformAction(LaborManagementActionType.WriteTimePolicy).MergeInto(result);

            if (result.HasError) return result;

            var addHoursSelected = _session.UnitOfWork.LaborManagementRepository
                .ClockClientTimePolicyQuery()
                .ByClockClientTimePolicyId(clockClientTimePolicyId)
                .ExecuteQueryAs(x => new ClockClientTimePolicyDtos.ClockClientTimePolicyDto
                {
                    ClockClientTimePolicyId = x.ClockClientTimePolicyId,
                    ClientId = x.ClientId,
                    Name = x.Name,
                    AddHoursSelected = x.AddHoursSelected.Select(a => new ClockClientAddHoursSelectedDto
                    {
                        ClockClientAddHoursId = a.ClockClientAddHoursId,
                        ClockClientTimePolicyId = a.ClockClientTimePolicyId
                    })
                })
                .FirstOrDefault()?
                .AddHoursSelected
                .ToList() ?? new List<ClockClientAddHoursSelectedDto>();

            foreach (var addHours in addHoursSelected)
            {
                _session.UnitOfWork.RegisterDeleted(new ClockClientAddHoursSelected
                {
                    ClockClientAddHoursId = addHours.ClockClientAddHoursId,
                    ClockClientTimePolicyId = addHours.ClockClientTimePolicyId
                });
            }

            if (addHoursSelected.Any())
                _session.UnitOfWork.Commit().MergeInto(result);

            return result;
        }

        IOpResult ILaborManagementProvider.DeleteRelatedClockClientOvertimes(int clientId,
            int clockClientTimePolicyId, List<ClockClientOvertimeSelectedDto> overtimes)
        {
            var result = new OpResult();

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(result);
            _session.CanPerformAction(LaborManagementActionType.WriteTimePolicy).MergeInto(result);

            if (result.HasError) return result;

            foreach (var overtime in overtimes)
            {
                _session.UnitOfWork.RegisterDeleted(new ClockClientOvertimeSelected
                {
                    ClockClientOvertimeSelectedId = overtime.ClockClientOvertimeSelectedId,
                    ClockClientOvertimeId = overtime.ClockClientOvertimeId,
                    ClockClientTimePolicyId = overtime.ClockClientTimePolicyId
                });
            }

            if (overtimes.Any())
                _session.UnitOfWork.Commit().MergeInto(result);

            return result;
        }

        IOpResult ILaborManagementProvider.DeleteRelatedClockClientOvertimes(int clientId, int clockClientTimePolicyId)
        {
            var result = new OpResult();

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(result);
            _session.CanPerformAction(LaborManagementActionType.WriteTimePolicy).MergeInto(result);

            if (result.HasError) return result;

            var overtimes = _session.UnitOfWork.LaborManagementRepository
                .ClockClientTimePolicyQuery()
                .ByClockClientTimePolicyId(clockClientTimePolicyId)
                .ExecuteQueryAs(x => new ClockClientTimePolicyDtos.ClockClientTimePolicyDto
                {
                    OvertimeSelected = x.OvertimeSelected.Select(o => new ClockClientOvertimeSelectedDto
                    {
                        ClockClientOvertimeSelectedId = o.ClockClientOvertimeSelectedId,
                        ClockClientOvertimeId = o.ClockClientOvertimeId,
                        ClockClientTimePolicyId = o.ClockClientTimePolicyId
                    })
                })
                .FirstOrDefault()?
                .OvertimeSelected
                .ToList() ?? new List<ClockClientOvertimeSelectedDto>();

            foreach (var overtime in overtimes)
            {
                _session.UnitOfWork.RegisterDeleted(new ClockClientOvertimeSelected
                {
                    ClockClientOvertimeSelectedId = overtime.ClockClientOvertimeSelectedId,
                    ClockClientOvertimeId = overtime.ClockClientOvertimeId,
                    ClockClientTimePolicyId = overtime.ClockClientTimePolicyId
                });
            }

            if (overtimes.Any())
                _session.UnitOfWork.Commit().MergeInto(result);

            return result;
        }

        IOpResult ILaborManagementProvider.DeleteRelatedClientShifts(int clientId, int clockClientTimePolicyId, List<ClientShiftSelectedDto> shifts)
        {
            var result = new OpResult();

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(result);
            _session.CanPerformAction(LaborManagementActionType.WriteTimePolicy).MergeInto(result);

            if (result.HasError) return result;

            foreach (var shift in shifts)
            {
                _session.UnitOfWork.RegisterDeleted(new ClientShiftSelected
                {
                    ClientShiftId = shift.ClientShiftId,
                    ClockClientTimePolicyId = shift.ClockClientTimePolicyId,
                    ClockClientShiftSelectedId = shift.ClockClientShiftSelectedId
                });
            }

            if (shifts.Any())
                _session.UnitOfWork.Commit().MergeInto(result);

            return result;
        }

        IOpResult ILaborManagementProvider.DeleteRelatedClientShifts(int clientId, int clockClientTimePolicyId)
        {
            var result = new OpResult();

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(result);
            _session.CanPerformAction(LaborManagementActionType.WriteTimePolicy).MergeInto(result);

            if (result.HasError) return result;

            var shifts = _session.UnitOfWork.LaborManagementRepository
                .ClockClientTimePolicyQuery()
                .ByClockClientTimePolicyId(clockClientTimePolicyId)
                .ExecuteQueryAs(x => new ClockClientTimePolicyDtos.ClockClientTimePolicyDto
                {
                    ShiftSelected = x.ShiftSelected.Select(s => new ClientShiftSelectedDto
                    {
                        ClockClientShiftSelectedId = s.ClockClientShiftSelectedId,
                        ClockClientTimePolicyId = s.ClockClientTimePolicyId,
                        ClientShiftId = s.ClientShiftId
                    })
                })
                .FirstOrDefault()?
                .ShiftSelected
                .ToList() ?? new List<ClientShiftSelectedDto>();

            foreach (var shift in shifts)
            {
                _session.UnitOfWork.RegisterDeleted(new ClientShiftSelected
                {
                    ClockClientShiftSelectedId = shift.ClockClientShiftSelectedId,
                    ClockClientTimePolicyId = shift.ClockClientTimePolicyId,
                    ClientShiftId = shift.ClientShiftId
                });
            }

            if (shifts.Any())
                _session.UnitOfWork.Commit().MergeInto(result);

            return result;
        }

        IOpResult<List<ClockClientLunchDto>> ILaborManagementProvider.GetClockClientLunchListByTimePolicy(
            int clientId, int clockClientTimePolicyId)
        {
            var result = new OpResult<List<ClockClientLunchDto>>();

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(result);
            _session.CanPerformAction(LaborManagementActionType.ReadTimePolicy).MergeInto(result);

            if (result.HasError) return result;

            return result.TrySetData(() => _session.UnitOfWork.LaborManagementRepository
                .ClockClientTimePolicyQuery()
                .ByClockClientTimePolicyId(clockClientTimePolicyId)
                .ExecuteQueryAs(x => new ClockClientTimePolicyDtos.ClockClientTimePolicyDto()
                {
                    ClockClientTimePolicyId = x.ClockClientTimePolicyId,
                    ClientId = x.ClientId,
                    Name = x.Name,
                    Lunches = x.LunchSelected.Select(l => new ClockClientLunchDto
                    {
                        ClockClientLunchId = l.Lunch.ClockClientLunchId,
                        ClientId = l.Lunch.ClientId,
                        AllPunchesClockRoundingTypeId = l.Lunch.AllPunchesClockRoundingTypeId,
                        AllPunchesGraceTime = l.Lunch.AllPunchesGraceTime,
                        AutoDeductedWorkedHours = l.Lunch.AutoDeductedWorkedHours,
                        ClientCostCenterId = l.Lunch.ClientCostCenterId,
                        GraceTime = l.Lunch.GraceTime,
                        Name = l.Lunch.Name,
                        StopTime = l.Lunch.StopTime,
                        InLateClockRoundingTypeId = l.Lunch.InLateClockRoundingTypeId,
                        InEarlyClockRoundingTypeId = l.Lunch.InEarlyClockRoundingTypeId,
                        InEarlyGraceTime = l.Lunch.InEarlyGraceTime,
                        InLateGraceTime = l.Lunch.InLateGraceTime,
                        IsAllowMultipleTimePeriods = l.Lunch.IsAllowMultipleTimePeriods,
                        IsAutoDeducted = l.Lunch.IsAutoDeducted,
                        IsDoEmployeesPunch = l.Lunch.IsDoEmployeesPunch,
                        IsMonday = l.Lunch.IsMonday,
                        IsTuesday = l.Lunch.IsTuesday,
                        IsWednesday = l.Lunch.IsWednesday,
                        IsThursday = l.Lunch.IsThursday,
                        IsFriday = l.Lunch.IsFriday,
                        IsSaturday = l.Lunch.IsSaturday,
                        IsSunday = l.Lunch.IsSunday,
                        IsShowPunches = l.Lunch.IsShowPunches,
                        IsUseStartStopTimes = l.Lunch.IsUseStartStopTimes,
                        Length = l.Lunch.Length,
                        OutEarlyClockRoundingTypeId = l.Lunch.OutEarlyClockRoundingTypeId,
                        OutLateGraceTime = l.Lunch.OutLateGraceTime,
                        OutLateClockRoundingTypeId = l.Lunch.OutLateClockRoundingTypeId,
                        OutEarlyGraceTime = l.Lunch.OutEarlyGraceTime,
                        PunchType = l.Lunch.PunchType,
                        StartTime = l.Lunch.StartTime,
                        IsPaid = l.Lunch.IsPaid,
                        IsMaxPaid = l.Lunch.IsMaxPaid,
                        MinutesToRestrictLunchPunch = l.Lunch.MinutesToRestrictLunchPunch
                    }).ToList()
                })
                .FirstOrDefault()?
                .Lunches
                .ToList());
        }

        IOpResult<List<ClockClientAddHoursDto>> ILaborManagementProvider.GetClockClientAddHoursListByTimePolicy(
            int clientId, int clockClientTimePolicyId)
        {
            var result = new OpResult<List<ClockClientAddHoursDto>>();

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(result);
            _session.CanPerformAction(LaborManagementActionType.ReadTimePolicy).MergeInto(result);

            if (result.HasError) return result;

            return result.TrySetData(() => _session.UnitOfWork.LaborManagementRepository
                .ClockClientTimePolicyQuery()
                .ByClockClientTimePolicyId(clockClientTimePolicyId)
                .ExecuteQueryAs(x => new ClockClientTimePolicyDtos.ClockClientTimePolicyDto()
                {
                    ClockClientTimePolicyId = x.ClockClientTimePolicyId,
                    ClientId = x.ClientId,
                    Name = x.Name,
                    AddHours = x.AddHoursSelected.Select(a => new ClockClientAddHoursDto
                    {
                        ClockClientAddHoursId = a.AddHours.ClockClientAddHoursId,
                        ClientId = a.AddHours.ClientId,
                        Name = a.AddHours.Name,
                        CalculationFrequency = a.AddHours.CalculationFrequency,
                        TimeWorkedThreshold = a.AddHours.TimeWorkedThreshold,
                        Award = a.AddHours.Award,
                        ClientEarningId = a.AddHours.ClientEarningId,
                        IsSunday = a.AddHours.IsSunday,
                        IsMonday = a.AddHours.IsMonday,
                        IsTuesday = a.AddHours.IsTuesday,
                        IsWednesday = a.AddHours.IsWednesday,
                        IsThursday = a.AddHours.IsThursday,
                        IsFriday = a.AddHours.IsFriday,
                        IsSaturday = a.AddHours.IsSaturday
                    }).ToList()
                })
                .FirstOrDefault()?
                .AddHours
                .ToList());
        }

        IOpResult<List<ClockClientOvertimeDto>> ILaborManagementProvider.GetClockClientOvertimeByTimePolicy(
            int clientId, int clockClientTimePolicyId)
        {
            var result = new OpResult<List<ClockClientOvertimeDto>>();

            var selectedList = Self.GetClockClientOvertimeSelectedList(clockClientTimePolicyId, clientId)
                .MergeInto(result).Data
                .Select(x => x.ClockClientOvertimeId)
                .ToList();

            if (result.HasError) return result;

            return result.TrySetData(() => _session.UnitOfWork.LaborManagementRepository
                .ClockClientOvertimeQuery()
                .ByClockClientOvertimeIdList(selectedList)
                .ExecuteQueryAs(new ClockClientOvertimeMaps.ToClockClientOvertimeDto())
                .ToList());
        }

        IOpResult<ClockClientTimePolicyDtos.ClockClientTimePolicyDto> ILaborManagementProvider.GetClockClientTimePolicy(
            int clientId, int clockClientTimePolicyId)
        {
            var result = new OpResult<ClockClientTimePolicyDtos.ClockClientTimePolicyDto>();

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(result);
            _session.CanPerformAction(LaborManagementActionType.ReadTimePolicy).MergeInto(result);

            if (result.HasError) return result;

            return result.TrySetData(() => _session.UnitOfWork.LaborManagementRepository
                .ClockClientTimePolicyQuery()
                .ByClockClientTimePolicyId(clockClientTimePolicyId)
                .ExecuteQueryAs(x => new ClockClientTimePolicyDtos.ClockClientTimePolicyDto()
                {
                    ClockClientTimePolicyId = x.ClockClientTimePolicyId,
                    ClientId = x.ClientId,
                    ClientDepartmentId = x.ClientDepartmentId,
                    ClientShiftId = x.ClientShiftId,
                    ClientStatusId = x.ClientStatusId,
                    ClockClientExceptionId = x.ClockClientExceptionId,
                    ClockClientHolidayId = x.ClockClientHolidayId,
                    ClockClientRulesId = x.ClockClientRulesId,
                    Name = x.Name,
                    IsAddToOtherPolicy = x.IsAddToOtherPolicy,
                    HasCombinedOtFrequencies = x.HasCombinedOtFrequencies,
                    PayType = x.PayType,
                    TimeZoneId = x.TimeZoneId,
                    Shifts = x.ShiftSelected.Select(s => new ClientShiftDto
                    {
                        ClientShiftId = s.Shift.ClientShiftId,
                        ClientId = s.Shift.ClientId,
                        AdditionalAmount = s.Shift.AdditionalAmount,
                        AdditionalAmountTypeId = s.Shift.AdditionalAmountTypeId,
                        AdditionalPremiumAmount = s.Shift.AdditionalPremiumAmount,
                        Description = s.Shift.Description,
                        Destination = s.Shift.Destination,
                        StartTime = s.Shift.StartTime,
                        StopTime = s.Shift.StopTime,
                        ShiftStartTolerance = s.Shift.ShiftStartTolerance,
                        ShiftEndTolerance = s.Shift.ShiftEndTolerance,
                        Limit = s.Shift.Limit,
                        IsSunday = s.Shift.IsSunday,
                        IsMonday = s.Shift.IsMonday,
                        IsTuesday = s.Shift.IsTuesday,
                        IsWednesday = s.Shift.IsWednesday,
                        IsThursday = s.Shift.IsThursday,
                        IsFriday = s.Shift.IsFriday,
                        IsSaturday = s.Shift.IsSaturday
                    })
                })
                .FirstOrDefault());
        }

        IOpResult<List<ClientShiftDto>> ILaborManagementProvider.GetClientShiftListByTimePolicy(int clientId,
            int clockClientTimePolicyId)
        {
            // Check to make sure the user has rights
            var result = new OpResult<List<ClientShiftDto>>();
            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(result);
            _session.CanPerformAction(LaborManagementActionType.ReadTimePolicy).MergeInto(result);
            if (result.HasError) return result;

            return new OpResult<List<ClientShiftDto>>(_session.UnitOfWork.LaborManagementRepository
                .ClockClientTimePolicyQuery()
                .ByClockClientTimePolicyId(clockClientTimePolicyId)
                .ExecuteQueryAs(x => new ClockClientTimePolicyDtos.ClockClientTimePolicyDto
                {
                    ClockClientTimePolicyId = x.ClockClientTimePolicyId,
                    ClientId = x.ClientId,
                    Name = x.Name,
                    Shifts = x.ShiftSelected.Select(s => new ClientShiftDto
                    {
                        ClientShiftId = s.Shift.ClientShiftId,
                        ClientId = s.Shift.ClientId,
                        Description = s.Shift.Description,
                        AdditionalAmount = s.Shift.AdditionalAmount,
                        AdditionalAmountTypeId = s.Shift.AdditionalAmountTypeId,
                        AdditionalPremiumAmount = s.Shift.AdditionalPremiumAmount,
                        Destination = s.Shift.Destination,
                        Limit = s.Shift.Limit,
                        ShiftStartTolerance = s.Shift.ShiftStartTolerance,
                        ShiftEndTolerance = s.Shift.ShiftEndTolerance,
                        StartTime = s.Shift.StartTime,
                        StopTime = s.Shift.StopTime,
                        IsSunday = s.Shift.IsSunday,
                        IsMonday = s.Shift.IsMonday,
                        IsTuesday = s.Shift.IsTuesday,
                        IsWednesday = s.Shift.IsWednesday,
                        IsThursday = s.Shift.IsThursday,
                        IsFriday = s.Shift.IsFriday,
                        IsSaturday = s.Shift.IsSaturday
                    }).ToList()
                })
                .FirstOrDefault()?
                .Shifts
                .ToList());
        }

        IOpResult ILaborManagementProvider.SaveNewClockClientLunchSelectedList(int clientId, int clockClientTimePolicyId, List<ClockClientLunchSelected> lunches)
        {
            var result = new OpResult();

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(result);
            _session.CanPerformAction(LaborManagementActionType.WriteTimePolicy).MergeInto(result);

            foreach (var lunch in lunches)
            {
                _session.UnitOfWork.RegisterNew(new ClockClientLunchSelected
                {
                    ClockClientLunchId = lunch.ClockClientLunchId,
                    ClockClientTimePolicyId = clockClientTimePolicyId
                });
            }

            _session.UnitOfWork.Commit().MergeInto(result);

            return result;
        }

        IOpResult ILaborManagementProvider.SaveNewClockClientAddHoursSelectedList(int clientId, int clockClientTimePolicyId, List<ClockClientAddHoursSelected> addHoursSelected)
        {
            var result = new OpResult();

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(result);
            _session.CanPerformAction(LaborManagementActionType.WriteTimePolicy).MergeInto(result);

            foreach (var addHours in addHoursSelected)
            {
                _session.UnitOfWork.RegisterNew(new ClockClientAddHoursSelected
                {
                    ClockClientAddHoursId = addHours.ClockClientAddHoursId,
                    ClockClientTimePolicyId = clockClientTimePolicyId
                });
            }

            _session.UnitOfWork.Commit().MergeInto(result);

            return result;
        }

        IOpResult ILaborManagementProvider.SaveNewClockClientOvertimeSelectedList(int clientId,
            int clockClientTimePolicyId, List<ClockClientOvertimeSelected> overtimes)
        {
            var result = new OpResult();

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(result);
            _session.CanPerformAction(LaborManagementActionType.WriteTimePolicy).MergeInto(result);

            foreach (var overtime in overtimes)
            {
                _session.UnitOfWork.RegisterNew(new ClockClientOvertimeSelected
                {
                    ClockClientOvertimeSelectedId = CommonConstants.NEW_ENTITY_ID,
                    ClockClientOvertimeId = overtime.ClockClientOvertimeId,
                    ClockClientTimePolicyId = clockClientTimePolicyId
                });
            }

            _session.UnitOfWork.Commit().MergeInto(result);

            return result;
        }

        IOpResult ILaborManagementProvider.SaveNewClientShiftSelectedList(int clientId, int clockClientTimePolicyId,
            List<ClientShiftSelected> shifts)
        {
            var result = new OpResult();

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(result);
            _session.CanPerformAction(LaborManagementActionType.WriteTimePolicy).MergeInto(result);

            foreach (var shift in shifts)
            {
                _session.UnitOfWork.RegisterNew(new ClientShiftSelected
                {
                    ClockClientShiftSelectedId = CommonConstants.NEW_ENTITY_ID,
                    ClientShiftId = shift.ClientShiftId,
                    ClockClientTimePolicyId = clockClientTimePolicyId
                });
            }

            _session.UnitOfWork.Commit().MergeInto(result);

            return result;
        }

        IOpResult<IEnumerable<OnShiftPunchesLayout>> ILaborManagementProvider.GetBenefitRecordsByEmployeeDateRange(int employeeId, DateTime startDate, DateTime endDate)
        {
            return new OpResult<IEnumerable<OnShiftPunchesLayout>>(_session.UnitOfWork.LaborManagementRepository
                .ClockEmployeeBenefitQuery()
                .ByEmployeeId(employeeId)
                .ByDateRange(startDate, endDate)
                .ExecuteQueryAs(b => new OnShiftPunchesLayout
                {
                    punchDate = (DateTime)b.EventDate,
                    inPunch = null,
                    outPunch = null,
                    totalHoursWorked = b.Hours ?? 0
                })
                .GroupBy(g => g.punchDate.Date, (k, g) => new
                {
                    DayOfWeek = k.DayOfWeek,
                    Benefits = g
                })
                .Select(x => new OnShiftPunchesLayout
                {
                    punchDate = x.Benefits.First().punchDate,
                    inPunch = null,
                    outPunch = null,
                    totalHoursWorked = x.Benefits.Sum(b => b.totalHoursWorked)
                }));
        }

        IOpResult<IEnumerable<ClockClientScheduleDto>> ILaborManagementProvider.GetEmployeeSelectedSchedules(int employeeId)
        {
            return new OpResult<IEnumerable<ClockClientScheduleDto>>(_session.UnitOfWork.TimeClockRepository
                .GetClockEmployeeQuery()
                .ByEmployeeId(employeeId)
                .ExecuteQueryAs(x => new ClockEmployeeSetupDto
                {
                    EmployeeId = x.Employee.EmployeeId,
                    SelectedSchedules = x.Employee.SelectedSchedules.Select(s => new EmployeeScheduleSetupDto
                    {
                        ScheduleId = s.ClockClientScheduleId,
                        ScheduleDetails = new ClockClientScheduleDto
                        {
                            ClockClientScheduleId = s.ClockClientScheduleId,
                            StartTime = s.ClockSchedule.StartTime,
                            StopTime = s.ClockSchedule.StopTime,
                            StartDate = s.ClockSchedule.StartDate,
                            EndDate = s.ClockSchedule.EndDate,
                            IsIncludeOnHolidays = s.ClockSchedule.IsIncludeOnHolidays,
                            IsOverrideSchedules = s.ClockSchedule.IsOverrideSchedules,
                            IsSunday = s.ClockSchedule.IsSunday,
                            IsMonday = s.ClockSchedule.IsMonday,
                            IsTuesday = s.ClockSchedule.IsTuesday,
                            IsWednesday = s.ClockSchedule.IsWednesday,
                            IsThursday = s.ClockSchedule.IsThursday,
                            IsFriday = s.ClockSchedule.IsFriday,
                            IsSaturday = s.ClockSchedule.IsSaturday,
                            Name = s.ClockSchedule.Name,
                            PayType = s.ClockSchedule.PayType,
                            RecurEveryOption = s.ClockSchedule.RecurEveryOption,
                            RecurrenceOption = s.ClockSchedule.RecurrenceOption,
                            RepeatInterval = s.ClockSchedule.RepeatInterval
                        }
                    })
                })
                .FirstOrDefault()
                .SelectedSchedules
                .GroupBy(g => g.ScheduleId, (k, c) => new { ScheduleId = k, Schedules = c.Select(cc => cc.ScheduleDetails) })
                .SelectMany(x => x.Schedules));
        }

        IOpResult<PunchActivitySprocResults> ILaborManagementProvider.GetTcaEmployeePunchActivityList(TimeCardAuthorizationSearchOptions options)
        {
            var result = new OpResult<PunchActivitySprocResults>();

            var args = SetSprocArgs(options).MergeInto(result).Data;

            if (result.HasError) return result;

            return result.TrySetData(() => _session.UnitOfWork.LaborManagementRepository
                .GetClockEmployeePunchListByDateAndFilterPaginated(args));
        }

        IOpResult<EmployeePunchListCountAndResultLengthDto> ILaborManagementProvider.GetClockEmployeePunchListPaginationCount(TimeCardAuthorizationSearchOptions options)
        {
            var result = new OpResult<EmployeePunchListCountAndResultLengthDto>();

            var args = SetSprocArgs(options).MergeInto(result).Data;

            if (result.HasError) return result;

            var employeeLengthResult = Self.GetEmployeePagedResultLength(args).MergeInto(result).Data;

            if (result.HasError) return result;

            return result.SetDataOnSuccess(employeeLengthResult);
        }

        IOpResult<EmployeePunchListCountAndResultLengthDto> ILaborManagementProvider.GetEmployeePagedResultLength(ClockEmployeePunchListByDateAndFilterPaginatedCountArgs args)
        {
            var result = new OpResult<EmployeePunchListCountAndResultLengthDto>();
            return result.TrySetData(() =>
                _session.UnitOfWork.LaborManagementRepository.GetClockEmployeePunchListByDateAndFilterCount(args));
        }

        private IOpResult<ClockEmployeePunchListByDateAndFilterPaginatedCountArgs> SetSprocArgs(TimeCardAuthorizationSearchOptions options)
        {
            var result = new OpResult<ClockEmployeePunchListByDateAndFilterPaginatedCountArgs>();

            var dates = CalculatePayPeriodDates(options).MergeInto(result).Data;

            if (result.HasError) return result;

            int specialOption;

            var employeeFilter = options.Category1Dropdown.Value;
            if (employeeFilter == (int)TCAEmployeeFilterType.CostCenter || employeeFilter == (int)TCAEmployeeFilterType.Department
                || employeeFilter == (int)TCAEmployeeFilterType.Group || employeeFilter == (int)TCAEmployeeFilterType.Shift)
            {
                specialOption = 1;
            }
            else
            {
                specialOption = 4;
            }

            var args = new ClockEmployeePunchListByDateAndFilterPaginatedCountArgs
            {
                ClientId = options.ClientId,
                UserId = _session.LoggedInUserInformation.UserId,
                StartDate = dates.StartDate,
                EndDate = dates.EndDate,
                Filter = options.Filter1Dropdown.Value,
                FilterCategory = options.Category1Dropdown.Value,
                FilterCategory2 = options.Category2Dropdown.Value,
                Filter2 = options.Filter2Dropdown.Value,
                PageSize = options.PageSize ?? 10,
                SpecialOption = specialOption
            };

            return result.SetDataOnSuccess(args);
        }
        private IOpResult<TcaPayPeriodDates> CalculatePayPeriodDates(TimeCardAuthorizationSearchOptions options)
        {
            var result = new OpResult<TcaPayPeriodDates>();
            DateTime startDate;
            DateTime endDate;

            if (options.PayPeriodDropdownSelectedValue == 0)
            {
                var sprocArgs = new GetTimeClockCurrentPeriodArgsDto
                {
                    ClientID = options.ClientId
                };

                var currentPayPeriod = _session.UnitOfWork.LaborManagementRepository
                    .GetTimeClockCurrentPeriod(sprocArgs);

                if (currentPayPeriod == null) return result.SetToFail("Unable to find specified pay period.");

                startDate = currentPayPeriod.StartDate;
                endDate = currentPayPeriod.EndDate;
            }
            else if (options.PayPeriodDropdownSelectedValue == 2)
            {
                startDate = options.StartDate;
                endDate = options.EndDate;
            }
            else
            {
                var parsedDates = options.PayPeriodDropdownSelectedItemText.Split('-');
                var parsedStart = parsedDates.FirstOrDefault()?.Trim();
                var parsedEnd = parsedDates.LastOrDefault()?.Trim();

                if (string.IsNullOrWhiteSpace(parsedStart) || string.IsNullOrWhiteSpace(parsedEnd))
                    return result.SetToFail("Unable to find specified pay period.");

                startDate = DateTime.Parse(parsedStart);
                endDate = DateTime.Parse(parsedEnd);
            }

            return result.SetDataOnSuccess(new TcaPayPeriodDates
            {
                StartDate = startDate,
                EndDate = endDate
            });
        }

        IOpResult ILaborManagementProvider.DeleteFutureHolidayHours(int timePolicyId, int clockClientHolidayId, DateTime startDate)
        {
            var result = new OpResult();

            result.TryCatch(() =>
            {
                // Get employees with the given time policy
                var employees = _session.UnitOfWork.TimeClockRepository
                    .GetClockEmployeeQuery()
                    .ByTimePolicyId(timePolicyId)
                    .ExecuteQueryAs(e => new ClockEmployeeDto
                    {
                        EmployeeId = e.EmployeeId
                    })
                    .ToOrNewList();

                if (!employees.Any()) return;

                // For the given holiday rule, get the holidays that have not yet occurred based on the given start date and the event date.
                var holidayDetail = _session.UnitOfWork.LaborManagementRepository
                    .ClockClientHolidayDetailQuery()
                    .ByClockClientHolidayId(clockClientHolidayId)
                    .ByEventDateOnOrAfter(startDate)
                    .ExecuteQueryAs(h => new ClockClientHolidayDetailDto
                    {
                        ClockClientHolidayDetailId = h.ClockClientHolidayDetailId,
                        ClockClientHolidayId = h.ClockClientHolidayId,
                        EventDate = h.EventDate
                    })
                    .ToOrNewList();

                if (!holidayDetail.Any()) return;

                // Get ClockEmployeeBenefit entities for the employees and the holidays that have not yet occurred 
                var benefits = _session.UnitOfWork.LaborManagementRepository
                    .ClockEmployeeBenefitQuery()
                    .ByClockClientHolidayDetailIdExists()
                    .ByEmployeeIds(employees.Select(e => e.EmployeeId).ToArray())
                    .ByHolidayDetailList(holidayDetail.Select(h => h.ClockClientHolidayDetailId).ToOrNewList())
                    .ExecuteQuery()
                    .ToOrNewList();

                if (!benefits.Any()) return;

                // Delete the ClockEmployeeBenefit entities for the holidays that have not yet occurred
                foreach (var benefit in benefits)
                {
                    _session.UnitOfWork.RegisterDeleted(benefit);
                }
            });

            return result;
        }

        IOpResult ILaborManagementProvider.InsertFutureHolidayHours(int timePolicyId, int clockClientHolidayId,
            DateTime startDate)
        {
            var result = new OpResult();

            result.TryCatch(() => 
            { 
                // Get employees with the given time policy
                var employees = _session.UnitOfWork.TimeClockRepository
                    .GetClockEmployeeQuery()
                    .ByTimePolicyId(timePolicyId)
                    .ByActiveEmployee(true)
                    .ExecuteQueryAs(e => new EmployeeFullDto
                    {
                        EmployeeId = e.EmployeeId,
                        ClientId = e.ClientId,
                        HireDate = e.Employee.HireDate,
                        RehireDate = e.Employee.RehireDate,
                        AnniversaryDate = e.Employee.AnniversaryDate
                    })
                    .ToOrNewList();

                if (!employees.Any()) return;

                // For the given holiday rule, get the paid holidays that have not yet occurred based on the given start date and the event date
                var holidays = _session.UnitOfWork.LaborManagementRepository
                    .ClockClientHolidayDetailQuery()
                    .ByClockClientHolidayId(clockClientHolidayId)
                    .ByEventDateOnOrAfter(startDate)
                    .ByIsPaid()
                    .ExecuteQueryAs(h => new ClockClientHolidayDetailDto
                    {
                        ClockClientHolidayDetailId = h.ClockClientHolidayDetailId,
                        EventDate = h.EventDate,
                        OverrideHours = h.OverrideHours,
                        OverrideClientEarningId = h.OverrideClientEarningId,
                        ClockClientHoliday = new ClockClientHolidayDto
                        {
                            ClockClientHolidayId = h.ClockClientHolidayId,
                            ClientEarningId = h.ClockClientHoliday.ClientEarningId,
                            Hours = h.ClockClientHoliday.Hours,
                            HolidayWaitingPeriodDateId = h.ClockClientHoliday.HolidayWaitingPeriodDateId,
                            WaitingPeriod = h.ClockClientHoliday.WaitingPeriod
                        }
                    })
                    .ToOrNewList();

                if (!holidays.Any()) return;

                // Loop thru each employee and each holiday and create benefit records
                foreach (var employee in employees)
                {
                    foreach (var holiday in holidays)
                    {

                        // Determine the earning ID that should be used
                        var clientEarningId = holiday.OverrideClientEarningId ??
                                              holiday.ClockClientHoliday.ClientEarningId;

                        if (!employee.ClientId.HasValue || !clientEarningId.HasValue) continue;

                        var waitingPeriodDate = CommonConstants.NO_DATE_SELECTED_DT;

                        switch (holiday.ClockClientHoliday.HolidayWaitingPeriodDateId)
                        {
                            // Determine which date to use to determine if EE is eligible for the holiday
                            case 1:
                                waitingPeriodDate = employee.RehireDate ??
                                                    employee.HireDate.GetValueOrDefault();
                                break;
                            case 2:
                                waitingPeriodDate = employee.AnniversaryDate ??
                                                    employee.RehireDate ??
                                                    employee.HireDate.GetValueOrDefault();
                                break;
                        }

                        // Check if the holiday occurs before the employee is eligible for the holiday
                        if (holiday.EventDate < waitingPeriodDate.AddDays(holiday.ClockClientHoliday.WaitingPeriod))
                            continue;

                        // Create the ClockEmployeeBenefit record for the holiday
                        var benefit = new ClockEmployeeBenefit
                        {
                            EmployeeId = employee.EmployeeId,
                            ClientEarningId = clientEarningId.Value,
                            Hours = holiday.OverrideHours.HasValue && holiday.OverrideHours >= 0 ? holiday.OverrideHours : holiday.ClockClientHoliday.Hours,
                            EventDate = holiday.EventDate,
                            IsApproved = true,
                            ClockClientHolidayDetailId = holiday.ClockClientHolidayDetailId,
                            Comment = "Auto Generated Holiday",
                            ClientId = employee.ClientId.Value,
                            ApprovedBy = _session.LoggedInUserInformation.UserId
                        };

                        _session.SetModifiedProperties(benefit);

                        _session.UnitOfWork.RegisterNew(benefit);
                    }
                }
            });

            return result;
        }
    }

    class TcaPayPeriodDates
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
