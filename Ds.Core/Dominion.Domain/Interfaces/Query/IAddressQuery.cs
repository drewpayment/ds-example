using Dominion.Domain.Entities.Contact;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IAddressQuery : IQuery<Address, IAddressQuery>
    {
        IAddressQuery ByAddressId(int addressId);
    }
}
