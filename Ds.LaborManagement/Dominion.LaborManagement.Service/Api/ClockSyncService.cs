using Dominion.Core.Dto.Client;
using Dominion.Core.Dto.Employee;
using Dominion.Core.Dto.Labor;
using Dominion.Core.Services.Interfaces;
using Dominion.Core.Services.Security.Authorization;
using Dominion.LaborManagement.Dto.Clock;
using Dominion.LaborManagement.Dto.Department;
using Dominion.LaborManagement.Dto.JobCosting;
using Dominion.LaborManagement.Dto.Scheduling;
using Dominion.LaborManagement.Service.Internal.Providers;
using Dominion.LaborManagement.Service.Internal.Security;
using Dominion.LaborManagement.Service.Mapping;
using Dominion.LaborManagement.Service.Mapping.Clock;
using Dominion.Utility.Msg.Specific;
using Dominion.Utility.OpResult;
using System;
using System.Collections.Generic;
using System.Linq;
using Dominion.Utility.Query;
using Dominion.Pay.Services.Internal.Mapping;
using Dominion.Pay.Dto.Earnings;
using Dominion.Core.Services.Dto.Employee;
using Dominion.Utility.Security;

namespace Dominion.LaborManagement.Service.Api
{
    public class ClockSyncService : IClockSyncService
    {

        private readonly IBusinessApiSession _session;
        private readonly IEmployeePunchProvider _punchProvider;
        private readonly IEmployeeManager _employeeManager;
        private readonly ISchedulingProvider _schedulingProvider;
        private readonly IServiceAuthorizer _authorizer;
        private readonly IJobCostingProvider _jobCostingProvider;
        private readonly IClockSyncProvider _clockSyncProvider;

        public ClockSyncService(IBusinessApiSession session, IEmployeePunchProvider punchProvider,
            ISchedulingProvider schedulingProvider, IEmployeeManager employeeManager,
            IJobCostingProvider jobCostingProvider, IClockSyncProvider clockSyncProvider)
        {
            _session = session;
            _punchProvider = punchProvider;
            _schedulingProvider = schedulingProvider;
            _employeeManager = employeeManager;
            _jobCostingProvider = jobCostingProvider;
            _authorizer = new ServiceAuthorizer(session);
            _clockSyncProvider = clockSyncProvider;
        }


        public IOpResult<IEnumerable<TimeClockClientDto>> GetAccessibleClients()
        {
            return _punchProvider.GetAccessibleClients(
                _session.LoggedInUserInformation.AccessibleClientIds.ToArray());
        }

        IOpResult<IEnumerable<ClientDepartmentDto>> IClockSyncService.GetClientDepartments(int? clientId)
        {
            return _authorizer.Handle(a =>
            {
                return a.CheckClientId(clientId ?? _session.LoggedInUserInformation.ClientId ?? 0)
                    .CheckActionType(ClockActionType.User)
                    .Then(d =>
                    {
                        var result = new OpResult<IEnumerable<ClientDepartmentDto>>();

                        result.PerformOnMulitpleClients(
                            clientIds: _session.LoggedInUserInformation.AccessibleClientIds,
                            queryDelegate: id =>
                            {
                                var op = new OpResult<IEnumerable<ClientDepartmentDto>>();

                                op.TrySetData(() => _session.UnitOfWork.LaborManagementRepository
                                    .ClientDepartmentQuery()
                                    .ByClientId(id)
                                    .ByIsActive(true)
                                    .ExecuteQueryAs(cd => new ClientDepartmentDto()
                                        {
                                            ClientId = cd.ClientId,
                                            ClientDepartmentId = cd.ClientDepartmentId,
                                            Code = cd.Code,
                                            IsActive = cd.IsActive,
                                            Name = cd.Name
                                        })
                                    .ToList());

                                return op;
                            });
                        return result;
                    });
            });
        }

