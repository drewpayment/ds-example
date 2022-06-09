using Dominion.Domain.Entities.Payroll;
using Dominion.Utility.Query;
using System.Collections.Generic;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IPayrollFederalTaxCreditQuery : IQuery<PayrollFederalTaxCredit, IPayrollFederalTaxCreditQuery>
    {
        /// <summary>
        /// Filters history for a single client.
        /// </summary>
        /// <param name="clientId">ID of client to filter by.</param>
        /// <returns></returns>
        IPayrollFederalTaxCreditQuery ByClientId(int clientId);

        /// <summary>
        /// Gets Paycheck Tax Data by PayrollID
        /// </summary>
        /// <param name="payrollId"></param>
        /// <returns></returns>
        IPayrollFederalTaxCreditQuery ByPayrollId(int payrollId);
    }
}
