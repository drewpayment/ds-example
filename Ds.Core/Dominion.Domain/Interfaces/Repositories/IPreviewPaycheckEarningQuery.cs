using Dominion.Domain.Entities.Payroll;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Performs queries on <see cref="PreviewPaycheckEarning"/> entities.
    /// </summary>
    public interface IPreviewPaycheckEarningQuery : IQuery<PreviewPaycheckEarning, IPreviewPaycheckEarningQuery>
    {
        /// <summary>
        /// Filters earnings by those belonging to the given paycheck.
        /// </summary>
        /// <param name="previewPaycheckId">ID of the paycheck the earning should be from.</param>
        /// <returns></returns>
        IPreviewPaycheckEarningQuery ByPreviewPaycheckId(int previewPaycheckId);
    }
}
