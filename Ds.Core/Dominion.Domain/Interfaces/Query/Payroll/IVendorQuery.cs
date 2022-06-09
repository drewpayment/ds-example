using Dominion.Domain.Entities.Payroll;
using Dominion.Utility.Query;


namespace Dominion.Domain.Interfaces.Query.Payroll
{
    public interface IVendorQuery : IQuery<Vendor, IVendorQuery>
    {
        IVendorQuery ByVendorId(int VendorId);
    }
}
