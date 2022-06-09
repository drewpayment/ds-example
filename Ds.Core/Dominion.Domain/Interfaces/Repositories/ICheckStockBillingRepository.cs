using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Interfaces.Query;

namespace Dominion.Domain.Interfaces.Repositories
{
    public interface ICheckStockBillingRepository : IRepository, IDisposable
    {
        ICheckStockBillingQuery QueryCheckStockBilling();
    }
}