        /// <inheritdoc />
        IOpResult<IEnumerable<ClientDivisionDto>> IClockSyncService.GetClientDivisions(int? clientId)
        {
            return _authorizer.Handle(a =>
            {
                return a.CheckClientId(clientId ?? _session.LoggedInUserInformation.ClientId ?? 0)
                    .Then(d =>
                    {
                        var result = new OpResult<IEnumerable<ClientDivisionDto>>();

                        result.PerformOnMulitpleClients(
                            clientIds: _session.LoggedInUserInformation.AccessibleClientIds,
                            queryDelegate: id =>
                            {
                                var op = new OpResult<IEnumerable<ClientDivisionDto>>();

                                op.TrySetData(() => _session.UnitOfWork.ClientRepository
                                    .ClientDivisionQuery()
                                    .ByClientId(id)
                                    .ByIsActive(true)
                                    .OrderByName()
                                    .ExecuteQueryAs(cd =>  new ClientDivisionDto()
                                    {
                                        ClientId = cd.ClientId,
                                        ClientDivisionId = cd.ClientDivisionId,
                                        IsActive = cd.IsActive,
                                        Name = cd.Name
                                    }).ToList());

                                return op;
                            });
                        return result;
                    });
            });
        }

        IOpResult<IEnumerable<ClientEarningCompleteDto>> IClockSyncService.GetEmployeeClientEarnings(int clientId)
        {
            var result = new OpResult<IEnumerable<ClientEarningCompleteDto>>();

            _session.CanPerformAction(ClockActionType.User).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessByAccessibleClientIds(clientId).MergeInto(result);

            if (result.HasError) return result;

            var query = _session.UnitOfWork.PayrollRepository
                .QueryClientEarnings()
                .ByClientId(clientId)
                .ByIsBlockedFromTimeClock(false);

            return result.TrySetData(() => new ClientEarningCompleteMapper().Map(query.ExecuteQuery()));
        }

        IOpResult<IEnumerable<ClockClientHolidayDto>> IClockSyncService.GetClientHolidays()
        {
            var result = new OpResult<IEnumerable<ClockClientHolidayDto>>();
            return _authorizer.Handle(a =>
            {
                return a.CheckActionType(ClockActionType.ReadUser)
                    .Then(b => result.PerformOnMulitpleClients(
                        clientIds: _session.LoggedInUserInformation.AccessibleClientIds,
                        queryDelegate: clientId => _punchProvider.GetClientHolidays(clientId)
                    ));
            });
        }

        IOpResult<IEnumerable<ClientJobCostingDto>> IClockSyncService.GetClientJobCostings(int? clientId)
        {
            return _authorizer.Handle(a =>
            {
                return a.CheckClientId(clientId ?? _session.LoggedInUserInformation.ClientId ?? 0)
                    .CheckActionType(ClockActionType.ReadUser)
                    .Then(d =>
                    {
                        var result = new OpResult<IEnumerable<ClientJobCostingDto>>();

                        result.PerformOnMulitpleClients(
                            clientIds: _session.LoggedInUserInformation.AccessibleClientIds,
                            queryDelegate: id => _jobCostingProvider.GetClientJobCostingList(id));

                        return result;
                    }
                    );
            });
        }

