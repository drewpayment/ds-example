using Dominion.Domain.Entities.Tax;
using Dominion.Taxes.Dto.TaxOptions;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Query on a <see cref="FilingStatusInfo"/> data source.
    /// </summary>
    public interface IFilingStatusInfoQuery : IQuery<FilingStatusInfo, IFilingStatusInfoQuery>
    {
        /// <summary>
        /// Filters the query to a particular filing status.
        /// </summary>
        /// <param name="status">Filing status type to get info for.</param>
        /// <returns></returns>
        IFilingStatusInfoQuery ByFilingStatusType(FilingStatus status);

        /// <summary>
        /// Filters the filing statuses by those that are actually regulated by the government.
        /// </summary>
        /// <returns></returns>
        IFilingStatusInfoQuery ByIsRegulatoryStatus();

        IFilingStatusInfoQuery ByIsDefaultFilingStatus();
    }
}
