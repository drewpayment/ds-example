using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Payroll;

namespace Dominion.Core.Dto.Client
{
    public class ClientTaxFileStatusDto
    {
        public int ClientID { get; set; }
        public int TurboTaxFileStatus { get; set; }
        public string Code { get; set; }

    }
}
