using System.Collections.Generic;
using Dominion.Core.Dto.Core;
using Dominion.Domain.Entities.Core;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IImageResourceQuery : IQuery<ImageResource, IImageResourceQuery>
    {
        IImageResourceQuery ByResource(int resourceId);
        IImageResourceQuery ByResources(IEnumerable<int> resourceIds);
        IImageResourceQuery ByImageType(ImageType imageTypeId);
        IImageResourceQuery ByImageSizeType(ImageSizeType imageSizeTypeId);
        IImageResourceQuery ByImageSizeType(IEnumerable<ImageSizeType> imageSizeTypeIds);
        IImageResourceQuery ByEmployees(IEnumerable<int> employeeIds);

        IImageResourceQuery ByEmployee(int employeeId);
        IImageResourceQuery ByClient(int clientId);
    }
}
