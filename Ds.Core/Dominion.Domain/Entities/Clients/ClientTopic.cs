using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Client;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Clients
{
    public partial class ClientTopic : Entity<ClientTopic>, IHasModifiedStringUserIdData
    {
        public virtual int ClientTopicId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual string Description { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual string ModifiedBy { get; set; }

        public virtual ICollection<ClientSubTopic> ClientSubTopics { get; set; }
    }
}
