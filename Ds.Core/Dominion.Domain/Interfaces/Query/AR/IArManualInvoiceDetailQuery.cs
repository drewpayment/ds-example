using Dominion.Domain.Entities.AR;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.AR
{
    public interface IArManualInvoiceDetailQuery : IQuery<ArManualInvoiceDetail, IArManualInvoiceDetailQuery>
    {
        IArManualInvoiceDetailQuery ByArManualInvoiceDetailId(int arManualInvoiceDetailId);

        IArManualInvoiceDetailQuery ByArManualInvoiceId(int arManualInvoiceId);
    }
}
