using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Services.Api;
using Dominion.Core.Services.Interfaces;
using Dominion.Domain.Entities.Labor;
using Dominion.LaborManagement.Dto.GroupScheduling;
using Dominion.LaborManagement.Dto.Scheduling;
using Dominion.LaborManagement.Service.Internal.Providers;
using Dominion.LaborManagement.Service.Internal.Security;
using Dominion.LaborManagement.Service.Mapping;
using Dominion.Utility.OpResult;
using Dominion.Utility.Security;

namespace Dominion.LaborManagement.Service.Api
{
    public class GroupScheduleService : IGroupScheduleService
    {
        #region Variables and Properties
        
        private readonly IBusinessApiSession _session;
        private readonly IGroupScheduleProvider _groupScheduleProvider;
        private readonly IScheduleGroupProvider _scheduleGroupProvider;
        private readonly ISchedulingProvider _schedulingProvider;

        #endregion

        #region Constructors and Initializers

        public GroupScheduleService(
            IBusinessApiSession session, 
            IGroupScheduleProvider groupScheduleProvider,
            IScheduleGroupProvider scheduleGroupProvider,
            ISchedulingProvider schedulingProvider)
        {
            _session = session;
            _groupScheduleProvider = groupScheduleProvider;
            _scheduleGroupProvider = scheduleGroupProvider;
            _schedulingProvider = schedulingProvider;
        }

        #endregion

        #region IGroupScheduleService

        /// <summary>
        /// Get a list of group schedules for display in a list.
        /// Basic data.
        /// </summary>
        /// <param name="isActive">Null by default, otherwise results are filtered by isActive</param>
        /// <returns></returns>
        IOpResult<IEnumerable<GroupScheduleDtos.GroupScheduleDto>> IGroupScheduleService.GetClientGroupScheduleList(bool? isActive)
        {
            var opResult = new OpResult<IEnumerable<GroupScheduleDtos.GroupScheduleDto>>();

            opResult.CombineSuccessAndMessages(_session.CanPerformAction(LaborManagementActionType.LaborPlanAdministrator));
            
            var isLaborPlanSupervisor = _session.CanPerformAction(LaborManagementActionType.LaborPlanSupervisor).Success;

            opResult.TryCatchIfSuccessful(() =>
            {
                var query = _session.UnitOfWork
                    .LaborManagementRepository
                    .GroupScheduleQuery()
                    .ByClientId(_session.LoggedInUserInformation.ClientId.Value);

                if (isActive != null)
                {
                    query.ByIsActive(isActive.Value);
                }

                if (isLaborPlanSupervisor)
                {
                    var accessibleScheduleGroupIds = _scheduleGroupProvider.GetCurrentUserAccessibleScheduleGroupIds().Data.ToList();
                    query.ByScheduleGroupIds(accessibleScheduleGroupIds);
                }

                opResult.Data = query
                    .ExecuteQueryAs(new GroupScheduleMaps.ToGroupScheduleDto());
            });

            return opResult;
        }

        /// <summary>
        /// Get a schedule with full group scheduling data.
        /// </summary>
        /// <param name="groupScheduleId">The group schedule you want data for.</param>
        /// <returns></returns>
        IOpResult<GroupScheduleDtos.GroupScheduleWithScheduleGroupsDto> IGroupScheduleService.GetGroupSchedule(
            int groupScheduleId)
        {
            var opResult = new OpResult<GroupScheduleDtos.GroupScheduleWithScheduleGroupsDto>();
            opResult.CombineSuccessAndMessages(_session.CanPerformAction(LaborManagementActionType.LaborPlanAdministrator));

            var isLaborPlanSupervisor = _session.CanPerformAction(LaborManagementActionType.LaborPlanSupervisor).Success;

            opResult.TryCatchIfSuccessful(() =>
            {
                var results = _groupScheduleProvider.GetGroupScheduleGroupedData(groupScheduleId);

                //mark schedule groups that are not accessible by the user as read-only
                if (results.HasNoErrorAndHasData && isLaborPlanSupervisor)
                    _groupScheduleProvider.UpdateScheduleGroupAccess(results.Data, _scheduleGroupProvider.GetCurrentUserAccessibleScheduleGroupIds().Data);

                opResult.CombineAll(results);
            });

            return opResult;
        }

