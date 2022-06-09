using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Reporting;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Reporting
{
    public interface ISavedReportCustomFieldQuery : IQuery<SavedReportCustomField, ISavedReportCustomFieldQuery>
    {
        ISavedReportCustomFieldQuery ByClientId(int clientId);
    }
}
