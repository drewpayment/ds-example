using Dominion.Core.Dto.Labor;
using Dominion.Core.Services.Interfaces;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Entities.TimeClock;
using Dominion.LaborManagement.Service.Internal.Providers;
using Dominion.LaborManagement.Service.Internal.Security;
using Dominion.LaborManagement.Service.Internal.Validation;
using Dominion.LaborManagement.Service.Mapping.Clock;
using Dominion.Utility.ExtensionMethods;
using Dominion.Utility.OpResult;
using Dominion.Utility.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dominion.Core.Dto.Client;
using Dominion.Core.Dto.Employee;
using Dominion.Core.Dto.Misc;
using Dominion.Core.Dto.Payroll;
using Dominion.Core.Dto.TimeClock;
using Dominion.Core.Dto.User;
using Dominion.Core.Services.Api.DataServicesInjectors;
using Dominion.Core.Services.Dto.Employee;
using Dominion.Domain.Entities.Clients;
using Dominion.LaborManagement.Dto.Clock;
using Dominion.LaborManagement.Service.Api.DataServicesInjectors;
using Dominion.Utility.Constants;
using Dominion.Utility.Msg.Specific;
using ClockClientTimePolicySearchLists = Dominion.Core.Dto.Labor.ClockClientTimePolicySearchLists;
using OpResult = Dominion.Utility.OpResult.OpResult;
using Dominion.Core.Dto.Sprocs;
using Dominion.Core.Dto.TimeCard;
using Dominion.LaborManagement.Dto.Sproc;
using PunchOptionType = Dominion.Core.Dto.Labor.PunchOptionType;
using Dominion.Core.Services.Internal.Providers;
using Dominion.LaborManagement.Dto.ApplicantTracking;
using Dominion.Core.Dto.TimeCard.Result;
using Dominion.LaborManagement.Dto.Enums;
using System.Data.SqlClient;
using Dominion.Utility.Configs;
using Microsoft.SqlServer.Server;
using Dominion.LaborManagement.EF.Query;


namespace Dominion.LaborManagement.Service.Api
{
    public class LaborManagementService : ILaborManagementService
    {
        private readonly IBusinessApiSession _session;
        private readonly ILaborManagementProvider _provider;
        private readonly ILaborManagementValidationProvider _validationProvider;
        private readonly IEmployeePunchProvider _employeePunchProvider;
        private readonly IDsDataServiceClockClientService _dsClockClientService;
        private readonly IDlEmployeeApproveHours _dlEmployeeApproveHours;
        private readonly IUserManager _userService;
        private readonly IClockService _clockService;
        private ILaborManagementService Self;
        private readonly IClientSettingProvider _clientSettingsProvider;
        private const string DivisionLabel = "Division";
        private const string DepartmentLabel = "Department";
        private const string CostCenterLabel = "Cost Center";
        private readonly IEmployeePunchProvider _punchProvider;

        internal LaborManagementService(
            IBusinessApiSession session, 
            ILaborManagementProvider provider, 
            ILaborManagementValidationProvider validationProvider, 
            IEmployeePunchProvider employeePunchProvider,
            IDsDataServiceClockClientService dsClockClientService,
            IDlEmployeeApproveHours employeeApproveHours,
            IUserManager userService,
            IClockService clockService,
            IEmployeePunchProvider punchProvider,
            IClientSettingProvider clientSettingsProvider
        ) {
            _session = session;
            _provider = provider;
            _validationProvider = validationProvider;
            _employeePunchProvider = employeePunchProvider;
            _dsClockClientService = dsClockClientService;
            _dlEmployeeApproveHours = employeeApproveHours;
            _userService = userService;
            _clockService = clockService;
            _clientSettingsProvider = clientSettingsProvider;
            _punchProvider = punchProvider;
            Self = this;
        }

        #region Shared

        IOpResult<IEnumerable<ClientEarningDto>> ILaborManagementService.GetClientEarnings(int clientId)
        {
            var result = new OpResult<IEnumerable<ClientEarningDto>>();

            _session.CanPerformAction(LaborManagementActionType.ReadTimePolicy).MergeInto(result);
            if (result.HasError) return result; 

            return result.TrySetData(() => _session.UnitOfWork.PayrollRepository
                    .QueryClientEarnings()
                    .ByClientId(clientId)
                    .ExecuteQueryAs(x => new
                    {
                        x.ClientEarningId,
                        x.Description,
                        x.IsActive
                    })
                    .Select(c => new ClientEarningDto
                    {
                        ClientEarningId = c.ClientEarningId,
                        Description = c.Description,
                        IsActive = c.IsActive
                    })
                    .ToList());
        }

        IOpResult ILaborManagementService.CanReadTimePolicy(int clientId)
        {
            var result = new OpResult();
            _session.CanPerformAction(LaborManagementActionType.ReadTimePolicy).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(result);
            return result;
        }

        #endregion

        #region Company ---> Notes

        IOpResult<IEnumerable<ClockClientNoteDto>> ILaborManagementService.GetClientNotes(int clientId)
        {
            var result = new OpResult<IEnumerable<ClockClientNoteDto>>();

            _session.CanPerformAction(LaborManagementActionType.ReadTimePolicy).MergeInto(result);
            if (result.HasError)
                return result;

            var notes = _session.UnitOfWork
                .LaborManagementRepository
                .ClockClientNotesQuery()
                .ByClient(clientId)
                .ExecuteQueryAs(new ClockClientNoteMaps.ToClockClientNoteDto())
                .ToList();

            //notes.OrderByDescending(x => x.IsActive).ThenBy(y => y.Note);

            result.Data = notes;

            return result;
            
        }

        IOpResult<ClockClientNoteDto> ILaborManagementService.SaveClockClientNote(ClockClientNoteDto dto)
        {
            var result = new OpResult<ClockClientNoteDto>();

            _session.CanPerformAction(LaborManagementActionType.EditTimePolicy).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, dto.ClientId).MergeInto(result);
            _validationProvider.ValidateClockClientNote(dto).MergeInto(result);

            if (result.HasError) return result;

            var entity = new ClockClientNote
            {
                ClientId           = dto.ClientId,
                Note               = dto.Note,
                IsHideFromEmployee = dto.IsHideFromEmployee,
                IsActive           = dto.IsActive
            };

            if (dto.IsNewEntity(e => e.ClockClientNoteId))
            {
                _provider.RegisterNewClockClientNote(entity, dto).MergeInto(result);
            }
            else
            {
                _provider.RegisterExistingClockClientNote(entity, dto).MergeInto(result);
            }

            if (result.Success)
            {
                _session.UnitOfWork.Commit().MergeInto(result);
                result.SetDataOnSuccess(dto);
            }

            return result;
        }

        /// <summary>
        /// Attempts to delete an existing note.  Currently does not check if note is in use.
        /// </summary>
        /// <param name="clockClientNoteId"></param>
        /// <returns></returns>
        IOpResult ILaborManagementService.DeleteClockClientNote(int clockClientNoteId)
        {
            var result = new OpResult();

            _session.CanPerformAction(LaborManagementActionType.WriteTimePolicy).MergeInto(result);
            if (result.HasError) return result;

            // check if exists
            var current = _provider.GetClockClientNote(clockClientNoteId).MergeInto(result).Data;
            if (result.CheckForNotFound(current).HasError) return result;

            // check if user has access to client being modified
            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, current.ClientId).MergeInto(result);
            if (result.HasError) return result;

            // check if any dependent tables use the note
            _provider.CheckUsageForClockClientNote(current).MergeInto(result);

            if (result.Success)
            {
                _session.UnitOfWork.RegisterDeleted(new ClockClientNote { ClockClientNoteId = clockClientNoteId });
                _session.UnitOfWork.Commit().MergeInto(result);
            }

