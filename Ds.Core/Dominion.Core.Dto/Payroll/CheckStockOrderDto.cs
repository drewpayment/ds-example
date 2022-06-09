using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Client;
using Dominion.Core.Dto.User;

namespace Dominion.Core.Dto.Payroll
{
    public partial class CheckStockOrderDto
    {
        public int CheckStockOrderId { get; set; }
        public int ClientId { get; set; }
        public DateTime RequestedDate { get; set; }
        public int NextCheckNumber { get; set; }
        public bool IsDelivery { get; set; }
        public int TotalChecks { get; set; }
        public int CheckEnvelopes { get; set; }
        public int W2Envelopes { get; set; }
        public int ACAEnvelopes { get; set; }
        public decimal OrderPrice { get; set; }
        public DateTime? DatePrinted { get; set; }
        public int RequestedByUserId { get; set; }
        public int? PrintedByUserId { get; set; }
        public ClientCodeAndNameDto ClientDto { get; set; }
        public UserUsernameDto RequestedByUser { get; set; }
        public UserUsernameDto PrintedByUser { get; set; }
    }
}
