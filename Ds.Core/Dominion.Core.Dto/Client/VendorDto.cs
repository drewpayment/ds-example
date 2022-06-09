using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Client
{
    public class VendorDeductionDto
    {
        public string Name { get; set; }
        public int ClientVendorID { get; set; }
        public int ClientID { get; set; }
    }
}
