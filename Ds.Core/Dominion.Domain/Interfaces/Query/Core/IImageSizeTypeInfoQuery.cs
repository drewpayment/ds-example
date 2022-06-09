using System.Collections.Generic;
using Dominion.Core.Dto.Core;
using Dominion.Domain.Entities.Core;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Core
{
    public interface IImageSizeTypeInfoQuery : IQuery<ImageSizeTypeInfo, IImageSizeTypeInfoQuery>
    {
        IImageSizeTypeInfoQuery ByImageSizeType(ImageSizeType imageSizeTypeId);
        IImageSizeTypeInfoQuery ByImageSizeTypes(IEnumerable<ImageSizeType> imageSizeTypeIds);
    }
}
