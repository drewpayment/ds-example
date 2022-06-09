using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Client
{
    public class GeneralLedgerTaxGridDto
    {
        public string sTaxId            { get; set; }
        public int    ClientTaxId       { get; set; }
        public string Description       { get; set; }
        public string StateAbbreviation { get; set; }
        public int    StateId           { get; set; }
        public string StateName         { get; set; }
    }
}
