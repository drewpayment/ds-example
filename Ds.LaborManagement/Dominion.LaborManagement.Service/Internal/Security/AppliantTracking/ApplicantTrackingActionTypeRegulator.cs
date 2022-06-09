using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Misc;
using Dominion.Core.Dto.User;
using Dominion.Core.Services.Security;
using Dominion.Domain.Entities.User;
using Dominion.Domain.Interfaces.UnitOfWork;
using Dominion.Utility.Security;

namespace Dominion.LaborManagement.Service.Internal.Security
{
    using Dominion.Core.Dto.User;

    internal class ApplicantTrackingActionTypeRegulator : IActionTypeRegulator
    {
        private IBusinessUnitOfWork _uow;

        string IActionTypeRegulator.Name
        {
            get { return "Applicant tracking Management Action Type Regulator"; }
        }

        public ApplicantTrackingActionTypeRegulator(IBusinessUnitOfWork uow)
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
            var isAnApplicant = _uow.ApplicantTrackingRepository
                .ApplicantsQuery()
                .ByUserId(user.UserId)
                .Result
                .Any();

            var isApplicantTrackingEnabled = _uow.ClientAccountFeatureRepository
                .ClientAccountFeatureQuery()
                .ByClientId(user.LastClientId ?? user.EmployeeClientId.GetValueOrDefault())
                .ByAccountFeatureId(AccountFeatureEnum.ApplicantTracking)
                .Result
                .Any();

            var removalRules = new List<ActionRemover>
            {
                ActionRemover.For(ApplicantTrackingActionType.ApplicantAdmin)
                    .RemoveWhenNot(() => user.UserType == UserType.SystemAdmin || (user.IsApplicantTrackingAdmin && isApplicantTrackingEnabled)),
                ActionRemover.For(ApplicantTrackingActionType.InternalApplicant)
                    .RemoveWhenNot(() => isApplicantTrackingEnabled && isAnApplicant),
                ActionRemover.For(ApplicantTrackingActionType.ExternalApplicant)
                    .RemoveWhenNot(() => user.UserType == UserType.Applicant && isApplicantTrackingEnabled),
                ActionRemover.For(ApplicantTrackingActionType.WriteApplicantInfo)
                    .RemoveWhenNot(() => (user.UserType == UserType.Applicant || user.EmployeeId.HasValue) && isApplicantTrackingEnabled),
                ActionRemover.For(ApplicantTrackingActionType.ReadApplicantInfo)
                    .RemoveWhenNot(() => 
                        (user.UserType == UserType.SystemAdmin) || 
                        (isApplicantTrackingEnabled && (user.IsApplicantTrackingAdmin || user.UserType == UserType.Applicant || user.EmployeeId.HasValue))
                        )
            };

            removalRules.ForEach(remover => remover.RemoveActionFrom(actions));

            return actions;
        }
    }
}

