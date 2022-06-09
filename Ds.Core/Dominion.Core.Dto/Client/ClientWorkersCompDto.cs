using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Client
{
    public class ClientWorkersCompDto
    {
        public int ClientWorkersCompId { get; set; }
        public int ClientId { get; set; }
        public string Class { get; set; }
        public string Description { get; set; }
        public DateTime EffectiveDatae { get; set; }
        public double Rate { get; set; }
        public bool IsActive { get; set; }
        public DateTime Modified { get; set; }
        public string ModifiedBy { get; set; }
    }
}
