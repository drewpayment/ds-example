using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Forms
{
    public class FormDefinitionDto
    {
        public int FormDefinitionId { get; set; }
        public int FormTypeId { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
    }
}
