using Dominion.Domain.Interfaces.Query;

namespace Dominion.Domain.Interfaces.Repositories
{
    public interface IContactRepository
    {
        IPersonQuery PersonQuery();

        IAddressQuery AddressQuery();
    }
}
