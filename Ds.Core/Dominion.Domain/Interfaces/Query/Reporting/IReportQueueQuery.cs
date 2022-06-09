using Dominion.Core.Dto.Reporting;
using Dominion.Domain.Entities.Reporting;
using Dominion.Utility.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Interfaces.Query.Reporting
{
    public interface IReportQueueQuery : IQuery<ReportQueue, IReportQueueQuery>
    {
        IReportQueueQuery ByReportQueueId(int reportQueueId);
        IReportQueueQuery ByClientId(int clientId);
        IReportQueueQuery ByPayrollId(int payrollId);
        IReportQueueQuery ByUserId(int userId);
        IReportQueueQuery ByStandardReportId(StandardReportEnum standardReportId);
        IReportQueueQuery ByIsDeleted(bool isDeleted);
    }
}
