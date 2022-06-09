using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Client
{
    public class ClientGLSubPayrollTransactionDto
    {
        public int ForeignKeyId { get; set; }
        public string Description { get; set; }
        public GeneralLedgerTypeEnum GeneralLedgerTypeId { get; set; }
    }
}
