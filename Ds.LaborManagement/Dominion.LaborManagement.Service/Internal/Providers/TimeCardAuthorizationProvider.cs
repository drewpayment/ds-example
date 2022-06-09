using System;
using System.Collections.Generic;
using System.Linq;
using Dominion.Core.Dto.Labor;
using Dominion.Core.Dto.Labor.Enum;
using Dominion.Core.Dto.Sprocs;
using Dominion.Core.Services.Api.DataServicesInjectors;
using Dominion.Core.Services.Interfaces;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Interfaces.Repositories;
using Dominion.LaborManagement.Dto.Sproc;
using Dominion.Utility.Constants;
using Dominion.Utility.Containers;
using Dominion.Utility.Msg;
using Dominion.Utility.Msg.Specific;
using Dominion.Utility.OpResult;
using Renci.SshNet.Messages;

namespace Dominion.LaborManagement.Service.Internal.Providers
{
    public class TimeCardAuthorizationProvider : ITimeCardAuthorizationProvider
    {
        private readonly IBusinessApiSession _session;
        private readonly ITimeCardAuthorizationProvider _self;
        private readonly IDsDataServicesClockCalculateActivityService _clockCalculateActivityService;
        private readonly IClientService _clientService;

        public TimeCardAuthorizationProvider(
            IBusinessApiSession session, 
            IDsDataServicesClockCalculateActivityService clockCalculateActivityService,
            IClientService clientService
        ) {
            _session = session;
            _self = this;
            _clockCalculateActivityService = clockCalculateActivityService;
            _clientService = clientService;
        }

        IOpResult<GetTimeCardAuthorizationDataResult> ITimeCardAuthorizationProvider.GetData(TimeCardAuthorizationDataArgs args)
        {
            var result = new OpResult<GetTimeCardAuthorizationDataResult>();
            result.TrySetData(() =>
            {
                return new GetTimeCardAuthorizationDataResult()
                {
                    //TODO: We should move the sproc calls to the LaborManagementRepository
                    ClientJobCostingInfoResults =
                        _session.UnitOfWork.LaborManagementRepository.GetClientJobCostingInfoByClientID(
                            new GetClientJobCostingInfoByClientIDArgsDto()
                            {
                                ClientID = args.clientId
                            }),

                    ClockEmployeeApproveHoursOptions =
                        _session.UnitOfWork.LaborManagementRepository.GetClockEmployeeApproveHoursOptions(
                            new GetClockEmployeeApproveHoursOptionsArgsDto()
                            {
                                ControlID = args.controlId
                            }),

                    ClockEmployeeApproveHoursSettings = _session.UnitOfWork.EmployeeRepository
                        .ClockEmployeeApproveHoursSettingsQuery()
                        .ByClientID(args.clientId)
                        .ByUserID(args.userId)
                        .ExecuteQueryAs(x => new GetClockEmployeeApproveHoursSettingsDto
                        {
                            ClockEmployeeApproveHoursSettingsID = x.ClockEmployeeApproveHoursSettingsId,
                            ClientId = x.ClientId,
                            UserId = x.UserId,
                            DefaultDaysFilter = x.DefaultDaysFilter,
                            EmployeesPerPage = x.EmployeesPerPage,
                            HideActivity = x.HideActivity,
                            HideDailyTotals = x.HideDailyTotals,
                            HideGrandTotals = x.HideGrandTotals.Value,
                            HideNoActivity = x.HideNoActivity,
                            HideWeeklyTotals = x.HideWeeklyTotals.Value,
                            PayPeriod = x.PayPeriod, 
                            ShowAllDays = x.ShowAllDays
                        }),

                    ClockFilterCategory =
                        _session.UnitOfWork.LaborManagementRepository.GetClockFilterCategory(
                            new GetClockFilterCategoryArgsDto()
                            {
                                CategoryString = args.categoryString
                            }),

                    ClockPayrollList =
                        _session.UnitOfWork.LaborManagementRepository.GetClockPayrollListByClientIDPayrollRunID(
                            new GetClockPayrollListByClientIDPayrollRunIDArgsDto()
                            {
                                ClientID = args.clientId,
                                PayrollRunID = args.payrollRunId,
                                HideCustomDateRange = args.hideCustomDateRange

                            }),
                    ClientNotes = 
                        _session.UnitOfWork.TimeClockRepository.GetClockClientNoteList(new GetClockClientNoteListArgsDto()
                        {
                            ClientID = args.clientId
                        })
                };
            });
            return result;
        }