        IOpResult<IEnumerable<ClientJobCostingAssignmentDto>> IClockSyncService.GetClientJobCostingAssignments(int? clientId)
        {
            return _authorizer.Handle(a =>
            {
                return a.CheckActionType(ClockActionType.ReadUser)
                    .CheckClientIds(clientId ?? _session.LoggedInUserInformation.ClientId ?? 0)
                    .Then(d =>
                    {
                        var result = new OpResult<IEnumerable<ClientJobCostingAssignmentDto>>();

                        result.PerformOnMulitpleClients(
                            clientIds: _session.LoggedInUserInformation.AccessibleClientIds,
                            queryDelegate: id =>
                            {
                                var op = new OpResult<IEnumerable<ClientJobCostingAssignmentDto>>();

                                op.TrySetData(() =>
                                {
                                    return _session.UnitOfWork.JobCostingRepository
                                        .GetJobCostingAssignmentQuery()
                                        .ByClientId(id)
                                        .ByIsEnabled(true)
                                        .ExecuteQueryAs(j => new ClientJobCostingAssignmentDto()
                                        {
                                            ClientId = j.ClientId,
                                            ClientJobCostingId = j.ClientJobCostingId,
                                            ClientJobCostingAssignmentId = j.ClientJobCostingAssignmentId,
                                            Description = j.Description,
                                            IsEnabled = j.IsEnabled,
                                            Code = j.Code,
                                            ForeignKeyId = j.ForeignKeyId
                                        }).ToList();
                                });
                                return op;
                            });
                        return result;
                    });
            });
        }

        IOpResult<IEnumerable<ClientJobCostingAssignmentSelectedDto>> IClockSyncService.GetClientJobCostingAssignmentSelected(int? clientId)
        {
            return _authorizer.Handle(a =>
            {
                return a.CheckActionType(ClockActionType.ReadUser)
                    .Then(d =>
                    {
                        var result = new OpResult<IEnumerable<ClientJobCostingAssignmentSelectedDto>>();

                        result.PerformOnMulitpleClients(
                            clientIds: _session.LoggedInUserInformation.AccessibleClientIds,
                            queryDelegate: id =>
                            {
                                var op = new OpResult<IEnumerable<ClientJobCostingAssignmentSelectedDto>>();

                                op.TrySetData(() => _session.UnitOfWork.JobCostingRepository
                                    .GetJobClientJobCostingAssignmentSelectedQuery()
                                    .ByClientId(id)
                                    .ByIsEnabled()
                                    .ExecuteQueryAs(s => new ClientJobCostingAssignmentSelectedDto()
                                    {
                                        ClientId = s.ClientId,
                                        ClientJobCostingAssignmentId = s.ClientJobCostingAssignmentId,
                                        ClientJobCostingAssignmentId_Selected = s.ClientJobCostingAssignmentId_Selected,
                                        ClientJobCostingAssignmentSelectedId = s.ClientJobCostingAssignmentSelectedId,
                                        ClientJobCostingId_Selected = s.ClientJobCostingId_Selected,
                                        ForeignKeyId_Selected = s.ForeignKeyId_Selected,
                                        IsEnabled = s.IsEnabled
                                    }
                                    ).ToList());
                                return op;
                            });
                        return result;
                    });
            });
        }

        IOpResult<IEnumerable<ClockClientLunchDto>> IClockSyncService.GetClientLunches()
        {
            var result = new OpResult<IEnumerable<ClockClientLunchDto>>();
            return _authorizer.Handle(a =>
            {
                return a.CheckActionType(ClockActionType.ReadUser)
                    .Then(b => result.PerformOnMulitpleClients(
                        clientIds: _session.LoggedInUserInformation.AccessibleClientIds,
                        queryDelegate: clientId => _punchProvider.GetClientLunches(clientId)
                    ));
            });
        }

        IOpResult<IEnumerable<ClockClientLunchSelectedDto>> IClockSyncService.GetClockClientLunchesSelected()
        {
            var result = new OpResult<IEnumerable<ClockClientLunchSelectedDto>>();
            return _authorizer.Handle(a =>
            {
                return a.CheckActionType(ClockActionType.ReadUser)
                    .Then(b => result.PerformOnMulitpleClients(
                        clientIds: _session.LoggedInUserInformation.AccessibleClientIds,
                        queryDelegate: clientId => _punchProvider.GetClientLunchesSelected(clientId)
                        ));
            });
        }

