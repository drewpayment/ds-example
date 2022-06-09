using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Client
{
    public class ClientGLMappingFliterOptionsDto
    {
        public bool ControlExists      { get; set; }
        public bool SplitByCostCenter  { get; set; }
        public bool SplitByDepartment  { get; set; }
        public bool SplitByCustomClass { get; set; }
        public bool IncludeAccrual     { get; set; }
        public bool IncludeProject     { get; set; }
        public bool IncludeSequence    { get; set; }
        public bool IncludeClassGroups { get; set; }
        public bool IncludeOffset      { get; set; }
        public bool IncludeDetail      { get; set; }

        public ICollection<GeneralLedgerTypeDto> CashGLTypes { get; set; }
        public ICollection<GeneralLedgerTypeDto> LiabilityGLTypes { get; set; }
        public ICollection<GeneralLedgerTypeDto> ExpenseGLTypes { get; set; }
        public ICollection<GeneralLedgerTypeDto> PaymentGLTypes { get; set; }
        public ICollection<ClientGLSubPayrollTransactionDto> SubPayrollTransactions { get; set; }

    }
}