        IOpResult ITimeCardAuthorizationProvider.DeleteClockEmployeeApproveDate(int id, bool commit)
        {
            var result = new OpResult();

            if (id < 1) return result.SetToFail("Not a valid approval record.");

            var entity = new ClockEmployeeApproveDate
            {
                ClockEmployeeApproveDateId = id
            };

            _session.UnitOfWork.RegisterDeleted(entity);

            if (commit) _session.UnitOfWork.Commit().MergeInto(result);

            return result;
        }

        IOpResult<ClockEmployeeApproveDateDto> ITimeCardAuthorizationProvider.SaveTimeCardApprovalRow(ClockEmployeeApproveDateDto dto, bool isApproveByCostCenter, bool holdCommit)
        {
            var result = new OpResult<ClockEmployeeApproveDateDto>(dto);

            var qry = _session.UnitOfWork.LaborManagementRepository
                .ClockEmployeeApproveDateQuery()
                .ByEmployeeId(dto.EmployeeId)
                .ByEventDate(dto.EventDate);

            if (isApproveByCostCenter)
                qry.ByCostCenterId(dto.ClientCostCenterId.GetValueOrDefault());

            var approvalRows = qry
                .ExecuteQueryAs(x => new ClockEmployeeApproveDateDto
                {
                    ClockEmployeeApproveDateId = x.ClockEmployeeApproveDateId,
                    EmployeeId = x.EmployeeId,
                    ClientId = x.ClientId,
                    ClientCostCenterId = x.ClientCostCenterId,
                    ClientEarningId = x.ClientEarningId,
                    ClockClientNoteId = x.ClockClientNoteId,
                    EventDate = x.EventDate,
                    ApprovedDate = x.ApprovedDate,
                    IsApproved = x.IsApproved,
                    IsPayToSchedule = x.IsPayToSchedule,
                    PayrollId = x.PayrollId,
                    ApprovedBy = x.ApprovedBy
                })
                .ToList();

            // There are some conditions where data integrity has been jeopardized on ClockEmployeeApproveDate 
            // and the employee can have multiple records on the same day... if this happens we need to make sure 
            // that we clean up the data and delete extraneous approval records. This ONLY APPLIES when the client 
            // is NOT using ApprovalOptionType.Cost_Center in their TCA company options.
            if (!isApproveByCostCenter && approvalRows.Count > 1)
            {
                for (var i = 0; i < approvalRows.Count; i++)
                {
                    if (i == approvalRows.Count - 1)
                    {
                        _self.SaveClockEmployeeApproveDate(dto, approvalRows[i]);
                    }
                    else
                    {
                        _self.DeleteClockEmployeeApproveDate(approvalRows[i].ClockEmployeeApproveDateId).MergeInto(result);
                    }
                }
            }
            else
            {
                _self.SaveClockEmployeeApproveDate(dto, approvalRows.FirstOrDefault());
            }

            if (!holdCommit) _session.UnitOfWork.Commit().MergeInto(result);

            return result;
        }

        private ClockEmployeeApproveDateDto CheckApprovedByInfo(ClockEmployeeApproveDateDto dto, ClockEmployeeApproveDateDto curr)
        {
            // The approval being passed in has already been approved sometime in the past and 
            // we shouldn't be updating the approveby or approveddate fields
            if (curr != null && dto.IsApproved.GetValueOrDefault() && curr.IsApproved.GetValueOrDefault())
            {
                dto.ApprovedBy = curr.ApprovedBy;
                dto.ApprovedDate = curr.ApprovedDate;
            }
            else if (!dto.IsApproved.GetValueOrDefault()) // User is unapproving the record, so we need to remove the metadata
            {
                dto.ApprovedBy = null;
                dto.ApprovedDate = null;
            }

            return dto;
        }

