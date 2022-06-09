using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Client
{
    public class ClientDivisionLogoDto
    {
        public virtual int ClientDivisionLogoId { get; set; }
        public virtual int ClientDivisionId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual byte[] DivisionLogo { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual int ModifiedBy { get; set; }
    }
}
