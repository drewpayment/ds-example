using System;
using Dominion.Core.Dto.SftpConfiguration;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Misc;

namespace Dominion.Domain.Entities.Clients
{
    public class ClientSftpConfiguration : Entity<ClientSftpConfiguration>
    {
        public virtual int      ClientSftpConfigurationId { get; set; }
        public virtual int?      ClientId                  { get; set; }
        public virtual SftpType SftpTypeId                { get; set; }
        public virtual string   Host                      { get; set; }
        public virtual string   Username                  { get; set; }
        public virtual string   Password                  { get; set; }
        public virtual int?     Port                      { get; set; }
        public virtual string   FileName                  { get; set; }
        public virtual string   FileType                  { get; set; }
        public virtual string   RemoteDirectory           { get; set; }
        public virtual string   LocalDirectory            { get; set; }
        public virtual string   LogDirectory              { get; set; }
        public virtual string   LogFileNameFormat         { get; set; }
        public virtual DateTime Modified                  { get; set; }
        public virtual int      ModifiedBy                { get; set; }
        public virtual string NotificationEmail { get; set; }

        // FOREIGN KEYS
        public virtual Client Client { get; set; }
        public virtual SftpTypeEntity SftpTypeEntity { get; set; }
    }
}
