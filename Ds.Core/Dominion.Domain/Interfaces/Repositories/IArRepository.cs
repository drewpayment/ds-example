using System;
using Dominion.Domain.Interfaces.Query.AR;

namespace Dominion.Domain.Interfaces.Repositories
{
    public interface IArRepository : IRepository, IDisposable
    {
        IArDepositQuery QueryArDeposits();

        IArManualInvoiceQuery QueryArManualInvoices();

        IArManualInvoiceDetailQuery QueryArManualInvoiceDetails();

        IArNsfPaymentQuery QueryArNsfPayments();

        IArPaymentQuery QueryArPayments();

        IArReportQuery QueryArReports();
        IArBillingHistoryQuery QueryBillingHistory();
    }
}
