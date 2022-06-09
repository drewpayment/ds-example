using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Clients
{
    public class ClientGLClassGroup : Entity<ClientGLClassGroup>, IHasModifiedData
    {
        public virtual int      ClientGLClassGroupId { get; set; }
        public virtual int      ClientId             { get; set; }
        public virtual string   ClassGroupCode       { get; set; }
        public virtual string   ClassGroupDesc       { get; set; }
        public virtual DateTime Modified             { get; set; }
        public virtual int      ModifiedBy           { get; set; }
    }
}
