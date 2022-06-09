using System;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Labor;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IJobTitleQuery : IQuery<JobTitle, IJobTitleQuery>
    {
        /// <summary>
        /// Filters job titles for a single client.
        /// </summary>
        /// <param name="clientId">Client to filter by.</param>
        /// <returns></returns>
        IJobTitleQuery ByClient(int clientId);

        /// <summary>
        /// Filters by one or more particular job titles.
        /// </summary>
        /// <param name="jobTitleIds">One or more job title ID(s) to filter by.</param>
        /// <returns></returns>
        IJobTitleQuery ByJobTitle(params int[] jobTitleIds);
    }
}
