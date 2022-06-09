using Dominion.Domain.Entities.Aca;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Constructs a query on <see cref="AcaErrorCode"/> data.
    /// </summary>
    public interface IAcaErrorCodeQuery : IQuery<AcaErrorCode, IAcaErrorCodeQuery>
    {
        /// <summary>
        /// Filter by the provided IRS error code.
        /// </summary>
        /// <param name="code">IRS error code to filter error codes by.</param>
        /// <returns></returns>
        IAcaErrorCodeQuery ByIrsCode(string code);
    }
}
