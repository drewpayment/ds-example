using Dominion.Domain.Entities.AR;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.AR
{
    public interface IArPaymentQuery : IQuery<ArPayment, IArPaymentQuery>
    {
        IArPaymentQuery ByArPaymentId(int arPaymentId);

        IArPaymentQuery ByClientId(int clientId);

        IArPaymentQuery ByManualInvoiceId(int manualInvoiceId);

        IArPaymentQuery ByArDepositId(int arDepositId);

        IArPaymentQuery ByGenBillingHistoryId(int genBillingHistoryId);
    }
}
