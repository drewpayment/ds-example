using Dominion.Domain.Entities.Payroll;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IPayrollPayDataInterfaceQuery : IQuery<PayrollPayDataInterface, IPayrollPayDataInterfaceQuery>
    {
        /// <summary>
        /// Filters the payroll pay data interface results by the unique inteface type.
        /// </summary>
        /// <param name="type">The interface (import) type to filter by.</param>
        /// <returns></returns>
        IPayrollPayDataInterfaceQuery ByPayrollPayDataInterfaceType(PayrollPayDataInterfaceType type);
    }
}