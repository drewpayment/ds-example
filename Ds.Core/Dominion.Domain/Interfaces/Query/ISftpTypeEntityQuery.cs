using Dominion.Core.Dto.SftpConfiguration;
using Dominion.Domain.Entities.Misc;
using Dominion.Utility.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Interfaces.Query
{
    public interface ISftpTypeEntityQuery : IQuery<SftpTypeEntity, ISftpTypeEntityQuery>
    {
        ISftpTypeEntityQuery BySftpTypeId(SftpType sftpTypeId);
    }
}
