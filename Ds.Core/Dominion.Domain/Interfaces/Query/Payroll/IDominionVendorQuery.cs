using Dominion.Domain.Entities.Payroll;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Payroll
{
    public interface IDominionVendorQuery : IQuery<DominionVendor, IDominionVendorQuery>
    {
        IDominionVendorQuery ByDominionVendorId(int dominionVendorId);
    }
}