        IOpResult<IEnumerable<ClockClientRulesDto>> IClockSyncService.GetClientRules()
        {
            var result = new OpResult<IEnumerable<ClockClientRulesDto>>();
            return _authorizer.Handle(a =>
            {
                return a.CheckActionType(ClockActionType.ReadUser)
                    .Then(b => result.PerformOnMulitpleClients(
                        clientIds: _session.LoggedInUserInformation.AccessibleClientIds,
                        queryDelegate: clientId => _punchProvider.GetClientRules(clientId)
                    ));
            });
        }

        IOpResult<IEnumerable<ClockClientScheduleSelectedDto>> IClockSyncService.GetClockClientScheduleSelected(int? employeeId)
        {
            var result = new OpResult<IEnumerable<ClockClientScheduleSelectedDto>>();
            return _authorizer.Handle(a =>
            {
                return a.CheckActionType(ClockActionType.ReadUser)
                    .Then(b => result.PerformOnMulitpleClients(
                        clientIds: _session.LoggedInUserInformation.AccessibleClientIds,
                        queryDelegate: clientId =>
                        {
                            var opr = new OpResult<IEnumerable<ClockClientScheduleSelectedDto>>();
                            opr.TrySetData(() =>
                            {
                                var query = _session.UnitOfWork.EmployeeRepository.QueryEmployees()
                                    .ByClientId(clientId)
                                    .HasSchedules();
                                if (employeeId != null)
                                    query.ByEmployeeId(employeeId.Value);

                                return query.ExecuteQueryAs(employee => new ClockClientScheduleSelectedDto()
                                {
                                    ClientId = employee.ClientId,
                                    EmployeeId = employee.EmployeeId,
                                    ClockClientScheduleId = employee.SelectedSchedules.FirstOrDefault().ClockClientScheduleId
                                });
                            });
                            return opr;
                        }));
            });
        }

        IOpResult<IEnumerable<ClockClientScheduleDto>> IClockSyncService.GetClockClientSchedules()
        {
            var result = new OpResult<IEnumerable<ClockClientScheduleDto>>();
            return _authorizer.Handle(a =>
            {
                return a.CheckActionType(ClockActionType.ReadUser)
                    .Then(b => result.PerformOnMulitpleClients(
                       clientIds: _session.LoggedInUserInformation.AccessibleClientIds,
                        queryDelegate: clientId => _schedulingProvider.GetClockClientSchedules(clientId)
                    ));
            });
        }

        IOpResult<IEnumerable<ClockClientTimePolicyDtos.ClockClientTimePolicyDto>> IClockSyncService.GetClockClientTimePolicies()
        {
            var result = new OpResult<IEnumerable<ClockClientTimePolicyDtos.ClockClientTimePolicyDto>>();
            return _authorizer.Handle(a =>
            {
                return a.CheckActionType(ClockActionType.ReadUser)
                    .Then(b => result.PerformOnMulitpleClients(
                        clientIds: _session.LoggedInUserInformation.AccessibleClientIds,
                        queryDelegate: clientId => _punchProvider.GetClientTimePolicies(clientId)));
            });
        }

        IOpResult<IEnumerable<ClockEmployeeDto>> IClockSyncService.GetClockEmployees()
        {
            var result = new OpResult<IEnumerable<ClockEmployeeDto>>();
            return _authorizer.Handle(a =>
            {
                return a.CheckActionType(ClockActionType.ReadUser)
                    .Then(b => result.PerformOnMulitpleClients(
                        clientIds: _session.LoggedInUserInformation.AccessibleClientIds,
                        queryDelegate: clientId => _punchProvider.GetClockEmployees(clientId)
                    ));
            });
        }

        IOpResult<IEnumerable<ClockTimePolicyEmployeeDto>> IClockSyncService.GetEmployeesByTimePolicy(IEnumerable<int> timePolicyIds)
        {
            var result = new OpResult<IEnumerable<ClockTimePolicyEmployeeDto>>();
            var user = _session.LoggedInUserInformation;

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, user.ClientId.GetValueOrDefault()).MergeInto(result);
            _session.CanPerformAction(ClockActionType.ReadUser).MergeInto(result);

