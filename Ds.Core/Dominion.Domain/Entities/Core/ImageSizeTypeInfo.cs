using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Core;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Core
{
    public partial class ImageSizeTypeInfo : Entity<ImageSizeTypeInfo>
    {
        public virtual ImageSizeType ImageSizeTypeId { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual int Height { get; set; }
        public virtual int Width { get; set; }

        //REVERSE NAVIGATION
        public virtual ICollection<ImageResource> ImageResource { get; set; } // many-to-one;
    }
}
