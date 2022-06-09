using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Client
{
    public class ClientGLControlItemDto
    {
        public int    ClientGLControlItemId { get; set; }
        public int    ClientGLContolId      { get; set; }
        public int    ClientId              { get; set; }
        public string Description           { get; set; }
        public int    GeneralLedgerTypeId   { get; set; }
        public int?   ForeignKeyId          { get; set; }
        public int    AssignmentMethodId    { get; set; }
        public double  SequenceId            { get; set; }

        public ClientGLControlDto ClientGLControl { get; set; }
    }
}
