using System.Collections.Generic;
using Dominion.Core.Dto.Core;
using Dominion.Domain.Entities.Core;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IFileFormatQuery : IQuery<FileFormat, IFileFormatQuery>
    {
        IFileFormatQuery ByExcludeFileFormats(List<FileFormatEnum> fileFormatIdsToExclude);
    }
}
