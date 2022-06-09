using System;
using Dominion.Core.Dto.Misc;
using Dominion.Core.Dto.SftpConfiguration;

namespace Dominion.Domain.Exceptions.SftpConfiguration
{
    public class ClientSftpConfigurationNotFoundForSftpFileType : Exception
    {
        public ClientSftpConfigurationNotFoundForSftpFileType()
        {
        }

        public ClientSftpConfigurationNotFoundForSftpFileType(AccountFeatureEnum clientFeature, SftpType sftpType)
            : base($"An SFTP configuration was not found for the client, though they have the {clientFeature} feature enabled. Please add a ClientSftpConfiguration record for the client for the {sftpType} file type.")
        {
        }
    }
}
