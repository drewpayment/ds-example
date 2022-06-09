using System.Collections.Generic;

namespace Dominion.Core.Dto.Client
{
    public interface IGroupedClientSftpConfigurationDto
    {
        IList<int> ClientIds { get; }
        IClientSftpConfigurationDto ClientSftpConfigurationDto { get; }
    }

    public class GroupedClientSftpConfigurationDto : IGroupedClientSftpConfigurationDto
    {
        public List<int> ClientIds { get; set; }
        public ClientSftpConfigurationDto ClientSftpConfigurationDto { get; set; }

        IList<int> IGroupedClientSftpConfigurationDto.ClientIds => ClientIds;

        IClientSftpConfigurationDto IGroupedClientSftpConfigurationDto.ClientSftpConfigurationDto => ClientSftpConfigurationDto;
    }
}
