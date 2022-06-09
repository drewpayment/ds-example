using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Utility.Containers;

namespace Dominion.Core.Dto.Core
{
    public class AzureViewDto : ResourceDto
    {
        public string ClientGuid { get; set; }
        public string ResourceGuid { get; set; }
        public new string Name { get; set; }
        public string Token { get; set; }
        public ImageSizeType Size { get; set; }
        public ImageType Type { get; set; }
    }
}
