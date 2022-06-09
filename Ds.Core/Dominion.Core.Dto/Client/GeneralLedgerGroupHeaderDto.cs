using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Client
{
    public class GeneralLedgerGroupHeaderDto
    {
        public int    GeneralLedgerGroupHeaderId { get; set; }
        public string Description                { get; set; }
        public double  SequenceId                 { get; set; }
        public int    GeneralLedgerGroupId       { get; set; }

        public ICollection<ClientGLControlItemDto> ClientGLControlItems  { get; set; }
        public ICollection<ClientGLAssignmentDto> ClientGLAssignments { get; set; }
    }
}
