using Dominion.Core.Dto.SftpConfiguration;
using Dominion.Domain.Entities.Clients;
using Dominion.Utility.Query;
using System.Collections.Generic;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClientSftpConfigurationQuery : IQuery<ClientSftpConfiguration, IClientSftpConfigurationQuery>
    {
        IClientSftpConfigurationQuery ByClientId(int? clientId);
        IClientSftpConfigurationQuery ByClientIds(int[] clientIds);
        IClientSftpConfigurationQuery BySftpType(SftpType sftpTypeId);
        IClientSftpConfigurationQuery BySftpTypes(IEnumerable<SftpType> sftpTypeIds);
    }
}