            if (result.HasError) return result.SetToFail("You do not have access to this client");

            return _authorizer.Handle(a =>
            {
                return a.CheckActionType(ClockActionType.ReadUser)
                    .Then(b => _punchProvider.GetEmployeesByTimePolicy(timePolicyIds));
            });
        }

        IOpResult<IEnumerable<ClockEmployeePunchDto>> IClockSyncService.GetClockEmployeePunches(int count)
        {
            var result = new OpResult<IEnumerable<ClockEmployeePunchDto>>();
            return _authorizer.Handle(a =>
            {
                return a.CheckActionType(ClockActionType.ReadUser)
                    .Then(b => result.PerformOnMulitpleClients(
                        clientIds: _session.LoggedInUserInformation.AccessibleClientIds,
                        queryDelegate: clientId =>
                        {
                            var opResult = new OpResult<IEnumerable<ClockEmployeePunchDto>>();

                            var client = _session.UnitOfWork.ClientRepository.GetClient(clientId);
                            if (client == null)
                            {
                                opResult.AddMessage(new GenericMsg("Invalid Client ID"));
                            }
                            else
                            {
                                opResult.TrySetData(() =>
                                {
                                    
                                    var dtoList = _session.UnitOfWork.TimeClockRepository.GetClockEmployeePunchQuery()
                                        .ByClientId(clientId)
                                        .ByDates(DateTime.Today.AddDays(-30), DateTime.Today.AddDays(1))
                                        .OrderByRawPunch(SortDirection.Descending)
                                        .Result
                                        .Take(count)
                                        .MapTo(new ClockEmployeePunchMaps.ClockEmployeePunchMap())
                                        .Execute();
                                                                        
                                                                        
                                    return dtoList;
                                });
                            }

                            return opResult;
                        }));
            });
        }

        IOpResult<IEnumerable<ClockEmployeeScheduleDto>> IClockSyncService.GetClockEmployeeSchedules(DateTime startDate, DateTime endDate)
        {
            var result = new OpResult<IEnumerable<ClockEmployeeScheduleDto>>();
            return _authorizer.Handle(a =>
            {
                return a.CheckActionType(ClockActionType.ReadUser)
                    .Then(b => result.PerformOnMulitpleClients(
                        clientIds: _session.LoggedInUserInformation.AccessibleClientIds,
                        queryDelegate: clientId => _schedulingProvider.GetClockEmployeeSchedules(clientId, startDate, endDate)
                    ));
            });
        }

        IOpResult<IEnumerable<ClientCostCenterDto>> IClockSyncService.GetAvailableCostCenters(int? clientId)
        {
            var result = new OpResult<IEnumerable<ClientCostCenterDto>>();

            _session.CanPerformAction(ClockActionType.User).MergeInto(result);
            
            if(clientId.HasValue)
                _session.ResourceAccessChecks.CheckAccessByAccessibleClientIds(clientId.Value).MergeInto(result);

            if(result.Success)
            {
                result.PerformOnMulitpleClients(
                    clientIds: clientId.HasValue ? new [] {clientId.Value} : _session.LoggedInUserInformation.AccessibleClientIds,
                    queryDelegate: cId =>
                    {
                        var opr = new OpResult<IEnumerable<ClientCostCenterDto>>();
                        opr.TrySetData(() => _session.UnitOfWork.LaborManagementRepository.ClientCostCenterQuery()
                            .ByClientId(cId)
                            .ByIsActive(true)
                            .ExecuteQueryAs(new ClientCostCenterMaps.DefaultClientCostCenterMap())
                            .OrderBy(x => x.Description).ThenBy(x => x.Code));
                        return opr;
                    }
                );
            }

            return result;
        }

