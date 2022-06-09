using Dominion.Core.Dto.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Labor
{
    public class ServiceFrequencyDto : IIdAndDescription
    {
        public int    Id          { get; set; }
        public string Description { get; set; }
    }
}
