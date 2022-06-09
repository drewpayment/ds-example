using Dominion.Domain.Entities.Payroll;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IPayrollPayDataQuery : IQuery<PayrollPayData, IPayrollPayDataQuery>
    {
        /// <summary>
        /// Filters the payroll pay data results by the unique Payroll ID.
        /// </summary>
        /// <param name="payrollId">Payroll ID to filter the payroll pay data results by.</param>
        /// <returns></returns>
        IPayrollPayDataQuery ByPayrollId(int payrollId);

        /// <summary>
        /// Filters the payroll pay data results by the specified employee.
        /// </summary>
        /// <param name="employeeId">ID of the employee to return pay data results for.</param>
        /// <returns></returns>
        IPayrollPayDataQuery ByEmployeeId(int employeeId);

        /// <summary>
        /// Filters the payroll pay data results by the specified pay data interface type (import type).
        /// </summary>
        /// <param name="type">Type of import to filter the pay data results by.</param>
        /// <returns></returns>
        IPayrollPayDataQuery ByPayrollPayDataInterfaceType(PayrollPayDataInterfaceType type);

        IPayrollPayDataQuery ByClientId(int clientId);
    }
}