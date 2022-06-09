using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Payroll;
using Dominion.Domain.Interfaces.Query;
using Dominion.Domain.Interfaces.Query.Clients;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Repositories
{
    public interface IClientRepository : IDisposable
    {
        // Client methods
        Client GetClient(int id);
        IEnumerable<TResult> GetClients<TResult>(QueryBuilder<Client, TResult> queryBuilder) where TResult : class;

        #region CLIENT_USER_GROUP

        /// <summary>
        /// Get the ClientUserGroup with the specified id.
        /// </summary>
        /// <param name="id">ID of the entity to be found.</param>
        /// <returns>Entity with the specified ID or null if the id was not found.</returns>
        ClientUserGroup GetClientUserGroup(int id);

        /// <summary>
        /// Get the client user group information that matches the given criteria.
        /// </summary>
        /// <typeparam name="TResult">The type of object to be returned.</typeparam>
        /// <param name="queryBuilder">The search criteria.</param>
        /// <returns>The client user group information that matches the given criteria.</returns>
        IEnumerable<TResult> GetClientUserGroups<TResult>(QueryBuilder<ClientUserGroup, TResult> queryBuilder)
            where TResult : class;

        /// <summary>
        /// Get the user groups associated with the given client id. This includes 'system' user
        /// groups that aren't associated with any clients.
        /// </summary>
        /// <param name="clientId">Id of the client for which user groups are retrieved.</param>
        /// <returns>The user groups associated with the given client id.</returns>
        IEnumerable<ClientUserGroup> GetClientUserGroupsForClient(int? clientId);

        /// <summary>
        /// Get the user group action type information that matches the given criteria.
        /// </summary>
        /// <typeparam name="TResult">The type of object to be returned.</typeparam>
        /// <param name="queryBuilder">The search criteria.</param>
        /// <returns>The user group action types information that matches the given criteria.</returns>
        IEnumerable<TResult> GetClientUserGroupActionTypes<TResult>(
            QueryBuilder<ClientUserGroupActionType, TResult> queryBuilder)
            where TResult : class;
        /// <summary>
        /// Get the user group action types associated with the given client id, user id and
        /// action designation. This includes system user groups.
        /// </summary>
        /// <param name="clientId">Id of the client for which results are limited. A value of
        /// zero (0) indicates only return recs related to system user groups.</param>
        /// <param name="userId">Id of the user for which results are limited.</param>
        /// <param name="actionTypeDesignation">Action type for which results are limited.</param>
        /// <returns>The user group action types associated with the given client id, user id and
        /// action designation.</returns>
        IEnumerable<ClientUserGroupActionType> GetClientUserGroupActionTypes(int clientId, int userId, 
            string actionTypeDesignation);

        #endregion //CLIENT_USER_GROUP

        /// <summary>
        /// Constructs a new query on <see cref="Client"/> data.
        /// </summary>
        /// <returns></returns>
        IClientQuery QueryClients();

        IClientContactQuery ClientContactQuery();

        /// <summary>
        /// Queries <see cref="ClientCalendar"/> data.
        /// </summary>
        /// <returns></returns>
        IClientCalendarQuery QueryClientCalendars();
        
        /// <summary>
        /// Queries the client Rates
        /// </summary>
        /// <returns></returns>
        IClientRateQuery ClientRateQuery();

        IClientDisciplineLevelQuery ClientDisciplineLevelQuery();
        ICompanyResourceQuery CompanyResourceQuery();
        ICompanyResourceFolderQuery CompanyResourceFolderQuery();
        IClientTaxQuery QueryClientTaxes();
        IClientEssOptionsQuery QueryClientEssOptions();
        IClientBankInfoQuery QueryClientBankInfo();
        IClientBankQuery QueryClientBanks();
        IBankAccountNextCheckQuery QueryBankAccountNextCheck();
        IClientPayrollQuery QueryClientPayroll();
        IClientPickupQuery QueryClientPickupMethod();
        IClientDeductionQuery QueryClientDeductions();

        /// <summary>
        /// Creates a new query on <see cref="ClientMatch"/> data.
        /// </summary>
        /// <returns></returns>
        IClientMatchQuery QueryClientMatches();
        IClientIpSecurityQuery QueryClientIpSecurity();
        IClientEmploymentClassQuery QueryClientEmploymentClasses();
        TResult GetClient<TResult>(int clientID, Expression<Func<Client, TResult>> selector) where TResult : class;
        
        IJobProfileQuery JobProfileQuery();
        IClientWorkersCompQuery WorkersCompQuery();
        IJobProfileClassificationsQuery JobProfileClassificationsQuery();
        IJobProfileCompensationQuery JobProfileCompensationQuery();
        IJobResponsibilitiesQuery JobResponsibilitiesQuery();
        IJobProfileResponsibilitiesQuery JobProfileResponsibilitiesQuery();
        IJobSkillsQuery JobSkillsQuery();
        IJobProfileSkillsQuery JobProfileSkillsQuery();
        IJobProfileAccrualsQuery JobProfileAccrualsQuery();
        IJobProfileOnboardingWorkflowQuery JobProfileOnboardingWorkflowQuery();
        IClientGroupQuery ClientGroupQuery();
        IClientShiftQuery ClientShiftQuery();
        IClientTopicQuery ClientTopicQuery();
        IClientSubTopicQuery ClientSubTopicQuery();
        IClientDivisionQuery ClientDivisionQuery();
        IClientDivisionAddressQuery ClientDivisionAddressQuery();
        IClientDivisionLogoQuery ClientDivisionLogoQuery();
        IClientRelationQuery ClientRelationQuery();
        IClientHRInfoQuery ClientHRInfoQuery();

        IClient401KSetupQuery Client401KSetupQuery();
        IClientCostCenterQuery ClientCostCenterQuery();
        bool CheckStuff(int clientTopicId);
        IClientSMTPSettingQuery ClientSMTPSettingQuery();
        IGeneralLedgerAccountQuery GeneralLedgerAccountQuery();
        IClientGLSettingsQuery ClientGLSettingsQuery();
        IClientGLAssignmentQuery ClientGLAssignmentQuery();
        IClientGLClassGroupQuery ClientGLClassGroupQuery();
        IGeneralLedgerTypeQuery GeneralLedgerTypeQuery();
        IClientGLControlQuery ClientGLControlQuery();
        IClientGLControlItemQuery ClientGLControlItemQuery();
        IGeneralLedgerGroupHeaderQuery GeneralLedgerGroupHeaderQuery();
        IClientGLCustomClassQuery ClientGLCustomClassQuery();
        IClientTurboTaxQuery QueryClientTurboTax();
        IClientTurboTaxTrackingQuery QueryClientTurboTaxTracking();
        IClientWorkNumberPolicyQuery ClientWorkNumberPolicyQuery();
        IClientUnemploymentSetupQuery QueryClientUnemploymentSetup();
        IClientSftpConfigurationQuery QueryClientSftpConfiguration();

        /// <summary>
        /// Constructs a new query on <see cref="Client"/> data.
        /// </summary>
        /// <returns></returns>
        IClientRelationQuery QueryClientRelationshipClients();
        IClientW2OptionQuery QueryClientW2Options();
    }
}
