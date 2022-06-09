using System;
using Dominion.Core.Dto.Misc;
using Dominion.Domain.Entities.AR;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.AR
{
    public interface IArManualInvoiceQuery : IQuery<ArManualInvoice, IArManualInvoiceQuery>
    {
        IArManualInvoiceQuery ByArManualInvoiceId(int arManualInvoiceId);
        IArManualInvoiceQuery ByClientId(int clientId);
        IArManualInvoiceQuery ByIsPaid(bool isPaid = true);
        IArManualInvoiceQuery ByInvoiceDateRange(DateTime startDate, DateTime endDate);
        IArManualInvoiceQuery ByIsUnpaid();
        IArManualInvoiceQuery ByDominionVendorOption(DominionVendorOption dominionVendorOption);
        IArManualInvoiceQuery ByClientCode(string clientCode);
    }
}
