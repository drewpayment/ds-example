using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Core
{
    public partial class ImageResourceDto
    {
        public int ResourceId { get; set; }
        public int ImageTypeId { get; set; }
        public int ImageSizeTypeId { get; set; }
    }
}
