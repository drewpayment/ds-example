using Dominion.Domain.Entities.Payroll;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Payroll
{
    public interface IGenW2ClientHistoryQuery : IQuery<GenW2ClientHistory, IGenW2ClientHistoryQuery>
    {
        IGenW2ClientHistoryQuery ByClientId(int clientId);

        IGenW2ClientHistoryQuery ByW2Year(int w2Year);
    }
}
