using System;
using System.Collections.Generic;
using System.Linq;
using Dominion.Core.Services.Interfaces;
using Dominion.LaborManagement.Dto.GroupScheduling;
using Dominion.LaborManagement.Service.Internal.Security;
using Dominion.LaborManagement.Service.Mapping;
using Dominion.Utility.OpResult;

namespace Dominion.LaborManagement.Service.Internal.Providers
{
    internal class ScheduleGroupProvider : IScheduleGroupProvider
    {
        #region Variables and Properties
        
        private readonly IBusinessApiSession _session;

        internal IScheduleGroupProvider Self { get; set; }
        
        #endregion

        #region Constructors and Initializers

        public ScheduleGroupProvider(IBusinessApiSession session)
        {
            Self = this;

            _session = session;
        }

        #endregion

        /// <summary>
        /// This doesn't goto the database directly. This call sets up a call to the database.
        /// There will be different queries based on the type and if using supervisor security.
        /// </summary>
        /// <param name="useSupervisorSecurity">If we need to use supervisor security.</param>
        /// <param name="clientId">The client.</param>
        /// <param name="scheduleGroupType">The type of schedule groups.</param>
        /// <param name="scheduleGroupId">A particular schedule group.</param>
        /// <param name="withScheduleGroupShiftNames">True if you want to include the schedule group shift names (sub-groups)</param>
        /// <returns></returns>
        IOpResult<IEnumerable<GroupScheduleDtos.ScheduleGroupDto>> IScheduleGroupProvider.GetScheduleGroupsByGroupType(
            bool useSupervisorSecurity,
            int clientId,
            ScheduleGroupType scheduleGroupType,
            int? scheduleGroupId, 
            bool withScheduleGroupShiftNames)
        {
            var opResult = new OpResult<IEnumerable<GroupScheduleDtos.ScheduleGroupDto>>();

            opResult.TryCatchIfSuccessful(() =>
            {
                switch(scheduleGroupType)
                {
                    case ScheduleGroupType.ClientCostCenter:
                        opResult.Data = ((IScheduleGroupProvider)this).GetClientCostCenterScheduleGroups(
                            userId: 
                                useSupervisorSecurity 
                                ? _session.LoggedInUserInformation.UserId 
                                : default(int?),
                            clientId:                    clientId,
                            scheduleGroupId:             scheduleGroupId,
                            withScheduleGroupShiftNames: withScheduleGroupShiftNames);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException("scheduleGroupType");
                }
            });

            return opResult;
        }

        /// <summary>
        /// Getting schedule group information based on cost centers and the user requesting the data.
        /// Schedule groups are wrappers for cost centers (possibly others upcoming).
        /// </summary>
        /// <param name="clientId">The client.</param>
        /// <param name="userId">User if concerned with supervisor security on cost centers.</param>
        /// <param name="scheduleGroupId">A particular schedule group.</param>
        /// <param name="withScheduleGroupShiftNames">True if you want to include the schedule group shift names (sub-groups)</param>
        /// <returns></returns>
        IEnumerable<GroupScheduleDtos.ScheduleGroupDto> IScheduleGroupProvider.GetClientCostCenterScheduleGroups(
            int  clientId,
            int? userId,
            int? scheduleGroupId,
            bool withScheduleGroupShiftNames)
        {
            var qry = _session.UnitOfWork
                .LaborManagementRepository
                .ClientCostCenterQuery()
                .ByIsActive(true)
                .ByClientId(clientId);

            if(userId.HasValue)
                qry.ByUserSupervisorSecurity(clientId, userId.Value);

            if(scheduleGroupId.HasValue)
                qry.ByScheduleGroupId(scheduleGroupId.Value);

            if(withScheduleGroupShiftNames)
            {
                var data = qry
                    .ExecuteQueryAs(new ClientCostCenterMaps.ToScheduleGroupWithSubGroups())
                    .ToList();

                return data;
            }
            else
            {
                var data = qry
                    .ExecuteQueryAs(new ClientCostCenterMaps.ToScheduleGroup())
                    .ToList();

                return data;
            }
        }

        /// <summary>
        /// Returns the schedule groups that are accessible by the current user (eg: if a supervisor will only return
        /// the groups they have supervisor access to).
        /// </summary>
        /// <returns></returns>
        IOpResult<IEnumerable<int>> IScheduleGroupProvider.GetCurrentUserAccessibleScheduleGroupIds()
        {
            var isLaborPlanAdmin      = _session.CanPerformAction(LaborManagementActionType.LaborPlanAdministrator).Success;
            var isLaborScheduler      = _session.CanPerformAction(LaborManagementActionType.LaborScheduleAdministrator).Success;
            var isLaborPlanSupervisor = _session.CanPerformAction(LaborManagementActionType.LaborPlanSupervisor).Success;

            return new OpResult<IEnumerable<int>>().TrySetData(Self
                .GetScheduleGroupsByGroupType(
                    useSupervisorSecurity:          isLaborPlanSupervisor || (isLaborScheduler && !isLaborPlanAdmin), 
                    clientId:                       _session.LoggedInUserInformation.ClientId.Value, 
                    scheduleGroupType:              ScheduleGroupType.ClientCostCenter,
                    withScheduleGroupShiftNames:    false)
                .Data
                .Where(x => x.ScheduleGroupId.HasValue)
                .Select(x => x.ScheduleGroupId.Value)
                .ToList);
        }
    }
}
