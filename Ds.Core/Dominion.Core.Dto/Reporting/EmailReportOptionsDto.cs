using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Reporting
{
    public class EmailReportOptionsDto
    {
        public bool ShowEmailGeneralLedger { get; set; }
        public bool ShowEmailPayStubs      { get; set; }
    }
}