            return result;
        }

        #endregion

        #region Company ---> Overtime Rule

        IOpResult ILaborManagementService.DeleteClockClientOvertime(int clockClientOvertimeId)
        {
            var result = new OpResult();

            _session.CanPerformAction(LaborManagementActionType.WriteTimePolicy).MergeInto(result);
            if (result.HasError) return result;

            var current = _provider.GetClockClientOvertime(clockClientOvertimeId).MergeInto(result).Data;
            if (result.CheckForNotFound(current).HasError) return result;

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, current.ClientId).MergeInto(result);
            if (result.HasError) return result;

            _provider.CheckUsageForClockClientOvertimeSelected(current).MergeInto(result);

            if (result.Success)
            {
                _session.UnitOfWork.RegisterDeleted(new ClockClientOvertime
                {
                    ClockClientOvertimeId = clockClientOvertimeId
                });
                _session.UnitOfWork.Commit().MergeInto(result);
            }

            return result;
        }

        IOpResult<ClockClientOvertimeDto> ILaborManagementService.SaveClockClientOvertime(ClockClientOvertimeDto dto)
        {
            var result = new OpResult<ClockClientOvertimeDto>();
            _session.CanPerformAction(LaborManagementActionType.WriteTimePolicy).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, dto.ClientId).MergeInto(result);
            _validationProvider.ValidateClockClientOvertime(dto).MergeInto(result);

            if (result.HasError) return result;

            var entity = new ClockClientOvertime
            {
                ClockClientOvertimeId = dto.ClockClientOvertimeId,
                ClientId = dto.ClientId,
                ClientEarningId = dto.ClientEarningId,
                Name = dto.Name,
                ClockOvertimeFrequencyId = dto.ClockOvertimeFrequencyId,
                Hours = dto.Hours,
                IsSunday = dto.IsSunday,
                IsMonday = dto.IsMonday,
                IsTuesday = dto.IsTuesday,
                IsWednesday = dto.IsWednesday,
                IsThursday = dto.IsThursday,
                IsFriday = dto.IsFriday,
                IsSaturday = dto.IsSaturday
            };

            if (dto.IsNewEntity(x => x.ClockClientOvertimeId))
            {
                _provider.RegisterNewClockClientOvertime(entity, dto);
            }
            else
            {
                _provider.RegisterExistingClockClientOvertime(entity, dto);
            }

            if (result.Success)
            {
                _session.SetModifiedProperties(entity);
                _session.UnitOfWork.Commit().MergeInto(result);
                result.SetDataOnSuccess(dto);
            }

            return result;
        }

        IOpResult<IEnumerable<ClockClientOvertimeDto>> ILaborManagementService.GetOvertimeRules(int clientId)
        {
            var result = new OpResult<IEnumerable<ClockClientOvertimeDto>>();

            _session.CanPerformAction(LaborManagementActionType.ReadTimePolicy).MergeInto(result);
            if (result.HasError)
                return result;

            return result.TrySetData(_session.UnitOfWork
                .LaborManagementRepository
                .ClockClientOvertimeQuery()
                .ByClientId(clientId)
                .ExecuteQueryAs(new ClockClientOvertimeMaps.ToClockClientOvertimeDto())
                .ToList);
        }

        IOpResult<IEnumerable<ClockOvertimeFrequencyDto>> ILaborManagementService.GetOvertimeFrequencies(int clientId)
        {
            var result = new OpResult<IEnumerable<ClockOvertimeFrequencyDto>>();

            _session.CanPerformAction(LaborManagementActionType.ReadTimePolicy).MergeInto(result);
            if (result.HasError)
                return result;

            return result.TrySetData(_session.UnitOfWork
                .LaborManagementRepository
                .ClockOvertimeFrequencyQuery()
                .ExecuteQueryAs(x => new ClockOvertimeFrequencyDto()
                {
                    ClockOvertimeFrequencyId = x.ClockOvertimeFrequencyId,
                    OvertimeFrequency = x.OvertimeFrequency
                })
                .ToList);
        }

        #endregion

        #region Company ---> Exceptions

        IOpResult<IEnumerable<ClockExceptionTypeInfoDto>> ILaborManagementService.GetExceptions()
        {
            var result = new OpResult<IEnumerable<ClockExceptionTypeInfoDto>>();

            _session.CanPerformAction(LaborManagementActionType.ReadTimePolicy).MergeInto(result);
            if (result.HasError) return result;

            return result.TrySetData(_session.UnitOfWork
                .LaborManagementRepository
                .ClockExceptionQuery()
                .ExecuteQueryAs(new ClockExceptionMaps.ToClockExceptionDto())
                .ToList);
        }

        IOpResult<IEnumerable<ClientExceptionDetailDto>> ILaborManagementService.GetStandardExceptionsDetail(int clientId)
        {
            var resultDtos = new OpResult<List<ClientExceptionDetailDto>>();
            var result = new OpResult<IEnumerable<ClientExceptionDetailDto>>();

            _session.CanPerformAction(LaborManagementActionType.ReadTimePolicy).MergeInto(result);
            if (result.HasError) return result;

            result.TrySetData(_session.UnitOfWork.LaborManagementRepository
                .ClockExceptionQuery()
                .ExecuteQueryAs(new ClockExceptionMaps.ToClientExceptionDetailDto())
                .ToList);

            var clientLunches = _employeePunchProvider.GetClientLunches(clientId);

            foreach (var sl in result.Data.Where(x => x.ClockExceptionId == ClockExceptionType.Long || x.ClockExceptionId == ClockExceptionType.Short))
            {
                foreach (var l in clientLunches.Data)
                {
                    if (resultDtos.Data == null) resultDtos.Data = new List<ClientExceptionDetailDto>();
                    resultDtos.Data.Add(new ClientExceptionDetailDto
                    {
                        ClockClientExceptionDetailId = sl.ClockClientExceptionDetailId,
                        ClockClientExceptionId = sl.ClockClientExceptionId,
                        ClockExceptionId = sl.ClockExceptionId,
                        Amount = sl.Amount,
                        ClockExceptionType = sl.ClockExceptionType,
                        ClockClientLunchId = l.ClockClientLunchId,
                        ExceptionName = sl.ExceptionName + " " + l.Name,
                        IsSelected = sl.IsSelected,
                        HasAmountTextBox = sl.HasAmountTextBox,
                        HasPunchTimeOption = sl.HasPunchTimeOption,
                        PunchTimeOption = sl.PunchTimeOption,
                        IsHour = sl.IsHour
                    });
                }
            }

            // Remove standard exception types 'long' and 'short' that are only used for titles and 
            // should be matched to qualifying lunch types from the client lunch table
            result.Data = result.Data.Except(result.Data.Where(x => (x.ClockExceptionId == ClockExceptionType.Long 
                || x.ClockExceptionId == ClockExceptionType.Short) 
                && (x.ClockClientLunchId == null 
                || x.ClockClientLunchId <= 0)));
            
            if (result.Success && resultDtos.Data!=null) result.Data = result.Data.Concat(resultDtos.Data);

            return result;
        }

        IOpResult<IEnumerable<ClockClientExceptionDto>> ILaborManagementService.GetClientExceptionRules(int clientId)
        {
            var result = new OpResult<IEnumerable<ClockClientExceptionDto>>();

            _session.CanPerformAction(LaborManagementActionType.ReadTimePolicy).MergeInto(result);
            if (result.HasError) return result;

            return result.TrySetData(_session.UnitOfWork
                .LaborManagementRepository
                .ClockClientExceptionQuery()
                .ByClientId(clientId)
                .ExecuteQueryAs(new ClockClientExceptionMaps.ToClockClientExceptionDto())
                .ToList);
        }

        IOpResult<IEnumerable<ClientExceptionDetailDto>> ILaborManagementService.GetClockClientExceptionDetailList(
            int clockClientExceptionId, int clientId)
        {
            var result = new OpResult<IEnumerable<ClientExceptionDetailDto>>();
            var resultDtos = new List<ClientExceptionDetailDto>();

            var standardExceptions = _provider.GetStandardExceptionsAsExceptionDetail().Data.ToList();
            var clientLunches = _employeePunchProvider.GetClientLunches(clientId);
            var clockClientExceptionDetailList = _provider.GetClientExceptionDetailByClientException(clockClientExceptionId).Data.ToList();

            // Loop through standard exceptions that do not have anything to do with lunches,
            // look for existing detail records and then create dtos for each that will be returned.
            foreach (var se in standardExceptions)
            {
                var existing = clockClientExceptionDetailList.FirstOrDefault(x => x.ClockExceptionId == se.ClockExceptionId);
                if (existing != null && (se.ClockExceptionId != ClockExceptionType.Long && se.ClockExceptionId != ClockExceptionType.Short))
                {
                    resultDtos.Add(new ClientExceptionDetailDto
                    {
                        ClockClientExceptionDetailId = existing.ClockClientExceptionDetailId,
                        ClockClientExceptionId = existing.ClockClientExceptionId,
                        ClockExceptionId = existing.ClockExceptionId,
                        Amount = existing.Amount,
                        ClockExceptionType = se.ClockExceptionType,
                        ClockClientLunchId = existing.ClockClientLunchId,
                        ExceptionName = se.ExceptionName,
                        HasAmountTextBox = se.HasAmountTextBox,
                        HasPunchTimeOption = se.HasPunchTimeOption,
                        IsSelected = existing.IsSelected ?? false,
                        IsHour = se.IsHour,
                        PunchTimeOption = existing.PunchTimeOption
                    });
                }
                else if (se.ClockExceptionId != ClockExceptionType.Long && se.ClockExceptionId != ClockExceptionType.Short)
                {
                    resultDtos.Add(new ClientExceptionDetailDto
                    {
                        ClockClientExceptionDetailId = se.ClockClientExceptionDetailId,
                        ClockClientExceptionId = se.ClockClientExceptionId,
                        ClockExceptionId = se.ClockExceptionId,
                        Amount = se.Amount,
                        ClockExceptionType = se.ClockExceptionType,
                        ClockClientLunchId = se.ClockClientLunchId,
                        ExceptionName = se.ExceptionName,
                        IsSelected = se.IsSelected,
                        HasAmountTextBox = se.HasAmountTextBox,
                        HasPunchTimeOption = se.HasPunchTimeOption,
                        IsHour = se.IsHour,
                        PunchTimeOption = se.PunchTimeOption
                    });
                }
            }

            // Find standard exceptions that are related to lunch/break exceptions and then
            // loop through the resulting list. With each standard lunch/break type exception, we check 
            // to see if there are existing client records related. If there are none, we loop through 
            // all lunches for the particular client, building dtos for each lunch and standard lunch/break exception type.
            var standardLunchExceptions = standardExceptions.Where(x => x.ClockExceptionId == ClockExceptionType.Long || x.ClockExceptionId == ClockExceptionType.Short).ToList();
            foreach (var sl in standardLunchExceptions)
            {
                var existingLunches = new List<ClockClientLunchDto>();
                var existingDetail = clockClientExceptionDetailList.FirstOrDefault(x => x.ClockExceptionId == sl.ClockExceptionId);

                if (existingDetail != null)
                    existingLunches = clientLunches.Data.Where(x => x.ClockClientLunchId == existingDetail.ClockClientLunchId).ToList();

                if (existingLunches.Count > 0)
                {
                    foreach (var l in existingLunches)
                    {
                        resultDtos.Add(new ClientExceptionDetailDto
                        {
                            ClockClientExceptionDetailId = existingDetail.ClockClientExceptionDetailId,
                            ClockClientExceptionId = existingDetail.ClockClientExceptionId,
                            ClockExceptionId = existingDetail.ClockExceptionId,
                            Amount = existingDetail.Amount,
                            ClockExceptionType = sl.ClockExceptionType,
                            ClockClientLunchId = l.ClockClientLunchId,
                            ExceptionName = sl.ExceptionName + " " + l.Name,
                            IsSelected = existingDetail.IsSelected ?? false,
                            HasAmountTextBox = sl.HasAmountTextBox,
                            HasPunchTimeOption = sl.HasPunchTimeOption,
                            PunchTimeOption = existingDetail.PunchTimeOption,
                            IsHour = sl.IsHour
                        });
                    }
                }

                foreach (var l in clientLunches.Data)
                {
                    // we check to see if we have already added the detail record for a lunch and standard exception
                    // if there is a record already on the list, we skip this iteration and continue onto the next.
                    var existingLunchDetailDto = resultDtos.FirstOrDefault(x => x.ClockClientLunchId == l.ClockClientLunchId && x.ClockExceptionId == sl.ClockExceptionId);
                    if (existingLunchDetailDto != null) continue;

                    resultDtos.Add(new ClientExceptionDetailDto
                    {
                        ClockClientExceptionDetailId = sl.ClockClientExceptionDetailId,
                        ClockClientExceptionId = sl.ClockClientExceptionId,
                        ClockExceptionId = sl.ClockExceptionId,
                        Amount = sl.Amount,
                        ClockExceptionType = sl.ClockExceptionType,
                        ClockClientLunchId = l.ClockClientLunchId,
                        ExceptionName = sl.ExceptionName + " " + l.Name,
                        IsSelected = sl.IsSelected,
                        HasAmountTextBox = sl.HasAmountTextBox,
                        HasPunchTimeOption = sl.HasPunchTimeOption,
                        PunchTimeOption = sl.PunchTimeOption,
                        IsHour = sl.IsHour
                    });
                }
            }

            result.Data = resultDtos;
            result.SetToSuccess();
            return result;
        }

        IOpResult<ClockClientExceptionDto> ILaborManagementService.SaveClockClientException(ClockClientExceptionDto dto, int clientId)
        {
            var result = new OpResult<ClockClientExceptionDto>();
            _session.CanPerformAction(LaborManagementActionType.WriteTimePolicy).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(result);
            dto.ClientId = dto.ClientId > 0 ? dto.ClientId : clientId;
            _validationProvider.ValidateClockClientException(dto).MergeInto(result);

            if (result.HasError) return result;

            var entity = new ClockClientException
            {
                ClockClientExceptionId = dto.ClockClientExceptionId,
                ClientId = clientId,
                Name = dto.Name
            };

            if (dto.IsNewEntity(x => x.ClockClientExceptionId))
            {
                _provider.RegisterNewClockClientException(entity, dto);
            }
            else
            {
                _provider.RegisterExistingClockClientException(entity, dto);
            }

            foreach (var d in dto.ExceptionDetails)
            {
                
                var dEntity = new ClockClientExceptionDetail
                {
                    ClockClientExceptionDetailId = Convert.ToInt32(d.ClockClientExceptionDetailId),
                    ClockExceptionId = d.ClockExceptionId,
                    ClockClientLunchId = d.ClockClientLunchId,
                    Amount = d.Amount,
                    IsSelected = d.IsSelected,
                    PunchTimeOption = d.PunchTimeOption
                };

                if (entity.IsNewEntity(x => x.ClockClientExceptionId))
                    dEntity.ClientException = entity;
                else
                {
                    dEntity.ClockClientExceptionId = entity.ClockClientExceptionId;
                }
                
                if (dEntity.IsNewEntity(x => x.ClockClientExceptionDetailId))
                {
                    _provider.RegisterNewClockClientExceptionDetail(dEntity, d);
                }
                else
                {
                    _provider.RegisterExistingClockClientExceptionDetail(dEntity, d);
                }

                _session.SetModifiedProperties(dEntity);
            }

            if (!result.Success) return result;

            _session.SetModifiedProperties(entity);
            _session.UnitOfWork.Commit().MergeInto(result);
            result.SetDataOnSuccess(dto);
            return result;
        }


        IOpResult ILaborManagementService.DeleteClockClientException(int clientId, int clockClientExceptionId)
        {
            var result = new OpResult();
            _session.CanPerformAction(LaborManagementActionType.WriteTimePolicy).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(result);
            if (result.HasError) return result;

            var current = _provider.GetClockClientException(clockClientExceptionId).MergeInto(result).Data;
            if (result.CheckForNotFound(current).HasError) return result;

            _provider.CheckUsageForClockClientException(current).MergeInto(result);
            if (result.HasError) return result;

            var currDetails = _provider.GetClientExceptionDetailByClientException(clockClientExceptionId).Data;
            foreach (var c in currDetails)
            {
                if (result.Success)
                    _session.UnitOfWork.RegisterDeleted(new ClockClientExceptionDetail { ClockClientExceptionDetailId = c.ClockClientExceptionDetailId });
            }

            if (result.HasError) return result;
            _session.UnitOfWork.RegisterDeleted(new ClockClientException { ClockClientExceptionId = clockClientExceptionId });
            _session.UnitOfWork.Commit().MergeInto(result);
            return result;
        }

        IOpResult<IEnumerable<GetClockEmployeeExceptionHistoryByEmployeeIDListDto>> ILaborManagementService.GetStandardExceptionsDetailByRange(int clientId, DateTime startDate, DateTime endDate, IEnumerable<int> employeeIds)
        {
            var result = new OpResult<IEnumerable<GetClockEmployeeExceptionHistoryByEmployeeIDListDto>>();
            if (employeeIds.Count() > 0)
            {
                var dvwExceptionHistorySorted = _session.UnitOfWork.EmployeeRepository.GetClockEmployeeExceptionHistoryByEmployeeIDList(new GetClockEmployeeExceptionHistoryByEmployeeIDListArgsDto()
                {
                    ClientID = clientId,
                    EmployeeIDs = GetEmployeeIDListTVP(employeeIds.ToList()),
                    StartDate = startDate,
                    EndDate = endDate
                });
                result.Data = dvwExceptionHistorySorted;
            }
            else
            {
                result.Data = null;
            }

            if (result.HasData)
            {
                result.Data = result.Data.OrderBy(s => s.FullName).ToList();
            }

            return result;
        }

        private List<SqlDataRecord> GetEmployeeIDListTVP(List<int> employeeIds)
        {
            var idList = new List<Microsoft.SqlServer.Server.SqlDataRecord>();
            var tvp = new SqlMetaData("nnnnnn", SqlDbType.Int);

            foreach( var id in employeeIds)
            {
                var rec = new SqlDataRecord(tvp);
                rec.SetInt32(0, id);
                idList.Add(rec);
            }
            if (employeeIds.Any())
            {
                return idList;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Company Holidays

        IOpResult<CompanyHolidayDto> ILaborManagementService.GetCompanyHolidayVmDto(int clientId, Core.Dto.Payroll.ClientEarningCategory clientEarningType)
        {
            var user = _session.UnitOfWork.UserRepository
                .GetUser(_session.LoggedInUserInformation.UserId);

            var result = new OpResult<CompanyHolidayDto>(new CompanyHolidayDto());

            result.Data.HolidayRules = _session.UnitOfWork.LaborManagementRepository
                .ClockClientHolidayQuery()
                .ByClient(clientId)
                .ExecuteQueryAs(x => new ClockClientHolidayDto
                {
                    ClockClientHolidayId = x.ClockClientHolidayId,
                    ClientId = x.ClientId,
                    Name = x.Name,
                    ClientEarningId = x.ClientEarningId,
                    HolidayWaitingPeriodDateId = x.HolidayWaitingPeriodDateId,
                    HolidayWorkedClientEarningId = x.HolidayWorkedClientEarningId,
                    WaitingPeriod = x.WaitingPeriod,
                    Hours = x.Hours,
                    ClientHolidayDetails = x.ClientHolidayDetails.Select(h => new ClockClientHolidayDetailDto
                    {
                        ClockClientHolidayId = h.ClockClientHolidayId,
                        ClockClientHolidayDetailId = h.ClockClientHolidayDetailId,
                        ClientHolidayName = h.ClientHolidayName,
                        EventDate = h.EventDate,
                        IsPaid = h.IsPaid,
                        OverrideHours = h.OverrideHours,
                        OverrideClientEarningId = h.OverrideClientEarningId,
                        OverrideHolidayWorkedClientEarningId = h.OverrideHolidayWorkedClientEarningId
                    }).ToList()
                }).ToList();

            result.Data.HolidayEarnings = _session.UnitOfWork.PayrollRepository
                .QueryClientEarnings()
                .ByClientId(clientId)
                .ByEarningCategoryId(clientEarningType)
                .ExecuteQueryAs(x => new ClientEarningDto
                {
                    ClientEarningId = x.ClientEarningId,
                    Description = x.Description,
                    Code = x.Code
                })
                .ToList();

            if (user.UserTypeId == UserType.SystemAdmin)
            {
                result.Data.HolidayWorkedEarnings = _session.UnitOfWork.PayrollRepository
                    .QueryClientEarnings()
                    .ByClientId(clientId)
                    .ExecuteQueryAs(x => new ClientEarningDto
                    {
                        ClientEarningId = x.ClientEarningId,
                        Description = x.Description,
                        Code = x.Code
                    })
                    .ToList();
            }
            else if (user.UserTypeId > UserType.SystemAdmin)
            {
                result.Data.HolidayWorkedEarnings = _session.UnitOfWork.PayrollRepository
                    .QueryClientEarnings()
                    .ByClientId(clientId)
                    .ByActivity(true)
                    .ExecuteQueryAs(x => new ClientEarningDto
                    {
                        ClientEarningId = x.ClientEarningId,
                        Description = x.Description,
                        Code = x.Code
                    })
                    .ToList();
            }
            else
            {
                result.Data.HolidayWorkedEarnings = default(List<ClientEarningDto>);
            }

            result.Data.DefaultHolidays = result.Data.DefaultHolidays ?? _provider.GetAvailableHolidayYears().MergeInto(result).Data.ToList();

            var minDefaultYear = result.Data.DefaultHolidays.Select(h => h.DateObserved.Year).Min();
            
            foreach (var rule in result.Data.HolidayRules)
            {
                var mergeDates = new List<ClockClientHolidayDetailDto>();
                // build list of rule years
                rule.RuleYears = rule.ClientHolidayDetails
                    ?.DistinctBy(x => x.EventDate.Year)
                    ?.Select(e => e.EventDate.Year)
                    ?.ToList();

                var minRuleYear = rule.RuleYears?.Count > 0 ?
                    rule.RuleYears.Aggregate((curr, x) => curr < x ? curr : x) : 0;

                var resultYear = minRuleYear > 0 && minRuleYear < minDefaultYear ? minRuleYear : minDefaultYear;

                if (resultYear < minDefaultYear) result.Data.DefaultHolidays = _provider.GetAvailableHolidayYears(resultYear).MergeInto(result).Data.ToList();

                var defaultYears = result.Data.DefaultHolidays.Select(x => x.DateObserved.Year).ToList();
                if(defaultYears.Any()) rule.RuleYears.AddRange(defaultYears);
                rule.RuleYears = rule.RuleYears?.Distinct().ToList();

                // merge default holidays into clock client holiday details... 
                foreach(var d in result.Data.DefaultHolidays)
                {
                    if (!rule.ClientHolidayDetails.Any(chd => chd.EventDate.Date == d.DateObserved.Date))
                        mergeDates.Add(new ClockClientHolidayDetailDto
                        {
                            ClockClientHolidayDetailId = default(int),
                            ClientHolidayName = d.Holiday.HolidayName,
                            ClockClientHolidayId = rule.ClockClientHolidayId,
                            EventDate = d.DateObserved,
                            IsPaid = false,
                            OverrideHours = null,
                            OverrideClientEarningId = null,
                            OverrideHolidayWorkedClientEarningId = null
                        });
                }

                if (mergeDates.Any())
                {
                    if (rule.ClientHolidayDetails.Any())
                        rule.ClientHolidayDetails = rule.ClientHolidayDetails.Concat(mergeDates);
                    else
                        rule.ClientHolidayDetails = mergeDates;
                }
            }

            return result;
        }

        IOpResult<ClockClientHolidayDto> ILaborManagementService.SaveClockClientHoliday(ClockClientHolidayDto dto)
        {
            var result = new OpResult<ClockClientHolidayDto>();

            // if we are editing a holiday rule, we check the edit time policy action type
            if (dto.ClockClientHolidayId > 0)
            {
                _session.CanPerformAction(LaborManagementActionType.EditTimePolicy).MergeInto(result);
            }
            else
            {
                _session.CanPerformAction(LaborManagementActionType.WriteTimePolicy).MergeInto(result);
            }

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, dto.ClientId).MergeInto(result);
            _validationProvider.ValidateClockClientHoliday(dto).MergeInto(result);

            if (result.HasError) return result;

            var entity = new ClockClientHoliday
            {
                ClockClientHolidayId = dto.ClockClientHolidayId,
                ClientId = dto.ClientId,
                HolidayWaitingPeriodDateId = dto.HolidayWaitingPeriodDateId,
                ClientEarningId = dto.ClientEarningId,
                HolidayWorkedClientEarningId = dto.HolidayWorkedClientEarningId,
                Hours = dto.Hours?.GetType() == typeof(string) ? Convert.ToDouble(dto.Hours) : dto.Hours,
                Name = dto.Name,
                WaitingPeriod = dto.WaitingPeriod
            };

            if (dto.IsNewEntity(x => x.ClockClientHolidayId))
            {
                _session.UnitOfWork.RegisterNew(entity);
                _session.UnitOfWork.RegisterPostCommitAction(() =>
                    dto.ClockClientHolidayId = entity.ClockClientHolidayId);
            }
            else
            {
                _provider.RegisterExistingClockClientHoliday(entity, dto);
            }

            if (result.HasError) return result;

            _session.SetModifiedProperties(entity);
            _session.UnitOfWork.Commit().MergeInto(result);

            if (result.HasError) return result;

            return result.TrySetData(() => _session.UnitOfWork.LaborManagementRepository
                .ClockClientHolidayQuery()
                .ByClockClientHolidayId(dto.ClockClientHolidayId)
                .ExecuteQueryAs(new ClockClientHolidayMaps.ToHolidayDtoWithDetail())
                .FirstOrDefault());
        }

        IOpResult<IEnumerable<ClockClientHolidayDetailDto>> ILaborManagementService.SaveClockClientHolidayDetailList(
            IEnumerable<ClockClientHolidayDetailDto> dtos, int clientId)
        {
            var result = new OpResult<IEnumerable<ClockClientHolidayDetailDto>>();

            var index = 0;
            var dtoList = dtos.ToList();
            foreach(var d in dtos)
            {
                dtoList[index] = Self.SaveClockClientHolidayDetail(d, clientId, true).MergeInto(result).Data;
                index++;
            }

            _session.UnitOfWork.Commit().MergeInto(result);

            if (result.HasError) return result;

            return result.SetDataOnSuccess(dtoList);
        }

        IOpResult<ClockClientHolidayDetailDto> ILaborManagementService.SaveClockClientHolidayDetail(
            ClockClientHolidayDetailDto dto, int clientId, bool holdCommit)
        {
            var result = new OpResult<ClockClientHolidayDetailDto>();

            if (dto.ClockClientHolidayId != CommonConstants.NEW_ENTITY_ID)
                _session.CanPerformAction(LaborManagementActionType.EditTimePolicy).MergeInto(result);
            else
                _session.CanPerformAction(LaborManagementActionType.WriteTimePolicy).MergeInto(result);
            
            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(result);
            _validationProvider.ValidateClockClientHolidayDetail(dto).MergeInto(result);

            if (result.HasError) return result;

            var entity = new ClockClientHolidayDetail
            {
                ClockClientHolidayDetailId = dto.ClockClientHolidayDetailId,
                ClockClientHolidayId = dto.ClockClientHolidayId,
                ClientHolidayName = dto.ClientHolidayName,
                EventDate = dto.EventDate.Date,
                IsPaid = dto.IsPaid,
                OverrideHours = dto.OverrideHours,
                OverrideClientEarningId = dto.OverrideClientEarningId,
                OverrideHolidayWorkedClientEarningId = dto.OverrideHolidayWorkedClientEarningId
            };

            if (dto.ClockClientHolidayDetailId == CommonConstants.NEW_ENTITY_ID)
            {
                _session.UnitOfWork.RegisterNew(entity);
                _session.UnitOfWork.RegisterPostCommitAction(() =>
                {
                    dto.ClockClientHolidayDetailId = entity.ClockClientHolidayDetailId;
                    dto.EventDate = entity.EventDate;
                });
            }
            else
            {
                _provider.RegisterExistingClockClientHolidayDetail(entity, dto);
            }

            if(!holdCommit) _session.UnitOfWork.Commit().MergeInto(result);
            
            return result.SetDataOnSuccess(dto);
        }

        IOpResult ILaborManagementService.DeleteCompanyHolidayRule(int clockClientHolidayId)
        {
            var result = new OpResult();

            _session.CanPerformAction(LaborManagementActionType.WriteTimePolicy).MergeInto(result);
            if (result.HasError) return result;

            var curr = _provider.GetClockClientHoliday(clockClientHolidayId).MergeInto(result).Data;
            if (result.CheckForNotFound(curr).HasError) return result;

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, curr.ClientId).MergeInto(result);
            if (result.HasError) return result;

            _provider.CheckUsageForClockClientHoliday(curr).MergeInto(result);

            if (result.HasError) return result;

            Self.DeleteCompanyHolidayDetail(clockClientHolidayId, true).MergeInto(result);
            if (result.HasError) return result;

            _session.UnitOfWork.RegisterDeleted(new ClockClientHoliday()
            {
                ClockClientHolidayId = clockClientHolidayId
            });

            _session.UnitOfWork.Commit().MergeInto(result);

            return result;
        }

        IOpResult ILaborManagementService.DeleteCompanyHolidayDetail(int clockClientHolidayId, bool holdCommit)
        {
            var result = new OpResult();

            _session.CanPerformAction(LaborManagementActionType.EditTimePolicy).MergeInto(result);
            if (result.HasError) return result;

            var currList = _provider.GetClockClientHolidayDetailList(clockClientHolidayId).MergeInto(result).Data.ToList();

            if (result.HasError || !currList.Any()) return result;

            if (!holdCommit) _provider.CheckUsageForClockClientHolidayDetail(currList.First()).MergeInto(result);

            if (result.HasError) return result;

            foreach (var curr in currList)
            {
                _provider.DeleteRelatedClockEmployeeBenefitRecords(curr.ClockClientHolidayDetailId, holdCommit);
                _session.UnitOfWork.RegisterDeleted(new ClockClientHolidayDetail()
                {
                    ClockClientHolidayDetailId = curr.ClockClientHolidayDetailId,
                    ClockClientHolidayId = clockClientHolidayId
                });
            }

            if (holdCommit) return result;

            _session.UnitOfWork.Commit().MergeInto(result);

            return result;
        }

        IOpResult ILaborManagementService.DeleteCompanyHolidayDetail(int clockClientHolidayDetailId)
        {
            var result = new OpResult();

            _session.CanPerformAction(LaborManagementActionType.EditTimePolicy).MergeInto(result);
            if (result.HasError) return result;

            var curr = _provider.GetClockClientHolidayDetail(clockClientHolidayDetailId).MergeInto(result).Data;

            _provider.DeleteRelatedClockEmployeeBenefitRecords(curr.ClockClientHolidayDetailId).MergeInto(result);
            if (result.HasError) return result;

            _session.UnitOfWork.RegisterDeleted(new ClockClientHolidayDetail
            {
                ClockClientHolidayDetailId = curr.ClockClientHolidayDetailId,
                ClockClientHolidayId = curr.ClockClientHolidayId
            });

            _session.UnitOfWork.Commit().MergeInto(result);

            return result;
        }

        IOpResult ILaborManagementService.ApplyHolidayRule(ClockClientHolidayDto dto, int selectedYear)
        {
            var result = new OpResult();
            var startDate = DateTime.Now.Year == selectedYear ? DateTime.Now.Date : new DateTime(selectedYear, 1, 1);
            var endDate = new DateTime(selectedYear, 12, 31).Date;

            var timepolicyIds = _session.UnitOfWork.LaborManagementRepository
                .ClockClientTimePolicyQuery()
                .ByClockClientHoliday(dto.ClockClientHolidayId)
                .ExecuteQueryAs(x => new ClockClientTimePolicyDtos.ClockClientTimePolicyDto
                {
                    ClockClientTimePolicyId = x.ClockClientTimePolicyId,
                    ClientId = x.ClientId,
                    ClockClientHolidayId = x.ClockClientHolidayId
                })
                .Select(t => t.ClockClientTimePolicyId)
                .ToList();
                
            var employees = _session.UnitOfWork.EmployeeRepository
                .QueryEmployees()
                .ByClientId(dto.ClientId)
                .ByTimePolicyList(timepolicyIds)
                .ByActiveStatus()
                .ExecuteQueryAs(x => new ClockEmployeeDto
                {
                    EmployeeId = x.EmployeeId,
                    ClientId = x.ClientId,
                    ClockClientTimePolicyId = x.ClockEmployee.ClockClientTimePolicyId,
                    HireDate = x.HireDate,
                    RehireDate = x.RehireDate,
                    AnniversaryDate = x.AnniversaryDate
                })
                .ToList();

            _session.UnitOfWork.NoChangeTracking();

            foreach (var e in employees)
            {
                var currClockBenefits = _session.UnitOfWork.LaborManagementRepository
                    .ClockEmployeeBenefitQuery()
                    .ByEmployeeId(e.EmployeeId)
                    .ByDateRange(startDate, endDate) //Handles and edge case where a holiday may be updated to a later date, and an existing holiday record (with a future date) is not deleted.
                    .ByClockClientHolidayDetailIdExists() //Where HolidayDetailId != null
                    .ExecuteQueryAs(x => new ClockEmployeeBenefitDto
                    {
                        ClockEmployeeBenefitId = x.ClockEmployeeBenefitId,
                        ClockClientHolidayDetailId = x.ClockClientHolidayDetailId,
                        ClientId = x.ClientId,
                        EmployeeId = x.EmployeeId

                    })
                    .Select(ccb => ccb.ClockEmployeeBenefitId)
                    .ToList();

                _provider.DeleteClockEmployeeBenefits(currClockBenefits, dto.ClientId);

                //Need to calculate the date the employee is eligible to start receiving Holidays
                DateTime eligibleDate;
                if (dto.HolidayWaitingPeriodDateId == 1)
                { 
                    //ID 1 = RehireDate > HireDate Option
                    eligibleDate = (e.RehireDate ?? e.HireDate.GetValueOrDefault());
                }
                else
                {
                    //ID 2 = AnniversaryDate > RehireDate > HireDate
                    eligibleDate = (e.AnniversaryDate ?? e.RehireDate ?? e.HireDate.GetValueOrDefault());
                }

                eligibleDate = eligibleDate.AddDays(dto.WaitingPeriod);

                // Get all detail records that are paid, occur after the employee is eligible and that have not already occurred.
                foreach (var detail in dto.ClientHolidayDetails.Where(h => h.IsPaid && h.EventDate >= eligibleDate && (h.EventDate >= startDate && h.EventDate <= endDate)).ToList())
                {
                    var cebDto = new ClockEmployeeBenefitDto
                    {
                        ClockClientHolidayDetailId = detail.ClockClientHolidayDetailId,
                        ClientId = e.ClientId,
                        ClientEarningId = detail.OverrideClientEarningId ?? Convert.ToInt32(dto.ClientEarningId),
                        EventDate = detail.EventDate,
                        EmployeeId = e.EmployeeId,
                        Hours = detail.OverrideHours ?? dto.Hours,
                        IsApproved = true,
                        IsWorkedHours = false,
                        Comment = "Auto Generated Holiday"
                    };

                    _provider.SaveClockEmployeeBenefit(cebDto, true).MergeInto(result);
                }
            }

            _session.UnitOfWork.Commit().MergeInto(result);

            return result;    
        }

        #endregion

        #region Company Rules

        IOpResult<List<ClockClientRulesDto>> ILaborManagementService.GetClockClientRulesList(int clientId)
        {
            return _provider.GetClockClientRulesByClient(clientId);
        }

        IOpResult<ClockCompanyRulesViewDto> ILaborManagementService.GetCompanyRulesViewDto(int clientId)
        {
            var daysOfWeekList = DayOfWeekDto.GetDayOfWeekDtoList();

            var result = new OpResult<ClockCompanyRulesViewDto>().TrySetData(() => new ClockCompanyRulesViewDto
            {
                ClockClientRules = _session.UnitOfWork.TimeClockRepository
                    .GetClockClientRules()
                    .ByClient(clientId)
                    .ExecuteQueryAs(new ClockClientRulesMaps.ToClockClientRulesDto())
                    .ToList(),
                DaysOfWeek = daysOfWeekList,
                RoundingTypes = _session.UnitOfWork.TimeClockRepository
                    .ClockRoundingTypeQuery()
                    .ExecuteQueryAs(x => new ClockRoundingTypeDto
                    {
                        ClockRoundingTypeId = x.ClockRoundingTypeId,
                        Description = x.Description,
                        Minutes = x.Minutes,
                        RoundDirection = x.RoundDirection
                    })
                    .ToList(),
                ClientFeatures = _session.UnitOfWork.ClientAccountFeatureRepository
                    .ClientAccountFeatureQuery()
                    .ByClientId(clientId)
                    .ExecuteQueryAs(x => new ClientAccountFeatureDto
                    {
                        ClientId = x.ClientId,
                        AccountFeature = x.AccountFeature
                    })
                    .ToList()
            });


            return result;
        }

        IOpResult<ClockClientRulesDto> ILaborManagementService.SaveClockClientRules(ClockClientRulesDto dto)
        {
            var result = new OpResult<ClockClientRulesDto>();

            if (dto.Name == null)
            {
                result.SetToFail();
                return result;
            }


            _session.CanPerformAction(LaborManagementActionType.WriteTimePolicy).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, dto.ClientId).MergeInto(result);

            if (result.HasError) return result;

            var entity = new ClockClientRules
            {
                ClockClientRulesId = dto.ClockClientRulesId,
                ClientId = dto.ClientId,
                Name = dto.Name,
                ClockTimeFormatId = dto.ClockTimeFormatId,
                AllPunchesClockRoundingTypeId = dto.AllPunchesClockRoundingTypeId,
                AllPunchesGraceTime = dto.AllPunchesGraceTime,
                AllowInputPunches = dto.AllowInputPunches,
                ApplyHoursOption = dto.ApplyHoursOption,
                BiWeeklyStartingDayOfWeekId = dto.BiWeeklyStartingDayOfWeekId,
                ClockAllocateHoursFrequencyId = dto.ClockAllocateHoursFrequencyId,
                ClockAllocateHoursOptionId = dto.ClockAllocateHoursOptionId,
                InEarlyAllowPunchTime = dto.InEarlyAllowPunchTime,
                InEarlyClockRoundingTypeId = dto.InEarlyClockRoundingTypeId,
                InEarlyGraceTime = dto.InEarlyGraceTime,
                InEarly_OutsideGraceTimeClockRoundingTypeId = (PunchRoundingType?)dto.InEarlyOutsideGraceTimeClockRoundingTypeId,
                InLateAllowPunchTime = dto.InLateAllowPunchTime,
                InLateClockRoundingTypeId = dto.InLateClockRoundingTypeId,
                InLateGraceTime = dto.InLateGraceTime,
                InLate_OutsideGraceTimeClockRoundingTypeId = (PunchRoundingType?)dto.InLateOutsideGraceTimeClockRoundingTypeId,
                IsAllowMobilePunch = dto.IsAllowMobilePunch,
                IsEditBenefits = dto.IsEditBenefits,
                IsEditPunches = dto.IsEditPunches,
                IsHideCostCenter = dto.IsHideCostCenter,
                IsHideDepartment = dto.IsHideDepartment,
                IsHideEmployeeNotes = dto.IsHideEmployeeNotes,
                IsHideJobCosting = dto.IsHideJobCosting,
                IsHideMultipleSchedules = dto.IsHideMultipleSchedules,
                IsHidePunchType = dto.IsHidePunchType,
                IsHideShift = dto.IsHideShift,
                IsImportBenefits = dto.IsImportBenefits,
                IsImportPunches = dto.IsImportPunches,
                IsIpLockout = dto.IsIpLockout,
                MaxShift = dto.MaxShift,
                MonthlyStartingDayOfWeekId = dto.MonthlyStartingDayOfWeekId,
                OutEarlyAllowPunchTime = dto.OutEarlyAllowPunchTime,
                OutEarlyGraceTime = dto.OutEarlyGraceTime,
                OutEarlyClockRoundingTypeId = dto.OutEarlyClockRoundingTypeId,
                OutEarly_OutsideGraceTimeClockRoundingTypeId = (PunchRoundingType?)dto.OutEarlyOutsideGraceTimeClockRoundingTypeId,
                OutLateAllowPunchTime = dto.OutLateAllowPunchTime,
                OutLateClockRoundingTypeId = dto.OutLateClockRoundingTypeId,
                OutLateGraceTime = dto.OutLateGraceTime,
                OutLate_OutsideGraceTimeClockRoundingTypeId = (PunchRoundingType?)dto.OutLateOutsideGraceTimeClockRoundingTypeId,
                PunchOption = dto.PunchOption,
                SemiMonthlyStartingDayOfWeekId = dto.SemiMonthlyStartingDayOfWeekId,
                ShiftInterval = dto.ShiftInterval,
                StartTime = dto.StartTime,
                StopTime = dto.StopTime,
                WeeklyStartingDayOfWeekId = dto.WeeklyStartingDayOfWeekId
            };

            if (dto.IsNewEntity(x => x.ClockClientRulesId))
            {
                _session.UnitOfWork.RegisterNew(entity);
                _session.UnitOfWork.RegisterPostCommitAction(() =>
                {
                    dto.ClockClientRulesId = entity.ClockClientRulesId;
                });
            }
            else
            {
                _provider.RegisterExistingClockClientRules(entity, dto).MergeInto(result);
            }

            if (result.HasError) return result;

            _session.SetModifiedProperties(entity);
            _session.UnitOfWork.Commit().MergeInto(result);
            result.SetDataOnSuccess(dto);
            return result;
        }

        IOpResult ILaborManagementService.DeleteClockClientRules(int clientId, int clockClientRulesId)
        {
            var result = new OpResult();

            _session.CanPerformAction(LaborManagementActionType.WriteTimePolicy).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(result);

            if (result.HasError) return result;

            var curr = _provider.GetClockClientRules(clockClientRulesId).MergeInto(result).Data;

            _provider.CheckUsageForClockClientRules(curr).MergeInto(result);

            if (result.Success)
            {

                _session.UnitOfWork.RegisterDeleted(new ClockClientRules
                {
                    ClockClientRulesId = curr.ClockClientRulesId
                });

                _session.UnitOfWork.Commit().MergeInto(result);
            }

            return result;
        }

        IOpResult<IEnumerable<ClockClientDailyRulesDto>> ILaborManagementService.GetClockClientDailyRules(int clientId,
            int clockClientDailyRulesId)
        {
            var result = new OpResult<IEnumerable<ClockClientDailyRulesDto>>();

            _session.CanPerformAction(LaborManagementActionType.ReadTimePolicy).MergeInto(result);
            if (result.HasError) return result;

            return result.TrySetData(_session.UnitOfWork
                .LaborManagementRepository
                .ClockClientDailyRulesQuery()
                .ByClockClientRule(clockClientDailyRulesId)
                .ExecuteQueryAs(x => new ClockClientDailyRulesDto
                {
                    ClockClientDailyRulesId = x.ClockClientDailyRulesId,
                    ClientEarningId = x.ClientEarningId,
                    DayOfWeekId = x.DayOfWeekId,
                    ClockClientRulesId = x.ClockClientRulesId,
                    IsApplyOnlyIfMinHoursMetPrior = x.IsApplyOnlyIfMinHoursMetPrior,
                    MinHoursWorked = x.MinHoursWorked
                })
                .ToList);
        }

        IOpResult<ClockClientDailyRulesDto> ILaborManagementService.SaveClockClientDailyRule(
            ClockClientDailyRulesDto dto, int clientId)
        {
            var result = new OpResult<ClockClientDailyRulesDto>();

            _session.CanPerformAction(LaborManagementActionType.WriteTimePolicy).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(result);

            if (result.HasError) return result;

            var entity = new ClockClientDailyRules
            {
                ClockClientDailyRulesId = dto.ClockClientDailyRulesId,
                ClockClientRulesId = dto.ClockClientRulesId,
                ClientEarningId = dto.ClientEarningId,
                DayOfWeekId = dto.DayOfWeekId,
                IsApplyOnlyIfMinHoursMetPrior = dto.IsApplyOnlyIfMinHoursMetPrior,
                MinHoursWorked = dto.MinHoursWorked
            };

            if (dto.IsNewEntity(x => x.ClockClientDailyRulesId))
            {
                _session.UnitOfWork.RegisterNew(entity);
                _session.UnitOfWork.RegisterPostCommitAction(() =>
                {
                    dto.ClockClientDailyRulesId = entity.ClockClientDailyRulesId;   
                });
            }
            else
            {
                _provider.RegisterExistingClockClientDailyRule(entity, dto).MergeInto(result);
                if (result.HasError) return result;
            }

            _session.SetModifiedProperties(entity);
            _session.UnitOfWork.Commit().MergeInto(result);

            result.TrySetData(() => dto);

            return result;
        }

        IOpResult ILaborManagementService.DeleteClockClientDailyRule(int clockClientDailyRulesId, int clientId)
        {
            var result = new OpResult();

            _session.CanPerformAction(LaborManagementActionType.WriteTimePolicy).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(result);

            if (result.HasError) return result;

            var curr = _session.UnitOfWork.LaborManagementRepository
                .ClockClientDailyRulesQuery()
                .ByClockClientDailyRule(clockClientDailyRulesId)
                .ExecuteQueryAs(x => new ClockClientDailyRulesDto
                {
                    ClockClientDailyRulesId = x.ClockClientDailyRulesId,
                    ClientEarningId = x.ClientEarningId,
                    ClockClientRulesId = x.ClockClientRulesId
                })
                .FirstOrDefault();

            if (curr == null)
            {
                result.SetToFail();
                return result;
            }

            var entity = new ClockClientDailyRules
            {
                ClockClientDailyRulesId = curr.ClockClientDailyRulesId,
                ClockClientRulesId = curr.ClockClientRulesId,
                ClientEarningId = curr.ClientEarningId
            };

            _session.UnitOfWork.RegisterDeleted(entity);
            _session.SetModifiedProperties(entity);

            _session.UnitOfWork.Commit().MergeInto(result);

            return result;
        }

        #endregion

        #region Company Lunch

        IOpResult<ClockClientLunchViewDto> ILaborManagementService.GetClockCompanyLunchViewData(int clientId)
        {
            return new OpResult<ClockClientLunchViewDto>(new ClockClientLunchViewDto
            {
                CostCenters = _provider.GetClientCostCenterListByClient(clientId).Data,
                ClockRoundingTypeList = _provider.GetClockRoundingTypeList().Data,
                LunchRules = _session.UnitOfWork.TimeClockRepository
                    .GetClockClientLunchQuery()
                    .ByClient(clientId)
                    .ExecuteQueryAs(new ClockClientLunchMaps.ToClockClientLunchDto())
                    .ToList(),
                PaidOptionRulesTypeList = _session.UnitOfWork.LaborManagementRepository
                    .ClockClientLunchPaidOptionRulesQuery()
                    .ExecuteQueryAs(x => new ClockClientLunchPaidOptionRulesDto
                    {
                        ClockClientLunchPaidOptionRulesId = x.ClockClientLunchPaidOptionRulesId,
                        Description = x.Description
                    })
                    .ToList()
            });
        }

        IOpResult<List<ClockClientLunchPaidOptionDto>> ILaborManagementService.GetClockClientLunchPaidOptions(int clientId,
            int clockClientLunchId)
        {
            var result = new OpResult<List<ClockClientLunchPaidOptionDto>>();

            _session.CanPerformAction(LaborManagementActionType.ReadTimePolicy).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(result);

            return result.TrySetData(() => _session.UnitOfWork.LaborManagementRepository
                .ClockClientLunchPaidOptionQuery()
                .ByClientId(clientId)
                .ByClockClientLunchId(clockClientLunchId)
                .ExecuteQueryAs(x => new ClockClientLunchPaidOptionDto
                {
                    ClockClientLunchPaidOptionId = x.ClockClientLunchPaidOptionId,
                    ClockClientLunchPaidOptionRulesId = x.ClockClientLunchPaidOptionRulesId,
                    ClockClientLunchId = x.ClockClientLunchId,
                    ClientId = x.ClientId,
                    FromMinutes = x.FromMinutes,
                    OverrideMinutes = x.OverrideMinutes,
                    ToMinutes = x.ToMinutes
                })
                .ToList());
        }

        IOpResult<List<ClockClientLunchPaidOptionDto>> ILaborManagementService.SaveClockClientLunchPaidOptions(
            List<ClockClientLunchPaidOptionDto> dtoList, int clientId, int clockClientLunchId)
        {
            var result = new OpResult<List<ClockClientLunchPaidOptionDto>>(new List<ClockClientLunchPaidOptionDto>());

            _session.CanPerformAction(LaborManagementActionType.WriteTimePolicy).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(result);

            if (result.HasError) return result;

            foreach (var dto in dtoList)
            {
                // make sure we are updating the dtos with the appropriate FK id
                dto.ClockClientLunchId = clockClientLunchId > 0 ? clockClientLunchId : dto.ClockClientLunchId; 
                var exists = _session.UnitOfWork.LaborManagementRepository
                    .ClockClientLunchPaidOptionQuery()
                    .ByClientId(dto.ClientId)
                    .ByClockClientLunchPaidOptionId(dto.ClockClientLunchPaidOptionId)
                    .Result
                    .Any();

                result.Data.Add(exists
                    ? _provider.RegisterExistingClockClientLunchPaidOption(dto).MergeInto(result).Data
                    : _provider.RegisterNewClockClientLunchPaidOption(dto).MergeInto(result).Data);
            }
            
            _session.UnitOfWork.Commit().MergeInto(result);

            return result;
        }

        IOpResult ILaborManagementService.DeleteClockClientPaidOption(int clientId, int clockClientLunchId,
            int clockClientLunchPaidOptionId)
        {
            var result = new OpResult();

            _session.CanPerformAction(LaborManagementActionType.WriteTimePolicy).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(result);

            if (result.HasError) return result;

            var curr = _session.UnitOfWork.LaborManagementRepository
                .ClockClientLunchPaidOptionQuery()
                .ByClockClientLunchPaidOptionId(clockClientLunchPaidOptionId)
                .ExecuteQueryAs(x => new ClockClientLunchPaidOptionDto
                {
                    ClockClientLunchPaidOptionId = x.ClockClientLunchPaidOptionId,
                    ClockClientLunchPaidOptionRulesId = x.ClockClientLunchPaidOptionRulesId,
                    ClientId = x.ClientId,
                    ClockClientLunchId = x.ClockClientLunchId
                })
                .FirstOrDefault();

            if (curr == null)
            {
                result.SetToFail();
                return result;
            }

            var entity = new ClockClientLunchPaidOption
            {
                ClockClientLunchPaidOptionId = curr.ClockClientLunchPaidOptionId,
                ClockClientLunchId = curr.ClockClientLunchId,
                ClientId = curr.ClientId
            };

            _session.UnitOfWork.RegisterDeleted(entity);
            _session.SetModifiedProperties(entity);

            _session.UnitOfWork.Commit().MergeInto(result);

            return result;
        }

        IOpResult<ClockClientLunchDto> ILaborManagementService.SaveClockClientLunch(ClockClientLunchDto dto)
        {
            var result = new OpResult<ClockClientLunchDto>(dto);

            _session.CanPerformAction(LaborManagementActionType.WriteTimePolicy).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, dto.ClientId).MergeInto(result);

            if (dto.MinutesToRestrictLunchPunch < 0 || dto.MinutesToRestrictLunchPunch > 720)
            {
                result.Success = false;
                result.AddMessage(new GenericMsg("Restrict Lunch Punch minutes must be between 0 and 720."));
            }

            if (result.HasError) return result;

            if (dto.ClockClientLunchId != CommonConstants.NEW_ENTITY_ID)
            {
                result.TrySetData(() => 
                    _provider.RegisterExistingClockClientLunch(dto)
                    .MergeInto(result).Data
                    );
            }
            else
            {
                result.TrySetData(() => 
                    _provider.RegisterNewClockClientLunch(dto)
                    .MergeInto(result).Data
                    );
            }

            return result;
        }

        IOpResult ILaborManagementService.DeleteClockClientLunch(int clockClientLunchId, int clientId)
        {
            var result = new OpResult();

            _session.CanPerformAction(LaborManagementActionType.WriteTimePolicy).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(result);

            if (result.HasError) return result;

            var curr = _session.UnitOfWork.LaborManagementRepository
                .ClockClientLunchQuery()
                .ByClockClientLunchId(clockClientLunchId)
                .ExecuteQueryAs(new ClockClientLunchMaps.ToClockClientLunchDto())
                .FirstOrDefault();

            if (curr == null) return result;

            var inUse = curr.TimePolicies.Any();

            if (inUse)
            {
                var timePolicies = curr.TimePolicies.ToList();
                var policyNames = "";
                for (var i = 0; i < timePolicies.Count; i++)
                {
                    var p = timePolicies[i];
                    if (i < timePolicies.Count - 1)
                    {
                        policyNames += p.Name + ", ";
                    }
                    else
                    {
                        policyNames += p.Name + ".";
                    }
                }

                result.AddExceptionMessage(new Exception("Lunch rule is currently used on the following Time Policies: " + policyNames));
                result.SetToFail();
            }

            if (result.HasError) return result;

            var paidOptions = _session.UnitOfWork.LaborManagementRepository
                .ClockClientLunchPaidOptionQuery()
                .ByClockClientLunchId(curr.ClockClientLunchId)
                .ExecuteQueryAs(x => new ClockClientLunchPaidOptionDto()
                {
                    ClockClientLunchPaidOptionId = x.ClockClientLunchPaidOptionId,
                    ClockClientLunchId = x.ClockClientLunchId,
                    ClientId = x.ClientId
                })
                .ToList();

            foreach (var p in paidOptions)
            {
                Self.DeleteClockClientPaidOption(p.ClientId, p.ClockClientLunchId ?? 0, p.ClockClientLunchPaidOptionId).MergeInto(result);
            }

            if (result.HasError) return result;

            var entity = new ClockClientLunch
            {
                ClockClientLunchId = curr.ClockClientLunchId,
                ClientId = curr.ClientId,
                Name = curr.Name
            };

            _session.UnitOfWork.RegisterDeleted(entity);
            _session.UnitOfWork.Commit().MergeInto(result);

            return result;
        }

        #endregion

        #region Company ---> Additional Hours / Attendance Awards
        IOpResult<ClockClientAddHoursViewDto> ILaborManagementService.GetClockClientAddHoursView(int clientId)
        {
            var result = new OpResult<ClockClientAddHoursViewDto>(new ClockClientAddHoursViewDto());

            result.Data.ClockClientAddHoursDtos = Self.GetClockClientAddHoursByClient(clientId).MergeInto(result).Data;
            result.Data.ClientEarningDtos = Self.GetClientEarnings(clientId).MergeInto(result).Data.ToList();

            return result;
        }

        IOpResult<List<ClockClientAddHoursDto>> ILaborManagementService.GetClockClientAddHoursByClient(int clientId)
        {
            var result = new OpResult<List<ClockClientAddHoursDto>>();

            _session.CanPerformAction(LaborManagementActionType.ReadTimePolicy).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(result);

            return result.TrySetData(() => _session.UnitOfWork.LaborManagementRepository
                .ClockClientAddHoursQuery()
                .ByClient(clientId)
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
                .ToList());
        }

        IOpResult<ClockClientAddHoursDto> ILaborManagementService.SaveClockClientAddHours(ClockClientAddHoursDto dto)
        {
            var result = new OpResult<ClockClientAddHoursDto>(dto);

            _session.CanPerformAction(LaborManagementActionType.WriteTimePolicy).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, dto.ClientId).MergeInto(result);

            if (result.HasError) return result;

            if (dto.ClockClientAddHoursId != CommonConstants.NEW_ENTITY_ID)
            {
                result.TrySetData(() =>
                    _provider.RegisterExistingClockClientAddHours(dto)
                    .MergeInto(result).Data
                    );
            }
            else
            {
                result.TrySetData(() =>
                    _provider.RegisterNewClockClientAddHours(dto)
                    .MergeInto(result).Data
                    );
            }

            return result;
        }

        IOpResult ILaborManagementService.DeleteClockClientAddHours(int clockClientAddHoursId, int clientId)
        {
            var result = new OpResult();

            _session.CanPerformAction(LaborManagementActionType.WriteTimePolicy).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(result);

            if (result.HasError) return result;

            var curr = _session.UnitOfWork.LaborManagementRepository
                .ClockClientAddHoursQuery()
                .ByClockClientAddHoursId(clockClientAddHoursId)
                .ExecuteQueryAs(x => new ClockClientAddHoursDto
                {
                    ClockClientAddHoursId = x.ClockClientAddHoursId,
                    ClientId = x.ClientId,
                    TimePolicies = x.AddHoursSelected.Where(l => l.TimePolicy != null)
                        .Select(t => new ClockClientTimePolicyDtos.ClockClientTimePolicyDto
                        {
                            ClockClientTimePolicyId = t.TimePolicy.ClockClientTimePolicyId,
                            ClientId = t.TimePolicy.ClientId,
                            Name = t.TimePolicy.Name,
                        })
                })
                .FirstOrDefault();

            if (curr == null) return result;

            var inUse = curr.TimePolicies.Any();

            if (inUse)
            {
                var timePolicies = curr.TimePolicies.ToList();
                var policyNames = "";
                for (var i = 0; i < timePolicies.Count; i++)
                {
                    var p = timePolicies[i];
                    if (i < timePolicies.Count - 1)
                    {
                        policyNames += p.Name + ", ";
                    }
                    else
                    {
                        policyNames += p.Name + ".";
                    }
                }

                result.AddExceptionMessage(new Exception("Attendance Award rule is currently used on the following Time Policies: " + policyNames));
                result.SetToFail();
            }

            if (result.HasError) return result;
            
            var entity = new ClockClientAddHours
            {
                ClockClientAddHoursId = curr.ClockClientAddHoursId,
                ClientId = curr.ClientId,
                Name = curr.Name
            };

            _session.UnitOfWork.RegisterDeleted(entity);
            _session.UnitOfWork.Commit().MergeInto(result);

            return result;
        }

        #endregion

        #region Company Time Policies

        IOpResult<ClockClientTimePolicyDtos.ClockClientTimePolicyViewModels> ILaborManagementService.
            GetClockClientTimePolicyViewData(int clientId)
        {
            var result = new OpResult<ClockClientTimePolicyDtos.ClockClientTimePolicyViewModels>();
            return result.TrySetData(() => new ClockClientTimePolicyDtos.ClockClientTimePolicyViewModels()
            {
                TimePolicies = _provider.GetClockClientTimePolicies(clientId).MergeInto(result).Data,
                CompanyRules = _provider.GetClockClientRulesByClient(clientId).MergeInto(result).Data,
                Exceptions = _provider.GetClockClientExceptionsByClient(clientId).MergeInto(result).Data,
                Holidays = _provider.GetClockClientHolidaysByClient(clientId).MergeInto(result).Data,
                TimeZones = _provider.GetTimeZones().MergeInto(result).Data,
                AutoPointsSwitchEnabled = _session.CanPerformAction(LaborManagementActionType.WriteAutoPoints).Success,
                AutoPointsSwitchVisible = _session.CanPerformAction(LaborManagementActionType.ReadAutoPoints).Success
            });
        }

        IOpResult<ClockClientTimePolicySearchLists> ILaborManagementService.GetTimePolicySearchLists(int clientId)
        {
            var result = new OpResult<ClockClientTimePolicySearchLists>(new ClockClientTimePolicySearchLists());

            // security stuff is handled on each of these provider calls
            result.Data.OvertimeList = _provider.GetClockClientOvertimeList(clientId).MergeInto(result).Data;
            result.Data.LunchBreakList = _provider.GetClockClientLunchBreakList(clientId).MergeInto(result).Data;
            result.Data.AddHoursList = _provider.GetClockClientAddHoursList(clientId).MergeInto(result).Data;
            result.Data.ShiftList = _provider.GetClientShiftList(clientId).MergeInto(result).Data;

            return result;
        }

        IOpResult<ClockClientTimePolicySearchLists> ILaborManagementService.GetTimePolicySelectedLists(int clockClientTimePolicyId, int clientId)
        {
            var result = new OpResult<ClockClientTimePolicySearchLists>(new ClockClientTimePolicySearchLists());

            result.Data.OvertimeList = _provider.GetClockClientOvertimeByTimePolicy(clientId, clockClientTimePolicyId).MergeInto(result).Data;
            result.Data.LunchBreakList = _provider.GetClockClientLunchListByTimePolicy(clientId, clockClientTimePolicyId).MergeInto(result).Data;
            result.Data.AddHoursList = _provider.GetClockClientAddHoursListByTimePolicy(clientId, clockClientTimePolicyId).MergeInto(result).Data;
            result.Data.ShiftList = _provider.GetClientShiftListByTimePolicy(clientId, clockClientTimePolicyId).MergeInto(result).Data;

            return result;
        }

        IOpResult<ClockClientTimePolicySearchLists> ILaborManagementService.SaveTimePolicySelectedLists(int clientId,
            int clockClientTimePolicyId, ClockClientTimePolicySearchLists lists)
        {
            var result = new OpResult<ClockClientTimePolicySearchLists>(lists);

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(result);
            _session.CanPerformAction(LaborManagementActionType.WriteTimePolicy).MergeInto(result);

            if (result.HasError) return result;

            var lunchSelectedList = _session.UnitOfWork.LaborManagementRepository
                .ClockClientTimePolicyQuery()
                .ByClockClientTimePolicyId(clockClientTimePolicyId)
                .ExecuteQueryAs(x => new ClockClientTimePolicyDtos.ClockClientTimePolicyDto()
                {
                    ClockClientTimePolicyId = x.ClockClientTimePolicyId,
                    ClientId = x.ClientId,
                    LunchSelected = x.LunchSelected.Select(l => new ClockClientLunchSelectedDto
                    {
                        ClockClientLunchId = l.ClockClientLunchId,
                        ClockClientTimePolicyId = l.ClockClientTimePolicyId
                    }),
                    AddHoursSelected = x.AddHoursSelected.Select(a => new ClockClientAddHoursSelectedDto
                    {
                        ClockClientAddHoursId =  a.ClockClientAddHoursId,
                        ClockClientTimePolicyId = a.ClockClientTimePolicyId
                    }),
                    OvertimeSelected = x.OvertimeSelected.Select(o => new ClockClientOvertimeSelectedDto
                    {
                        ClockClientOvertimeSelectedId = o.ClockClientOvertimeSelectedId,
                        ClockClientOvertimeId = o.ClockClientOvertimeId,
                        ClockClientTimePolicyId = o.ClockClientTimePolicyId
                    }),
                    ShiftSelected = x.ShiftSelected.Select(s => new ClientShiftSelectedDto
                    {
                        ClockClientShiftSelectedId = s.ClockClientShiftSelectedId,
                        ClientShiftId = s.ClientShiftId,
                        ClockClientTimePolicyId = s.ClockClientTimePolicyId
                    })
                })
                .FirstOrDefault();

            var lunches = lunchSelectedList?.LunchSelected.ToList() ?? new List<ClockClientLunchSelectedDto>();
            var addHours = lunchSelectedList?.AddHoursSelected.ToList() ?? new List<ClockClientAddHoursSelectedDto>();
            var overtimes = lunchSelectedList?.OvertimeSelected.ToList() ?? new List<ClockClientOvertimeSelectedDto>();
            var shifts = lunchSelectedList?.ShiftSelected.ToList() ?? new List<ClientShiftSelectedDto>();

            // Delete each related record and check for errors after... 
            // if there are errors we immediately bail out. 
            _provider.DeleteRelatedClockClientLunches(clientId, clockClientTimePolicyId, lunches).MergeInto(result);
            if (result.HasError) return result;

            _provider.DeleteRelatedClockClientAddHours(clientId, clockClientTimePolicyId, addHours).MergeInto(result);
            if (result.HasError) return result;

            _provider.DeleteRelatedClockClientOvertimes(clientId, clockClientTimePolicyId, overtimes).MergeInto(result);
            if (result.HasError) return result;

            _provider.DeleteRelatedClientShifts(clientId, clockClientTimePolicyId, shifts).MergeInto(result);
            if (result.HasError) return result;


            var lunchesToSave = lists.LunchBreakList.Select(newLunch => new ClockClientLunchSelected
                {
                    ClockClientLunchId = newLunch.ClockClientLunchId,
                    ClockClientTimePolicyId = clockClientTimePolicyId
                })
                .ToList();

            if (lunchesToSave.Any())
                _provider.SaveNewClockClientLunchSelectedList(clientId, clockClientTimePolicyId, lunchesToSave).MergeInto(result);

            var addHoursToSave = lists.AddHoursList.Select(newAddHours => new ClockClientAddHoursSelected
                {
                    ClockClientAddHoursId = newAddHours.ClockClientAddHoursId,
                    ClockClientTimePolicyId = clockClientTimePolicyId
                })
                .ToList();
            if (addHoursToSave.Any())
                _provider.SaveNewClockClientAddHoursSelectedList(clientId, clockClientTimePolicyId, addHoursToSave).MergeInto(result);


            var overtimesToSave = lists.OvertimeList.Select(newOvertime => new ClockClientOvertimeSelected
                {
                    ClockClientOvertimeSelectedId = CommonConstants.NEW_ENTITY_ID,
                    ClockClientOvertimeId = newOvertime.ClockClientOvertimeId,
                    ClockClientTimePolicyId = clockClientTimePolicyId
                }).ToList();

            if (overtimesToSave.Any())
                _provider.SaveNewClockClientOvertimeSelectedList(clientId, clockClientTimePolicyId, overtimesToSave).MergeInto(result);

            var shiftsToSave = lists.ShiftList.Select(newShift => new ClientShiftSelected
                {
                    ClockClientShiftSelectedId = CommonConstants.NEW_ENTITY_ID,
                    ClientShiftId = newShift.ClientShiftId,
                    ClockClientTimePolicyId = clockClientTimePolicyId
                }).ToList();

            if (shiftsToSave.Any())
                _provider.SaveNewClientShiftSelectedList(clientId, clockClientTimePolicyId, shiftsToSave).MergeInto(result);
            
            return result;
        }

        IOpResult<ClockClientTimePolicyDtos.ClockClientTimePolicyDto> ILaborManagementService.SaveClockClientTimePolicy(
            int clientId, ClockClientTimePolicyDtos.ClockClientTimePolicyDto dto)
        {
            var result = new OpResult<ClockClientTimePolicyDtos.ClockClientTimePolicyDto>();

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(result);
            _session.CanPerformAction(LaborManagementActionType.WriteTimePolicy).MergeInto(result);

            if (!_session.CanPerformAction(LaborManagementActionType.WriteAutoPoints).Success)
            {
                dto.AutoPointsEnabled = false;
            }
            

            if (result.HasError) return result;

            if (dto.ClockClientTimePolicyId > CommonConstants.NEW_ENTITY_ID)
                _provider.RegisterExistingClockClientTimePolicy(dto).MergeAll(result);
            else
                _provider.RegisterNewClockClientTimePolicy(dto).MergeAll(result);

            _session.UnitOfWork.Commit().MergeInto(result);

            // if none of the selected lists are defined then we just return now
            if (dto.Lunches == null && dto.AddHours == null && dto.Overtimes == null && dto.Shifts == null) return result;

            var lists = new ClockClientTimePolicySearchLists
            {
                LunchBreakList = dto.Lunches.ToList(),
                AddHoursList = dto.AddHours.ToList(),
                OvertimeList = dto.Overtimes.ToList(),
                ShiftList = dto.Shifts.ToList()
            };

            lists = Self.SaveTimePolicySelectedLists(clientId, result.Data.ClockClientTimePolicyId, lists).MergeInto(result).Data;

            if (result.HasError) return result;

            result.Data.Lunches = lists.LunchBreakList;
            result.Data.AddHours = lists.AddHoursList;
            result.Data.Overtimes = lists.OvertimeList;
            result.Data.Shifts = lists.ShiftList;

            return result;
        }

        IOpResult ILaborManagementService.AutoApplyClockClientTimePolicy(int clientId, int clockClientTimePolicyId)
        {
            var result = new OpResult();

            // HARDCODED IN EXISTING VB SOURCE CODE, THIS WILL ALL BE MIGRATED AND REPLACED
            const int whatToApply = 2;

            var autoApplyResult = _dsClockClientService.AutoApplyClockSchedulesAndTimePolicies(
                clientId, _session.LoggedInUserInformation.UserId, whatToApply, clockClientTimePolicyId);

            // what are the values that come back from the sproc and how do we check if it was successful? 
            if (autoApplyResult < 0) result.SetToFail();

            return result;
        }

        IOpResult ILaborManagementService.DeleteClockClientTimePolicy(int clientId, int clockClientTimePolicyId)
        {
            var result = new OpResult();

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(result);
            _session.CanPerformAction(LaborManagementActionType.WriteTimePolicy).MergeInto(result);

            var exists = _session.UnitOfWork.LaborManagementRepository
                .ClockClientTimePolicyQuery()
                .ByClockClientTimePolicyId(clockClientTimePolicyId)
                .Result
                .Any();

            if (!exists)
            {
                result.SetToFail();
                return result;
            }

            // Check the references and throw exceptions
            IEnumerable<int> employeeRefIds = _session.UnitOfWork.TimeClockRepository.GetClockEmployeeQuery()
                .ByClockClientTimePolicyIds(new List<int>() { clockClientTimePolicyId })
                .ExecuteQueryAs(x => x.EmployeeId);

            IEnumerable<int> scheduleRefIds = _session.UnitOfWork.LaborManagementRepository.ClockEmployeeScheduleQuery()
                .ByClockClientTimePolicyId(clockClientTimePolicyId)
                .ExecuteQueryAs(x => x.ClockEmployeeScheduleId);

            if (employeeRefIds.Count() > 0 || scheduleRefIds.Count() > 0)
            {
                return result.AddMessage(new GenericMsg("This Time Policy is either mapped directly to employee or employee clock schedules, and cannot be deleted."))
                    .SetToFail();
            }

            // delete all selected lunches, overtimes and shifts before we can delete the time policy
            _provider.DeleteRelatedClockClientLunches(clientId, clockClientTimePolicyId).MergeInto(result);
            _provider.DeleteRelatedClockClientAddHours(clientId, clockClientTimePolicyId).MergeInto(result);
            _provider.DeleteRelatedClockClientOvertimes(clientId, clockClientTimePolicyId).MergeInto(result);
            _provider.DeleteRelatedClientShifts(clientId, clockClientTimePolicyId).MergeInto(result);

            if (result.HasError) return result;

            _session.UnitOfWork.RegisterDeleted(new ClockClientTimePolicy
            {
                ClockClientTimePolicyId = clockClientTimePolicyId,
                ClientId = clientId
            });

            _session.UnitOfWork.Commit().MergeInto(result);

            return result;
        }

        IOpResult<TimeCardAuthorizationResult> ILaborManagementService.GetTimeCardAuthorizationPaged(int employeeId, TimeCardAuthorizationSearchOptions options)
        {
            var result = new OpResult<TimeCardAuthorizationResult>(new TimeCardAuthorizationResult
            {
                Page = options.Page,
                PageSize = options.PageSize
            });

            _session.CanPerformAction(LaborManagementActionType.ReadTimeCardAuthorization).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessByAccessibleClientIds(options.ClientId).MergeInto(result);
            if (employeeId > 0) _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Employee, employeeId).MergeInto(result);

            if (result.HasError) return result;

            var paginationInfo = _provider.GetClockEmployeePunchListPaginationCount(options).MergeInto(result).Data;

            if (result.HasError) return result;

            result.Data.TotalEmployees = paginationInfo.EmployeeCount;
            result.Data.TotalPages = paginationInfo.TotalPages;

            var activity = _provider.GetTcaEmployeePunchActivityList(options).MergeInto(result).Data;

            if (result.HasError) return result;

            var tcaEmployees = new List<TimeCardAuthorizationEmployeeResult>();
            //foreach(var e in activity.Employees)
            //{
            //    var rawEmployeeActivity = activity.Activity.Where(a => a.EmployeeId == e.EmployeeId);
            //    var rows = new List<TimeCardAuthorizationRowResult>();

            //    var activityByDates = rawEmployeeActivity.GroupBy(r => r.DateOfPunch.Date);

            //    foreach (var group in activityByDates) 
            //    {
            //        var date = group.Key;
            //        IEnumerable<EmployeePunchActivityDto> groupedDates = group
            //            .SelectMany<EmployeePunchActivityDto, EmployeePunchActivityDto>(g => g);

            //        var grouped = groupedDates.Chunks(4).ToList();

            //        foreach(var grp in grouped)
            //        {
            //            foreach(var r in grp)
            //            {
            //                rows.Add(new TimeCardAuthorizationRowResult
            //                {
            //                    EmployeeId = r.EmployeeId,
            //                    EventDate = date,
            //                    ClientCostCenterId = r.ClientCostCenterId,
            //                    IsBenefit = r.IsPendingBenefit,
            //                    DayColumnLabel = r.DateOfPunch.Date.DayOfWeek.ToString(),
            //                    DateColumnLabel = r.DateOfPunch.ToString(),
            //                    Schedule = "Edit Schedule",
            //                    IsTotalRow = false,
            //                    IsScheduleReal = false,
            //                    HideApprovalCheckbox = false,
            //                    IsApproved = false,
            //                    EmployeeActivity = 1,
            //                    LblNotesVisible = true,
            //                    AddAllocationVisible = false,
            //                    ImgCloclNameVisible = true,
            //                    LnkDateShowing = false,
            //                    Type = TimeCardAuthorizationResultType.DayDetail,
                                
            //                });
            //            }
            //        }

            //    }

            //    tcaEmployees.Add(new TimeCardAuthorizationEmployeeResult
            //    {
            //        EmployeeId = e.EmployeeId,
            //        EmployeeNumber = e.EmployeeNumber,
            //        ClockClientTimePolicyId = e.ClockClientTimePolicyId,
            //        ClockClientTimePolicyName = e.ClockClientTimePolicyName,
            //        Name = e.EmployeeName,
            //        //Rows = 
            //    });
            //}

            return result;
        }

        IOpResult<GetTimeClockCurrentPeriodDto> ILaborManagementService.GetCurrentPayPeriod(int clientId)
        {
            var result = new OpResult<GetTimeClockCurrentPeriodDto>();

            var sprocArgs = new GetTimeClockCurrentPeriodArgsDto
            {
                ClientID = clientId
            };

            return result.TrySetData(() => _session.UnitOfWork.LaborManagementRepository
                .GetTimeClockCurrentPeriod(sprocArgs));
        }

        IOpResult<EmployeePunchListCountAndResultLengthDto> ILaborManagementService.GetEmployeePagedResultLength(ClockEmployeePunchListByDateAndFilterPaginatedCountArgs args)
        {
            var result = new OpResult<EmployeePunchListCountAndResultLengthDto>();
            return result.TrySetData(() => 
                _session.UnitOfWork.LaborManagementRepository.GetClockEmployeePunchListByDateAndFilterCount(args));
        }

        IOpResult<GetTimeApprovalTableDto> ILaborManagementService.GetTimeApprovalTable(int? supervisorId, TcaSearchOptions options)
        {
            return TimeCardAuthorizationService.Authorize(() =>
            {
                var result = new OpResult<GetTimeApprovalTableDto>();
                var userType = _userService.GetUser(_session.LoggedInUserInformation.UserId);

                if (userType.UserTypeId != UserType.Supervisor && options.Category1Dropdown.Value == 6)
                {
                    supervisorId = options.Filter1Dropdown.Value;
                }

                //Dim totals As Totals = logic.LoadTotals(PrincipalDominion.UserID, supervisorId, PrincipalDominion.UserTypeID, PrincipalDominion.ClientID, PrincipalDominion.User_EmployeeID)
                var totals = _dlEmployeeApproveHours.LoadPaginatedTotals(_session.LoggedInUserInformation.UserId, 
                    supervisorId, 
                    (int)userType.UserTypeId, 
                    options.clientId,
                    _session.LoggedInUserInformation.UserEmployeeId.GetValueOrDefault(int.MinValue), 
                    options);
                //_dlEmployeeApproveHours.PrefillFiltersWithSupervisor(totals, supervisorId);
                //double employeeCount = Convert.ToDouble(totals.Employees.Count());
                //var numberOfPages = _dlEmployeeApproveHours.NumberOfPages(totals);
                //var paged = _dlEmployeeApproveHours.Page(totals, options.CurrentPage);

                //FillTotalsFilterValues(totals, userId, supervisorId, userTypeId, clientId, employeeId)
                //totals.Employees = GetEmployeeTotals(totals.Filters).ToArray()
                //set totals.Filters
                var stuff = _dlEmployeeApproveHours.ToDataTable(totals);


                result.TrySetData(() => new GetTimeApprovalTableDto()
                {
                    Table = stuff,
                    TotalPages = totals.Filters.TotalPages,
                    CurrentPage = options.CurrentPage
                })
                    .CheckForData(() => new GenericMsg("Unable to Retrieve Table"));

                var allocatedHoursArgs = new GetClockEmployeeAllocatedHoursDifferenceArgsDto()
                {
                    ClientId = options.clientId,
                    EmployeeId = 0,
                    UserId = _session.LoggedInUserInformation.UserId,
                    StartDate = totals.Filters.StartDate,
                    EndDate = totals.Filters.EndDate
                };

                var approveDateArgs = new GetClockEmployeeApproveDateArgsDto()
                {
                    ClientID = options.clientId,
                    EmployeeID = 0,
                    UserID = _session.LoggedInUserInformation.UserId,
                    StartDate = totals.Filters.StartDate,
                    EndDate = totals.Filters.EndDate,
                    OnlyPayToSchedule = false
                };


                return OnDataBind(result, allocatedHoursArgs, approveDateArgs, totals);
            }, _session, _userService);
        }

        IOpResult<IEnumerable<GetClockEmployeeHoursComparisonDto>> ILaborManagementService.GetClockEmployeeHoursComparisonSproc(int clientId, DateTime startDate, DateTime endDate, IEnumerable<int> employeeIds)
        {
            var result = new OpResult<IEnumerable<GetClockEmployeeHoursComparisonDto>>();
            var employeeList = employeeIds.ToList();
            startDate = startDate.Date; //.Date sets the time to midnight
            endDate = endDate.Date; // otherwise it will not capture schedules on the dates

            if (employeeList.Count() > 0)
            {
                //foreach (var employeeId in employeeList)
                //{
                    var hoursComparisonSorted = _session.UnitOfWork.EmployeeRepository
                    .GetClockEmployeeHoursComparisonSproc(new GetClockEmployeeHoursComparisonArgsDto()
                    {
                        ClientId = clientId,
                        UserId = 0,
                        EmployeeIDs = GetEmployeeIDListTVP(employeeIds.ToList()),
                        StartDate = startDate,
                        EndDate = endDate,
                        FilterCategory = 0,
                        Filter = 0,
                        PayType = 0,
                        Status = 0,
                    });
                    result.Data = hoursComparisonSorted;
                //}
            }
            else
            {
                result.Data = null;
            }
            return result;
        }

        private IOpResult<GetTimeApprovalTableDto> OnDataBind(IOpResult<GetTimeApprovalTableDto> data,
            GetClockEmployeeAllocatedHoursDifferenceArgsDto allocatedHourDiffArgs,
            GetClockEmployeeApproveDateArgsDto approveDateArgs,
            Totals totals)
        {
            Dictionary<string, GetClockEmployeeAllocatedHoursDifferenceDto> difference = new Dictionary<string, GetClockEmployeeAllocatedHoursDifferenceDto>();

            var shouldGetDifference = totals.Employees.DistinctBy(e => e.EmployeeID).Any(e => e.Rules?.ClockAllocateHoursFrequencyID == 1);
            var approveDatesList = _session.UnitOfWork.LaborManagementRepository.GetClockEmployeeApproveDate(approveDateArgs).ToList();
            var costCenterApproveDate = new Dictionary<string, GetClockEmployeeApproveDateDto>();
            foreach (var item in approveDatesList)
            {
                var key = item.EmployeeID.ToString() + item.Eventdate.ToString("MM/dd/yyyy") + (item.ClientCostCenterID.HasValue ? item.ClientCostCenterID.ToString() : null) + item.IsApproved.ToString();
                if (!costCenterApproveDate.ContainsKey(key))
                {
                    costCenterApproveDate.Add(key, item);
                }
            }
            //approveDatesList
            //.ToDictionary(x => x.EmployeeID.ToString() + x.Eventdate.ToString("MM/dd/yyyy") + (x.ClientCostCenterID.HasValue ? x.ClientCostCenterID.ToString() : null) + x.IsApproved.ToString());
            var empClockRules = _punchProvider.GetClockClientRulesSummary(new ClockClientRulesMaps.ToClockClientRulesSummaryDto(), totals.Filters?.EmployeeID, totals.Filters?.ClientID).Data.Where(x => x.EmployeeId.HasValue).ToDictionary(x => x.EmployeeId.Value);
            //var empClockRules = _clockService.GetClockClientRulesSummary(totals.Filters?.EmployeeID, totals.Filters?.ClientID)
            
            if (shouldGetDifference)
            {
                difference = _session.UnitOfWork.LaborManagementRepository.GetClockEmployeeAllocatedHoursDifference(allocatedHourDiffArgs)
                    .ToDictionary(x => x.EmployeeId.ToString() + x.EventDate.ToString());
            }
            var dvwExceptionHistorySorted = _session.UnitOfWork.EmployeeRepository.GetClockEmployeeExceptionHistoryByEmployeeID(new GetClockEmployeeExceptionHistoryByEmployeeIDArgsDto()
            {
                ClientID = totals.Filters.ClientID,
                EmployeeID = 0,
                StartDate = totals.Filters.StartDate,
                EndDate = totals.Filters.EndDate
            });

            var punchColumns = new string[] { "In", "In2", "Out", "Out2", "lnkDate", "lblDate", "lnkSchedule" };
            AddPropertyColumnForProvidedColumns(punchColumns, "Class", typeof(string), data.Data.Table);
            AddPropertyColumnForProvidedColumns(punchColumns, "ToolTipContent", typeof(string), data.Data.Table);
            AddPropertyColumnForProvidedColumns(punchColumns, "Modal", typeof(string), data.Data.Table);
            AddPropertyColumnForProvidedColumns(new string[] { "lnkSchedule2", "Schedule3" }, "Modal", typeof(string), data.Data.Table);
            AddPropertyColumnForProvidedColumns(punchColumns, "Title", typeof(string), data.Data.Table);
            AddPropertyColumnForProvidedColumns(punchColumns, "Disabled", typeof(bool), data.Data.Table);
            data.Data.Table.Columns.Add("lnkDate", typeof(bool));
            data.Data.Table.Columns.Add("SelectHoursDisabled", typeof(bool));
            data.Data.Table.Columns.Add("SelectHoursShowing", typeof(bool));
            data.Data.Table.Columns.Add("lnkDateShowing", typeof(bool));
            data.Data.Table.Columns.Add("lblDateShowing", typeof(bool));
            data.Data.Table.Columns.Add("SelectHoursChecked", typeof(bool));
            data.Data.Table.Columns.Add("SelectHoursTooltip", typeof(string));
            data.Data.Table.Columns.Add("DisplayExceptionAsApproved", typeof(bool));
            data.Data.Table.Columns.Add("DateModal", typeof(string));
            data.Data.Table.Columns.Add("ChangesNotAllowed", typeof(bool));
            data.Data.Table.Columns.Add("AddAllocationVisible", typeof(bool));
            data.Data.Table.Columns.Add("AddPunchPopUp", typeof(string));
            data.Data.Table.Columns.Add("AddBenefitPopUp", typeof(string));
            data.Data.Table.Columns.Add("AddAllocationPopUp", typeof(string));
            data.Data.Table.Columns.Add("lblNotesVisible", typeof(string));
            data.Data.Table.Columns.Add("ExceptionStyle", typeof(string));
            data.Data.Table.Columns.Add("SetClockClientNoteID", typeof(int));
            data.Data.Table.Columns.Add("imgClockNameVisible", typeof(bool));
            data.Data.Table.Columns.Add("imgGeofencingVisible", typeof(bool));
            data.Data.Table.Columns.Add("clientId", typeof(int));
            data.Data.Table.Columns.Add("Schedule2Modal", typeof(string));

            var user = _userService.GetUser(_session.LoggedInUserInformation.UserId);

            var jobCostingInfo = _session.UnitOfWork.LaborManagementRepository.GetClientJobCostingInfoByClientID(
                            new GetClientJobCostingInfoByClientIDArgsDto()
                            {
                                ClientID = totals.Filters.ClientID
                            });
            var clientOptions = _clientSettingsProvider.GetClientAccountOptionSettings(totals.Filters.ClientID, new AccountOption[] { AccountOption.TimeClock_ShowCostCenterTooltip, AccountOption.TimeClock_ApprovalOptions })
                .MergeInto(data).Data;
            var showCostCenterTooltip = clientOptions.Any(x => x.AccountOption == AccountOption.TimeClock_ShowCostCenterTooltip && x.IsEnabled.GetValueOrDefault());
            var approvalOptionInDb = clientOptions
                .FirstOrDefault(x => x.AccountOption == AccountOption.TimeClock_ApprovalOptions)?.SelectedItem.AccountOptionItemId;
            var intApprovalOption = MapApprovalOptionFromClientOptionItemIDs(approvalOptionInDb.GetValueOrDefault());
            if (data.HasError)
            {
                data.Data.Table = null;
                return data;
            }
            var approveDateList = _session.UnitOfWork.LaborManagementRepository.GetClockEmployeeApproveDate(approveDateArgs).ToList();
            foreach (DataRow row in data.Data.Table.Rows)
            {

                row["clientId"] = totals.Filters.ClientID;

                var isFound = difference.TryGetValue((DBNull.Value.Equals(row["EmployeeID"]) ? "" : row["EmployeeID"]).ToString() + (DBNull.Value.Equals(row["EventDate"]) ? "" : row["EventDate"]), out var value);
                if (isFound && value.AllocatedHoursDifference != 0)
                {
                    row["SelectHoursDisabled"] = true;
                }

                var dictEmpKey = DBNull.Value.Equals(row["EmployeeID"]) ? "" : row["EmployeeID"].ToString();
                var dictDatePartKey = DBNull.Value.Equals(row["EventDate"]) ? "" : Convert.ToDateTime(row["EventDate"]).ToString("MM/dd/yyyy");
                var dictCostCenterKey = DBNull.Value.Equals(row["ClientCostCenterID"]) ? "" : row["ClientCostCenterID"].ToString();
                var dictIsApprovedKey = DBNull.Value.Equals(row["IsApproved"]) ? "" : row["IsApproved"].ToString();
                var dictKey = dictEmpKey + dictDatePartKey + dictCostCenterKey + dictIsApprovedKey;

                isFound = costCenterApproveDate.TryGetValue(dictKey, out var costCenterApproveDateDto);
                var isCostCenterApproved = false;
                if (isFound && costCenterApproveDateDto.IsApproved.GetValueOrDefault())
                {
                    isCostCenterApproved = true;
                }

                isFound = empClockRules.TryGetValue(Convert.ToInt32(row["EmployeeID"]), out var empRule);
                var dateHasValue = !DBNull.Value.Equals(row["Date"]);
                var rowIsInputHourBenefit = false;
                var punchOption = 0;
                if (isFound &&
                    dateHasValue &&
                    empRule?.PunchOption.GetValueOrDefault() == PunchOptionType.InputHours &&
                    Convert.ToBoolean(row["IsBenefit"]) == true)
                {
                    rowIsInputHourBenefit = true;
                }
                var rowDateNotEndWithENTERED = dateHasValue && !Convert.ToString(row["Date"]).ToUpper().EndsWith("ENTERED");
                var hideApprovalCheckboxIsTrue = Convert.ToBoolean(row["HideApprovalCheckbox"]);
                var approvalOptionTypeIsExceptions = totals.Filters.ApprovalOption == ApprovalOptionType.Exceptions;
                var exceptionsHasNoValue = string.IsNullOrWhiteSpace(Convert.ToString(row["Exceptions"]));
                var employeeActivityIsGreaterThanOne = Convert.ToInt32(row["EmployeeActivity"]) > 1;
                var approvalOptionIsAllOrEveryDay = totals.Filters.ApprovalOption == ApprovalOptionType.Everyday || totals.Filters.ApprovalOption == ApprovalOptionType.All_Activity;
                var isMissingPunch = Convert.ToString(row["Exceptions"]).ToUpper().Contains("MISSING PUNCH");
                var isNoPunchOnScheduledDay = Convert.ToString(row["Exceptions"]).ToUpper().Contains("NO PUNCHES ON SCHEDULED DAY");

                var isSelectHoursShowing = !hideApprovalCheckboxIsTrue && !isMissingPunch && ((employeeActivityIsGreaterThanOne &&
                    approvalOptionIsAllOrEveryDay) || !(approvalOptionTypeIsExceptions && (exceptionsHasNoValue || isNoPunchOnScheduledDay)) && (rowIsInputHourBenefit || rowDateNotEndWithENTERED));

                row["SelectHoursShowing"] = isSelectHoursShowing;

                var isEmployeeActivityTwo = Convert.ToInt32(row["EmployeeActivity"]) == 2;

                var isCostCenterRow = totals.Filters.ApprovalOption > ApprovalOptionType.All_Activity &&
                    string.IsNullOrEmpty(Convert.ToString(row["In"])) &&
                    !Convert.ToBoolean(row["IsTotalRow"]) && !DBNull.Value.Equals(row["Hours"]);

                var headerRow = (totals.Filters.ApprovalOption > ApprovalOptionType.None && DBNull.Value.Equals(row["Header"]) &&
                    (Convert.ToString(row["Day"]).ToUpper().Contains("DAY") || string.IsNullOrEmpty(Convert.ToString(row["In"])))) ||
                    (
                    totals.Filters.ApprovalOption == ApprovalOptionType.None &&
                    DBNull.Value.Equals(row["Hours"]) &&
                    !string.IsNullOrEmpty(Convert.ToString(row["Day"])) &&
                    string.IsNullOrEmpty(Convert.ToString(row["In"])) &&
                    !string.IsNullOrEmpty(Convert.ToString(row["Exceptions"]))
                    );

                row["SelectHoursChecked"] = ((employeeActivityIsGreaterThanOne && approvalOptionIsAllOrEveryDay && !isEmployeeActivityTwo) || !(employeeActivityIsGreaterThanOne && approvalOptionIsAllOrEveryDay)) &&
                    ((isCostCenterRow && isCostCenterApproved) || (headerRow && isCostCenterApproved));
                row["SelectHoursTooltip"] = isCostCenterApproved ? costCenterApproveDateDto.ApprovingUser : null;
                //row["ExceptionStyle"] = DetermineExceptionMessageStyling(
                //    employeeActivityIsGreaterThanOne, 
                //    approvalOptionIsAllOrEveryDay, 
                //    isEmployeeActivityTwo,
                //    hideApprovalCheckboxIsTrue,
                //    Convert.ToBoolean(row["IsApproved"]));
                //var intApprovalOption = totals.Filters.ApprovalOption;
                var strPassword = "&Password=ZSJ569OPL2N8HG";
                var strNewPassword = strPassword;
                int intEmployeeID = 0;
                DateTime? datEventDate = null;
                bool blnPayToSchedule;
                var intClockClientNoteID = 0;
                row["SelectHoursShowing"] = false;
                int intClockEmployeeBenefitID;
                PunchOptionType? intEEPunchOption = null;
                var intCostCenterCount = 1;

                // NEW: Reformatted for window.open() call
                var strValue2 = Popup.Create("", height: 700, width: 650, allowResize: false, includeStatusBar: false).OptionsParamJsonObject;

                // Punch In

                //aPunch = e.Item.FindControl("lnkPunchInTime");
                //aPunch.Style.Add("text-decoration", "underline");

                // Check if 'Missing' punch
                strNewPassword = CheckForMissingPunch((string)(DBNull.Value.Equals(row["In"]) ? "" : row["In"]), strNewPassword, row, "In");

                bool hasApprovalOption = intApprovalOption > (int)ApprovalOptionType.None;
                bool containsDayOrPunchIsEmpty = ((DBNull.Value.Equals(row["Day"]) ? "" : row["Day"]).ToString().ToUpper().Contains("DAY") || !string.IsNullOrEmpty((string)(DBNull.Value.Equals(row["In"]) ? null : row["In"])));
                if (((hasApprovalOption & DBNull.Value.Equals(row["Header"]) & containsDayOrPunchIsEmpty) || (intApprovalOption > (int)ApprovalOptionType.All_Activity && DBNull.Value.Equals(row["Hours"]) && (DBNull.Value.Equals(row["Day"]) ? "" : (string)row["Day"]) != "" && (DBNull.Value.Equals(row["In"]) ? "" : (string)row["In"]) == "" & (DBNull.Value.Equals(row["Exceptions"]) ? "" : (string)row["Exceptions"]) != "")))
                {
                    if (intApprovalOption > (int)ApprovalOptionType.All_Activity)
                        row["ExceptionStyle"] = "exception";

                    if (!IsDBNull(row["Date"]))
                    {
                        if (!((string)row["Date"]).ToUpper().EndsWith("ENTERED"))
                            row["SelectHoursShowing"] = true;
                    }
                    row["SelectHoursChecked"] = false;
                    //hdnSelectHoursOriginal.Value = false;

                    intEmployeeID = (int)(DBNull.Value.Equals(row["EmployeeID"]) ? default(int) : Convert.ToInt32(row["EmployeeID"]));
                    datEventDate = Convert.ToDateTime((DBNull.Value.Equals(row["EventDate"]) ? "" : row["EventDate"]));

                    string[] aTwoSearchValues = new string[2];

                    aTwoSearchValues[0] = intEmployeeID.ToString();
                    aTwoSearchValues[1] = datEventDate.Value.ToString("MM/dd/yyyy");

                    var intIndex = approveDateList.FirstOrDefault(x => x.EmployeeID == intEmployeeID && x.Eventdate.CompareTo(datEventDate) == 0);

                    if (intIndex != null)
                    {
                        blnPayToSchedule = intIndex.PayToSchedule.GetValueOrDefault();
                        intClockClientNoteID = !intIndex.ClockClientNoteID.HasValue ? int.MinValue : intIndex.ClockClientNoteID.Value;

                        if (intIndex.IsApproved == true)
                        {
                            if ((bool)(DBNull.Value.Equals(row["SelectHoursShowing"]) ? default(bool) : row["SelectHoursShowing"]))
                            {
                                row["SelectHoursChecked"] = true;
                                row["SelectHoursTooltip"] = intIndex.ApprovingUser.ToString();
                                //hdnSelectHoursOriginal.Value = true;
                            }

                            row["ExceptionStyle"] = "approved";
                        }
                    }
                }

                // Always fill the notes drop down
                string[] tSVal = new string[2];
                var approveDateEmpId = (DBNull.Value.Equals(row["EmployeeID"]) ? default(int) : Convert.ToInt32(row["EmployeeID"]));
                var eventDate = (DateTime)(DBNull.Value.Equals(row["EventDate"]) ? default(DateTime) : Convert.ToDateTime(row["EventDate"]));
                var inIndex = approveDateList.FirstOrDefault(x => x.EmployeeID == approveDateEmpId && eventDate.CompareTo(x.Eventdate) == 0);
                if (inIndex != null)
                {
                    blnPayToSchedule = inIndex.PayToSchedule.GetValueOrDefault();
                    intClockClientNoteID = !inIndex.ClockClientNoteID.HasValue ? int.MinValue : inIndex.ClockClientNoteID.Value;
                }

                // Per Cost Center Option
                // Only works for earning rows
                if (intApprovalOption > (int)ApprovalOptionType.All_Activity && string.IsNullOrEmpty((string)(DBNull.Value.Equals(row["In"]) ? "" : row["In"])) && Convert.ToBoolean(row["IsTotalRow"]) == false && !IsDBNull(row["Hours"]) && string.IsNullOrEmpty((string)(IsDBNull(row["Day"]) ? "" : row["Day"])))
                {
                    row["SelectHoursChecked"] = false;
                    //hdnSelectHoursOriginal.Value = false;
                    row["ExceptionStyle"] = "exception";

                    datEventDate = Convert.ToDateTime(row["EventDate"]);
                    intEmployeeID = (DBNull.Value.Equals(row["EmployeeID"]) ? default(int) : Convert.ToInt32(row["EmployeeID"]));
                    intClockEmployeeBenefitID = IsDBNull(row["ClockEmployeeBenefitID"]) ? 0 : int.Parse((string)row["ClockEmployeeBenefitID"]);

                    if (intApprovalOption == (int)ApprovalOptionType.Cost_Center)
                    {

                        // Filter the to the EE's rule
                        // dvwClockRules.RowFilter = "EmployeeID=" & intEmployeeID
                        IEnumerable<KeyValuePair<int, ClockClientRulesSummaryDto>> filteredClockRules = from employeeRules in empClockRules
                                                                                                 where employeeRules.Key == intEmployeeID
                                                                                                               select employeeRules;
                        // Make sure we found a rule
                        // If dvwClockRules.Count > 0 Then
                        if (filteredClockRules.Any())
                        {
                            var rule = filteredClockRules.First().Value;
                            // Set the punch option
                            // If IsDBNull(dvwClockRules(0).Item("PunchOption")) Then
                            intEEPunchOption = rule.PunchOption.HasValue ? rule.PunchOption.Value : PunchOptionType.NormalPunch;
                        }
                    }

                    //if (!dvwEmployeeCostCenters == null & intClockEmployeeBenefitID != 1)
                    //{
                    //    dvwEmployeeCostCenters.RowFilter = "UserSecurityGroupID = 4 and isallowed > 0 AND ForeignKeyID=" + row["ClientCostCenterID"];
                    //    intCostCenterCount = dvwEmployeeCostCenters.Count;
                    //}
                    if (intCostCenterCount > 0 & intClockEmployeeBenefitID != 1)
                    {
                        if (!IsDBNull(row["Date"]))
                        {
                            if (PunchOptionType.InputHours == intEEPunchOption)
                            {
                                if (Convert.ToBoolean(row["IsBenefit"]))
                                    row["SelectHoursShowing"] = true;
                            }
                            else if (!((string)(DBNull.Value.Equals(row["Date"]) ? "" : row["Date"])).ToUpper().EndsWith("ENTERED"))
                                row["SelectHoursShowing"] = true;
                        }

                        row["ExceptionStyle"] = "exception";

                        //string[] aFourSearchValues = new string[4];
                        //int intIndex = 0;

                        //aFourSearchValues[0] = intEmployeeID;
                        //aFourSearchValues[1] = datEventDate.ToString("MM/dd/yyyy");

                        // Check to see if there is a Cost Center ID
                        // If not, we don't want to add it to the array that is used in the .Find method below.
                        int? clientCostCenterId = IsDBNull(row["ClientCostCenterID"]) ? null : (int?)Convert.ToInt32(row["ClientCostCenterID"]);
                        //aFourSearchValues[3] = "True";

                        var intIndex = approveDatesList.FirstOrDefault(x => x.EmployeeID == intEmployeeID && datEventDate.Value.CompareTo(x.Eventdate) == 0 && x.ClientCostCenterID == clientCostCenterId && true == x.IsApproved);
                        if (intIndex != null)
                        {
                            if (intIndex.IsApproved == true)
                            {
                                if (true == Convert.ToBoolean(row["SelectHoursShowing"]))
                                {
                                    row["SelectHoursChecked"] = true;
                                    row["SelectHoursTooltip"] = intIndex.ApprovingUser;
                                    //hdnSelectHoursOriginal.Value = true;
                                }
                                row["ExceptionStyle"] = "approved";
                            }
                        }
                        if (!IsDBNull(clientCostCenterId)) ;      
                            //hdnClientCostCenterID.Value = clientCostCenterId;
                    }
                }
                else if (intApprovalOption > (int)ApprovalOptionType.All_Activity & IsDBNull(row["Header"]) & containsDayOrPunchIsEmpty)
                {

                    datEventDate = Convert.ToDateTime(row["EventDate"]);
                    intEmployeeID = (int)(DBNull.Value.Equals(row["EmployeeID"]) ? default(int) : Convert.ToInt32(row["EmployeeID"]));
                    
                    var intIndex = approveDateList.FirstOrDefault(x => x.EmployeeID == intEmployeeID && datEventDate.Value.CompareTo(x.Eventdate) == 0);
                    if (intIndex != null)
                    {
                        if (intIndex.ClockClientNoteID.HasValue)
                            intClockClientNoteID = !intIndex.ClockClientNoteID.HasValue ? int.MinValue : intIndex.ClockClientNoteID.Value;
                    }
                    //hdnClientCostCenterID.Value = "";
                }
                //else
                //    hdnClientCostCenterID.Value = "";
                
                if (Convert.ToBoolean(row["HideApprovalCheckbox"]) == false)
                {
                    row["SelectHoursShowing"] = true;
                    if (Convert.ToBoolean(row["IsApproved"]))
                    {
                        row["SelectHoursChecked"] = true;
                        object approvingUser = row["ApprovingUser"];
                        if (!IsDBNull(approvingUser))
                            row["SelectHoursTooltip"] = approvingUser.ToString();
                        //hdnSelectHoursOriginal.Value = true;
                        row["ExceptionStyle"] = "approved";
                    }
                }

                var strValue = "";

                if (!IsDBNull(row["EventDate"]))
                    datEventDate = Convert.ToDateTime(row["EventDate"]);

                if (datEventDate <= Convert.ToDateTime(row["PeriodEnded"]).Date)
                {
                    //strLockMessage = "javascript:alert(" + "\"Changes are not allowed, please contact your company administrator\"" + ");return false;";
                    //strValue = strLockMessage;
                    row["ChangesNotAllowed"] = true;
                    row["SelectHoursDisabled"] = true;
                }

                if (row["Day"].ToString().ToUpper().Contains("DAY") & IsDBNull(row["Header"]))
                {

                    // Determine visibility of AP and AB link buttons
                    // ---------------------------------------------------------------------------------------------------------

                    // Check if the punch info we are on is for the Supervisor that is viewing the TCA
                    //LinkButton lnkAddPunch = (LinkButton)e.Item.FindControl("lnkAddPunch");
                    //LinkButton lnkAddBenefit = (LinkButton)e.Item.FindControl("lnkAddBenefit");
                    if (user.UserTypeId == UserType.Supervisor && user.EmployeeId == System.Convert.ToInt32(row["EmployeeID"]))
                    {

                        var filteredClockRules = from employeeRule in empClockRules
                                                 where employeeRule.Key == Convert.ToInt32(row["EmployeeID"])
                                                 select employeeRule.Value;


                         // Check to make sure we found a clock rule for the supervisor.
                         // If not, use the options from supervisor security
                        if (filteredClockRules.Any())
                        {
                            // Show/Hide the AP and AB link buttons based on the supervisors clock rule
                            {
                                var withBlock = filteredClockRules.First();
                                //lnkAddPunch.Visible = System.Convert.ToBoolean(withBlock.EditPunches);
                                //lnkAddBenefit.Visible = System.Convert.ToBoolean(withBlock.EditBenefits);
                            }
                        }
                        else
                        {
                            // Show/Hide the AP and AB link buttons based on the global variables
                            //lnkAddPunch.Visible = bolAllowAddPunches;
                            //lnkAddBenefit.Visible = bolAllowAddBenefits;
                        }
                    }
                    else
                    {
                        // Show/Hide the AP and AB link buttons based on the global variables
                        //lnkAddPunch.Visible = bolAllowAddPunches;
                        //lnkAddBenefit.Visible = bolAllowAddBenefits;
                    }

                    // ---------------------------------------------------------------------------------------------------------

                   

                    // Allocated Hours

                    if ((IsDBNull(row["AllocateHoursFrequencyID"]) ? 0 : Convert.ToInt32(row["AllocateHoursFrequencyID"])) == 1 & !IsDBNull(row["Hours"]))
                    {
                        row["AddAllocationVisible"] = true;
                        //lnkAddAllocation.Visible = true;
                        //strValue = "'ModalContainer.aspx?URL=ClockEmployeeAllocateHours.aspx&EventDate=" + row["EventDate"] + "&EmployeeID=" + row["EmployeeID"] + "'";

                        //strValue = "javascript:OpenWindow(" + strValue + "," + "'height=675px; width=1000px; resizable=no; status=no; top=10px; left=10px;'" + ",this.id," + e.Item.ItemIndex.ToString() + ");return false;" + "";
                        row["AddAllocationPopUp"] = "'ModalContainer.aspx?URL=ClockEmployeeAllocateHours.aspx&EventDate=" + row["EventDate"] + "&EmployeeID=" + row["EmployeeID"] + "'";
                        //lnkAddAllocation.Attributes.Add("onclick", strValue);

                        // Lookup hours difference
                        if (difference != null)
                        {
                            var found = difference.TryGetValue(intEmployeeID.ToString() + datEventDate.ToString(), out var intSearchIndex);
                            if (found && intSearchIndex != null)
                            {
                                if (intSearchIndex.AllocatedHoursDifference != 0)
                                    row["SelectHoursDisabled"] = true;
                            }
                        }
                    }


                    if (((int)(DBNull.Value.Equals(row["PunchOption"]) ? default(int) : Convert.ToInt32(row["PunchOption"]))) == 2)
                        strValue = "'ModalContainer.aspx?URL=ClockEmployeeBenefitEdit.aspx&AddDate=" + row["EventDate"] + "&EmployeeID=" + row["EmployeeID"] + " &PunchOption=" + 2 + "'";
                    else
                        strValue = "'ModalContainer.aspx?URL=ClockEmployeePunchEdit.aspx&AddDate=" + row["EventDate"] + "&EmployeeID=" + row["EmployeeID"] + "'";
                    row["AddPunchPopUp"] = strValue;


                    //lnkAddPunch.Attributes.Add("onclick", strValue);


                    if (((int)(DBNull.Value.Equals(row["PunchOption"]) ? default(int) : Convert.ToInt32(row["PunchOption"]))) == 2)
                        strValue = "'ModalContainer.aspx?URL=ClockEmployeeBenefitEdit.aspx&AddDate=" + row["EventDate"] + "&EmployeeID=" + row["EmployeeID"] + " &PunchOption=" + 3 + "'";
                    else
                        strValue = "'ModalContainer.aspx?URL=ClockEmployeeBenefitEdit.aspx&AddDate=" + row["EventDate"] + "&EmployeeID=" + row["EmployeeID"] + "'";

                    row["AddBenefitPopUp"] = strValue;

                    //lnkAddBenefit.Attributes.Add("onclick", strValue);
                }

                bool blnSupViewingSelf = false;
                if (user.UserTypeId == UserType.Supervisor && user.EmployeeId == System.Convert.ToInt32(row["EmployeeID"]))
                    blnSupViewingSelf = true;
                

                // Case 5640 kgowdy - Add tooltip for input hours.
                //HtmlAnchor lnkDate = (HtmlAnchor)e.Item.FindControl("lnkDate");
                //if (lnkDate != null)
                    CheckTooltipOptionsForInputHours("lnkDate", row, showCostCenterTooltip, jobCostingInfo);

                // Check for lunch/break
                if (!IsDBNull(row["InClockClientLunchID"]))
                    FormatLunchBreakPunch(row, "In", LunchBreakType.EndOf);

                CheckTimeCardException((IsDBNull(row["InEmployeePunchID"]) ? "" : (string)row["InEmployeePunchID"]), "In", row, dvwExceptionHistorySorted, intApprovalOption);

                // CheckTooltipOptions(aPunch, IsDBNull(row["InCostCenterDesc"])? "": row["InCostCenterDesc"])
                CheckTooltipOptions(row, PunchType.InPunch, showCostCenterTooltip, jobCostingInfo);

                if ((DBNull.Value.Equals(row["In"]) ? "" : (string)row["In"]).Contains("Pending"))
                    AddLinkButtonAttribute("In", (int)(DBNull.Value.Equals(row["EmployeeID"]) ? default(int) : Convert.ToInt32(row["EmployeeID"])), (string)(DBNull.Value.Equals(row["EventDate"]) ? "" : row["EventDate"]), ((int)(DBNull.Value.Equals(row["InEmployeePunchID"]) ? default(int) : Convert.ToInt32(row["InEmployeePunchID"]))).ToString(), row);
                else
                {
                    strValue = "'ModalContainer.aspx?URL=ClockEmployeePunchEdit.aspx&ClockEmployeePunchID=" + row["InEmployeePunchID"] + "&EmployeeID=" + row["EmployeeID"] + strNewPassword + "&StopPostback=1" + "'";
                    strValue = "javascript:OpenWindow(" + strValue + "," + strValue2 + ",this.id," + data.Data.Table.Rows.IndexOf(row) + ");return false;" + "";

                    row["InModal"] = "'ModalContainer.aspx?URL=ClockEmployeePunchEdit.aspx&ClockEmployeePunchID=" + row["InEmployeePunchID"] + "&EmployeeID=" + row["EmployeeID"] + strNewPassword + "&StopPostback=1" + "'";
                    //aPunch.Attributes.Add("onclick", strValue);
                }


                // Punch In 2

                //aPunch = e.Item.FindControl("lnkPunchIn2Time");

                // Check for lunch/break
                if (!IsDBNull(row["In2ClockClientLunchID"]))
                    FormatLunchBreakPunch(row, "In2", LunchBreakType.EndOf);

                CheckTimeCardException(IsDBNull(row["In2EmployeePunchID"]) ? "" : (string)row["In2EmployeePunchID"], "In2", row, dvwExceptionHistorySorted, intApprovalOption);

                CheckTooltipOptions(row, PunchType.In2Punch, showCostCenterTooltip, jobCostingInfo);

                if (((string)(DBNull.Value.Equals(row["In2"]) ? "" : row["In2"])).Contains("Pending"))
                    AddLinkButtonAttribute("In2", (DBNull.Value.Equals(row["EmployeeID"]) ? default(int) : Convert.ToInt32(row["EmployeeID"])), (string)(DBNull.Value.Equals(row["EventDate"]) ? "" : row["EventDate"]), IsDBNull(row["In2EmployeePunchID"])? "":(string)row["In2EmployeePunchID"], row);
                else
                {
                    strValue = "'ModalContainer.aspx?URL=ClockEmployeePunchEdit.aspx&ClockEmployeePunchID=" + row["In2EmployeePunchID"] + "&EmployeeID=" + row["EmployeeID"] + strNewPassword + "&StopPostback=1" + "'";
                    //strValue = "javascript:OpenWindow(" + strValue + "," + strValue2 + ",this.id," + e.Item.ItemIndex.ToString() + ");return false;" + "";

                    row["In2Modal"] = "'ModalContainer.aspx?URL=ClockEmployeePunchEdit.aspx&ClockEmployeePunchID=" + row["In2EmployeePunchID"] + "&EmployeeID=" + row["EmployeeID"] + strNewPassword + "&StopPostback=1" + "'";

                    //aPunch.Attributes.Add("onclick", strValue);
                }

                // Punch Out

                //aPunch = e.Item.FindControl("lnkPunchOutTime");

                // Check if 'Missing' punch
                strNewPassword = CheckForMissingPunch((string)(DBNull.Value.Equals(row["Out"]) ? "" : row["Out"]), strNewPassword, row, "Out");

            // Check for lunch/break
                if (!IsDBNull(row["OutClockClientLunchID"]))
                    FormatLunchBreakPunch(row, "Out", LunchBreakType.StartOf);

                CheckTimeCardException(IsDBNull(row["OutEmployeePunchID"]) ? "" : (string)row["OutEmployeePunchID"], "Out", row, dvwExceptionHistorySorted, intApprovalOption);

                CheckTooltipOptions(row, PunchType.OutPunch, showCostCenterTooltip, jobCostingInfo);


                // Show punch request popup if Pending punch
                var outString = IsDBNull(row["Out"]) ? "" : (string)row["Out"];
                if (outString.Contains("Pending") || (outString.Contains("Missing") & blnSupViewingSelf))
                    AddLinkButtonAttribute("Out", (DBNull.Value.Equals(row["EmployeeID"]) ? default(int) : Convert.ToInt32(row["EmployeeID"])), (string)(DBNull.Value.Equals(row["EventDate"]) ? "" : row["EventDate"]), IsDBNull(row["OutEmployeePunchID"])? "": (string)row["OutEmployeePunchID"], row);
                else
                {
                    strValue = "'ModalContainer.aspx?URL=ClockEmployeePunchEdit.aspx&ClockEmployeePunchID=" + row["OutEmployeePunchID"] + "&EmployeeID=" + row["EmployeeID"] + strNewPassword + "&StopPostback=1" + "'";
                    strValue = "javascript:OpenWindow(" + strValue + "," + strValue2 + ",this.id," + data.Data.Table.Rows.IndexOf(row) + ");return false;" + "";


                    row["OutModal"] = "'ModalContainer.aspx?URL=ClockEmployeePunchEdit.aspx&ClockEmployeePunchID=" + row["OutEmployeePunchID"] + "&EmployeeID=" + row["EmployeeID"] + strNewPassword + "&StopPostback=1" + "'";
                    //aPunch.Attributes.Add("onclick", strValue);
                }


                // Punch Out 2

                //aPunch = e.Item.FindControl("lnkPunchOut2Time");

                // Check if 'Missing' punch
                CheckForMissingPunch((string)(DBNull.Value.Equals(row["Out2"]) ? "" : row["Out2"]), strNewPassword, row, "Out2");

                // Check for lunch/break
                if (!IsDBNull(row["Out2ClockClientLunchID"]))
                    FormatLunchBreakPunch(row, "Out2", LunchBreakType.StartOf);

                CheckTimeCardException(IsDBNull(row["Out2EmployeePunchID"])? "": (string)row["Out2EmployeePunchID"], "Out2", row, dvwExceptionHistorySorted, intApprovalOption);

                CheckTooltipOptions(row, PunchType.Out2Punch, showCostCenterTooltip, jobCostingInfo);


                // Show punch request popup if Pending punch
                var out2String = IsDBNull(row["Out2"]) ? "" : (string)row["Out2"];
                if (out2String.Contains("Pending") || (out2String.Contains("Missing") & blnSupViewingSelf))
                    AddLinkButtonAttribute("Out2", (DBNull.Value.Equals(row["EmployeeID"]) ? default(int) : Convert.ToInt32(row["EmployeeID"])), (string)(DBNull.Value.Equals(row["EventDate"]) ? "" : row["EventDate"]), IsDBNull(row["Out2EmployeePunchID"])? "": (string)row["Out2EmployeePunchID"], row);
                else
                {
                    strValue = "'ModalContainer.aspx?URL=ClockEmployeePunchEdit.aspx&ClockEmployeePunchID=" + row["Out2EmployeePunchID"] + "&EmployeeID=" + row["EmployeeID"] + strNewPassword + "&StopPostback=1" + "'";
                    strValue = "javascript:OpenWindow(" + strValue + "," + strValue2 + ",this.id," + data.Data.Table.Rows.IndexOf(row) + ");return false;" + "";

                    row["Out2Modal"] = "'ModalContainer.aspx?URL=ClockEmployeePunchEdit.aspx&ClockEmployeePunchID=" + row["Out2EmployeePunchID"] + "&EmployeeID=" + row["EmployeeID"] + strNewPassword + "&StopPostback=1" + "'";

                    //aPunch.Attributes.Add("onclick", strValue);
                }


                // Edit Benefit

                //Label lblDate = (Label)e.Item.FindControl("lblDate");
                if (!IsDBNull(row["ClockEmployeeBenefitID"]))
                {
                    row["lblDateShowing"] = false;
                    //lblDate.Visible = false;
                    row["lnkDateShowing"] = true;
                    //lnkDate.Visible = true;

                    if ((Convert.ToInt32(row["PunchOption"])) == 2)
                        strValue = "'ModalContainer.aspx?URL=ClockEmployeeBenefitEdit.aspx&ClockEmployeeBenefitID=" + row["ClockEmployeeBenefitID"] + " &EmployeeID=" + row["EmployeeID"] + strPassword + " &PunchOption=" + 2 + "&StopPostback=1" + "'";
                    else
                        strValue = "'ModalContainer.aspx?URL=ClockEmployeeBenefitEdit.aspx&ClockEmployeeBenefitID=" + row["ClockEmployeeBenefitID"] + " &EmployeeID=" + row["EmployeeID"] + strPassword + "&StopPostback=1" + "'";

                    strValue = "javascript:OpenWindow(" + strValue + "," + strValue2 + ",this.id," + data.Data.Table.Rows.IndexOf(row) + ");return false;" + "";

                    row["lnkDateModal"] = strValue;

                    //lnkDate.Attributes.Add("onclick", strValue);
                    //lnkDate.Attributes.CssStyle.Add(HtmlTextWriterStyle.Cursor, "pointer");
                }


                // Edit Request Time Off rrice 11/23/2010 CR 4681

                if (!IsDBNull(row["RequestTimeOffID"]))
                {
                    strValue = "'ModalContainer.aspx?URL=RequestTimeOffPopup.aspx&strRequestTimeOffID=" + row["RequestTimeOffID"] + "&IsFromLaborSource=1" + "'";
                    strValue = "javascript:OpenWindow(" + strValue + "," + strValue2 + ",this.id," + data.Data.Table.Rows.IndexOf(row) + ");return false;" + "";
                    row["lnkDateModal"] = strValue;
                    //lnkDate.Attributes.Add("onclick", strValue);
                    //lnkDate.Attributes.CssStyle.Add(HtmlTextWriterStyle.Cursor, "pointer");
                }


                // Schedule

                //LinkButton lnkSchedule = (LinkButton)e.Item.FindControl("lnkSchedule");
                if (!IsDBNull(row["Schedule"]))
                {
                    string schedule = (string)(DBNull.Value.Equals(row["Schedule"]) ? "" : row["Schedule"]);

                    // If we have a schedule or if we're not displaying a time policy
                    if (!IsDBNull(row["ClockEmployeeScheduleID"]) || !schedule.StartsWith("Time Policy", StringComparison.OrdinalIgnoreCase))
                    {
                        strValue = "'ModalContainer.aspx?URL=ClockEmployeeScheduleEdit.aspx&ClockEmployeeScheduleID=" + row["ClockEmployeeScheduleID"] + " &EventDate=" + row["EventDate"] + " &EmployeeID=" + row["EmployeeID"] + "'";
                        strValue2 = Popup.Create("", height: 500, width: 650, allowResize: false, includeStatusBar: false).OptionsParamJsonObject;
                        strValue = "javascript:OpenWindow(" + strValue + "," + strValue2 + ",this.id," + data.Data.Table.Rows.IndexOf(row) + ");return false;" + "";
                        row["lnkScheduleModal"] = strValue;
                        //lnkSchedule.Attributes.Add("onclick", strValue);

                        //Label lblRealSchedule = (Label)e.Item.FindControl("lblRealSchedule");
                        if (Convert.ToBoolean(row["IsScheduleReal"]) == true) ;
                        // Put the checkmark character in the label
                        //lblRealSchedule.Text = Strings.ChrW(0xF00);
                    }
                    else
                        row["lnkScheduleDisabled"] = true;
                        //lnkSchedule.Enabled = false;
                }


                // Second Schedule

                if (!IsDBNull(row["Schedule2"]))
                    row["Schedule2Modal"] = strValue;
                    //(LinkButton)e.Item.FindControl("lnkSchedule2").Attributes.Add("onclick", strValue);


                // Third Schedule

                if (!IsDBNull(row["Schedule3"]))
                    row["Schedule3Modal"] = strValue;
                //(LinkButton)e.Item.FindControl("lnkSchedule3").Attributes.Add("onclick", strValue);


                // Special Settings

                //objLabelDate = e.Item.FindControl("lblDate");

                if (!IsDBNull(row["Header"]))
                    row[GetClassColumnForPunch("lblDate")] = "time-card-employee-header";
                //e.Item.CssClass = "time-card-employee-header";
                else if (Convert.ToBoolean(row["IsTotalRow"]) == true)
                    row[GetClassColumnForPunch("lblDate")] = "totals-row";
                else if (!((string)(DBNull.Value.Equals(row[GetTooltipColumnForPunch("lblDate")]) ? "" : row[GetTooltipColumnForPunch("lblDate")])).Contains("/") && !string.IsNullOrEmpty((string)(DBNull.Value.Equals(row[GetTooltipColumnForPunch("lblDate")]) ? "" : row[GetTooltipColumnForPunch("lblDate")])))
                    //e.Item.Font.Bold = true;


                // Show/Hide exceptions checkbox

                if (intApprovalOption == (int)ApprovalOptionType.Exceptions)
                {
                        if (string.IsNullOrEmpty((string)(DBNull.Value.Equals(row["Exceptions"]) ? "" : row["Exceptions"])))
                            row["SelectHoursShowing"] = false;
                        //rwChkSelectHours.Visible = false;
                }

                //Label lblNotes = (Label)e.Item.FindControl("lblNotes");
                row["lblNotesVisible"] = totals.Filters.HasNotes;
                //lblNotes.Visible = true;
                //lblNotes.Font.Bold = false;

                if (totals.Filters.HasNotes)
                {
                    if (IsDBNull(row["Header"]) && Convert.ToBoolean(row["IsTotalRow"]) == false)
                    {
                        if (!IsDBNull(row["Schedule"]) && !string.IsNullOrEmpty((string)row["Schedule"]))
                        {
                            row["SetClockClientNoteID"] = intClockClientNoteID;
                        }
                    }
                }

                // rrice 8/13/2009 Hide ClockName Field if there are none
                if (!IsDBNull(row["ClockName"]) && !string.IsNullOrEmpty((string)row["ClockName"]))
                {
                    row["imgClockNameVisible"] = true;
                }

                if (!IsDBNull(row["Header"]) && Convert.ToBoolean(row["HasGeofencing"]) == true) 
                {
                    row["imgGeofencingVisible"] = true;
                }

                // rrice 8/31/2009 check employees with no activity 
                if (((int)(DBNull.Value.Equals(row["EmployeeActivity"]) ? default(int) : Convert.ToInt32(row["EmployeeActivity"]))) > 1)
                {
                    if (intApprovalOption == (int)ApprovalOptionType.Everyday || intApprovalOption == (int)ApprovalOptionType.All_Activity)
                    {
                        row["SelectHoursShowing"] = true;
                        row["Exceptions"] = "No Activity";
                        row["SelectHoursDisabled"] = false;
                        if (((int)(DBNull.Value.Equals(row["EmployeeActivity"]) ? default(int) : Convert.ToInt32(row["EmployeeActivity"]))) == 2)
                        {
                            //lblDate.CssClass = "exception";
                            row["ExceptionStyle"] = "exception";
                            row["SelectHoursChecked"] = false;
                        }
                        else
                        {
                            //lblDate.CssClass = "";
                            row["ExceptionStyle"] = "";
                            row["SelectHoursChecked"] = true;
                        }
                    }
                    else if (intApprovalOption > (int)ApprovalOptionType.None)
                        row["Exceptions"] = "No Activity";
                }

                // Hide checkbox if there is a missing punch
                if (((string)(DBNull.Value.Equals(row["Exceptions"]) ? "" : row["Exceptions"])).Contains("MISSING PUNCH"))
                    row["SelectHoursShowing"] = false;

                // Hide checkbox if there is no activity for the day
                if (Convert.ToBoolean(row["HideApprovalCheckBox"]) == true)
                    row["SelectHoursShowing"] = false;
            };



            return data;

        }

        public void AddLinkButtonAttribute(string punch, int employeeId, string dateOfPunch, string clockEmployeePunchRequestID, DataRow row)
        {
            string punchDate = "";
            string attributeText = "";
            string createPopup = "";
            string employeeIdString;
            string punchRequestId = "";


            if (DateTime.TryParse(dateOfPunch, out var result))
            {
                if ("Missing".CompareTo(punch) == 0  || punch.Contains("Pending"))
                {
                    //createPopup = Popup.Create("", height: 700, width: 600, allowResize: false, includeStatusBar: false).OptionsParamJsonObject;

                    employeeIdString = "EmployeeID=" + employeeId;
                    punchDate = "&PunchDate=" + dateOfPunch;

                    if (clockEmployeePunchRequestID == "")
                        clockEmployeePunchRequestID = "0";

                    punchRequestId = "&PunchRequestID=" + clockEmployeePunchRequestID;

                    attributeText = "'ModalContainer.aspx?URL=ClockEmployeePunchRequest.aspx&" + employeeIdString + punchDate + punchRequestId + "'";
                    //attributeText = "javascript:OpenWindow(" + attributeText + "," + createPopup + ",this.id," + e.Item.ItemIndex.ToString + ");return false;" + "";
                    row[punch + "Modal"] = attributeText;
                    //punch.Attributes.Add("onclick", attributeText);
                    row[punch + "Disabled"] = false;
                    row[GetClassColumnForPunch(punch)] = "exception";
                    //punch.Attributes.Add("class", "exception");
                }
            }
        }


        private void CheckTooltipOptionsForInputHours(string prefix, DataRow row, bool showCostCenterTooltip, GetClientJobCostingInfoByClientIDResultsDto jobCostInfo)
        {
            if (showCostCenterTooltip)
                CheckTooltipOptionsForPunchPrefix(prefix, row, "", jobCostInfo);
        }

        private void CheckTooltipOptions(DataRow row, PunchType punchType, bool showCostCenterTooltip, GetClientJobCostingInfoByClientIDResultsDto jobCostInfo)
        {

            // Check the tooltip company option
            if (showCostCenterTooltip)
            {
                string punchPrefix = "";
                // Determine what type of punch we have
                switch (punchType)
                {
                    case PunchType.InPunch:
                        {
                            punchPrefix = "In";
                            break;
                        }

                    case PunchType.OutPunch:
                        {
                            punchPrefix = "Out";
                            break;
                        }

                    case PunchType.In2Punch:
                        {
                            punchPrefix = "In2";
                            break;
                        }

                    case PunchType.Out2Punch:
                        {
                            punchPrefix = "Out2";
                            break;
                        }
                }
                CheckTooltipOptionsForPunchPrefix(punchPrefix, row, punchPrefix, jobCostInfo);
            }
        }

        private void FormatLunchBreakPunch(DataRow row, string punch, LunchBreakType type)
        {
            string lunchBreakType;

            if (type == LunchBreakType.StartOf)
                lunchBreakType = "Start";
            else if (type == LunchBreakType.EndOf)
                lunchBreakType = "End";
            else
                lunchBreakType = "";

            row[GetClassColumnForPunch(punch)] = "lunch-break";
            row[GetTooltipColumnForPunch(punch)] = "<b>" + lunchBreakType + " of Lunch/Break</b>";
        }

        private void CheckTooltipOptionsForPunchPrefix(string punch, DataRow row, string punchPrefix, GetClientJobCostingInfoByClientIDResultsDto jobCostInfo)
        {
            string tooltipText = "";

            bool includeCostCenter = true;
            string costCenterLabel = CostCenterLabel;
            string costCenterDesc = "";

            bool includeDepartment = true;
            string departmentLabel = DepartmentLabel;
            string departmentDesc = "";

            bool includeDivision = true;
            string divisionLabel = DivisionLabel;
            string divisionDesc = "";

            bool includeCustom = true;
            string customLabel = "";
            string customDesc = "";



            List<GetClientJobCostingInfoByClientIDDto.table1> jobCostList = jobCostInfo.results1.ToList();
            List<GetClientJobCostingInfoByClientIDDto.table2> jobCostAssignList = jobCostInfo.results2.ToList();
            List<GetClientJobCostingInfoByClientIDDto.table3> jobCostAssignDescList = jobCostInfo.results3.ToList();
            List<JobCostTooltipInfo> jobCostTooltipInfoList = new List<JobCostTooltipInfo>();
            JobCostTooltipInfo jobCostTooltipInfo;
            string jobCostColumn;
            int jobCostAssignId;

            // Intial setup for job costing info that will show up in the tooltip
            if (jobCostList != null)
            {

                // Loop through each Job Cost object
                foreach (GetClientJobCostingInfoByClientIDDto.table1 jobCost in jobCostList)
                {

                    // Create a new JobCostTooltipInfo object
                    jobCostTooltipInfo = new JobCostTooltipInfo(jobCost.ClientJobCostingID, jobCost.Description, jobCost.Level, (ClientJobCostingType)jobCost.JobCostingTypeID);

                    if (jobCostTooltipInfo != null)
                        // Add the object to the list
                        jobCostTooltipInfoList.Add(jobCostTooltipInfo);
                }
            }

            if (jobCostTooltipInfoList != null)
            {


                // Set the job costing assignment info for the tooltip
                // Job Costing Info (1-6)
                for (int i = 1; i <= 6; i++)
                {
                    jobCostAssignId = 0;

                    // Determine the column name
                    jobCostColumn = punchPrefix + "JobCostAssignID_" + i;

                    if (row.Table.Columns.Contains(jobCostColumn))
                    {

                        // Get the ID out of the column
                        if (!IsDBNull(row[jobCostColumn]))
                        {
                            if (!string.IsNullOrEmpty((string)row[jobCostColumn]))
                                jobCostAssignId = Convert.ToInt32(row[jobCostColumn]);
                        }


                        if (jobCostAssignId != 0)

                            // Set the job costing info for the tooltip (label & description)
                            DetermineJobCostTooltipInfo(jobCostTooltipInfoList, jobCostAssignId, jobCostAssignList, jobCostAssignDescList);
                    }
                }
                var searchArray = new ClientJobCostingType[]{
                    ClientJobCostingType.Division,
                    ClientJobCostingType.Department,
                    ClientJobCostingType.CostCenter
        };
                bool hasJobCostingSection = jobCostTooltipInfoList.Any(tt => !string.IsNullOrEmpty(tt.JobCostAssignDesc) && !searchArray.Contains(tt.JobCostType));


                // Now that the job costing info for the tooltip has been set, 
                // loop through each job costing item and add its details to the tooltip
                foreach (JobCostTooltipInfo tooltipItem in jobCostTooltipInfoList.OrderBy(info => info.JobCostLevel))
                {
                    {
                        var withBlock = tooltipItem;

                        // Check if the job cost type is Cost Center, Department or Division.
                        // If so, we need to make sure we don't add this info again outside the job costing section (i.e. below)
                        switch (withBlock.JobCostType)
                        {
                            case ClientJobCostingType.CostCenter:
                                {
                                    if (withBlock.JobCostAssignDesc == "")
                                    {
                                        withBlock.JobCostAssignDesc = IsDBNull(row[punchPrefix + "CostCenterDesc"]) ? "" : (string)row[punchPrefix + "CostCenterDesc"];

                                        if (!string.IsNullOrEmpty(withBlock.JobCostAssignDesc))
                                        {
                                            if (includeCostCenter)
                                                includeCostCenter = !hasJobCostingSection;
                                        }
                                    }

                                    break;
                                }

                            case ClientJobCostingType.Department:
                                {
                                    if (withBlock.JobCostAssignDesc == "")
                                    {
                                        withBlock.JobCostAssignDesc = IsDBNull(row[punchPrefix + "DepartmentDesc"]) ? "" : (string)row[punchPrefix + "DepartmentDesc"];

                                        if (includeDepartment)
                                            includeDepartment = !hasJobCostingSection;
                                    }

                                    break;
                                }

                            case ClientJobCostingType.Division:
                                {
                                    if (withBlock.JobCostAssignDesc == "")
                                    {
                                        withBlock.JobCostAssignDesc = IsDBNull(row[punchPrefix + "DivisionDesc"]) ? "" : (string)row[punchPrefix + "DivisionDesc"];

                                        if (includeDivision)
                                            includeDivision = !hasJobCostingSection;
                                    }

                                    break;
                                }
                            case ClientJobCostingType.Custom:
                            {
                                if (withBlock.JobCostAssignDesc == "")
                                {
                                    if (punchPrefix != "")
                                    {
                                        withBlock.JobCostAssignDesc = getAssignmentDescription(row, punchPrefix, "1");
                                        withBlock.JobCostAssignDesc = getAssignmentDescription(row, punchPrefix, "2");
                                        withBlock.JobCostAssignDesc = getAssignmentDescription(row, punchPrefix, "3");
                                    }

                                    if (includeCustom)
                                        includeCustom = !hasJobCostingSection;
                                }

                                break;
                            }

                            default:
                                {
                                    break;
                                }
                        }

                        // Check to see if we have a label and description for the job cost.
                        // If so, add them to the tooltip text.
                        if (!string.IsNullOrEmpty(withBlock.JobCostLabel) && !string.IsNullOrEmpty(withBlock.JobCostAssignDesc) && hasJobCostingSection)
                            AddLineBreaksAndInfoToTooltip(ref tooltipText, tooltipItem.JobCostLabel, tooltipItem.JobCostAssignDesc);
                    }
                }
            }

            if (includeDivision)
            {
                // Set division description
                divisionDesc = IsDBNull(row[punchPrefix + "DivisionDesc"]) ? "" : (string)row[punchPrefix + "DivisionDesc"];

                // Add division info to the tooltip
                AddLineBreaksAndInfoToTooltip(ref tooltipText, divisionLabel, divisionDesc);
            }

            if (includeDepartment)
            {
                // Set department description
                departmentDesc = IsDBNull(row[punchPrefix + "DepartmentDesc"]) ? "" : (string)row[punchPrefix + "DepartmentDesc"];

                // Add department info to the tooltip
                AddLineBreaksAndInfoToTooltip(ref tooltipText, departmentLabel, departmentDesc);
            }

            if (includeCostCenter)
            {
                // Set cost center description
                costCenterDesc = IsDBNull(row[punchPrefix + "CostCenterDesc"]) ? "" : (string)row[punchPrefix + "CostCenterDesc"];

                // Add cost center info to the tooltip
                AddLineBreaksAndInfoToTooltip(ref tooltipText, costCenterLabel, costCenterDesc);
            }

            if (includeCustom)
            {
                // Set custom description
                if (punchPrefix != "")
                {
                    updateToolTip(row, punchPrefix, "1", ref tooltipText);
                    updateToolTip(row, punchPrefix, "2", ref tooltipText);
                    updateToolTip(row, punchPrefix, "3", ref tooltipText);
                }
                else
                {
                    customDesc = "";
                    AddLineBreaksAndInfoToTooltip(ref tooltipText, customLabel, customDesc);
                }
            }

            // Make sure we have text for the tooltip
            if (!string.IsNullOrEmpty(tooltipText))
                {
                    string resultingCssClass;
                    string currentCssClass;

                    // Get the current class
                    currentCssClass = IsDBNull(row[GetClassColumnForPunch(punch)]) ? "" : (string)row[GetClassColumnForPunch(punch)];

                    // Check if the class includes 'js-popover'. 
                    // If not, we need to add it so that the tooltip shows
                    if (!currentCssClass.Contains("js-popover"))
                    {

                        // Add the 'js-popover' class
                        resultingCssClass = AppendCssClass(currentCssClass, "js-popover");
                    row[GetClassColumnForPunch(punch)] = resultingCssClass;
                    }

                // Set the text of the tooltip
                row[GetTooltipColumnForPunch(punch)] = tooltipText;

                // Set the title of the tooltip
                row[GetTitleColumnForPunch(punch)] = "Job Classifications";
                    //aPunch.Attributes("data-original-title") = "Job Classifications";
                }
        }

        private string getAssignmentDescription(DataRow row, string punchPrefix, string pos)
        {
            var tmpString = IsDBNull(row[punchPrefix + "JobCostAssignID_" + pos]) ? "" : (string)row[punchPrefix + "JobCostAssignID_" + pos];
            var retVal = "";

            if (tmpString != "")
            {
                var assign = _session.UnitOfWork.JobCostingRepository
                    .GetJobCostingAssignmentQuery()
                    .ByClientJobCostingAssignmentId(Int32.Parse(tmpString)).FirstOrDefault();
                retVal = assign.Description;
            }

            return retVal;
        }

        private void updateToolTip(DataRow row, string punchPrefix, string pos, ref string tooltipText)
        {
            var tmpString = IsDBNull(row[punchPrefix + "JobCostAssignID_" + pos]) ? "" : (string)row[punchPrefix + "JobCostAssignID_" + pos];
            var customDesc1 = "";
            var customLabel1 = "";
            if (tmpString != "")
            {
                var assign = _session.UnitOfWork.JobCostingRepository
                    .GetJobCostingAssignmentQuery()
                    .ByClientJobCostingAssignmentId(Int32.Parse(tmpString)).FirstOrDefault();
                customDesc1 = assign.Description;

                var jobCost = _session.UnitOfWork.JobCostingRepository.QueryClientJobCosting()
                    .ByClientJobCostingId(assign.ClientJobCostingId).FirstOrDefault();
                customLabel1 = jobCost.Description;
            }
            AddLineBreaksAndInfoToTooltip(ref tooltipText, customLabel1, customDesc1);
        }

        private int MapApprovalOptionFromClientOptionItemIDs(int approvalOption)
        {
            switch (approvalOption) 
            {
                case (int)ApprovalOptions.None:
                    {
                        return 1;
                    }

                case (int)ApprovalOptions.Hours_And_Benefits:
                    {
                        return 2;
                    }

                case (int)ApprovalOptions.Exceptions:
                    {
                        return 3;
                    }

                case (int)ApprovalOptions.Everyday:
                    {
                        return 4;
                    }

                case (int)ApprovalOptions.All_Activity:
                    {
                        return 5;
                    }

                case (int)ApprovalOptions.Cost_Center:
                    {
                        return 6;
                    }
            }
            return approvalOption;
        }


        private void AddLineBreaksAndInfoToTooltip(ref string tooltipText, string label, string description)
        {
            if (!string.IsNullOrEmpty(description))
            {
                if (!string.IsNullOrEmpty(tooltipText))
                    tooltipText += "<br><br>";

                label = "<b>" + label + ": </b>";

                tooltipText += label + description;
            }
        }

        private void CheckTimeCardException(string strEmployeePunchID,string punchName, DataRow row, IEnumerable<GetClockEmployeeExceptionHistoryByEmployeeIDDto> dvwExceptionHistorySorted, int intApprovalOption)
        {
            int intID;
            double dblHours;
            var currentCssClass = IsDBNull(row[punchName + "Class"]) ? "" : (string)row[punchName + "Class"];
            var currentPunchContent = IsDBNull(row[punchName + "ToolTipContent"]) ? "" : (string)row[punchName + "ToolTipContent"];

            if (!string.IsNullOrEmpty(strEmployeePunchID))
            {
                intID = int.Parse(strEmployeePunchID);

                var intIndex = dvwExceptionHistorySorted.FirstOrDefault(x => x.ClockEmployeePunchID == intID);
                if (intIndex != null)
                {
                    string resultingCssClass = "";
                    
                    if (intIndex.IsApproved == true)
                    {
                        if (intApprovalOption < (int)ApprovalOptionType.Cost_Center)
                            resultingCssClass = AppendCssClass(currentCssClass, "approved");
                    }
                    else
                    {
                        // If intApprovalOption < ApprovalOptionType.Cost_Center Then
                        resultingCssClass = AppendCssClass(currentCssClass, "exception");
                        // End If

                        // Check if the class includes 'js-popover'. 
                        // If not, we need to add it so that the tooltip shows
                        if (!resultingCssClass.Contains("js-popover"))
                            resultingCssClass = AppendCssClass(resultingCssClass, "js-popover");

                        dblHours = !intIndex.Hours.HasValue ? 0 : intIndex.Hours.Value;

                        if (dblHours > 0)
                            currentPunchContent = intIndex.ClockException + " " + FormatHoursAsString(Convert.ToInt32(dblHours * 60));
                        else
                            currentPunchContent = intIndex.ClockException;

                        //currentPunchContent += GetEmployeePointsString(intIndex.EmployeeID, intIndex.EventDate, intIndex.ClockExceptionID, intIndex.ClockEmployeeExceptionHistoryID);
                    }
                    row[punchName + "Class"] = resultingCssClass;
                    row[punchName + "ToolTipContent"] = currentPunchContent;
                }
            }
        }

        public string FormatHoursAsString(int intMinutes)
        {
            string strHours = "";
            strHours = Math.Truncate(TimeSpan.FromMinutes(intMinutes).TotalHours).ToString() + ":" + TimeSpan.FromMinutes(intMinutes).Minutes.ToString().PadLeft(2, '0');
            strHours = "00:00".CompareTo(strHours) == 0 ? "" : strHours;

            return strHours;
        }

        private string GetTooltipColumnForPunch(string punch)
        {
            return punch + "ToolTipContent";
        }

        private string GetTitleColumnForPunch(string punch)
        {
            return punch + "Title";
        }

        private string GetClassColumnForPunch(string punch)
        {
            return punch + "Class";
        }

        private void AddPropertyColumnForProvidedColumns(IEnumerable<string> existingColumns, string propertyColumn, Type columnType, DataTable table)
        {
            foreach(var x in existingColumns)
            {
                table.Columns.Add(x + propertyColumn, columnType);
            };
        }

        //private string GetEmployeePointsString(int intEmployeeID, DateTime datEvent, int intClockExceptionID, int intClockEmployeeExceptionHistoryID)
        //{
        //    string[] aSearchValuesPoints = new string[4];
        //    int intIndex_Points;
        //    double dblPoints;
        //    //if (dvwEmployeePoints != null)
        //    //{
        //    //    aSearchValuesPoints[0] = intEmployeeID;
        //    //    aSearchValuesPoints[1] = datEvent.ToString("MM/dd/yyyy");
        //    //    aSearchValuesPoints[2] = intClockExceptionID;
        //    //    aSearchValuesPoints[3] = intClockEmployeeExceptionHistoryID;

        //    //    intIndex_Points = dvwEmployeePoints.Find(aSearchValuesPoints);
        //    //    if (intIndex_Points >= 0)
        //    //    {
        //    //        dblPoints = dvwEmployeePoints(intIndex_Points)("Amount");
        //    //        if (!dblPoints == 0)
        //    //            return " (" + dblPoints.ToString() + ")";
        //    //    }
        //    //}
        //    return "";
        //}


        private string AppendCssClass(string currentCssClass, string cssClassToAppend)
        {
            string result = "";

            if (!string.IsNullOrEmpty(currentCssClass))
                result = currentCssClass + " " + cssClassToAppend;
            else
                result = cssClassToAppend;

            return result;
        }

        private void DetermineJobCostTooltipInfo(
            List<JobCostTooltipInfo> jobCostTooltipInfoList, 
            int jobCostAssignId, 
            List<GetClientJobCostingInfoByClientIDDto.table2> jobCostAssignList,
            List<GetClientJobCostingInfoByClientIDDto.table3> jobCostAssignDescList)
        {
            GetClientJobCostingInfoByClientIDDto.table2 jobCostAssignment;

            // Get the job costing assignment information based on the ID passed in
            jobCostAssignment = FindJobCostAssignDetailById(jobCostAssignList, jobCostAssignId);

            if (jobCostAssignment != null)
            {
                JobCostTooltipInfo jobCostTooltipInfo;

                // Get the job cost tooltip info associated with the assignment
                jobCostTooltipInfo = JobCostTooltipInfo.FindJobCosTooltipInfotById(jobCostTooltipInfoList, jobCostAssignment.JobCostID);

                jobCostTooltipInfo.JobCostAssignId = jobCostAssignment.ID;

                // Set the description of the job costing info
                if (jobCostAssignment.ForeignKeyID == 0)

                    // There is no foreign key, so use the description and code from the assignment object
                    jobCostTooltipInfo.JobCostAssignDesc = jobCostAssignment.Description;
                else
                {
                    GetClientJobCostingInfoByClientIDDto.table3 jobCostAssignDesc;

                    // There is foreign key, so use it to find the proper assignment detail info/object
                    jobCostAssignDesc = JobCostAssignmentDescription.FindJobCostAssignDescById(jobCostAssignDescList, jobCostAssignment.ForeignKeyID.GetValueOrDefault());

                    if (jobCostAssignDesc != null)
                        jobCostTooltipInfo.JobCostAssignDesc = jobCostAssignDesc.Description;
                }
            }
        }

        public class JobCostAssignmentDescription
        {
            private const string COL_ID = "ID";
            private const string COL_DESCRIPTION = "Description";
            private const string COL_TYPEID = "JobCostingTypeID";

            public int ID { get; set; }
            public string Description { get; set; }
            public int JobCostingTypeId { get; set; }

            public static List<JobCostAssignmentDescription> LoadJobCostingAssignmentDescriptions(DataSet data, int tableIndex = 0)
            {
                List<JobCostAssignmentDescription> list = new List<JobCostAssignmentDescription>();

                if ((data.Tables.Count > tableIndex))
                {
                    foreach (DataRow row in data.Tables[tableIndex].Rows)
                        list.Add(new JobCostAssignmentDescription()
                        {
                            ID = row.GetValueOrDefault<int>(COL_ID, 0),
                            Description = row.GetValueOrDefault<string>(COL_DESCRIPTION, ""),
                            JobCostingTypeId = row.GetValueOrDefault<int>(COL_TYPEID, 0)
                        });
                }

                return list;
            }

            public static GetClientJobCostingInfoByClientIDDto.table3 FindJobCostAssignDescById(List<GetClientJobCostingInfoByClientIDDto.table3> assignDescList, int foreignKeyId)
            {
                GetClientJobCostingInfoByClientIDDto.table3 jobCostAssignDesc = new GetClientJobCostingInfoByClientIDDto.table3();

                jobCostAssignDesc = assignDescList.FirstOrDefault(x => x.ID == foreignKeyId);

                return jobCostAssignDesc;
            }
        }


        public static GetClientJobCostingInfoByClientIDDto.table2 FindJobCostAssignDetailById(List<GetClientJobCostingInfoByClientIDDto.table2> assignDetailList, int assignmentId)
        {
            var jobCostAssignDetail = new GetClientJobCostingInfoByClientIDDto.table2();

            jobCostAssignDetail = assignDetailList.FirstOrDefault(x => x.ID == assignmentId);

            return jobCostAssignDetail;
        }



        private string DetermineExceptionMessageStyling(
            bool employeeActivityIsGreaterThanOne, 
            bool approvalOptionIsAllOrEveryDay, 
            bool isEmployeeActivityTwo,
            bool hideApprovalCheckboxIsTrue,
            bool RowIsApproved,
            bool costCenterIsApproved)
        {
            var result = "";
            if ((!hideApprovalCheckboxIsTrue && RowIsApproved) || costCenterIsApproved)
            {
                result = "approved";
            }
            if (employeeActivityIsGreaterThanOne && approvalOptionIsAllOrEveryDay)
            {
                if (isEmployeeActivityTwo)
                {
                    result = "exception";
                }
                else
                {
                   result = "";
                }
            }

            return result;
        }

        private bool IsDBNull(object value)
        {
            return DBNull.Value.Equals(value);
        }

        private string CheckForMissingPunch(string punchText, string oldPass, DataRow row, string punchColumnName)
        {
            if ("Missing".CompareTo(punchText) == 0)
            {
                row[punchColumnName + "Class"] = "exception";
                return "&AddDate=" + (DBNull.Value.Equals(row["EventDate"]) ? "" : row["EventDate"]) as string;
            }
            return oldPass;
        }
        

        /// <summary>
        /// TODO: Use this for enhancement case, but EmployeePay doesn't seem to be populating... 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="payType"></param>
        /// <param name="employeeStatusType"></param>
        /// <param name="departmentId"></param>
        /// <param name="clientShiftId"></param>
        /// <returns></returns>
        IOpResult<List<EmployeeDto>> ILaborManagementService.GetFilteredEmployees(int clientId, PayType? payType,
            EmployeeStatusType? employeeStatusType, int? departmentId, int? clientShiftId)
        {
            var result = new OpResult<List<EmployeeDto>>();

            _session.CanPerformAction(LaborManagementActionType.ReadTimePolicy).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(result);

            if (result.HasError) return result;

            // Table Structure
            // client department -> Employee
            // pay type (type) -> EmployeePay
            // employee status type (employeepaystatus) -> EmployeePay
            // shift (clientshiftid) -> EmployeePay

            var employees = _session.UnitOfWork.EmployeeRepository
                .QueryEmployees()
                .ByClientId(clientId)
                .ByActiveStatus()
                .ExecuteQueryAs(x => new EmployeeDto
                {
                    EmployeeId = x.EmployeeId,
                    ClientId = x.ClientId,
                    ClientDepartmentId = x.ClientDepartmentId,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                })
                .ToList();

            if (departmentId != null) employees.RemoveAll(e => e.ClientDepartmentId != departmentId);
            if (payType != null) employees.RemoveAll(e => e.EmployeePay?.FirstOrDefault(ep => ep.EmployeeId == e.EmployeeId
                && ep.ClientId == e.ClientId)?.Type != payType);
            if (employeeStatusType != null) employees.RemoveAll(e => e.EmployeePay?.FirstOrDefault(ep => ep.EmployeeId == e.EmployeeId
                 && ep.ClientId == e.ClientId)?.EmployeeStatusId != employeeStatusType);
            if (clientShiftId != null) employees.RemoveAll(e => e.EmployeePay?.FirstOrDefault(ep => ep.EmployeeId == e.EmployeeId
                 && ep.ClientId == e.ClientId)?.ClientShiftId != clientShiftId);

            result.TrySetData(() => employees)
                .CheckForData(() => new GenericMsg("No Employees Exist"));

            return result;
        }

        public class JobCostTooltipInfo
        {
            public int JobCostId { get; set; }
            public string JobCostLabel { get; set; }
            public int JobCostLevel { get; set; }
            public ClientJobCostingType JobCostType { get; set; }
            public int JobCostAssignId { get; set; }
            public string JobCostAssignDesc { get; set; }

            public JobCostTooltipInfo(int jobCostId, string jobCostLabel, int jobCostLevel, ClientJobCostingType jobCostType)
            {
                {
                    var withBlock = this;
                    withBlock.JobCostId = jobCostId;
                    withBlock.JobCostLabel = jobCostLabel;
                    withBlock.JobCostLevel = jobCostLevel;
                    withBlock.JobCostType = jobCostType;
                    withBlock.JobCostAssignId = 0;
                    withBlock.JobCostAssignDesc = "";
                }
            }

            public static JobCostTooltipInfo FindJobCosTooltipInfotById(List<JobCostTooltipInfo> list, int jobCostId)
            {
                JobCostTooltipInfo jobCostTooltipInfo;

                jobCostTooltipInfo = list.FirstOrDefault(x => x.JobCostId == jobCostId);

                return jobCostTooltipInfo;
            }
        }

        private enum PunchType
        {
            InPunch = 1,
            OutPunch = 2,
            In2Punch = 3,
            Out2Punch = 4
        }

        private enum LunchBreakType
        {
            StartOf = 1,
            EndOf = 2
        }
        IOpResult<IEnumerable<GetReportClockEmployeeOnSiteDto>> ILaborManagementService.GetReportClockEmployeeOnSite(int clientId, IEnumerable<int> employeeIds)
        {
            var result = new OpResult<IEnumerable<GetReportClockEmployeeOnSiteDto>>();
            if (employeeIds.Count() > 0)
            {
                var dvwExceptionHistorySorted = _session.UnitOfWork.EmployeeRepository.GetReportClockEmployeeOnSite(new GetReportClockEmployeeOnSiteArgsDto()
                {
                    ClientID = clientId,
                    UserID = 0,
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(1)
                });

                result.Data = dvwExceptionHistorySorted;
            }
            else
            {
                result.Data = null;
            }
            var opResult = _session.UnitOfWork.EmployeeRepository.QueryEmployees()
                .ByClientId(clientId)
                .ByEmployeeIds(employeeIds)
                .ExecuteQueryAs(x => new EmployeeFullDto
                {
                    EmployeeId = x.EmployeeId,
                    LastName = x.DirectSupervisor.LastName,
                    FirstName = x.DirectSupervisor.FirstName
                }).ToList();

            foreach (var item in result.Data)
                foreach (var ee in opResult)
                {
                    {
                        if (item.EmployeeID == ee.EmployeeId)
                        {
                            item.Filter = ee.LastName + ", " + ee.FirstName;
                        }
                    }
                }
            return result;
        }

        #endregion

    }
}
