using System;
using System.Collections.Generic;

using Dominion.Core.Dto.User;
using Dominion.Core.Services.Security;
using Dominion.Domain.Entities.User;
using Dominion.Domain.Interfaces.UnitOfWork;
using Dominion.Utility.Security;

namespace Dominion.LaborManagement.Service.Internal.Security
{
    using Dominion.Core.Dto.User;

    internal class BudgetingActionTypeRegulator : IActionTypeRegulator
    {
        private IBusinessUnitOfWork _uow;

        string IActionTypeRegulator.Name 
        {
            get { return "Budgeting Action Type Regulator"; } 
        }

        public BudgetingActionTypeRegulator(IBusinessUnitOfWork uow)
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
            

            var removalRules = new List<ActionRemover>
            {

                ActionRemover.For(BudgetingActionType.BudgetingAdministrator).RemoveWhenNot(() =>
                    user.UserType == UserType.SystemAdmin || clientConfig.IsBudgetingEnabled)
            };

            removalRules.ForEach(remover => remover.RemoveActionFrom(actions));

            return actions;
        }
    }
}