        IOpResult<IEnumerable<ClockEmployeeScheduleListDto>> IClockSyncService.GetClockEmployeeScheduleListByDate(DateTime startDate,
            DateTime endDate,
            int? employeeId)
        {
            var result = new OpResult<IEnumerable<ClockEmployeeScheduleListDto>>();
            return _authorizer.Handle(a =>
            {
                return a.CheckActionType(ClockActionType.ReadUser)
                    .Then(b => result.PerformOnMulitpleClients(
                        clientIds: _session.LoggedInUserInformation.AccessibleClientIds,
                        queryDelegate: clientId =>
                        {
                            var opr = new OpResult<IEnumerable<ClockEmployeeScheduleListDto>>();
                            opr.TrySetData(() =>
                            {
                                var roundedStartDate = startDate.Date;
                                var roundedEndDate = endDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(999);

                                var query = _session.UnitOfWork.LaborManagementRepository.ClockEmployeeScheduleQuery()
                                        .ByEventDateRange(roundedStartDate, roundedEndDate)
                                        .ByClientId(clientId);

                                if (employeeId != null)
                                    query.ByEmployeeIds(new int[] { employeeId.Value });

                                if (employeeId != null)
                                    query.ByEmployeeIds(new int[] { employeeId.Value });

                                var results = query.ExecuteQueryAs(new ClockEmployeeScheduleMaps.ToClockEmployeeScheduleListDto());
                                return results.OrderBy(dto => dto.EmployeeId).ThenBy(dto => dto.EventDate);

                            });
                            return opr;
                        }));
            });
        }

        IOpResult<IEnumerable<ClockEmployeeBenefitListDto>> IClockSyncService.GetClockEmployeeBenefitListByDate(DateTime startDate,
            DateTime endDate,
            int? employeeId,
            int isWorked)
        {
            var result = new OpResult<IEnumerable<ClockEmployeeBenefitListDto>>();
            return _authorizer.Handle(a =>
            {
                return a.CheckActionType(ClockActionType.ReadUser)
                    .Then(b => result.PerformOnMulitpleClients(
                        clientIds: _session.LoggedInUserInformation.AccessibleClientIds,
                        queryDelegate: clientId =>
                        {
                            var opResult = new OpResult<IEnumerable<ClockEmployeeBenefitListDto>>();
                            var roundedStartDate = startDate.Date;
                            var roundedEndDate = endDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                            var query = _session.UnitOfWork.LaborManagementRepository.ClockEmployeeBenefitQuery()
                                    .ByClientId(clientId)
                                    .ByDateRange(roundedStartDate, roundedEndDate);

                            if (employeeId != null)
                                query.ByEmployeeId(employeeId.Value);

                            var results = query.ExecuteQueryAs(new ClockEmployeeBenefitMaps.ToClockEmployeeBenefitListDto());
                            opResult.Data = results.OrderBy(x => x.EventDate).ToList();//ref Sproc Line 132 : Order By EventDate Asc

                            return opResult;
                        }));
            });
        }

        IOpResult<IEnumerable<EmployeeBasicDto>> IClockSyncService.GetEmployees()
        {
            var result = new OpResult<IEnumerable<EmployeeBasicDto>>();

            return _authorizer.Handle(a =>
            {
                return a.CheckActionType(ClockActionType.ReadUser)
                    .Then(b => result.PerformOnMulitpleClients(
                        _session.LoggedInUserInformation.AccessibleClientIds,
                        clientId => _employeeManager.GetEmployees(clientId)));
            });
        }

        public IOpResult<IEnumerable<EmployeeDto>> UpdateClockEmployeesGeofence(IEnumerable<EmployeeDto> employees)
        {
            var result = new OpResult<IEnumerable<EmployeeDto>>();
            var user = _session.LoggedInUserInformation;

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, user.ClientId.GetValueOrDefault()).MergeInto(result);
            _session.CanPerformAction(ClockActionType.CanEditClockEmployee).MergeInto(result);

            if (result.HasError) return result.SetToFail("You do not have access to this client");

            _clockSyncProvider.UpdateClockEmployeesGeofence(employees).MergeInto(result);

            return result;
        }


    }
}