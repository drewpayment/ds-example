using System.Collections.Generic;
using Dominion.Core.Services.Security;
using Dominion.Domain.Interfaces.UnitOfWork;
using Dominion.Utility.Security;

namespace Dominion.LaborManagement.Service.Internal.Security
{
    using Dominion.Core.Dto.Client;
    using Dominion.Core.Dto.User;
    using static Dominion.Core.Dto.Labor.ClockClientTimePolicyDtos;

    internal class ClockActionTypeRegulator : IActionTypeRegulator
    {
        private IBusinessUnitOfWork _uow;

        string IActionTypeRegulator.Name
        {
            get { return "Clock Action Type Regulator"; }
        }

        public ClockActionTypeRegulator(IBusinessUnitOfWork uow)
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
            var isSelfEmployee = user.EmployeeId.HasValue;

            var allowEditEmployeeSetup = false;
            var supervisorSecuritySettings = _uow.SecurityRepository.QueryUserSupervisorSecuritySettings() //Query UserSupervisorSecuritySettings for isAllowEditEmployeeSetup, used below
                .ByUserId(user.UserId).FirstOrDefault();

            if (supervisorSecuritySettings != null) //Make sure the query returned a row, if not, the user is not a supervisor
            {
                allowEditEmployeeSetup = supervisorSecuritySettings.IsAllowEditEmployeeSetup;
            }

            // Most basic requirement of being able to punch on the timeclock or via the ESS mobile app 
            // is that the employee is required to have a TimePolicy assigned to them. If they don't, 
            // they are not allowed to punch until resolved. 
            var clockEmp = _uow.TimeClockRepository
                .GetClockEmployeeQuery()
                .ByEmployeeId(user?.EmployeeId ?? 0)
                .FirstOrDefaultAs(x => new 
                { 
                    x.ClockClientTimePolicyId,
                    TimePolicy = x.TimePolicy != null ? new ClockClientTimePolicyDto
                    {
                        ClockClientTimePolicyId = x.TimePolicy.ClockClientTimePolicyId,
                        ClientId = x.TimePolicy.ClientId,
                        ClockClientRulesId = x.TimePolicy.ClockClientRulesId,
                        Name = x.TimePolicy.Name,
                        Modified = x.TimePolicy.Modified,
                        ModifiedBy = x.TimePolicy.ModifiedBy
                    } : null
                });

            var clientFeature = _uow.ClientAccountFeatureRepository
                .ClientAccountFeatureQuery()
                .ByAccountFeatureId(Core.Dto.Misc.AccountFeatureEnum.TimeClock)
                .ByClientId(user.LastClientId.HasValue ? user.LastClientId.Value : 0)
                .ExecuteQueryAs(x => new ClientAccountFeatureDto
                {
                    AccountFeature = x.AccountFeature
                });

            var hasClientfeature = clientFeature != null;

            var hasClockEmployee = clockEmp != null 
                && clockEmp.ClockClientTimePolicyId > 0 
                && clockEmp.TimePolicy != null 
                && clockEmp.TimePolicy.ClockClientTimePolicyId == clockEmp.ClockClientTimePolicyId;

            var removalRules = new List<ActionRemover>
            {
                ActionRemover.For(ClockActionType.ClockEmployeeAdministrator).RemoveWhenNot(() =>
                    user.UserType == UserType.SystemAdmin ||
                    user.UserType == UserType.CompanyAdmin ||
                    user.UserType == UserType.Supervisor ||
                    user.UserType == UserType.Employee && isSelfEmployee),

                ActionRemover.For(ClockActionType.MobileTimeClockAccess).RemoveWhen(() => !clientConfig.IsTimeClockEnabled || !hasClockEmployee),
                ActionRemover.For(ClockActionType.CanEditClockEmployee).RemoveWhen(() => user.UserType == UserType.Supervisor && (!allowEditEmployeeSetup || user.LastEmployeeId == user.EmployeeId)), //Admins have access, and supervisor access is taken away if they do not have permission to EditEmployeeSetup
                ActionRemover.For(ClockActionType.CanOptInToCompanyFeature).RemoveWhenNot(() =>  user.UserType == UserType.SystemAdmin || user.UserType == UserType.CompanyAdmin && hasClientfeature),
            };

            removalRules.ForEach(remover => remover.RemoveActionFrom(actions));

            return actions;
        }
    }
}

