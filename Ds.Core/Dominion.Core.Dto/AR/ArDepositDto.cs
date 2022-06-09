using System;
using System.Collections.Generic;

namespace Dominion.Core.Dto.AR
{
    public class ArDepositDto
    {
        public int       ArDepositId { get; set; }
        public decimal   Total       { get; set; }
        public DateTime? PostedDate  { get; set; }
        public int?      PostedBy    { get; set; }
        public string PostedByUsername { get; set; }
        public DateTime  CreatedDate { get; set; }
        public string    Type { get; set; }
        public int       CreatedBy   { get; set; }
        public string CreatedByUsername { get; set; }

        public List<ArPaymentDto> ArPayments { get; set; }
    }
}
