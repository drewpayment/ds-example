using System;
using Dominion.Core.Dto.Misc;
using Dominion.Core.Dto.SftpConfiguration;

namespace Dominion.Domain.Exceptions.SftpConfiguration
{
    public class NoSftpConfigurationsFoundForSftpFileType : Exception
    {
        public NoSftpConfigurationsFoundForSftpFileType()
        {
        }

        public NoSftpConfigurationsFoundForSftpFileType(AccountFeatureEnum clientFeature, SftpType sftpType)
            : base(
                $"No SFTP configurations found, though there are clients who have the {clientFeature} feature enabled. Please add a ClientSftpConfiguration record for each client that has this feature enabled, for the {sftpType} file type.")
        {
        }
        
    }
}
