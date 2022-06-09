using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Client
{
    public class ClientGLAssignmentCustomDto
    { 
        public bool IncludeAccrual     { get; set; }
        public bool IncludeProject     { get; set; }
        public bool IncludeSequence    { get; set; }
        public bool IncludeClassGroups { get; set; }
        public bool IncludeOffset      { get; set; }
        public bool IncludeDetail      { get; set; }
        public int  SaveGroupId        { get; set; }
    
        public ICollection<GeneralLedgerGroupHeaderDto> CashAssignmentHeaders { get; set; }
        public ICollection<GeneralLedgerGroupHeaderDto> LiabilityAssignmentHeaders { get; set; }
        public ICollection<GeneralLedgerGroupHeaderDto> ExpenseAssignmentHeaders { get; set; }
        public ICollection<GeneralLedgerGroupHeaderDto> PaymentAssignmentHeaders { get; set; }
    }
}
