using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Client
{
    [Serializable]
    public partial class ClientBankInfoDto
    {
        public int ClientId { get; set; } 
		public int? AchBankId { get; set; } 
		public string BankAccount { get; set; } 
		public int? RoutingId { get; set; } 
		public string AltBankAccount { get; set; } 
		public int? AltRoutingId { get; set; } 
		public string TaxAccount { get; set; } 
		public int? TaxRoutingId { get; set; } 
		public string DebitAccount { get; set; } 
		public int? DebitRoutingId { get; set; } 
		public string NachaPrefix { get; set; } 
		public string TaxAccountNachaPrefix { get; set; } 
		public DateTime Modified { get; set; } 
		public string ModifiedBy { get; set; } 
		public bool IsSepChkNumbers { get; set; } 
		public decimal AchLimit { get; set; } 
    }
}
