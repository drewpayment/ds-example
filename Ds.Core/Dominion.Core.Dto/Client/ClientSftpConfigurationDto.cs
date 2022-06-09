using System;
using Dominion.Core.Dto.SftpConfiguration;

namespace Dominion.Core.Dto.Client
{
    public interface IClientSftpConfigurationDto
    {
        int? ClientId { get; }
        int ClientSftpConfigurationId { get; }
        string FileName { get; }
        string FileNameWithFileType { get; }
        string FileType { get; }
        string Host { get; }
        string LocalDirectory { get; }
        string LogDirectory { get; }
        string LogFileNameFormat { get; }
        DateTime Modified { get; }
        int ModifiedBy { get; }
        string NotificationEmail { get; }
        string Password { get; }
        int? Port { get; }
        string RemoteDirectory { get; }
        SftpType SftpTypeId { get; }
        string Username { get; }
    }

    public class ClientSftpConfigurationDto : IClientSftpConfigurationDto
    {
        public int ClientSftpConfigurationId { get; set; }
        public int? ClientId { get; set; }
        public SftpType SftpTypeId { get; set; }
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int? Port { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string RemoteDirectory { get; set; }
        public string LocalDirectory { get; set; }
        public string LogDirectory { get; set; }
        public string LogFileNameFormat { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }
        public string NotificationEmail { get; set; }

        /// <summary>
        /// Returns one of:
        /// $"{FileName}.{FileType}"
        /// $"{FileName}"
        /// string.Empty
        /// </summary>
        public string FileNameWithFileType =>
            (string.IsNullOrEmpty(FileName) && string.IsNullOrEmpty(FileType))
                ? string.Empty
                : (string.IsNullOrEmpty(FileType))
                    ? FileName ?? string.Empty
                    : (string.IsNullOrEmpty(FileName))
                        ? string.Empty
                        : $"{FileName}.{FileType}";

        public static ClientSftpConfigurationDto DeepClone(IClientSftpConfigurationDto source)
        {
            ClientSftpConfigurationDto result = new ClientSftpConfigurationDto();

            result.ClientId = source.ClientId;
            result.ClientSftpConfigurationId = source.ClientSftpConfigurationId;
            result.SftpTypeId = source.SftpTypeId;
            result.Modified = source.Modified;
            result.Port = source.Port;

            result.Host = StringCopySafe(source.Host);
            result.Username = StringCopySafe(source.Username);
            result.Password = StringCopySafe(source.Password);
            result.FileName = StringCopySafe(source.FileName);
            result.FileType = StringCopySafe(source.FileType);
            result.RemoteDirectory = StringCopySafe(source.RemoteDirectory);
            result.LocalDirectory = StringCopySafe(source.LocalDirectory);
            result.LogDirectory = StringCopySafe(source.LogDirectory);
            result.LogFileNameFormat = StringCopySafe(source.LogFileNameFormat);
            result.NotificationEmail = StringCopySafe(source.NotificationEmail);

            return result;
        }

        private static string StringCopySafe(string s) => (s is null) ? null : string.Copy(s);
    }
}