        /// <summary>
        /// Get schedule groups based on the client and schedule group type.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="scheduleGroupType">The schedule group type.</param>
        /// <returns></returns>
        IOpResult<IEnumerable<GroupScheduleDtos.ScheduleGroupDto>> IGroupScheduleService.GetScheduleGroups(
            ScheduleGroupType scheduleGroupType,
            int? scheduleGroupId,
            bool withScheduleGroupShiftNames)
        {
            var opResult = new OpResult<IEnumerable<GroupScheduleDtos.ScheduleGroupDto>>();

            var isLaborPlanAdmin      = _session.CanPerformAction(LaborManagementActionType.LaborPlanAdministrator);
            var isLaborScheduler      = _session.CanPerformAction(LaborManagementActionType.LaborScheduleAdministrator);
            var isLaborPlanSupervisor = _session.CanPerformAction(LaborManagementActionType.LaborPlanSupervisor);

            if(isLaborPlanAdmin.HasError && isLaborScheduler.HasError)
            {
                opResult.CombineSuccessAndMessages(isLaborScheduler);
            }
            else
            {
                opResult.TryCatchIfSuccessful(() =>
                {
                    // ReSharper disable once ConvertToLambdaExpression
                    opResult.CombineAll(_scheduleGroupProvider
                        .GetScheduleGroupsByGroupType(
                            useSupervisorSecurity:          isLaborPlanAdmin.HasError || isLaborPlanSupervisor.Success, 
                            clientId:                       _session.LoggedInUserInformation.ClientId.Value, 
                            scheduleGroupType:              ScheduleGroupType.ClientCostCenter,
                            scheduleGroupId:                scheduleGroupId,
                            withScheduleGroupShiftNames:    withScheduleGroupShiftNames));
                });

            }

            return opResult;
        }

        /// <summary>
        /// Saves or updates a group schedule.
        /// </summary>
        /// <param name="data">The group schedule data.</param>
        /// <returns>A result with the group schedule obj that was constructed from the DTO.</returns>
        IOpResult<GroupScheduleDtos.GroupScheduleWithScheduleGroupsDto> IGroupScheduleService.SaveOrUpdateGroupSchedule(
            GroupScheduleDtos.GroupScheduleWithScheduleGroupsDto data)
        {
            var opResult = new OpResult<GroupScheduleDtos.GroupScheduleWithScheduleGroupsDto>();
            opResult.CombineSuccessAndMessages(_session.CanPerformAction(LaborManagementActionType.LaborPlanAdministrator));
            
            var isLaborPlanSupervisor = _session.CanPerformAction(LaborManagementActionType.LaborPlanSupervisor).Success;

            //jay: fix100 (FIXED)
            opResult.TryCatchIfSuccessful(() =>
            {
                var results = _groupScheduleProvider.SaveOrUpdateGroupSchedule(data);
                opResult.CombineSuccessAndMessages(results);

                if (opResult.Success)
                {
                    opResult.Data = GroupScheduleMaps.MapTo(results.Data);

                    if (isLaborPlanSupervisor)
                        _groupScheduleProvider.UpdateScheduleGroupAccess(opResult.Data, _scheduleGroupProvider.GetCurrentUserAccessibleScheduleGroupIds().Data);
                }
            });

            return opResult;
        }

        /// <summary>
        /// Get group schedule data with shifts that only have a reference wo the schedule group.
        /// highFix: jay: the scheduleGroupId isn't being checked for security because only scheduleGroupIds the caller has access to should be coming in. If it doesn't then they're hacking the system. So we need to add a security check.
        /// </summary>
        /// <param name="scheduleGroupId">The scheduleGroupId to do filtering with.</param>
        /// <returns></returns>
        IOpResult<IEnumerable<GroupScheduleDtos.GroupScheduleForSchedulingDto>> IGroupScheduleService.GetGroupScheduleForScheduling(
            int scheduleGroupId)
        {
            var opResult = new OpResult<IEnumerable<GroupScheduleDtos.GroupScheduleForSchedulingDto>>();
            var isLaborPlanAdmin = _session.CanPerformAction(LaborManagementActionType.LaborPlanAdministrator);
            var isLaborScheduler = _session.CanPerformAction(LaborManagementActionType.LaborScheduleAdministrator);

            if(isLaborPlanAdmin.HasError && isLaborScheduler.HasError)
            {
                opResult.CombineSuccessAndMessages(isLaborScheduler);
            }
            else
            {
                opResult.TryCatchIfSuccessful(() =>
                {
                    var results = _groupScheduleProvider.GetGroupScheduleForScheduling(scheduleGroupId);
                    opResult.CombineAll(results);
                });                
            }

            return opResult;
        }

