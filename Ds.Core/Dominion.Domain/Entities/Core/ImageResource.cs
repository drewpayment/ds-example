using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Core;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Core
{
    public partial class ImageResource : Entity<ImageResource>
    {
        public virtual int ResourceId { get; set; }
        public virtual ImageType ImageTypeId { get; set; }
        public virtual ImageSizeType ImageSizeTypeId { get; set; }

        //FOREIGN KEYS
        public virtual Resource Resource { get; set; }
        public virtual ImageTypeInfo ImageTypeInfo { get; set; }
        public virtual ImageSizeTypeInfo ImageSizeTypeInfo { get; set; }
    }
}
