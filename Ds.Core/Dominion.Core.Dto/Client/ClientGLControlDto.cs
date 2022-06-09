using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Client
{
    public class ClientGLControlDto
    {
        public int  ClientGLControlId  { get; set; }
        public int  ClientId           { get; set; }
        public bool IncludeAccrual     { get; set; }
        public bool IncludeProject     { get; set; }
        public bool IncludeSequence    { get; set; }
        public bool IncludeClassGroups { get; set; }
        public bool IncludeOffset      { get; set; }
        public bool IncludeDetail      { get; set; }
        public int  SaveGroupId        { get; set; }

        public ICollection<ClientGLControlItemDto> ClientGLControlItems  { get; set; }

        public ICollection<GeneralLedgerGroupHeaderDto> CashControlHeaders      { get; set; }
        public ICollection<GeneralLedgerGroupHeaderDto> ExpenseControlHeaders   { get; set; }
        public ICollection<GeneralLedgerGroupHeaderDto> LiabilityControlHeaders { get; set; }
        public ICollection<GeneralLedgerGroupHeaderDto> PaymentControlHeaders   { get; set; }

    }
}
