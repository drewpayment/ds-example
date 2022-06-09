using Dominion.Domain.Entities.Misc;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Query on a <see cref="Country"/> data source.
    /// </summary>
    public interface ICountryQuery : IQuery<Country, ICountryQuery>
    {
       
    }
}
