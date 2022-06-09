using Dominion.Core.Dto.Performance;
using Dominion.Domain.Entities.Payroll;
using Dominion.Utility.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Interfaces.Query.Payroll
{
    public interface IOneTimeEarningQuery : IQuery<OneTimeEarning, IOneTimeEarningQuery>
    {
        IOneTimeEarningQuery ByApprovalStatus(ApprovalStatus approvalStatus);
        IOneTimeEarningQuery ByClientId(int clientId);
        IOneTimeEarningQuery ByNotIncludedInAnotherPayroll();
        IOneTimeEarningQuery ByBeforePayPeriodEndDate(DateTime payPeriodEndDate);
        IOneTimeEarningQuery ByProposals(ICollection<int> proposals);
    }
}
