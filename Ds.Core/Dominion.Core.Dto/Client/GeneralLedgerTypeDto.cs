using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Client
{
    public class GeneralLedgerTypeDto
    {
        public int     GeneralLedgerTypeId        { get; set; }
        public string  Description                { get; set; }
        public int?    GeneralLedgerGroupId       { get; set; }
        public int?    TaxTypeId                  { get; set; }
        public double  SequenceId                 { get; set; }
        public int?    GeneralLedgerGroupHeaderId { get; set; }
        public bool    CanBeAccrued               { get; set; }
        public bool    CanBeOffset                { get; set; }
        public bool    CanBeDetail                { get; set; }
    }
}