        /// <summary>
        /// Get the scheduled information based on the parameters passed in.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="scheduleGroupId">The schedule group id.</param>
        /// <param name="scheduleGroupSourceId">The source id of the source group (cost center id).</param>
        /// <returns></returns>
        IOpResult<IEnumerable<EmployeeSchedulesDto>> IGroupScheduleService.GetEmployeeScheduleShifts(
            DateTime startDate,
            DateTime endDate,
            int scheduleGroupId,
            int scheduleGroupSourceId)
        {
            var opResult = new OpResult<IEnumerable<EmployeeSchedulesDto>>();
            var isLaborPlanAdmin = _session.CanPerformAction(LaborManagementActionType.LaborPlanAdministrator);
            var isLaborScheduler = _session.CanPerformAction(LaborManagementActionType.LaborScheduleAdministrator);

            if(isLaborPlanAdmin.HasError && isLaborScheduler.HasError)
            {
                opResult.CombineSuccessAndMessages(isLaborScheduler);
            }
            else
            {
                opResult.TryCatchIfSuccessful(() =>
                {
                    var results = _schedulingProvider.GetEmployeeScheduleShifts(
                        _session.LoggedInUserInformation.ClientId.Value,
                        startDate,
                        endDate,
                        scheduleGroupId,
                        scheduleGroupSourceId);

                    opResult.CombineAll(results);
                });                
            }

            return opResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="scheduleGroupSourceId"></param>
        /// <returns></returns>
        IOpResult<IEnumerable<EmployeeSchedulesDto>> IGroupScheduleService.SaveOrUpdateEmployeeSchedule(EmployeeSchedulesPersistDto data)
        {
            var opResult = new OpResult<IEnumerable<EmployeeSchedulesDto>>();
            var isLaborPlanAdmin = _session.CanPerformAction(LaborManagementActionType.LaborPlanAdministrator);
            var isLaborScheduler = _session.CanPerformAction(LaborManagementActionType.LaborScheduleAdministrator);

            if(isLaborPlanAdmin.HasError && isLaborScheduler.HasError)
            {
                opResult.CombineSuccessAndMessages(isLaborScheduler);
            }
            else
            {
                opResult.TryCatchIfSuccessful(() =>
                {
                    var results = _schedulingProvider.SaveOrUpdateEmployeeScheduleShifts(data);

                    opResult.CombineSuccessAndMessages(results);

                    if(opResult.HasNoError)
                    {
                        opResult.CombineAll(
                            _schedulingProvider.GetEmployeeScheduleShifts(
                                _session.LoggedInUserInformation.ClientId.Value,
                                data.StartDate,
                                data.EndDate,
                                data.ScheduleGroupId,
                                data.ScheduleSourceId));
                    }
                });
            }

            return opResult;
        }

        /// <summary>
        /// Get schedules associated with a given cost-center, if any exist.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientCostCenterId"></param>
        /// <returns></returns>
        IOpResult<IEnumerable<IGroupScheduleDto>> IGroupScheduleService.GetGroupSchedulesAssociatedWithClientCostCenter(int clientId, int clientCostCenterId)
        {
            var opResult = new OpResult<IEnumerable<IGroupScheduleDto>>();

            // Do permission checks
            _session.CanPerformAction(ClientsActionType.ReadClientCostCenterSetup).MergeInto(opResult);
            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(opResult);

            opResult.CombineSuccessAndMessages(_session.CanPerformAction(LaborManagementActionType.LaborPlanAdministrator));
            var isLaborPlanSupervisor = _session.CanPerformAction(LaborManagementActionType.LaborPlanSupervisor).Success;

            if (opResult.HasError)
                return opResult;

            opResult.TrySetData(() =>
            {
                // Get groupSchedulesQuery
                var groupSchedulesQuery = _session.UnitOfWork.LaborManagementRepository
                    .GroupScheduleQuery()
                    .ByClientId(clientId)
                    .ByCostCenterId(clientCostCenterId);

                // Filter if necessary
                if (isLaborPlanSupervisor)
                {
                    var accessibleScheduleGroupIds = _scheduleGroupProvider.GetCurrentUserAccessibleScheduleGroupIds().Data; //.ToList();
                    groupSchedulesQuery.ByScheduleGroupIds(accessibleScheduleGroupIds);
                }

                // Get groupSchedules
                var groupSchedules = groupSchedulesQuery.ExecuteQuery();

                // Map to DTOs
                var groupScheduleDtos = groupSchedules.Select(x => new GroupScheduleDtos.GroupScheduleDto()
                {
                    GroupScheduleId = x.GroupScheduleId,
                    ClientId        = x.ClientId,
                    Name            = x.Name,
                    IsActive        = x.IsActive
                });

                return groupScheduleDtos;
            });

            return opResult;
        }

        /// <summary>
        /// Check whether cost-center has any associated group schedules.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientCostCenterId"></param>
        /// <returns></returns>
        IOpResult<bool> IGroupScheduleService.GetClientCostCenterHasAssociatedGroupSchedule(int clientId, int clientCostCenterId)
        {
            var opResult = new OpResult<bool>();

            // Do permission checks
            _session.CanPerformAction(ClientsActionType.ReadClientCostCenterSetup).MergeInto(opResult);
            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(opResult);

            if (opResult.HasError)
                return opResult;

            opResult.TrySetData(() =>
            {
                // Get groupSchedulesQuery
                var groupSchedules = _session.UnitOfWork.LaborManagementRepository
                    .GroupScheduleQuery()
                    .ByClientId(clientId)
                    .ByCostCenterId(clientCostCenterId)
                    .ExecuteQuery();

                return groupSchedules.Any();
            });

            return opResult;
        }

        #endregion

    }
}