        void ITimeCardAuthorizationProvider.SaveClockEmployeeApproveDate(ClockEmployeeApproveDateDto dto, ClockEmployeeApproveDateDto curr)
        {
            // Let's make sure our approvedby and approveddate fields have the proper values
            dto = CheckApprovedByInfo(dto, curr);

            var e = new ClockEmployeeApproveDate
            {
                ClockEmployeeApproveDateId = curr?.ClockEmployeeApproveDateId ?? CommonConstants.NEW_ENTITY_ID,
                EmployeeId = dto.EmployeeId,
                ClientId = dto.ClientId,
                ClientCostCenterId = dto.ClientCostCenterId == 0 ? null : dto.ClientCostCenterId,
                ClientEarningId = dto.ClientEarningId > 0 ? dto.ClientEarningId : null,
                ClockClientNoteId = dto.ClockClientNoteId > 0 ? dto.ClockClientNoteId : null,
                EventDate = dto.EventDate,
                ApprovedDate = dto.ApprovedDate,
                IsApproved = !dto.IsApproved.Value ? null : dto.IsApproved,
                IsPayToSchedule = dto.IsPayToSchedule,
                PayrollId = dto.PayrollId > 0 ? dto.PayrollId : null,
                ApprovedBy = dto.ApprovedBy
            };

            if (curr != null)
            {
                var props = new PropertyList<ClockEmployeeApproveDate>();

                if (curr.ClientEarningId != e.ClientEarningId)
                    props.Include(x => x.ClientEarningId);
                if (curr.ClockClientNoteId != e.ClockClientNoteId)
                    props.Include(x => x.ClockClientNoteId);
                if (curr.ClientCostCenterId != e.ClientCostCenterId)
                    props.Include(x => x.ClientCostCenterId);
                if (curr.ApprovedDate != e.ApprovedDate)
                    props.Include(x => x.ApprovedDate);
                if (curr.ApprovedBy != e.ApprovedBy)
                    props.Include(x => x.ApprovedBy);
                if (curr.IsApproved != e.IsApproved)
                    props.Include(x => x.IsApproved);
                if (curr.IsPayToSchedule != e.IsPayToSchedule)
                    props.Include(x => x.IsPayToSchedule);
                if (curr.PayrollId != e.PayrollId)
                    props.Include(x => x.PayrollId);

                props.Include(x => x.Modified);
                props.Include(x => x.ModifiedBy);

                if (props.Any()) _session.SetModifiedProperties(e);
                _session.UnitOfWork.RegisterModified(e, props);
            }
            else
            {
                _session.UnitOfWork.RegisterPostCommitAction(() => dto.ClockEmployeeApproveDateId = e.ClockEmployeeApproveDateId);
                _session.SetModifiedProperties(e);
                _session.UnitOfWork.RegisterNew(e);
            }
        }

        IOpResult<IEnumerable<ClockEmployeeApproveDateDto>> ITimeCardAuthorizationProvider.RecalculateWeeklyActivityByApproveDates(TimeCardAuthorizationSaveArgs args)
        {
            var result = new OpResult<IEnumerable<ClockEmployeeApproveDateDto>>();

            if (!args.isRecalcActivity) return result;

            var approveDates = _self.GetClockEmployeeApproveDate(new GetClockEmployeeApproveDateArgsDto
            {
                ClientID = args.ClientId,
                StartDate = args.StartDate,
                EndDate = args.EndDate,
                UserID = args.UserId,
                OnlyPayToSchedule = true
            }).MergeInto(result).Data;

            if (result.HasError) return result;

            var lastEmployeeId = 0;
            foreach(var dt in approveDates)
            {
                if (lastEmployeeId != dt.EmployeeID)
                {
                    var evtDate = dt.Eventdate;
                    var isDuplicate = _clockCalculateActivityService.CalculateWeeklyActivity(
                        ClientID: args.ClientId,
                        EmployeeID: dt.EmployeeID,
                        PunchDate: ref evtDate,
                        IncludeAutoLunch: false,
                        RoundPunch: false,
                        ClockEmployeePunchID: 0,
                        clientService: _clientService
                    );

                    if (isDuplicate) return result.SetToFail("Duplicate found.");
                }
                lastEmployeeId = dt.EmployeeID;
            }

            // I DON'T THINK THIS IS HAPPENING ANYMORE? 
            //if (args.EmployeeIds.Any())
            //{
            //    var eventDate = entries.Last().ClockEmployeeApproveDate.EventDate.Value;
            //    foreach (var empId in args.EmployeeIds)
            //    {
            //        if (empId > 0)
            //        {
            //            _clockCalculateActivityService.CalculateWeeklyActivity(
            //                ClientID: args.ClientId,
            //                EmployeeID: empId,
            //                PunchDate: ref eventDate,
            //                IncludeAutoLunch: false,
            //                RoundPunch: false,
            //                ClockEmployeePunchID: 0,
            //                clientService: _clientService
            //            );
            //        }
            //    }
            //}

            return result.TrySetData(() => approveDates.Select(a => new ClockEmployeeApproveDateDto
            {
                EmployeeId = a.EmployeeID,
                EventDate = a.Eventdate,
                IsApproved = a.IsApproved,
                ClientCostCenterId = a.ClientCostCenterID,
                ClientEarningId = a.ClientEarningID,
                IsPayToSchedule = a.PayToSchedule.Value,
                ClockClientNoteId = a.ClockClientNoteID,
                ApprovedBy = Convert.ToInt32(a.ApprovingUser)
            }));
        }

