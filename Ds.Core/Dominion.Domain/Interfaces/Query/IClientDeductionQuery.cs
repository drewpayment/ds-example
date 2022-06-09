using System.Collections.Generic;
using Dominion.Domain.Entities.Payroll;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Builds a query on <see cref="ClientDeduction"/> data.
    /// </summary>
    public interface IClientDeductionQuery : IQuery<ClientDeduction, IClientDeductionQuery>
    {
        /// <summary>
        /// Filter by client deduction ID.
        /// </summary>
        /// <param name="clientDeductionId"></param>
        /// <returns></returns>
        IClientDeductionQuery ByClientDeductionId(int clientDeductionId);

        /// <summary>
        /// Filter by a list of client deduction IDs.
        /// </summary>
        /// <param name="clientDeductionIds"></param>
        /// <returns></returns>
        IClientDeductionQuery ByClientDeductionIds(IEnumerable<int> clientDeductionIds);

        /// <summary>
        /// Filter by a list of client deduction codes.
        /// </summary>
        /// <param name="codes"></param>
        /// <returns></returns>
        IClientDeductionQuery ByClientDeductionCodes(IEnumerable<string> codes);

        /// <summary>
        /// Filter by the client ID.
        /// </summary>
        /// <param name="clientId">The ID of the client.</param>
        /// <returns></returns>
        IClientDeductionQuery ByClientId(int clientId);
        IClientDeductionQuery ByClientIds(params int[] clientIds);

        IClientDeductionQuery ByIsMemoDeduction();
    }
}