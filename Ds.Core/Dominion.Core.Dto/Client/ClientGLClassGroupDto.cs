using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Client
{
    public class ClientGLClassGroupDto
    {
        public int      ClientGLClassGroupId { get; set; }
        public int      ClientId             { get; set; }
        public string   ClassGroupCode       { get; set; }
        public string   ClassGroupDesc       { get; set; }
        public DateTime Modified             { get; set; }
        public int      ModifiedBy           { get; set; }
    }
}
