using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Core;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Core
{
    public partial class ImageTypeInfo : Entity<ImageTypeInfo>
    {
        public virtual ImageType ImageTypeId { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }

        //REVERSE NAVIGATION
        public virtual ICollection<ImageResource> ImageResource { get; set; } // many-to-one;
        
    }
}
