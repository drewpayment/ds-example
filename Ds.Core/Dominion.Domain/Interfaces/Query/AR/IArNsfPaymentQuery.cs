using Dominion.Domain.Entities.AR;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.AR
{
    public interface IArNsfPaymentQuery : IQuery<ArNsfPayment, IArNsfPaymentQuery>
    {
        IArNsfPaymentQuery ByArNsfPaymentId(int arNsfPaymentId);

        IArNsfPaymentQuery ByArDepositId(int arDepositId);

        IArNsfPaymentQuery ByClientId(int clientId);

        IArNsfPaymentQuery ByManualInvoiceId(int manualInvoiceId);

        IArNsfPaymentQuery ByGenBillingHistoryId(int genBillingHistoryId);
    }
}
