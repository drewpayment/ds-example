using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Benefits
{
    public class BenefitResourceDto
    {
        public int ResourceId { get; set; }
        public int ClientId { get; set; }
        public int ResourceTypeId { get; set; }
        public bool IsAFile { get; set; }
        public string ResourceLocation { get; set; }
        public string Name { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }


    }
}