using Dominion.Domain.Entities.Payroll;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Payroll
{
    public interface ITaxVendorQuery : IQuery<TaxVendor, ITaxVendorQuery>
    {
        ITaxVendorQuery ByTaxVendorId(int taxVendorId);
    }
}
