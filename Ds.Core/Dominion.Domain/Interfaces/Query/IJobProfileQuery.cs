using System.Collections.Generic;
using Dominion.Core.Dto.SftpConfiguration;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Labor;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IJobProfileQuery : IQuery<JobProfile, IJobProfileQuery>
    {
        IJobProfileQuery ByJobProfileId(int jobProfileId);
        IJobProfileQuery ByClientId(int clientId);
        IJobProfileQuery ByIsActive(bool isActive);
        IJobProfileQuery ByHideInActiveExceptProfileId(bool hideInActive, int jobProfileId = 0);
        IJobProfileQuery OrderByJobProfileId();
        IJobProfileQuery OrderByJobDescription();
        IJobProfileQuery ByClientIds(List<int> clientIds);
        IJobProfileQuery HasJobProfileClasification();
        IJobProfileQuery ByOnboardingTaskListId(int taskListId);

        /// <summary>
        /// Filters by one or more particular job titles.
        /// </summary>
        /// <param name="jobProfileIds">One or more job title ID(s) to filter by.</param>
        /// <returns></returns>
        IJobProfileQuery ByJobProfiles(params int[] jobProfileIds);
        IJobProfileQuery ByJobProfileDescription(string description);
        IJobProfileQuery ByHasJobProfileClasificationWithSftpTypeId(SftpType sftpTypeId);
    }
}
