using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Payroll;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.OpResult;

namespace Dominion.Domain.Interfaces.Repositories
{
    public interface ICheckStockOrderRepository : IRepository, IDisposable
    {
        /// <summary>
        /// Gets the CheckStockOrder for the given id.
        /// </summary>
        /// <param name="orderId"></param>The ID of the CheckStockOrder.
        /// <returns></returns>
        ICheckStockOrdersQuery GetCheckStockOrder(int orderId);
        
        ICheckStockOrdersQuery QueryCheckStockOrders();
    }
}
