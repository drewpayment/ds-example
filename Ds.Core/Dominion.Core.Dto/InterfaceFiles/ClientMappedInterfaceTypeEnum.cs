using System;
using System.Collections.Generic;
using System.Text;

namespace Dominion.Core.Dto.InterfaceFiles
{
    /// <summary>
    /// Taken from <c>ClientMappedInterfaceType</c> table.
    /// 
    /// Table uses byte for this column,
    /// but legacy enum as already implemented as an int.
    /// So, in the interest of avoiding any regressions,
    /// just keeping it as an int for now.
    /// </summary>
    public enum ClientMappedInterfaceTypeEnum // : byte
    {
        Excel = 1,
        CSV = 2
    }
}
