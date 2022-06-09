using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.W2
{
    public class W2ProcessingSubmittedDto
    {
        public string UniqueId { get; set; }
        public IEnumerable<W2ProcessingDto> ClientList { get; set; }
    }
}
