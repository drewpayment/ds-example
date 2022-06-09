using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Core
{
    public enum ImageType
    {
        Profile = 1,
        CompanyLogo,
        CompanyHero
    }

    public partial class ImageTypeInfoDto
    {
        public ImageType    ImageTypeId { get; set; } 
        public string       Name        { get; set; }
        public string       Description { get; set; }
    }
}
