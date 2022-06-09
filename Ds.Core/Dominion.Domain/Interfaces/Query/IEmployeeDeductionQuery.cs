using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.InstantPay;
using Dominion.Core.Dto.Payroll;
using Dominion.Domain.Entities.Employee;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Allows <see cref="EmployeeDeduction"/> data to be queried.
    /// </summary>
    public interface IEmployeeDeductionQuery : IQuery<EmployeeDeduction, IEmployeeDeductionQuery>
    {
        /// <summary>
        /// Filters employee deductions by the specifed employee.
        /// </summary>
        /// <param name="employeeId">The employee ID to filter by.</param>
        /// <returns>The query to be further filtered.</returns>
        IEmployeeDeductionQuery ByEmployeeId(int employeeId);

        IEmployeeDeductionQuery ByEmployeeDeductionAmountType(EmployeeDeductionAmountType amountType);

        IEmployeeDeductionQuery ByEmployeeDeductionId(int employeeDeductionId);

        IEmployeeDeductionQuery ByLessThanSubSequenceNumber(int subSequenceNum);

        IEmployeeDeductionQuery ByIsInActiveDirectDepositDeductionWithSubSequenceNum();

        IEmployeeDeductionQuery ByEmployeeBankId(int employeeBankId);

        /// <summary>
        /// Filters employee deductions by the specifed client.
        /// </summary>
        /// <param name="clientId">The client ID to filter by.</param>
        /// <returns>The query to be further filtered.</returns>
        IEmployeeDeductionQuery ByClientId(int clientId);

        IEmployeeDeductionQuery ByClientIds(params int[] clientIds);

        /// <summary>
        /// Filters employee deductions which represent Direct Deposit deduction amounts.
        /// </summary>
        /// <returns>The query to be further filtered.</returns>
        IEmployeeDeductionQuery ByIsActiveDirectDepositDeductionWithSubSequenceNum();

        IEmployeeDeductionQuery SortBySubSeqNo(SortDirection direction);
        /// <summary>
        /// Filters employee deductions by whether they are present in the given list of employee IDs.
        /// </summary>
        /// <param name="employeeIds"></param>
        /// <returns></returns>
        IEmployeeDeductionQuery ByEmployeeIds(IEnumerable<int> employeeIds);

        /// <summary>
        /// Filters employee deductions by whether they have a client deduction code that is in the given list of codes.
        /// </summary>
        /// <param name="codes"></param>
        /// <returns></returns>
        IEmployeeDeductionQuery ByClientDeductionCodes(IEnumerable<string> codes);

        IEmployeeDeductionQuery ByEmployeeNumbers(string[] employeeIds);
        IEmployeeDeductionQuery ByClientPlanId(int clientPlanId);

        /// <summary>
        /// Filters active employee deductions which represent Direct Deposit deduction amounts with or without a SubSequenceNum.
        /// </summary>
        /// <returns></returns>
        IEmployeeDeductionQuery ByIsActiveDirectDepositDeduction();

        /// <summary>
        /// Filters Inactive employee deductions which represent Direct Deposit deduction amounts with or without a SubSequenceNum.
        /// </summary>
        /// <returns></returns>
        IEmployeeDeductionQuery ByIsInactiveDirectDepositDeduction();

        IEmployeeDeductionQuery ByEmployeeBankInstantPayProviderId(InstantPayProvider instantPayProviderId);

        /// <summary>
        /// Filters employee deductions which represent Direct Deposit deduction amounts.
        /// Returns active and inactive deductions with or without a SubSequenceNum.
        /// </summary>
        /// <returns></returns>
        IEmployeeDeductionQuery ByIsDirectDepositDeduction();

        /// <summary>
        /// Filters employee deductions which represent Direct Deposit deduction amounts with a SubSequenceNum.
        /// Returns active and inactive deductions.
        /// </summary>
        /// <returns></returns>
        IEmployeeDeductionQuery ByIsDirectDepositDeductionWithSubSequenceNum();
    }
}
