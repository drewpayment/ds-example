using Dominion.Core.Dto.Misc;
using Dominion.Domain.Entities.Billing;
using Dominion.Utility.Query;
using System;

namespace Dominion.Domain.Interfaces.Query.AR
{
    public interface IArBillingHistoryQuery : IQuery<BillingHistory, IArBillingHistoryQuery>
    {
        IArBillingHistoryQuery ByInvoiceDateRange(DateTime startDate, DateTime endDate);
        IArBillingHistoryQuery ByInvoiceNumber(string invoiceNumber);
        IArBillingHistoryQuery ByIsUnpaid();
        IArBillingHistoryQuery ByDominionVendorOption(DominionVendorOption dominionVendorOption);
        IArBillingHistoryQuery ByClientHasStarted();
        IArBillingHistoryQuery ByClientId(int clientId);
    }
}
