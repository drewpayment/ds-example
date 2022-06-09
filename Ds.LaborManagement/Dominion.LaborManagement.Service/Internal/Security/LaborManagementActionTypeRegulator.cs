using System;
using System.Collections.Generic;
using Dominion.Core.Dto.User;
using Dominion.Core.Services.Security;
using Dominion.Domain.Entities.User;
using Dominion.Domain.Interfaces.UnitOfWork;
using Dominion.Utility.Query.LinqKit;
using Dominion.Utility.Security;

namespace Dominion.LaborManagement.Service.Internal.Security
{
    using Dominion.Core.Dto.User;

    internal class LaborManagementActionTypeRegulator : IActionTypeRegulator
    {
        private IBusinessUnitOfWork _uow;

        string IActionTypeRegulator.Name 
        {
            get { return "Labor Management Action Type Regulator"; } 
        }

        public LaborManagementActionTypeRegulator(IBusinessUnitOfWork uow)
        {
            _uow = uow;
        }

        /// <summary>
        /// Right now this is only going to remove action types.
        /// </summary>
        /// <param name="actions">The action types loaded from the database.</param>
        /// <param name="user">The current user.</param>
        /// <param name="clientConfig">The current client configuration.</param>
        Dictionary<string, ActionType> IActionTypeRegulator.Regulate(Dictionary<string, ActionType> actions, UserPrincipalDto user, ClientAccountConfiguration clientConfig)
        {
            var isSupervisorPlanner = user.UserType == UserType.Supervisor && _uow.SecurityRepository
                .QueryUserSupervisorSecuritySettings()
                .ByUserId(user.UserId)
                .ByIsGroupPlannerEditingAllowed()
                .Result
                .Count() > 0;

            Func<bool> isTimeClockClient = 
                () => user.UserType == UserType.SystemAdmin || clientConfig.IsTimeClockEnabled;

            var removalRules = new List<ActionRemover>
            {
                ActionRemover.For(LaborManagementActionType.LaborPlanSupervisor).RemoveWhenNot(() =>
                    isSupervisorPlanner),
                ActionRemover.For(LaborManagementActionType.LaborPlanAdministrator).RemoveWhenNot(() =>
                    user.UserType == UserType.SystemAdmin || (clientConfig.IsGroupSchedulerEnabled && (user.UserType != UserType.Supervisor || isSupervisorPlanner))),
                ActionRemover.For(LaborManagementActionType.LaborScheduleAdministrator).RemoveWhenNot(() =>
                    user.UserType == UserType.SystemAdmin || clientConfig.IsGroupSchedulerEnabled),
                ActionRemover.For(LaborManagementActionType.WriteAutoPoints).RemoveWhenNot(() =>
                    clientConfig.IsEmployeePointsAndIncidentsAutoEnabled),
                ActionRemover.For(LaborManagementActionType.ReadAutoPoints).RemoveWhenNot(() =>
                    clientConfig.IsEmployeePointsAndIncidentsAutoEnabled),
                ActionRemover.For(LaborManagementActionType.ReadTimeCardAuthorization).RemoveWhenNot(() =>
                    clientConfig.IsTimeClockEnabled || user.UserType == UserType.SystemAdmin),
                ActionRemover.For(LaborManagementActionType.UnassignedFilterOptionIsVisible).RemoveWhenNot(() =>
                    clientConfig.IsTimeClockEnabled || user.UserType == UserType.SystemAdmin),
                ActionRemover.For(LaborManagementActionType.CanImportPunches).RemoveWhenNot(() =>
                    clientConfig.IsTimeClockEnabled || user.UserType == UserType.SystemAdmin)
            };
            ClockActionType.GetAll()
                .ForEach(at => removalRules.Add(ActionRemover.For(at).RemoveWhenNot(isTimeClockClient)));

            removalRules.ForEach(remover => remover.RemoveActionFrom(actions));

            return actions;
        }
    }
}

