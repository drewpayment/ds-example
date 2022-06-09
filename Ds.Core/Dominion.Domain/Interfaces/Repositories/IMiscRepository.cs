using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Sprocs;
using Dominion.Domain.Entities.Configs;
using Dominion.Domain.Entities.Misc;
using Dominion.Domain.Interfaces.Query;
using Dominion.Domain.Interfaces.Query.Reporting;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Repositories
{
    public interface IMiscRepository : IDisposable
    {
        // SecretQuestion methods
        SecretQuestion GetSecretQuestion(int secredQuesitonId);
        IEnumerable<SecretQuestion> GetAllSecretQuestions();

        /// <summary>
        /// Get a list of countries.
        /// </summary>
        /// <param name="qb">Limit what is returned in the list.</param>
        /// <returns>List of countries matching the query builder criteria.</returns>
        IEnumerable<Country> GetCountryList(QueryBuilder<Country, Country> qb);

        /// <summary>
        /// Get a list of states.
        /// The automapper query builder is used here since we'll have a one to one mapping of properties.
        /// </summary>
        /// <param name="qb">Limit what is returned in the list.</param>
        /// <returns>List of states matching the query builder criteria.</returns>
        IEnumerable<TResult> GetStateList<TResult>(QueryBuilderAutoMap<State, TResult> qb) where TResult : class;

        /// <summary>
        /// Get a list of counties.
        /// The automapper query builder is used here since we'll have a one to one mapping of properties.
        /// </summary>
        /// <param name="qb">Limit what is returned in the list.</param>
        /// <returns>List of counties matching the query builder criteria.</returns>
        IEnumerable<TResult> GetCountyList<TResult>(QueryBuilderAutoMap<County, TResult> qb) where TResult : class;

        IReciprocalStateQuery ReciprocalStateQuery();

        /// <summary>
        /// Returns the currently available account features with their default settings.
        /// </summary>
        /// <returns></returns>
        IEnumerable<AccountFeatureInfo> GetCurrentAccountFeatures();

        /// <summary>
        /// Returns the currently available account options.
        /// </summary>
        /// <returns></returns>
        IEnumerable<AccountOptionInfo> GetCurrentAccountOptions();

        /// <summary>
        /// Build and execute a query on <see cref="AccountOptionInfo"/>.
        /// </summary>
        /// <returns></returns>
        IAccountOptionQuery QueryAccountOptionInfo();

        /// <summary>
        /// Build and execute a query on <see cref="AccountFeatureInfo"/>.
        /// </summary>
        /// <returns></returns>
        IAccountFeatureQuery QueryAccountFeatureInfo();

		/// <summary>
        /// Build and execute a query on <see cref="ConfigSetting"/>.
        /// </summary>
        /// <returns></returns>
        IConfigSettingsQuery QueryConfigSettings();

        IClientTopicQuery QueryClientTopics();
        IClientSubTopicQuery QueryClientSubTopics();
        ISavedReportQuery QuerySavedReports();
        ISavedReportFieldQuery QuerySavedReportFields();
        IScheduledReportVisibleQuery QueryScheduledReportVisibility();
        IEffectiveDateQuery EffectiveDateQuery();
        object GetClockEmployeeApproveDate(GetClockEmployeeApproveDateArgsDto arg);
        ISftpTypeEntityQuery SftpTypeEntityQuery();
        INoteSourceQuery NoFilter();
    }
}