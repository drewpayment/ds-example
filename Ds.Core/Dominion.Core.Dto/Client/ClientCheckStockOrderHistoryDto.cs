using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Client
{
    public class ClientCheckStockOrderHistoryDto
    {
        public int? ClientId { get; set; }
        public DateTime? RequestedDateStart { get; set; }
        public DateTime? RequestedDateEnd { get; set; }
    }
}