        IOpResult<int> ITimeCardAuthorizationProvider.SaveTimeCardAuthorizationData(InsertClockEmployeeApproveDateArgsDto args)
        {
            var opResult = new OpResult<int>();
            opResult.TrySetData(() => _session.UnitOfWork.LaborManagementRepository.InsertClockEmployeeApproveDate(args));
            return opResult;
        }

        IOpResult<IEnumerable<GetClockEmployeeApproveDateDto>> ITimeCardAuthorizationProvider.GetClockEmployeeApproveDate(GetClockEmployeeApproveDateArgsDto args)
        {
            var opResult = new OpResult<IEnumerable<GetClockEmployeeApproveDateDto>>();
            opResult.TrySetData(() => _session.UnitOfWork.LaborManagementRepository.GetClockEmployeeApproveDate(args));
            return opResult;
        }

        IOpResult<IEnumerable<GetClockFilterIdsDto>> ITimeCardAuthorizationProvider.GetClockFilterIDs(GetClockFilterIdsArgsDto args)
        {
            var opResult = new OpResult<IEnumerable<GetClockFilterIdsDto>>();
            opResult.TrySetData(() => _session.UnitOfWork.LaborManagementRepository.GetClockFilterIDs(args));
            return opResult;
        }

        public IOpResult<IEnumerable<GetClockFilterIdsDto>> FillFilterIDDropdown(int filterCategory, int noOfPayPeriodOptions, int payPeriod, string payPeriodText, int clientId)
        {

            var result = new OpResult<IEnumerable<GetClockFilterIdsDto>>();
            
                var startDate = "1/1/1900";
                var endDate = "1/1/1900";
                if(noOfPayPeriodOptions > 0)
                {
                if (payPeriod == 2)
                {
                    startDate = DateTime.Now.Date.ToShortDateString();
                    endDate = DateTime.Now.Date.ToShortDateString();
                } else if(payPeriod == 0)
                {
                    var currentPeriod = _session.UnitOfWork.LaborManagementRepository.GetTimeClockCurrentPeriod(new GetTimeClockCurrentPeriodArgsDto()
                    {
                        ClientID = clientId
                    });

                    startDate = currentPeriod.StartDate.ToShortDateString();
                    endDate = currentPeriod.EndDate.ToShortDateString();
                } else
                {
                    var dates = payPeriodText.Split('-');
                    startDate = dates[0];
                    startDate = dates[1];
                    }

                }

                return result.TrySetData(() => _session.UnitOfWork.LaborManagementRepository.GetClockEmployeePunchListByDateAndFilter(new GetClockEmployeePunchListByDateAndFilterArgsDto()
                {
                    intClientID = clientId,
                    intUserID = _session.LoggedInUserInformation.UserId,
                    intEmployeeID = 0,
                    strStartDate = startDate,
                    strEndDate = endDate,
                    intFilter = 0,
                    intFilterCategory = 0,
                    intSpecialOption = 4,
                    intFilter2 = 0,
                    intFilterCategory2 = 0
                }).Select(x => new GetClockFilterIdsDto()
                {
                    Filter = x.EmployeeName,
                    Id = x.EmployeeId
                }));
        }

        public IOpResult<IEnumerable<GetClockFilterIdsDto>> FillFilterIDDropdown(int filterCategory, int clientId)
        {

            var result = new OpResult<IEnumerable<GetClockFilterIdsDto>>();
            return result.TrySetData(() => _session.UnitOfWork.LaborManagementRepository.GetClockFilterIDs(new GetClockFilterIdsArgsDto()
            {
                ClientID = clientId,
                ClockFilterID = filterCategory
            }));
        }

