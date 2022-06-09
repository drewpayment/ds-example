using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Core
{
    public enum ImageSizeType
    {
        None = 0,
        XL = 1,
        LG = 2,
        MD = 3,
        SM = 4,
        CompanyLogo,
        CompanyHero
    }
    public partial class ImageSizeTypeInfoDto
    {
        public ImageSizeType ImageSizeTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
    }
}
