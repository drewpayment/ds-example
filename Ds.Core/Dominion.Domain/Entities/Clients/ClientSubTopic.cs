using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Employee;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Clients
{
    public class ClientSubTopic : Entity<ClientSubTopic>, IHasModifiedStringUserIdData
    {
        public virtual int ClientSubTopicId { get; set; }
        public virtual int ClientTopicId { get; set; }
        public virtual string Description { get; set; }
        public DateTime Modified { get; set; }
        public virtual string ModifiedBy { get; set; }

        public virtual ClientTopic ClientTopic { get; set; }
        public virtual ICollection<EmployeeEvent> EmployeeEvents { get; set; }
    }
}