        IOpResult<ClockEmployeeApproveHoursSettings> ITimeCardAuthorizationProvider.CreateNewDisplaySettings(ClockEmployeeApproveHoursSettings entity)
        {
            var result = new OpResult<ClockEmployeeApproveHoursSettings>(entity);
            _session.UnitOfWork.RegisterNew(entity);
            return result;
        }

        public IOpResult<GetClockEmployeeApproveHoursSettingsDto> SaveDisplaySettings(GetClockEmployeeApproveHoursSettingsDto dto)
        {
            var result = new OpResult<GetClockEmployeeApproveHoursSettingsDto>();

            dto.UserId = _session.LoggedInUserInformation.UserId;
            var curr = new GetClockEmployeeApproveHoursSettingsDto();

            var entity = new ClockEmployeeApproveHoursSettings
            {
                ClientId = dto.ClientId,
                DefaultDaysFilter = dto.DefaultDaysFilter,
                HideActivity = dto.HideActivity,
                HideDailyTotals = dto.HideDailyTotals,
                HideGrandTotals = dto.HideGrandTotals,
                HideNoActivity = dto.HideNoActivity,
                HideWeeklyTotals = dto.HideWeeklyTotals,
                EmployeesPerPage = dto.EmployeesPerPage,
                ShowAllDays = dto.ShowAllDays,
                ClockEmployeeApproveHoursSettingsId = dto.ClockEmployeeApproveHoursSettingsID,
                UserId = dto.UserId,
                PayPeriod = dto.PayPeriod
            };

            if (dto.ClockEmployeeApproveHoursSettingsID > 0)
            {
                curr = _session.UnitOfWork.EmployeeRepository
                    .ClockEmployeeApproveHoursSettingsQuery()
                    .ByClientID(dto.ClientId)
                    .ByUserID(dto.UserId)
                    .ExecuteQueryAs(x => new GetClockEmployeeApproveHoursSettingsDto()
                    {
                        EmployeesPerPage = x.EmployeesPerPage,
                        ShowAllDays = x.ShowAllDays,
                        HideWeeklyTotals = x.HideWeeklyTotals ?? false,
                        HideNoActivity = x.HideNoActivity,
                        HideDailyTotals = x.HideDailyTotals,
                        DefaultDaysFilter = x.DefaultDaysFilter,
                        HideGrandTotals = x.HideGrandTotals ?? false,
                        HideActivity = x.HideActivity,
                        ClientId = x.ClientId,
                        UserId = x.UserId,
                        ClockEmployeeApproveHoursSettingsID = x.ClockEmployeeApproveHoursSettingsId,
                        PayPeriod = x.PayPeriod
                    })
                    .FirstOrDefault();

                // Unable to find an existing settings object for this user
                // Create a new one instead and return the result
                if (curr == null)
                {
                    _self.CreateNewDisplaySettings(entity).MergeInto(result);
                }
                else
                {
                    var props = new PropertyList<ClockEmployeeApproveHoursSettings>();

                    if (entity.DefaultDaysFilter != curr.DefaultDaysFilter)
                        props.Add(x => x.DefaultDaysFilter);
                    if (entity.HideActivity != curr.HideActivity)
                        props.Add(x => x.HideActivity);
                    if (entity.HideDailyTotals != curr.HideDailyTotals)
                        props.Add(x => x.HideDailyTotals);
                    if (entity.HideWeeklyTotals != curr.HideWeeklyTotals)
                        props.Add(x => x.HideWeeklyTotals);
                    if (entity.HideGrandTotals != curr.HideGrandTotals)
                        props.Add(x => x.HideGrandTotals);
                    if (entity.HideNoActivity != curr.HideNoActivity)
                        props.Add(x => x.HideNoActivity);
                    if (entity.ShowAllDays != curr.ShowAllDays)
                        props.Add(x => x.ShowAllDays);
                    if (entity.PayPeriod != curr.PayPeriod)
                        props.Add(x => x.PayPeriod);

                    if (props.Any()) _session.UnitOfWork.RegisterModified(entity, props);
                }
            }
            else
            {
                _self.CreateNewDisplaySettings(entity).MergeInto(result);
            }

            _session.UnitOfWork.RegisterPostCommitAction(() => 
                dto.ClockEmployeeApproveHoursSettingsID = entity.ClockEmployeeApproveHoursSettingsId);
            _session.UnitOfWork.Commit().MergeInto(result);

            return result.SetDataOnSuccess(dto);
        }
    }
}
