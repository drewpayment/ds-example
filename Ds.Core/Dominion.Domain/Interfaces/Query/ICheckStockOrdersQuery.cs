using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Payroll;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface ICheckStockOrdersQuery : IQuery<CheckStockOrder, ICheckStockOrdersQuery>
    {
        ICheckStockOrdersQuery ByClientId(int clientId);

        ICheckStockOrdersQuery ByOrderId(int orderId);
        ICheckStockOrdersQuery ByNotPrinted();
        ICheckStockOrdersQuery ByDateRange(DateTime start, DateTime end);
    }
}
