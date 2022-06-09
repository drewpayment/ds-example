using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Accounting
{
    public class ClientGLInterfaceDto
    {
        public int ClientGLInterfaceId      { get; set; }
        public int GeneralLedgerInterfaceId {  get; set; }
        public string Description           { get; set; }
        public string FileName              { get; set; }
        public string Email                 { get; set; }
        public int ClientId                 { get; set; }
        public int TaxFrequencyId           { get; set; }
        public DateTime Modified            { get; set; }
        public string ModifiedBy            { get; set; }
        public int StartedTransactionId     { get; set; }
        public string DefaultClass          { get; set; }
        public int MemoOptionId             { get; set; }
        public bool IsTabDelimited          { get; set; }
    }
}
