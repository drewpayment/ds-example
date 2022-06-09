using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Clients
{
    public class ClientGLClassGroupChangeHistory : Entity<ClientGLClassGroupChangeHistory>, IHasModifiedData
    {
        public int ChangeId { get; set; }
        public int ClientGLClassGroupId { get; set; }
        public int ClientId { get; set; }
        public string ClassGroupCode { get; set; }
        public string ClassGroupDesc { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }
        public string ChangeMode { get; set; }
    }
}

