using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Accounting
{
    public class GeneralLedgerAccountDto
    {
        public int AccountId { get; set; }
        public int ClientId { get; set; }
        public string Description { get; set; }
        public string Number { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }
        public string ErrorInfo { get; set; }
    }

    public class ImportResultDto
    {
        public int TotalRecords { get; set; }
        public IEnumerable<GeneralLedgerAccountDto> ErrorRecords { get; set; }
    }
}
