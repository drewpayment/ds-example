using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Reporting;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IScheduledReportQuery : IQuery<ScheduledReport, IScheduledReportQuery>
    {
        /// <summary>
        /// Filters scheduled report by email Id.
        /// </summary>
        /// <param name="ScheduledReportEmailAddressId">ID of client to filter by.</param>
        /// <returns></returns>
        IScheduledReportQuery ByScheduledReportEmailAddressId(int ScheduledReportEmailAddressId);

        IScheduledReportQuery ByClientId(int clientId);
    }
}
